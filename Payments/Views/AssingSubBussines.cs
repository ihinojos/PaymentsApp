using DevExpress.XtraGrid.Views.Grid;
using Payments.Models;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace Payments.Views
{
    public partial class AssingSubBussines : Form
    {
        #region Attributes

        private readonly SqlConnection connection;
        private readonly string idFile;
        private readonly string idBussiness;
        private readonly string selectedFilePath;
        private readonly string queryStringSubBussinesFiles;
        private string pathToThisBussinesWaitingAuth;

        #endregion Attributes

        #region Constructor

        public AssingSubBussines(string fileSelected, string pathToFile, string idBussiness, string id)
        {
            InitializeComponent();
            connection = new SqlConnection(DB.cn.Replace(@"\\", @"\"));
            lblFileSelected.Text = fileSelected;
            this.idBussiness = idBussiness;
            Console.WriteLine(fileSelected);
            Console.WriteLine(pathToFile);
            Console.WriteLine(idBussiness);
            Console.WriteLine(id);
            LoadCombo("SELECT * FROM [t_subBussiness] WHERE idBussiness = '" + idBussiness + "';");
            idFile = id;
            selectedFilePath = pathToFile;
            axAcroPDF1.src = pathToFile;
            try
            {
                queryStringSubBussinesFiles = "SELECT s.nameSub FROM [t_invoices] i, [t_subBussiness] s  WHERE i.[id] = '" + idFile + "' AND s.id = i.idSubBussiness;";
                LoadTable(queryStringSubBussinesFiles);
            }
            catch (Exception ex)
            {
                MessageBox.Show("The query had a problem:" + ex);
            }
        }

        #endregion Constructor

        #region Methods

        private void LoadTable(string queryString)
        {
            gridControl1.DataSource = null;
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Connection.Open();
            command.ExecuteNonQuery();
            DataTable FullDT = new DataTable();
            using (SqlDataAdapter DA = new SqlDataAdapter(command))
            {
                DA.Fill(FullDT);
            }
            gridControl1.RefreshDataSource();
            GridView gv = gridView1;
            gridControl1.DataSource = FullDT;
            gridView1.PopulateColumns();
            gridView1.Columns["nameSub"].Caption = "Sub_Bussiness Name";
            gridView1.RowCellClick += gridView1_RowCellClick;
            gridControl1.Update();
            gridControl1.Refresh();
            command.Connection.Close();
        }

        private void LoadCombo(string queryString)
        {
            comboBoxSubBussiness.Items.Clear();
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                comboBoxSubBussiness.Items.Add(reader.GetValue(1).ToString());
            }
            reader.Close();
            command.Connection.Close();
        }

        

        #endregion Methods

        #region Clicks

        private void btnCapture_Click(object sender, EventArgs e)
        {
           
            if (String.IsNullOrEmpty(textBoxTransaction.Text) || String.IsNullOrEmpty(textBoxAmount.Text))
            {
                MessageBox.Show("Verify the inserted information and try again.");
            }
            else
            {
                double amount = Double.Parse(textBoxAmount.Text);
                string newPathForRename = "";
                var dateTimeOffset = new DateTimeOffset(DateTime.Now);
                var formatDate = dateTimeOffset.ToUnixTimeSeconds();
                string newFormat = formatDate + "_" + "waitingAuth" + "_" + textBoxTransaction.Text.ToString() + ".pdf";
                string[] strlist = selectedFilePath.Split(new char[] { '\\' },
                       20, StringSplitOptions.None);
                for (int i = 0; i < strlist.Length - 2; i++)
                {
                    if (i == 0)
                    {
                        newPathForRename = strlist[i];
                    }
                    else
                    {
                        newPathForRename = newPathForRename + "\\" + strlist[i];
                        pathToThisBussinesWaitingAuth = newPathForRename;
                    }
                }
                newPathForRename = newPathForRename + "\\" + "waiting-auth" + "\\" + newFormat;
                axAcroPDF1.src = "";

                try
                {
                    string query = "UPDATE [t_invoices] SET " +
                        "fileName = '" + NewMain.LastElement(newPathForRename) + "', " +
                        "folder= '" + pathToThisBussinesWaitingAuth + "\\" + "waiting-auth\\" + "'," +
                        "status_name = 'waiting-auth'," +
                        "date_modified = GETDATE()," +
                        "transId = '" + textBoxTransaction.Text + "'," +
                        "amount = " + amount + 
                        " WHERE id = '" + idFile + "';";
                    Console.WriteLine(idFile);
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();

                    PdfDocument file = NewMain.AddWaterMark(PdfReader.Open(selectedFilePath, PdfDocumentOpenMode.Modify), "Unsigned invoice");
                    file.Save(newPathForRename);
                    File.Delete(selectedFilePath);

                    MessageBox.Show("Invoice captured correctly");
                    MainViewModel.GetInstance().NewMain.FullRefresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                this.Close();
            }
        }

        private void gridView1_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            GridView gv = gridView1;
            lblSubSelected.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "idSubBussiness").ToString();
        }

        private void btnAssingTo_Click(object sender, EventArgs e)
        {
            try
            {
                string subBussiness = comboBoxSubBussiness.SelectedItem.ToString();
                string idSubBussiness = "";
                string query = "SELECT * FROM [t_subBussiness] WHERE [nameSub] = '" + subBussiness + "' AND [idBussiness] = '"+idBussiness+"';";
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    idSubBussiness = reader[0].ToString();
                }
                reader.Close();
                string queryStringNew = "SELECT * FROM [t_invoices] WHERE [id] = '" + idFile + "' AND [idSubBussiness] ='" + idSubBussiness + "';";
                command.CommandText = queryStringNew;
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    MessageBox.Show("The sub-bussiness already exists");
                    reader.Close();
                    command.Connection.Close();
                }
                else
                {
                    reader.Close();
                    string newSub = lblSubSelected.Text;
                    string queryString2 = "UPDATE [t_invoices] SET [idSubBussiness] = '"+idSubBussiness+"' WHERE [id] = '"+idFile+"';";
                    command.CommandText = queryString2;
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                    LoadTable(queryStringSubBussinesFiles);
                    MessageBox.Show("Assignation sucessful for the file: " + lblFileSelected.Text + " for the sub-bussiness: " + comboBoxSubBussiness.SelectedItem.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion Clicks
    }
}