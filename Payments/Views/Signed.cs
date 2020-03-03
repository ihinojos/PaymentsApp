using Payments.Models;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Payments.Views
{
    public partial class Signed : Form
    {
        #region Attributes

        private readonly SqlConnection connection;
        private T_SubBussines[] allSubs;
        private string id;
        private string invoiceID;
        private string folder;
        private string pathToOldFile;
        private string pathToNewFile;

        #endregion Attributes

        #region Constructor

        public Signed(string Bussiness, string invoiceID)
        {
            InitializeComponent();
            connection = new SqlConnection(DB.cn.Replace(@"\\", @"\"));
            lblBussiness.Text = Bussiness;
            this.invoiceID = invoiceID;
            lblTransID.Text = invoiceID;
            SearchInvoice(invoiceID);
        }

        #endregion Constructor

        #region Methods

        public void PutCroppedPdf(string file)
        {
            pathToNewFile = file;
            axAcroPDF2.src = pathToNewFile;
        }



        private void ObtainSubBussinesRelationated()
        {
            treeView1.Nodes.Clear();
            string queryobtainid = "select f.*, s.nameSub  from [t_fileSubs] f, [t_subBussiness] s where f.idFile = '" + id + "' AND f.idSubBussiness = s.id;";
            SqlCommand command = new SqlCommand(queryobtainid, connection);
            command.Connection.Open();
            using (var reader = command.ExecuteReader())
            {
                var list = new List<T_SubBussines>();
                while (reader.Read())
                    list.Add(new T_SubBussines { Id = reader.GetString(1), IdFile = reader.GetString(0), IdSubBussiness = reader.GetString(2) });
                allSubs = list.ToArray();
                reader.Close();
            }
            foreach (T_SubBussines record in allSubs)
            {
                treeView1.BeginUpdate();
                treeView1.Nodes.Add(record.IdSubBussiness);
                treeView1.EndUpdate();
            }
            command.Connection.Close();
        }

        private void CreateNewNomenclature()
        {
            var dateTimeOffset = new DateTimeOffset(DateTime.Now);
            var formatDate = dateTimeOffset.ToUnixTimeSeconds();
            string newFormat = formatDate + "_" + "Bill-Signed" + "_" + lblTransID.Text + ".pdf";
            string path = MainViewModel.GetInstance().NewMain.newpath;
            path += "\\" + lblBussiness.Text;
            path += "\\" + "Signed" + "\\" + newFormat;

            try
            {
                //Hacer update de los cambios recientes, de nomenclatura, nuevo estado y nuevo id de transaccion
                folder = folder.Replace("waiting-auth", "signed");
                string queryStringDelete1 = "UPDATE [t_invoices] SET " +
                    "fileName = '" + newFormat + "', " +
                    "folder= '" + folder + "'," +
                    "status_name = 'signed'" +
                    "date_modified = GETDATE() WHERE id = '" + id + "';";
                SqlCommand command = new SqlCommand(queryStringDelete1, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
                //Hacer update de los cambios recientes, de nomenclatura, nuevo estado y nuevo id de transaccion

                PdfDocument combined = NewMain.Combine(PdfReader.Open(pathToOldFile, PdfDocumentOpenMode.Import), PdfReader.Open(pathToNewFile, PdfDocumentOpenMode.Import));

                combined.Save(path);

                System.IO.File.Delete(pathToOldFile);
                //Hacer insercion de los cambios recientes, de nomenclatura, nuevo estado y nuevo id de transaccion
                MessageBox.Show("Invoice signed successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please check if the file already exists o... " + ex);
            }
            MainViewModel.GetInstance().NewMain.FullRefresh();
            this.Close();
        }



        private void SearchInvoice(string invoiceID)
        {
            string querystringstatus = "SELECT * FROM [TESTPAY].[dbo].[t_invoices] WHERE [id] = '" + invoiceID + "';";
            SqlCommand command = new SqlCommand(querystringstatus, connection);
            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                if (reader[5].ToString() == "signed")
                {
                    MessageBox.Show("This transaction number is not valid for this action. \n" +
                        "Current status: " + reader[5].ToString());
                }
                else
                {
                    lblTransID.Text = reader[5].ToString();
                    lblTransAmount.Text = "$"+reader[6].ToString();
                    string fileName = reader[1].ToString();
                    folder = reader[2].ToString();
                    id = reader[0].ToString();
                    lblNameOldFile.Text = fileName;
                    pathToOldFile = folder + "\\" + fileName;
                    axAcroPDF1.src = pathToOldFile;
                }
            }
            reader.Close();
            command.Connection.Close();
            ObtainSubBussinesRelationated();

        }

        #endregion Methods

        #region Clicks

        private void button1_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.ShowDialog();
            axAcroPDF2.src = openFileDialog1.FileName;
            lblNameNewFile.Text = NewMain.LastElement(openFileDialog1.FileName);
            pathToNewFile = openFileDialog1.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (lblNameNewFile.Text == "" | lblNameOldFile.Text == "")
                {
                    MessageBox.Show("You must select a transaction first, then a file to add to that transaction then you can finish the movement");
                }
                else
                {
                    CreateNewNomenclature();
                    this.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ups" + ex);
            }
            Cursor.Current = Cursors.Default;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (lblNameNewFile.Text == "")
            {
                MessageBox.Show("You must select first a signed file");
            }
            else
            {
                var instance = MainViewModel.GetInstance().SplitPdf;
                if (instance != null) instance.Dispose();
                instance = MainViewModel.GetInstance().SplitPdf = new SplitPDF(pathToNewFile, "sign");
                instance.Show();
            }
        }

        #endregion Clicks
    }
}