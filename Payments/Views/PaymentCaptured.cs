using Payments.Models;
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
        private readonly string transId;
        private readonly string bussiness;
        private string pathToNewFile;
        public string newpath = "C:\\TestFiles";
        private T_SubBussines[] allSubs;
        private T_Files[] files;
        private string id;
        private string incomingFile;
        private string pathNewState;

        #endregion Attributes

        #region Constructor

        public PaymentCaptured(string bussiness, string transId)
        {
            InitializeComponent();
            this.transId = transId;
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
            string query = "SELECT * FROM [PAYMENTS].[dbo].[t_transactions] where [id] = '" + transId + "';";
            SqlCommand command = new SqlCommand(query, connection);
            command.Connection.Open();
            SqlDataReader read = command.ExecuteReader();
            if (read.Read())
                lblTransID.Text = read[0].ToString();
            read.Close();
            lblBussiness.Text = bussiness;
            lblTransNumber.Text = transId;
            string queryStringStatus = "SELECT * FROM [PAYMENTS].[dbo].[t_files] WHERE transId = '" + transId + "' and type ='2';";
            command.CommandText = queryStringStatus;
            read = command.ExecuteReader();
            if (read.Read())
            {
                if (read[4].ToString() != "making-payment")
                {
                    MessageBox.Show("This transaction number is in another state, choose a diferent one");
                }
                else
                {
                    string fileName = read[1].ToString();
                    string folder = read[2].ToString();
                    id = read[0].ToString();
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
            read.Close();
            string queryStringStatus3 = "SELECT * FROM [PAYMENTS].[dbo].[t_files] WHERE transId = '" + transId + "' and type ='1';";
            command.CommandText = queryStringStatus3;
            read = command.ExecuteReader();
            if (read.Read())
            {
                if (read[4].ToString() != "making-payment")
                {
                    MessageBox.Show("This transaction number is in another state, choose a diferent one");
                }
                else
                {
                    id = read[0].ToString();
                }
            }
            else
            {
                MessageBox.Show("There is nothing to show");
            }
            read.Close();
            command.Connection.Close();
            ObtainSubBussinesRelationated();
        }

        private void ObtainSubBussinesRelationated()
        {
            treeView1.Nodes.Clear();
            string queryobtainid = "select f.*, s.nameSub  from [PAYMENTS].[dbo].[t_filesSubs] f, [PAYMENTS].[dbo].[t_subbussiness] s where f.idFile = '" + id + "' AND f.idSubBussiness = s.id;";
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
            string newPathSigned = "";
            string newPathNoSigned = "";
            string newPathProof = "";
            var dateTimeOffset = new DateTimeOffset(DateTime.Now);
            var formatDate = dateTimeOffset.ToUnixTimeSeconds();
            string newFormat = formatDate + "_" + "Payment-Captured-Unsigned" + "_" + lblTransID.Text + ".pdf";
            string newFormat2 = formatDate + "_" + "Payment-Captured-Signed" + "_" + lblTransID.Text + ".pdf";
            string newFormat3 = formatDate + "_" + "Payment-Captured-Proof" + "_" + lblTransID.Text + ".pdf";

            string path = MainViewModel.GetInstance().NewMain.newpath;
            String[] strlist = path.Split(new char[] { '\\' },
                   20, StringSplitOptions.None);
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
            newPathSigned = newPathSigned + "\\" + NewMain.LastElement(bussiness);
            newPathNoSigned = newPathSigned;
            newPathProof = newPathSigned;
            pathNewState = newPathSigned;
            newPathSigned = newPathSigned + "\\" + "payment-captured" + "\\" + newFormat2;
            newPathNoSigned = newPathNoSigned + "\\" + "payment-captured" + "\\" + newFormat;
            newPathProof = newPathProof + "\\" + "payment-captured" + "\\" + newFormat3;
            string queryobtainid = "select * from [PAYMENTS].[dbo].[t_transactions] where [transactionId] = '" + lblTransID.Text + "';";
            SqlCommand command = new SqlCommand(queryobtainid, connection);
            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                string queryObtainPaths = "select * from [PAYMENTS].[dbo].[t_files] where transId = '" + reader[1].ToString() + "';";
                command.CommandText = queryObtainPaths;
                reader.Close();
                SqlDataReader reader2 = command.ExecuteReader();
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
                            string queryUpdateNotSigned = "UPDATE [PAYMENTS].[dbo].[t_files] SET fileName = '" + NewMain.LastElement(newPathNoSigned) + "', folder='" + pathNewState + "\\payment-captured\\" + "',status_name='payment-captured' WHERE id LIKE '%" + item.Id + "%' and type='1';";
                            command.CommandText = queryUpdateNotSigned;
                            command.ExecuteNonQuery();
                        }
                        if (pathito.Contains("Signed"))
                        {
                            System.IO.File.Move(pathito, newPathSigned);
                            string queryUpdateSigned = "UPDATE [PAYMENTS].[dbo].[t_files] SET fileName = '" + NewMain.LastElement(newPathSigned) + "', folder='" + pathNewState + "\\payment-captured\\" + "',status_name='payment-captured' WHERE id LIKE '%" + item.Id + "%' and type='2';";
                            command.CommandText = queryUpdateSigned;
                            command.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("We had a problem moving the file, please review if the file already exists or:" + ex);
                    }
                }
                try
                {
                    string queryUpdateSigned = "INSERT INTO [PAYMENTS].[dbo].[t_files] (id, fileName, folder, transId,status_name,type) VALUES (NEWID(), '" + NewMain.LastElement(newPathProof) + "', '" + pathNewState + "\\payment-captured\\" + "', '" + lblTransNumber.Text + "', 'payment-captured','3');";
                    command.CommandText = queryUpdateSigned;
                    command.ExecuteNonQuery();
                    string idsub = "SELECT f.id, fs.idSubBussiness FROM t_files f, t_filesSubs fs WHERE f.fileName = '" + NewMain.LastElement(newPathSigned) + "' AND f.id = fs.idFile";
                    command.CommandText = idsub;

                    using (var read = command.ExecuteReader())
                    {
                        if (read.Read())
                        {
                            idsub = read[1].ToString();
                        }
                        read.Close();
                    }


                    string idfile = "SELECT id FROM t_files WHERE fileName = '" + NewMain.LastElement(newPathProof) + "';";
                    command.CommandText = idfile;

                    using (var read = command.ExecuteReader())
                    {
                        if (read.Read())
                        {
                            idfile = read[0].ToString();
                        }
                        read.Close();
                    }

                    string q = "INSERT INTO t_filesSubs ([idFile], [idSubBussiness]) VALUES ('" + idfile + "','" + idsub + "')";
                    command.CommandText = q;
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                    System.IO.File.Move(incomingFile, newPathProof);
                    MessageBox.Show("Invoice marked as paid correctly");
                    MainViewModel.GetInstance().NewMain.FullRefresh();
                }
                catch (Exception ex2)
                {
                    MessageBox.Show("We had a problem moving the file, please review if the file already exists :" + ex2);
                }
            }
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