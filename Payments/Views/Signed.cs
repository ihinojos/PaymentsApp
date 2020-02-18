using DevExpress.XtraEditors.Controls;
using Payments.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Payments.Views
{
    public partial class Signed : Form
    {
        #region Attributes

        private SqlConnection cn;
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
            cn = new SqlConnection(DB.cn.Replace(@"\\", @"\"));
            lblBussiness.Text = Bussiness;
            this.Tid = Tid;
            lblTransID.Text = Tid;
            SearchTransaction(Tid);
        }

        #endregion Constructor

        #region Methods

        private void button1_Click_1(object sender, EventArgs e)
        {
            //Mostrar pdfs
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            var hola = openFileDialog1.ShowDialog();
            axAcroPDF2.src = openFileDialog1.FileName;
            lblNameNewFile.Text = lastElement(openFileDialog1.FileName);
            pathToNewFile = openFileDialog1.FileName;
            //Fin Mostrar pdfs
        }

        public void putCroppedPdf(string file)
        {
            axAcroPDF2.src = file;
        }

        public string lastElement(string splitme)
        {
            string[] strlist = splitme.Split(new char[] { '\\' },
                       20, StringSplitOptions.None);
            return strlist[strlist.Length - 1].ToString();
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

        private void createNewNomenclature()
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
            char[] spearator = { '\\' };
            Int32 count = 20;
            string path = MainViewModel.GetInstance().newmain.newpath;
            // Using the Method
            String[] strlist = path.Split(spearator,
                   count, StringSplitOptions.None);
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
                System.IO.File.Move(pathToOldFile, newPathForRenameOld);
                System.IO.File.Move(pathToNewFile, newPathForRenameNew);

                //Hacer update de los cambios recientes, de nomenclatura, nuevo estado y nuevo id de transaccion
                folder = folder.Replace("waiting-auth", "signed");
                string queryStringDelete1 = "UPDATE [PRUEBA1].[dbo].[t_files] SET " +
                    "fileName = '" + newFormat + "', " +
                    "folder= '" + folder + "'," +
                    "transId = '" + lblTransID.Text + "', " +
                    "status_name = 'signed'" +
                    " WHERE id = '" + id + "';";
                SqlCommand commandDelete1 = new SqlCommand(queryStringDelete1, cn);
                commandDelete1.Connection.Open();
                int row3 = commandDelete1.ExecuteNonQuery();
                commandDelete1.Connection.Close();
                //Hacer update de los cambios recientes, de nomenclatura, nuevo estado y nuevo id de transaccion

                //Hacer insercion de los cambios recientes, de nomenclatura, nuevo estado y nuevo id de transaccion
                string queryStringDelete12 = "INSERT INTO [PRUEBA1].[dbo].[t_files] (id, fileName, folder, idstatus, transId, status_name,type)" +
                    " VALUES (NEWID(), '" + newFormat2 + "', '" + folder + "', '', '" + lblTransID.Text + "', 'signed','2');";
                SqlCommand commandDelete12 = new SqlCommand(queryStringDelete12, cn);
                commandDelete12.Connection.Open();
                int row4 = commandDelete12.ExecuteNonQuery();
                commandDelete12.Connection.Close();
                //Hacer insercion de los cambios recientes, de nomenclatura, nuevo estado y nuevo id de transaccion
                MessageBox.Show("Invoice signed successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please check if the file already exists o... " + ex);
            }
            MainViewModel.GetInstance().newmain.fullRefresh();
            MainViewModel.GetInstance().newmain.LoadTable(MainViewModel.GetInstance().newmain.queryString);
            MainViewModel.GetInstance().newmain.gridControl1.Update();

            
            this.Close();
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
                    createNewNomenclature();
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
                if (MainViewModel.GetInstance().splitPDF == null)
                {
                    MainViewModel.GetInstance().splitPDF = new SplitPDF(pathToNewFile, "sign");
                    MainViewModel.GetInstance().splitPDF.FormClosed += MainViewModel.GetInstance().newmain.FormClosed;
                    MainViewModel.GetInstance().splitPDF.Show();
                }
                else MainViewModel.GetInstance().splitPDF.BringToFront();
            }
        }

        #endregion Methods



        private void SearchTransaction(string transId)
        {
            string querytransId = "SELECT * FROM [PRUEBA1].[dbo].[t_transactions] WHERE id = '" + transId + "';";
            SqlCommand cmd = new SqlCommand(querytransId, cn);
            cmd.Connection.Open();
            SqlDataReader read = cmd.ExecuteReader();
            string fid = null;
            if (read.Read())
            {
                Tid = read[0].ToString();
                fid = read[1].ToString();
                cmd.Connection.Close();
            }
            else
            {
                MessageBox.Show("Transaction not found.");
                axAcroPDF2.src = "";
            }
            if (!String.IsNullOrEmpty(fid))
            {
                string querystringstatus = "SELECT * FROM [PRUEBA1].[dbo].[t_files] WHERE transId = '" + fid + "';";
                lblTransID.Text = fid;
                lblTransNumber.Text = Tid;

                SqlCommand commandstatus2 = new SqlCommand(querystringstatus, cn);
                commandstatus2.Connection.Open();
                SqlDataReader reader = commandstatus2.ExecuteReader();
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
                        axAcroPDF1.src = "";
                        axAcroPDF1.src = pathToOldFile;
                    }
                }
                else
                {
                }
                commandstatus2.Connection.Close();
                obtainSubBussinesRelationated();
            }
        }


    }
}