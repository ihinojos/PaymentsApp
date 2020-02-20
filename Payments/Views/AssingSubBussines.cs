using DevExpress.XtraGrid.Views.Grid;
using Payments.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Payments.Views
{
    public partial class AssingSubBussines : Form
    {
        #region Attributes

        private readonly SqlConnection connection;
        public DataTable FullDT;
        private readonly string idFileSelected;
        private readonly string pathfileSelected;
        private readonly string queryString;
        private string idTansaction;
        private string queryStringSubBussinesFiles;
        private string pathToThisBussinesWaitingAuth;

        #endregion Attributes

        #region Constructor

        public AssingSubBussines(string fileSelected, string pathToFile, string idBussiness, string id)
        {
            InitializeComponent();
            connection = new SqlConnection(DB.cn.Replace(@"\\", @"\"));
            queryString = "SELECT * FROM [PAYMENTS].[dbo].[t_subbussiness] WHERE idBussiness = '" + idBussiness + "';";
            lblFileSelected.Text = fileSelected;
            LoadCombo(queryString);
            idFileSelected = id;
            pathfileSelected = pathToFile;
            axAcroPDF1.src = pathToFile;
            try
            {
                queryStringSubBussinesFiles = "SELECT * FROM [PAYMENTS].[dbo].[t_filesSubs] WHERE idFile = '" + idFileSelected + "';";
                LoadTable(queryStringSubBussinesFiles);
            }
            catch (Exception ex)
            {
                MessageBox.Show("The query had a problem:" + ex);
            }
        }

        #endregion Constructor

        #region Methods

        public string LastElement(string splitme)
        {
            string[] strlist = splitme.Split(new char[] { '\\' },
                       20, StringSplitOptions.None);
            return strlist[strlist.Length - 1].ToString();
        }

        private void LoadTable(string queryString)
        {
            gridControl1.DataSource = null;
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Connection.Open();
            command.ExecuteNonQuery();
            FullDT = new DataTable();
            using (SqlDataAdapter DA = new SqlDataAdapter(command))
            {
                DA.Fill(FullDT);
            }
            gridControl1.RefreshDataSource();
            GridView gv = gridView1;
            gridControl1.DataSource = FullDT;
            gridView1.PopulateColumns();
            gridView1.Columns["idFile"].Visible = false;
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
            do
            {
                while (reader.Read())
                {
                    comboBoxSubBussiness.Items.Add(reader.GetValue(1).ToString());
                }
            } while (reader.NextResult());
            reader.Close();
            command.Connection.Close();
        }

        #endregion Methods

        #region Clicks

        private void btnCapture_Click(object sender, EventArgs e)
        {
            int subBussiness = 0;
            Cursor.Current = Cursors.WaitCursor;
            queryStringSubBussinesFiles = "SELECT COUNT (idFile) FROM [PAYMENTS].[dbo].[t_filesSubs] WHERE idFile = '" + idFileSelected + "';";
            SqlCommand command = new SqlCommand(queryStringSubBussinesFiles, connection);
            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            do
            {
                while (reader.Read())
                {
                    subBussiness = Convert.ToInt32(reader.GetValue(0));
                }
            } while (reader.NextResult());
            reader.Close();

            if (textBoxTransaction.Text == "" || subBussiness <= 0)
            {
                MessageBox.Show("Verify if file has sub_bussiness or there is a transacion Id");
            }
            else
            {
                string newPathForRename = "";
                var dateTimeOffset = new DateTimeOffset(DateTime.Now);
                var formatDate = dateTimeOffset.ToUnixTimeSeconds();
                string newFormat = formatDate + "_" + "waitingAuth" + "_" + textBoxTransaction.Text.ToString() + ".pdf";
                string[] strlist = pathfileSelected.Split(new char[] { '\\' },
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
                    //Insertar nueva transaccion
                    string queryStringformat1 = "INSERT INTO [PAYMENTS].[dbo].[t_transactions]([id],[transactionId])" +
                                                               " VALUES( NEWID(),'" + textBoxTransaction.Text
                                                               + "')";
                    command.CommandText = queryStringformat1;
                    command.ExecuteNonQuery();
                    //Fin Insertar nueva transaccion

                    //Obtener el id creado en el paso anterior para posteriormente indexarlo a la tabla t_files
                    string queryObtainNewId = "select * from [PAYMENTS].[dbo].[t_transactions] where transactionId ='" + textBoxTransaction.Text + "';";
                    command.CommandText = queryObtainNewId;
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        idTansaction = reader[1].ToString();
                        string transactionNumber = reader[0].ToString();
                    }
                    reader.Close();

                    //Fin obtener el id creado en el paso anterior para posteriormente indexarlo a la tabla t_files

                    //Hacer update de los cambios recientes, de nomenclatura, nuevo estado y nuevo id de transaccion
                    string queryStringDelete1 = "UPDATE [PAYMENTS].[dbo].[t_files] SET " +
                        "fileName = '" + LastElement(newPathForRename) + "', " +
                        "folder= '" + pathToThisBussinesWaitingAuth + "\\" + "waiting-auth\\" + "'," +
                        "transId = '" + idTansaction + "', " +
                        "status_name = 'waiting-auth'" +
                        " WHERE id = '" + idFileSelected + "';";
                    command.CommandText = queryStringDelete1;
                    command.ExecuteNonQuery();

                    System.IO.File.Move(pathfileSelected, newPathForRename);

                    //Hacer update de los cambios recientes, de nomenclatura, nuevo estado y nuevo id de transaccion
                    MessageBox.Show("Invoice captured correctly");
                    MainViewModel.GetInstance().NewMain.FullRefresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                command.Connection.Close();
                Cursor.Current = Cursors.Default;
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
                string query = "SELECT * FROM [PAYMENTS].[dbo].[t_subbussiness] WHERE [nameSub] = '" + subBussiness + "';";
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    idSubBussiness = reader[0].ToString();
                }
                reader.Close();
                string queryStringNew = "SELECT * FROM [PAYMENTS].[dbo].[t_filesSubs] WHERE idFile = '" + idFileSelected + "' AND idSubBussiness ='" + idSubBussiness + "';";
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
                    string queryString2 = "INSERT INTO [PAYMENTS].[dbo].[t_filesSubs]([idFile],[idSubBussiness])" +
                                                           " VALUES('" + idFileSelected
                                                           + "','" + idSubBussiness
                                                                   + "')";
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