using DevExpress.XtraEditors.Controls;
using Payments.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Payments.Views
{
    public partial class MakingPayment : Form
    {
        #region Attributes

        private readonly SqlConnection connection;
        private readonly string transId;
        private Transactions[] allSubs;
        private Transactions[] allSubs2;
        private T_Files[] files;
        private string newFormat;
        private string idTransForQuery2;
        private string newFormat2;

        #endregion Attributes

        #region Constructor

        public MakingPayment(string transId)
        {
            InitializeComponent();
            this.transId = transId;
            connection = new SqlConnection(DB.cn.Replace(@"\\", @"\"));
            radioButton1.Checked = true;
            LoadTree();
            lookUpEdit1.Visible = false;
            treeView1.Visible = true;
        }

        #endregion Constructor

        #region Methods

        private void LoadTree()
        {
            treeView1.Nodes.Clear();
            string queryobtainid = "select * from [prueba1].[dbo].[t_transactions] where [id] = '" + transId + "';";
            SqlCommand command = new SqlCommand(queryobtainid, connection);
            command.Connection.Open();
            using (var reader = command.ExecuteReader())
            {
                var list = new List<Transactions>();
                while (reader.Read())
                    list.Add(new Transactions { Id = reader.GetString(1), TransactionQB = reader.GetString(0) });
                allSubs = list.ToArray();
                reader.Close();
            }
            int count = 0;
            foreach (Transactions record in allSubs)
            {
                string queryobtain = "select * from [prueba1].[dbo].[t_files] where transId ='" + record.Id + "';";
                command.CommandText = queryobtainid;
                using (var reader = command.ExecuteReader())
                {
                    var list = new List<T_Files>();
                    while (reader.Read())
                        list.Add(new T_Files { Id = reader.GetString(0), Name = reader.GetString(1), Fullroute = reader.GetString(2) });
                    files = list.ToArray();
                    reader.Close();
                }
                treeView1.BeginUpdate();
                treeView1.Nodes.Add(record.TransactionQB);
                foreach (T_Files file in files)
                {
                    treeView1.Nodes[count].Nodes.Add(file.Name);
                }
                count += 1;
                treeView1.EndUpdate();
            }
            command.Connection.Close();
        }

        private void LoadLookUpEdit()
        {
            string querystringstatus = "SELECT [transactionId],[id] FROM [PRUEBA1].[dbo].[t_transactions] where [id] = '" + transId + "';";
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand command = new SqlCommand(querystringstatus, connection);
            command.Connection.Open();
            command.CommandType = CommandType.Text;
            // Set the SqlDataAdapter's SelectCommand.
            adapter.SelectCommand = command;
            // Fill the DataSet.
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            lookUpEdit1.Properties.PopulateColumns();
            lookUpEdit1.Properties.Columns.Add(new LookUpColumnInfo("id", "id"));
            lookUpEdit1.Properties.Columns.Add(new LookUpColumnInfo("transactionId", "transactionId"));
            lookUpEdit1.Properties.DataSource = dataSet.Tables[0];
            lookUpEdit1.Properties.DisplayMember = "transactionId";
            lookUpEdit1.Properties.ValueMember = "id";
            command.Connection.Close();
        }

        private void CreateNewNomenclature()
        {
            try
            {
                string newPathForRenameOld = "";
                string newPathForRenameNew = "";
                //Nomenclatura = 12.12.2019_estado_#Transaction.pdf
                //Variable path vieja: pathfileSelected
                //Variable path nueva: newPathForRename
                var dateTimeOffset = new DateTimeOffset(DateTime.Now);
                var formatDate = dateTimeOffset.ToUnixTimeSeconds();
                if (radioButton1.Checked)
                {
                    newFormat = formatDate + "_" + "Making-Payment-Unsigned" + "_" + lblSelected.Text + ".pdf";
                    newFormat2 = formatDate + "_" + "Making-Payment-Signed" + "_" + lblSelected.Text + ".pdf";
                }
                if (radioButton2.Checked)
                {
                    newFormat = formatDate + "_" + "Making-Payment-Unsigned" + "_" + idTransForQuery2 + ".pdf";
                    newFormat2 = formatDate + "_" + "Making-Payment-Signed" + "_" + idTransForQuery2 + ".pdf";
                }
                string path = MainViewModel.GetInstance().NewMain.newpath;
                // Using the Method
                String[] strlist = path.Split(new char[] { '\\' },
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
                newPathForRenameOld = MainViewModel.GetInstance().NewMain.nameBussiness + "making-payment" + "\\" + newFormat;
                newPathForRenameNew = MainViewModel.GetInstance().NewMain.nameBussiness + "making-payment" + "\\" + newFormat2;

                string queryobtainid = "select * from [prueba1].[dbo].[t_transactions] where transactionId='" + lblSelected.Text + "';";
                SqlCommand command = new SqlCommand(queryobtainid, connection);
                command.Connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    var list2 = new List<Transactions>();
                    while (reader.Read())
                        list2.Add(new Transactions { Id = reader.GetString(1), TransactionQB = reader.GetString(0) });
                    allSubs2 = list2.ToArray();
                    reader.Close();
                }

                foreach (var item in allSubs2)
                {
                    string queryFile = "select * from [prueba1].[dbo].[t_files] where transId='" + item.Id + "';";
                    command.CommandText = queryFile;
                    using (var reader2 = command.ExecuteReader())
                    {
                        var list3 = new List<T_Files>();
                        while (reader2.Read())
                            list3.Add(new T_Files { Id = reader2.GetString(0), Name = reader2.GetString(1), Fullroute = reader2.GetString(2), TransId = reader2.GetString(4) });
                        files = list3.ToArray();
                        reader2.Close();
                    }
                }
                foreach (var item in files)
                {
                    string cadena = item.Name;

                    if (cadena.Contains("Unsigned"))
                    {
                        string oldRouteUsigned = item.Fullroute + item.Name;
                        System.IO.File.Move(oldRouteUsigned, newPathForRenameOld);
                        string queryFile = "UPDATE [PRUEBA1].[dbo].[t_files] SET fileName = '" + LastElement(newPathForRenameOld)
                            + "', status_name = 'making-payment', folder='" + MainViewModel.GetInstance().NewMain.nameBussiness + "making-payment\\" + "' WHERE transId LIKE '" + item.TransId + "%' and type='1';";
                        command.CommandText = queryFile;
                        command.ExecuteNonQuery();
                    }
                    if (cadena.Contains("Signed"))
                    {
                        string oldRouteSigned = item.Fullroute + item.Name;
                        System.IO.File.Move(oldRouteSigned, newPathForRenameNew);
                        string queryFile = "UPDATE[PRUEBA1].[dbo].[t_files] SET fileName = '" + LastElement(newPathForRenameNew)
                            + "', status_name = 'making-payment', folder='" + MainViewModel.GetInstance().NewMain.nameBussiness + "making-payment\\" + "' WHERE transId LIKE '" + item.TransId + "%' and type = '2';";
                        command.CommandText = queryFile;
                        command.ExecuteNonQuery();
                    }
                }
                command.Connection.Close();
                MainViewModel.GetInstance().NewMain.fullRefresh();
                MessageBox.Show("Invoice marked as payment in process");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please select a file or: " + ex);
            }
        }

        public string LastElement(string splitme)
        {
            string[] strlist = splitme.Split(new char[] { '\\' },
                        20, StringSplitOptions.None);
            return strlist[strlist.Length - 1].ToString();
        }

        #endregion Methods

        #region Clicks

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            treeView1.Visible = false;
            lookUpEdit1.Visible = true;
            LoadLookUpEdit();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            treeView1.Visible = true;
            lookUpEdit1.Visible = false;
            LoadTree();
        }

        private void lookUpEdit1_EditValueChanged_1(object sender, EventArgs e)
        {
            idTransForQuery2 = lookUpEdit1.GetColumnValue("transactionId").ToString();
            lblSelected.Text = idTransForQuery2;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode.Parent == null)
            {
                var id = treeView1.SelectedNode.Text;
                lblSelected.Text = id.ToString();
                lblFile.Text = "No file selected";
            }
            else
            {
                string id = treeView1.SelectedNode.Parent.ToString();
                id = id.Replace("TreeNode: ", "");
                lblSelected.Text = id.ToString();
                string name = treeView1.SelectedNode.ToString();
                name = name.Replace("TreeNode: ", "");
                lblFile.Text = name.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            CreateNewNomenclature();
            Cursor.Current = Cursors.Default;
        }

        #endregion Clicks
    }
}