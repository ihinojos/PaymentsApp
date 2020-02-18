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

        private SqlConnection cn;
        private List<Transactions> ListTrans = new List<Transactions>();
        private Transactions[] allSubs;
        private Transactions[] allSubs2;
        private T_Files[] files;
        private string newFormat;
        private string idTransForQuery2;
        private string newFormat2;
        private string transId;

        #endregion Attributes

        #region Constructor

        public MakingPayment(string transId)
        {
            InitializeComponent();
            this.transId = transId;
            cn = new SqlConnection(DB.cn.Replace(@"\\", @"\"));
            radioButton1.Checked = true;
            loadTree();
            lookUpEdit1.Visible = false;
            treeView1.Visible = true;
        }

        #endregion Constructor

        #region Methods

        private void loadTree()
        {
            treeView1.Nodes.Clear();
            string queryobtainid = "select * from [prueba1].[dbo].[t_transactions] where [id] = '"+transId+"';";
            SqlCommand commandid = new SqlCommand(queryobtainid, cn);
            if (commandid.Connection.State != ConnectionState.Open)
            {
                commandid.Connection.Close();
                commandid.Connection.Open();
            }
            int row = commandid.ExecuteNonQuery();
            using (var reader2 = commandid.ExecuteReader())
            {
                var list = new List<Transactions>();
                while (reader2.Read())
                    list.Add(new Transactions { Id = reader2.GetString(1), TransactionQB = reader2.GetString(0) });
                allSubs = list.ToArray();
            }
            int count = 0;
            foreach (Transactions record in allSubs)
            {
                string queryobtain = "select * from [prueba1].[dbo].[t_files] where transId ='" + record.Id + "';";
                SqlCommand command = new SqlCommand(queryobtain, cn);
                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Close();
                    command.Connection.Open();
                }
                int row2 = command.ExecuteNonQuery();
                using (var reader2 = command.ExecuteReader())
                {
                    var list = new List<T_Files>();
                    while (reader2.Read())
                        list.Add(new T_Files { Id = reader2.GetString(0), Name = reader2.GetString(1), Fullroute = reader2.GetString(2) });
                    files = list.ToArray();
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
            commandid.Connection.Close();
        }

        private void loadLookUpEdit()
        {
            string querystringstatus = "SELECT [transactionId],[id] FROM [PRUEBA1].[dbo].[t_transactions] where [id] = '"+transId+"';";
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand commandstatus2 = new SqlCommand(querystringstatus, cn);
            commandstatus2.Connection.Open();
            commandstatus2.ExecuteNonQuery();
            commandstatus2.CommandType = CommandType.Text;
            // Set the SqlDataAdapter's SelectCommand.
            adapter.SelectCommand = commandstatus2;
            // Fill the DataSet.
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            lookUpEdit1.Properties.PopulateColumns();
            lookUpEdit1.Properties.Columns.Add(new LookUpColumnInfo("id", "id"));
            lookUpEdit1.Properties.Columns.Add(new LookUpColumnInfo("transactionId", "transactionId"));
            lookUpEdit1.Properties.DataSource = dataSet.Tables[0];
            lookUpEdit1.Properties.DisplayMember = "transactionId";
            lookUpEdit1.Properties.ValueMember = "id";
            commandstatus2.Connection.Close();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            treeView1.Visible = false;
            lookUpEdit1.Visible = true;
            loadLookUpEdit();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            treeView1.Visible = true;
            lookUpEdit1.Visible = false;
            loadTree();
        }

        private void lookUpEdit1_EditValueChanged_1(object sender, EventArgs e)
        {
            string idTransForQuery = lookUpEdit1.GetColumnValue("id").ToString();
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
            createNewNomenclature();
            Cursor.Current = Cursors.Default;
        }

        private void createNewNomenclature()
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
                char[] spearator = { '\\' };
                Int32 count = 20;
                string path = MainViewModel.GetInstance().NewMain.newpath;
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
                newPathForRenameOld = MainViewModel.GetInstance().NewMain.nameBussiness + "making-payment" + "\\" + newFormat;
                newPathForRenameNew = MainViewModel.GetInstance().NewMain.nameBussiness + "making-payment" + "\\" + newFormat2;

                string queryobtainid = "select * from [prueba1].[dbo].[t_transactions] where transactionId='" + lblSelected.Text + "';";
                SqlCommand commandid = new SqlCommand(queryobtainid, cn);
                if (commandid.Connection.State != ConnectionState.Open)
                {
                    commandid.Connection.Close();
                    commandid.Connection.Open();
                }
                int row = commandid.ExecuteNonQuery();
                using (var reader2 = commandid.ExecuteReader())
                {
                    var list2 = new List<Transactions>();
                    while (reader2.Read())
                        list2.Add(new Transactions { Id = reader2.GetString(1), TransactionQB = reader2.GetString(0) });
                    allSubs2 = list2.ToArray();
                }

                foreach (var item in allSubs2)
                {
                    string queryFile = "select * from [prueba1].[dbo].[t_files] where transId='" + item.Id + "';";
                    SqlCommand commandFile = new SqlCommand(queryFile, cn);
                    if (commandFile.Connection.State != ConnectionState.Open)
                    {
                        commandFile.Connection.Close();
                        commandFile.Connection.Open();
                    }
                    int row4 = commandFile.ExecuteNonQuery();
                    using (var reader2 = commandFile.ExecuteReader())
                    {
                        var list3 = new List<T_Files>();
                        while (reader2.Read())
                            list3.Add(new T_Files { Id = reader2.GetString(0), Name = reader2.GetString(1), Fullroute = reader2.GetString(2), TransId = reader2.GetString(4) });
                        files = list3.ToArray();
                    }
                }
                foreach (var item in files)
                {
                    string cadena = item.Name;

                    if (cadena.Contains("Unsigned"))
                    {
                        string oldRouteUsigned = item.Fullroute + item.Name;
                        System.IO.File.Move(oldRouteUsigned, newPathForRenameOld);
                        string queryFile = "UPDATE [PRUEBA1].[dbo].[t_files] SET fileName = '" + lastElement(newPathForRenameOld)
                            + "', status_name = 'making-payment', folder='" + MainViewModel.GetInstance().NewMain.nameBussiness + "making-payment\\" + "' WHERE transId LIKE '" + item.TransId + "%' and type='1';";
                        SqlCommand commandFile = new SqlCommand(queryFile, cn);
                        if (commandFile.Connection.State != ConnectionState.Open)
                        {
                            commandFile.Connection.Close();
                            commandFile.Connection.Open();
                        }
                        int row4 = commandFile.ExecuteNonQuery();
                    }
                    if (cadena.Contains("Signed"))
                    {
                        string oldRouteSigned = item.Fullroute + item.Name;
                        System.IO.File.Move(oldRouteSigned, newPathForRenameNew);
                        string queryFile = "UPDATE[PRUEBA1].[dbo].[t_files] SET fileName = '" + lastElement(newPathForRenameNew)
                            + "', status_name = 'making-payment', folder='" + MainViewModel.GetInstance().NewMain.nameBussiness + "making-payment\\" + "' WHERE transId LIKE '" + item.TransId + "%' and type = '2';";
                        SqlCommand commandFile = new SqlCommand(queryFile, cn);
                        if (commandFile.Connection.State != ConnectionState.Open)
                        {
                            commandFile.Connection.Close();
                            commandFile.Connection.Open();
                        }
                        int row4 = commandFile.ExecuteNonQuery();
                    }
                }

                MainViewModel.GetInstance().NewMain.fullRefresh();
                MainViewModel.GetInstance().NewMain.LoadTable(MainViewModel.GetInstance().NewMain.queryString);
                MainViewModel.GetInstance().NewMain.gridControl1.Update();

                MessageBox.Show("Invoice marked as payment in process");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please select a file or: " + ex);
            }
        }

        public string lastElement(string splitme)
        {
            string[] strlist = splitme.Split(new char[] { '\\' },
                        20, StringSplitOptions.None);
            return strlist[strlist.Length - 1].ToString();
        }

        #endregion Methods
    }
}