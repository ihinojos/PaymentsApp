﻿using Payments.Models;
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
        private string Tid;
        private string folder;
        private string pathToOldFile;
        private string pathToNewFile;

        #endregion Attributes

        #region Constructor

        public Signed(string Bussiness, string Tid)
        {
            InitializeComponent();
            connection = new SqlConnection(DB.cn.Replace(@"\\", @"\"));
            lblBussiness.Text = Bussiness;
            this.Tid = Tid;
            lblTransID.Text = Tid;
            SearchTransaction(Tid);
        }

        #endregion Constructor

        #region Methods

        public void PutCroppedPdf(string file)
        {
            pathToNewFile = file;
            this.axAcroPDF2.src = file;
        }



        private void ObtainSubBussinesRelationated()
        {
            treeView1.Nodes.Clear();
            string queryobtainid = "select f.*, s.nameSub  from [TESTPAY].[dbo].[t_filesSubs] f, [TESTPAY].[dbo].[t_subbussiness] s where f.idFile = '" + id + "' AND f.idSubBussiness = s.id;";
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
            string newPathForRenameOld = "";
            string newPathForRenameNew = "";
            //Nomenclatura = 12.12.2019_estado_#Transaction.pdf
            //Variable path vieja: pathfileSelected
            //Variable path nueva: newPathForRename
            // Display date (uses calendar of current culture    by default).
            var dateTimeOffset = new DateTimeOffset(DateTime.Now);
            var formatDate = dateTimeOffset.ToUnixTimeSeconds();
            string newFormat = formatDate + "_" + "Bill-Unsigned" + "_" + Tid + ".pdf";
            string newFormat2 = formatDate + "_" + "Bill-Signed" + "_" + Tid + ".pdf";
            string path = MainViewModel.GetInstance().NewMain.newpath;
            string[] strlist = path.Split(new char[] { '\\' },
                   20, StringSplitOptions.None);
            for (int i = 0; i < strlist.Length; i++)
            {
                if (i == 0)
                {
                    newPathForRenameOld = strlist[i];
                }
                else
                {
                    newPathForRenameOld = newPathForRenameOld + "\\" + strlist[i];
                }
            }


            newPathForRenameOld = newPathForRenameOld + "\\" + lblBussiness.Text;
            newPathForRenameNew = newPathForRenameOld;
            newPathForRenameOld = newPathForRenameOld + "\\" + "Signed" + "\\" + newFormat;
            newPathForRenameNew = newPathForRenameNew + "\\" + "Signed" + "\\" + newFormat2;

            try
            {
                //Hacer update de los cambios recientes, de nomenclatura, nuevo estado y nuevo id de transaccion
                folder = folder.Replace("waiting-auth", "signed");
                string queryStringDelete1 = "UPDATE [TESTPAY].[dbo].[t_files] SET " +
                    "fileName = '" + newFormat + "', " +
                    "folder= '" + folder + "'," +
                    "transId = '" + lblTransID.Text + "', " +
                    "status_name = 'signed'" +
                    " WHERE id = '" + id + "';";
                SqlCommand command = new SqlCommand(queryStringDelete1, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
                //Hacer update de los cambios recientes, de nomenclatura, nuevo estado y nuevo id de transaccion

                //Hacer insercion de los cambios recientes, de nomenclatura, nuevo estado y nuevo id de transaccion
                string queryStringDelete12 = "INSERT INTO [TESTPAY].[dbo].[t_files] (id, fileName, folder, transId, status_name,type)" +
                    " VALUES (NEWID(), '" + newFormat2 + "', '" + folder + "', '" + lblTransID.Text + "', 'signed','2');";
                command.CommandText = queryStringDelete12;
                command.ExecuteNonQuery();

                string idsub = "SELECT f.id, fs.idSubBussiness FROM t_files f, t_filesSubs fs WHERE f.fileName = '" + newFormat + "' AND f.id = fs.idFile";
                command.CommandText = idsub;

                using (var read = command.ExecuteReader())
                {
                    if (read.Read())
                    {
                        idsub = read[1].ToString();
                    }
                    read.Close();
                }

                string idfile = "SELECT id FROM t_files WHERE fileName = '" + newFormat2 + "';";
                command.CommandText = idfile;

                using (var read = command.ExecuteReader())
                {
                    if (read.Read())
                    {
                        idfile = read[0].ToString();
                    }
                    read.Close();
                }

                string q = "INSERT INTO t_filesSubs ([idFile], [idSubBussiness]) VALUES ('"+idfile+"','"+idsub+"')";
                command.CommandText = q;
                command.ExecuteNonQuery();
                command.Connection.Close();

                System.IO.File.Move(pathToOldFile, newPathForRenameOld);
                System.IO.File.Move(pathToNewFile, newPathForRenameNew);

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

        private void SearchTransaction(string transId)
        {
            string querytransId = "SELECT * FROM [TESTPAY].[dbo].[t_transactions] WHERE id = '" + transId + "';";
            SqlCommand command = new SqlCommand(querytransId, connection);
            command.Connection.Open();
            SqlDataReader read = command.ExecuteReader();
            string fid = null;
            if (read.Read())
            {
                Tid = read[0].ToString();
                fid = read[1].ToString();
                read.Close();
            }
            else
            {
                MessageBox.Show("Transaction not found.");
                axAcroPDF2.src = "";
            }
            if (!String.IsNullOrEmpty(fid))
            {
                string querystringstatus = "SELECT * FROM [TESTPAY].[dbo].[t_files] WHERE transId = '" + fid + "';";
                lblTransID.Text = fid;
                lblTransNumber.Text = Tid;
                command.CommandText = querystringstatus;
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