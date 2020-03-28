using Payments.Models;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace Payments.Views
{
    public partial class PaymentCaptured : Form
    {
        #region Attributes

        private readonly string userDic = MainViewModel.GetInstance().NewMain.userDic;
        private readonly SqlConnection connection;
        private readonly string invoiceID;
        private readonly string bussiness;
        private string pathToNewFile;
        private string pathNewState;
        private T_Invoices[] files;
        private string id;

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

        private void LoadDocument()
        {
            string query = "SELECT * FROM [t_invoices] where [id] = '" + invoiceID + "';";
            SqlCommand command = new SqlCommand(query, connection);
            command.Connection.Open();
            SqlDataReader read = command.ExecuteReader();
            if (read.Read())
            {
                lblTransID.Text = read[5].ToString();
                decimal amnt = Convert.ToDecimal(read[6].ToString());
                lblAmount.Text = amnt.ToString("c2");
                string fileName = read[1].ToString();
                string folder = read[2].ToString();
                id = read[0].ToString();
                lblNameOldFile.Text = fileName;
                string pathToOldFile = folder + fileName;
                axAcroPDF2.src = "";
                axAcroPDF2.src = userDic + "\\" + pathToOldFile;
            }
            read.Close();
            lblBussiness.Text = bussiness;
            command.Connection.Close();
            ObtainSubBussinesRelationated();
        }

        private void ObtainSubBussinesRelationated()
        {
            treeView1.Nodes.Clear();
            string queryobtainid = "SELECT [idSubBussiness] FROM [t_invoices] WHERE [id] = '" + id + "';";
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
            string newPathSigned = MainViewModel.GetInstance().NewMain.rootPath;
            var dateTimeOffset = new DateTimeOffset(DateTime.Now);
            var formatDate = dateTimeOffset.ToUnixTimeSeconds();
            string newFormat = formatDate + "_" + "Bill-Paid-Proof" + "_" + lblTransID.Text + ".pdf";
            newPathSigned += "\\" + NewMain.ElementAt(bussiness, 1);
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
                        Id = reader[0].ToString()
                       ,
                        FileName = reader[1].ToString()
                       ,
                        Folder = reader[2].ToString()
                       ,
                        Status = reader[3].ToString()
                       ,
                        Date = reader[4].ToString()
                       ,
                        TransId = reader[5].ToString()
                       ,
                        Amount = Double.Parse(reader[6].ToString())
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
                        PdfDocument signed = NewMain.AddWaterMark(PdfReader.Open(pathToNewFile, PdfDocumentOpenMode.Modify), "Proof of payment");
                        string tempFile = Path.Combine(Path.GetTempPath(), "pop.pdf");
                        signed.Save(tempFile);
                        PdfDocument outPdf = NewMain.Combine(PdfReader.Open(userDic + "\\" + pathito, PdfDocumentOpenMode.Import), PdfReader.Open(tempFile, PdfDocumentOpenMode.Import));
                        string queryUpdateSigned = "UPDATE [t_invoices] SET fileName = '" + NewMain.ElementAt(newPathSigned, 1) + "', folder='" + pathNewState + "\\payment-captured\\" + "',status_name='payment-captured', " +
                            " date_modified = GETDATE() WHERE id = '" + item.Id + "';";
                        command.CommandText = queryUpdateSigned;
                        command.ExecuteNonQuery();
                        outPdf.Save(userDic + "\\" + newPathSigned);
                        File.Delete(userDic + "\\" + pathito);
                        File.Delete(tempFile);
                        if (pathToNewFile.Contains("crop")) File.Delete(pathToNewFile);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            MessageBox.Show("Invoice marked as paid correctly");

            MainViewModel.GetInstance().NewMain.FullRefresh();

            command.Connection.Close();
        }

        public void PutCroppedPdf(string file)
        {
            axAcroPDF1.src = file;
            pathToNewFile = file;
        }

        #endregion Methods

        #region Clicks

        private void Button1_Click(object sender, EventArgs e)
        {
            //Mostrar pdfs
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.ShowDialog();
            axAcroPDF1.src = openFileDialog1.FileName;
            lblNameNewFile.Text = NewMain.ElementAt(openFileDialog1.FileName, 1);
            pathToNewFile = openFileDialog1.FileName;
            //Fin Mostrar pdfs
        }

        private void Button3_Click(object sender, EventArgs e)
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

        private void Button2_Click(object sender, EventArgs e)
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

        #region Events

        private void WhenClosed(object sender, FormClosedEventArgs e)
        {
            MainViewModel.GetInstance().NewMain.BringToFront();
        }

        #endregion Events
    }
}