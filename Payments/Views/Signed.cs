using Payments.Models;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace Payments.Views
{
    public partial class Signed : Form
    {
        #region Attributes

        private readonly SqlConnection connection;
        private readonly string invoiceID;
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
            string queryobtainid = "SELECT [idSubBussiness] FROM [t_invoices] WHERE [id] = '" + invoiceID + "';";
            SqlCommand command = new SqlCommand(queryobtainid, connection);
            command.Connection.Open();
            string idsub = "";
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                    idsub = reader[0].ToString();
                reader.Close();
            }
            command.CommandText = "SELECT [nameSub] FROM [t_subBussiness] WHERE [id] = '" + idsub + "';";
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    treeView1.BeginUpdate();
                    treeView1.Nodes.Add(reader[0].ToString());
                    treeView1.EndUpdate();
                }
                reader.Close();
            }
            command.Connection.Close();
        }

        private void CreateNewNomenclature()
        {
            var dateTimeOffset = new DateTimeOffset(DateTime.Now);
            var formatDate = dateTimeOffset.ToUnixTimeSeconds();
            string newFormat = formatDate + "_" + "Bill-Signed" + "_" + lblTransID.Text + ".pdf";
            string path = MainViewModel.GetInstance().NewMain.rootPath;
            path += "\\" + lblBussiness.Text;
            path += "\\" + "Signed" + "\\" + newFormat;
            try
            {
                folder = folder.Replace("waiting-auth", "signed");
                string queryStringDelete1 = "UPDATE [t_invoices] SET " +
                    "fileName = '" + newFormat + "', " +
                    "folder= '" + folder + "'," +
                    "status_name = 'signed'," +
                    "date_modified = GETDATE() WHERE id = '" + invoiceID + "';";
                SqlCommand command = new SqlCommand(queryStringDelete1, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
                PdfDocument signed = NewMain.AddWaterMark(PdfReader.Open(pathToNewFile, PdfDocumentOpenMode.Modify), "Signed invoice");
                string tempFile = Path.Combine(Path.GetTempPath(), "signed.pdf");
                signed.Save(tempFile);
                PdfDocument combined = NewMain.Combine(PdfReader.Open(pathToOldFile, PdfDocumentOpenMode.Import), PdfReader.Open(tempFile, PdfDocumentOpenMode.Import));
                combined.Save(path);
                File.Delete(pathToOldFile);
                File.Delete(tempFile);
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
            string querystringstatus = "SELECT * FROM [t_invoices] WHERE [id] = '" + invoiceID + "';";
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
                    lblTransAmount.Text = "$" + reader[6].ToString();
                    string fileName = reader[1].ToString();
                    folder = reader[2].ToString();
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
            lblNameNewFile.Text = NewMain.ElementAt(openFileDialog1.FileName, 1);
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