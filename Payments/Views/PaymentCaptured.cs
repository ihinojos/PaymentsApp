using Payments.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Payments.Views
{
    public partial class PaymentCaptured : Form
    {
        #region Attributes

        private string pathToNewFile;
        private SqlConnection cn;
        public String newpath = "C:\\TestFiles";
        private T_SubBussines[] allSubs;
        private T_Files[] files;
        private string id;
        private string incomingFile;
        private string buss;
        private string pathNewState;
        private string transId;

        #endregion Attributes

        #region Constructor

        public PaymentCaptured(string bussiness, string transId)
        {
            InitializeComponent();
            this.transId = transId;
            buss = bussiness;
            cn = new SqlConnection(DB.cn.Replace(@"\\", @"\"));
            this.FormClosed += new FormClosedEventHandler(WhenClosed);

            loadDocument();
        }

        #endregion Constructor

        #region Methods

        private void button1_Click(object sender, EventArgs e)
        {
            //Mostrar pdfs
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            var hola = openFileDialog1.ShowDialog();
            incomingFile = openFileDialog1.FileName;
            axAcroPDF1.src = openFileDialog1.FileName;
            lblNameNewFile.Text = lastElement(openFileDialog1.FileName);
            pathToNewFile = openFileDialog1.FileName;
            //Fin Mostrar pdfs
        }

        private void WhenClosed(object sender, FormClosedEventArgs e)
        {
            MainViewModel.GetInstance().newmain.Visible = true;
        }

        public string lastElement(string splitme)
        {
            string[] strlist = splitme.Split(new char[] { '\\' },
                       20, StringSplitOptions.None);
            return strlist[strlist.Length - 1].ToString();
        }

        private void loadDocument()
        {
            //MessageBox.Show(lookUpEdit1.GetColumnValue("id").ToString());
            string query = "SELECT * FROM [PRUEBA1].[dbo].[t_transactions] where [id] = '" + transId + "';";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Connection.Open();
            SqlDataReader read = cmd.ExecuteReader();
            if (read.Read())
                lblTransID.Text = read[0].ToString();
            lblBussiness.Text = buss;
            cmd.Connection.Close();
            lblTransNumber.Text = transId;
            string querystringstatus = "SELECT * FROM [PRUEBA1].[dbo].[t_files] WHERE transId = '" + transId + "' and type ='2';";
            SqlCommand commandstatus2 = new SqlCommand(querystringstatus, cn);
            commandstatus2.Connection.Open();
            SqlDataReader reader = commandstatus2.ExecuteReader();
            if (reader.Read())
            {
                if (reader[5].ToString() != "making-payment")
                {
                    MessageBox.Show("This transaction number is in another state, choose a diferent one");
                }
                else
                {
                    string fileName = reader[1].ToString();
                    string folder = reader[2].ToString();
                    id = reader[0].ToString();
                    lblNameOldFile.Text = fileName;
                    string pathToOldFile = folder + fileName;
                    axAcroPDF2.src = "";
                    axAcroPDF2.src = pathToOldFile;
                }
            }
            else
            {
                MessageBox.Show("There is nothing to show");
            }
            commandstatus2.Connection.Close();
            string querystringstatus3 = "SELECT * FROM [PRUEBA1].[dbo].[t_files] WHERE transId = '" + transId + "' and type ='1';";
            SqlCommand commandstatus3 = new SqlCommand(querystringstatus3, cn);
            commandstatus3.Connection.Open();
            SqlDataReader reader2 = commandstatus3.ExecuteReader();
            if (reader2.Read())
            {
                if (reader2[5].ToString() != "making-payment")
                {
                    MessageBox.Show("This transaction number is in another state, choose a diferent one");
                }
                else
                {
                    id = reader2[0].ToString();
                }
            }
            else
            {
                MessageBox.Show("There is nothing to show");
            }
            commandstatus2.Connection.Close();
            commandstatus3.Connection.Close();
            obtainSubBussinesRelationated();
        }

        private void obtainSubBussinesRelationated()
        {
            treeView1.Nodes.Clear();
            string queryobtainid = "select * from [prueba1].[dbo].[t_filesSubs] where idFile = '" + id + "';";
            SqlCommand commandid = new SqlCommand(queryobtainid, cn);
            if (commandid.Connection.State != ConnectionState.Open)
            {
                commandid.Connection.Close();
                commandid.Connection.Open();
            }
            int row = commandid.ExecuteNonQuery();
            using (var reader2 = commandid.ExecuteReader())
            {
                var list = new List<T_SubBussines>();
                while (reader2.Read())
                    list.Add(new T_SubBussines { Id = reader2.GetString(0), IdFile = reader2.GetString(1), IdSubBussiness = reader2.GetString(2) });
                allSubs = list.ToArray();
            }
            foreach (T_SubBussines record in allSubs)
            {
                treeView1.BeginUpdate();
                treeView1.Nodes.Add(record.IdSubBussiness);
                treeView1.EndUpdate();
            }
            commandid.Connection.Close();
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
                    createNewNomenclature();
                    this.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ups" + ex);
            }
        }

        private void createNewNomenclature()
        {
            string newPathSigned = "";
            string newPathNoSigned = "";
            string newPathProof = "";
            var dateTimeOffset = new DateTimeOffset(DateTime.Now);
            var formatDate = dateTimeOffset.ToUnixTimeSeconds();
            string newFormat = formatDate + "_" + "Payment-Captured-Unsigned" + "_" + lblTransID.Text + ".pdf";
            string newFormat2 = formatDate + "_" + "Payment-Captured-Signed" + "_" + lblTransID.Text + ".pdf";
            string newFormat3 = formatDate + "_" + "Payment-Captured-Proof" + "_" + lblTransID.Text + ".pdf";
            char[] spearator = { '\\' };
            Int32 count = 20;
            string path = MainViewModel.GetInstance().newmain.newpath;
            String[] strlist = path.Split(spearator,
                   count, StringSplitOptions.None);
            for (int i = 0; i < strlist.Length; i++)
            {
                if (i == 0)
                {
                    newPathSigned = strlist[i];
                }
                else
                {
                    newPathSigned = newPathSigned + "\\" + strlist[i];
                }
            }
            newPathSigned = newPathSigned + "\\" + lastElement(buss);
            newPathNoSigned = newPathSigned;
            newPathProof = newPathSigned;
            pathNewState = newPathSigned;
            newPathSigned = newPathSigned + "\\" + "payment-captured" + "\\" + newFormat2;
            newPathNoSigned = newPathNoSigned + "\\" + "payment-captured" + "\\" + newFormat;
            newPathProof = newPathProof + "\\" + "payment-captured" + "\\" + newFormat3;
            string queryobtainid = "select * from [PRUEBA1].[dbo].[t_transactions] where [transactionId] = '" + lblTransID.Text + "';";
            SqlCommand commandid = new SqlCommand(queryobtainid, cn);
            if (commandid.Connection.State != ConnectionState.Open)
            {
                commandid.Connection.Close();
                commandid.Connection.Open();
            }
            int row = commandid.ExecuteNonQuery();
            SqlDataReader reader = commandid.ExecuteReader();
            if (reader.Read())
            {
                string queryobtainPaths = "select * from [prueba1].[dbo].[t_files] where transId = '" + reader[1].ToString() + "';";
                SqlCommand commandpaths = new SqlCommand(queryobtainPaths, cn);
                if (commandpaths.Connection.State != ConnectionState.Open)
                {
                    commandpaths.Connection.Close();
                    commandpaths.Connection.Open();
                    commandid.Connection.Close();
                }
                reader.Close();
                int row4 = commandpaths.ExecuteNonQuery();
                SqlDataReader reader2 = commandpaths.ExecuteReader();
                var list = new List<T_Files>();
                while (reader2.Read())
                    list.Add(new T_Files { Id = reader2.GetString(0), Name = reader2.GetString(1), Fullroute = reader2.GetString(2) });
                files = list.ToArray();
                reader2.Close();
                foreach (var item in files)
                {
                    try
                    {
                        string pathito = item.Fullroute + item.Name;
                        if (pathito.Contains("Unsigned"))
                        {
                            System.IO.File.Move(pathito, newPathNoSigned);
                            string queryUpdateNotSigned = "UPDATE [PRUEBA1].[dbo].[t_files] SET fileName = '" + lastElement(newPathNoSigned) + "', folder='" + pathNewState + "\\payment-captured\\" + "',status_name='payment-captured' WHERE id LIKE '%" + item.Id + "%' and type='1';";
                            SqlCommand commandUpdateNotSigned = new SqlCommand(queryUpdateNotSigned, cn);
                            if (commandUpdateNotSigned.Connection.State != ConnectionState.Open)
                            {
                                commandUpdateNotSigned.Connection.Close();
                                commandUpdateNotSigned.Connection.Open();
                            }
                            int rowNotSigned = commandUpdateNotSigned.ExecuteNonQuery();
                        }
                        if (pathito.Contains("Signed"))
                        {
                            System.IO.File.Move(pathito, newPathSigned);
                            string queryUpdateSigned = "UPDATE [PRUEBA1].[dbo].[t_files] SET fileName = '" + lastElement(newPathSigned) + "', folder='" + pathNewState + "\\payment-captured\\" + "',status_name='payment-captured' WHERE id LIKE '%" + item.Id + "%' and type='2';";
                            SqlCommand commandUpdateSigned = new SqlCommand(queryUpdateSigned, cn);
                            if (commandUpdateSigned.Connection.State != ConnectionState.Open)
                            {
                                commandUpdateSigned.Connection.Close();
                                commandUpdateSigned.Connection.Open();
                            }
                            int rowSigned = commandUpdateSigned.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("We had a problem moving the file, please review if the file already exists or:" + ex);
                    }
                }
                try
                {
                    string queryUpdateSigned = "INSERT INTO [PRUEBA1].[dbo].[t_files] (id, fileName, folder, idstatus, transId,status_name,type) VALUES (NEWID(), '" + lastElement(newPathProof) + "', '" + pathNewState + "\\payment-captured\\" + "', '', '" + lblTransNumber.Text + "', 'payment-captured','3');";
                    SqlCommand commandUpdateSigned = new SqlCommand(queryUpdateSigned, cn);
                    if (commandUpdateSigned.Connection.State != ConnectionState.Open)
                    {
                        commandUpdateSigned.Connection.Close();
                        commandUpdateSigned.Connection.Open();
                    }
                    int rowSigned = commandUpdateSigned.ExecuteNonQuery();

                    System.IO.File.Move(incomingFile, newPathProof);
                    MessageBox.Show("Invoice marked as paid correctly");
                    MainViewModel.GetInstance().newmain.fullRefresh();
                }
                catch (Exception ex2)
                {
                    MessageBox.Show("We had a problem moving the file, please review if the file already exists :" + ex2);
                }
            }
        }

        #endregion Methods

        private void button3_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(pathToNewFile))
            {
                if (MainViewModel.GetInstance().splitPDF == null)
                {
                    MainViewModel.GetInstance().splitPDF = new SplitPDF(pathToNewFile, "pay");
                    MainViewModel.GetInstance().splitPDF.FormClosed += MainViewModel.GetInstance().newmain.FormClosed;
                    MainViewModel.GetInstance().splitPDF.Show();
                }
                else MainViewModel.GetInstance().splitPDF.BringToFront();
            }
            else MessageBox.Show("Please select a file first");
        }

        public void putCroppedPdf(string file)
        {
            axAcroPDF1.src = file;
        }
    }
}