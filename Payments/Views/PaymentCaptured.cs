using Payments.Models;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Payments.Views
{
    public partial class PaymentCaptured : Form
    {
        #region Attributes

        private readonly SqlConnection connection;
        private readonly string invoiceID;
        private readonly string bussiness;
        private string pathToNewFile;
        private T_SubBussines[] allSubs;
        private T_Invoices[] files;
        private string id;
        private string incomingFile;
        private string pathNewState;

        #endregion Attributes

        #region Constructor

        public PaymentCaptured(string bussiness, string invoiceID)
        {
            InitializeComponent();
            this.invoiceID = invoiceID;
            this.bussiness = bussiness;
            connection = new SqlConnection(DB.cn.Replace(@"\\", @"\"));
            this.FormClosed += new FormClosedEventHandler(WhenClosed);
            LoadDocument();
        }

        #endregion Constructor

        #region Methods

        private void WhenClosed(object sender, FormClosedEventArgs e)
        {
            MainViewModel.GetInstance().NewMain.BringToFront();
        }


        private void LoadDocument()
        {
            string query = "SELECT * FROM [t_invoices] where [id] = '" + invoiceID + "';";
            SqlCommand command = new SqlCommand(query, connection);
            command.Connection.Open();
            SqlDataReader read = command.ExecuteReader();
            if (read.Read())
            {
                lblTransID.Text = read[5].ToString();
                lblAmount.Text = "$"+read[6].ToString();
                string fileName = read[1].ToString();
                string folder = read[2].ToString();
                id = read[0].ToString();
                lblNameOldFile.Text = fileName;
                string pathToOldFile = folder + fileName;
                axAcroPDF2.src = "";
                axAcroPDF2.src = pathToOldFile;
            }
            read.Close();
            lblBussiness.Text = bussiness;
            command.Connection.Close();
            ObtainSubBussinesRelationated();
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
                    list.Add(new T_SubBussines { Id = reader[1].ToString(), IdFile = reader[0].ToString(), IdSubBussiness = reader[2].ToString() }); ;
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
            string newPathSigned = MainViewModel.GetInstance().NewMain.newpath;
            var dateTimeOffset = new DateTimeOffset(DateTime.Now);
            var formatDate = dateTimeOffset.ToUnixTimeSeconds();
            string newFormat = formatDate + "_" + "Bill-Paid-Proof" + "_" + lblTransID.Text + ".pdf";
            newPathSigned += "\\" + NewMain.LastElement(bussiness);
            pathNewState = newPathSigned;
            newPathSigned += "\\" + "payment-captured" + "\\" + newFormat;
            string query = "select * from [t_invoices] where [id] = '" + invoiceID + "';";
            SqlCommand command = new SqlCommand(query, connection);
            command.Connection.Open();
            using (var reader = command.ExecuteReader())
            {
                var list = new List<T_Invoices>();
                while (reader.Read())
                    list.Add(new T_Invoices
                    {
                        Id = reader[0].ToString(),
                        FileName = reader[1].ToString(),
                        Folder = reader[2].ToString(),
                        Status = reader[3].ToString(),
                        Date = reader[4].ToString(),
                        TransId = reader[5].ToString(),
                        Amount = Double.Parse(reader[6].ToString()),
                    });
                files = list.ToArray();
                reader.Close();
            }
            foreach (var item in files)
            {
                try
                {
                    string pathito = item.Folder + item.FileName;
                    if (pathito.Contains("Paying"))
                    {
                        MessageBox.Show("has paying");
                        PdfDocument outPdf = NewMain.Combine(PdfReader.Open(pathito, PdfDocumentOpenMode.Import), PdfReader.Open(incomingFile, PdfDocumentOpenMode.Import));
                        string queryUpdateSigned = "UPDATE [t_invoices] SET fileName = '" + NewMain.LastElement(newPathSigned) + "', folder='" + pathNewState + "\\payment-captured\\" + "',status_name='payment-captured', " +
                            " date_modified = GETDATE() WHERE id = '" + item.Id + "';";
                        command.CommandText = queryUpdateSigned;
                        command.ExecuteNonQuery();
                        outPdf.Save(newPathSigned);
                        System.IO.File.Delete(pathito);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("We had a problem moving the file, please review if the file already exists or:" + ex);
                }
            }
            MessageBox.Show("Invoice marked as paid correctly");
            MainViewModel.GetInstance().NewMain.FullRefresh();

            command.Connection.Close();
        }

        public void PutCroppedPdf(string file)
        {
            incomingFile = file;
            axAcroPDF1.src = file;
        }

        #endregion Methods

        #region Clicks

        private void button1_Click(object sender, EventArgs e)
        {
            //Mostrar pdfs
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.ShowDialog();
            incomingFile = openFileDialog1.FileName;
            axAcroPDF1.src = openFileDialog1.FileName;
            lblNameNewFile.Text = NewMain.LastElement(openFileDialog1.FileName);
            pathToNewFile = openFileDialog1.FileName;
            //Fin Mostrar pdfs
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(pathToNewFile))
            {
                var instance = MainViewModel.GetInstance().SplitPdf;
                if (instance != null) instance.Dispose();
                instance = MainViewModel.GetInstance().SplitPdf = new SplitPDF(pathToNewFile, "pay");
                instance.Show();
            }
            else MessageBox.Show("Please select a file first");
        }

        private void button2_Click(object sender, EventArgs e)
        {
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
        }

        #endregion Clicks
    }
}