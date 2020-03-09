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
        private readonly string invoiceID;
        private T_Invoices[] invoices;
        private string newFormat;
        private string idTransForQuery2;
        private string newFormat2;

        #endregion Attributes

        #region Constructor

        public MakingPayment(string invoiceID)
        {
            InitializeComponent();
            this.invoiceID = invoiceID;
            connection = new SqlConnection(DB.cn.Replace(@"\\", @"\"));
            radioButton1.Checked = true;
            LoadTree();
            lookUpEdit1.Visible = false;
            treeView1.Visible = true;
            lblFile.Text = invoices[0].FileName;
        }

        #endregion Constructor

        #region Methods

        private void LoadTree()
        {
            treeView1.Nodes.Clear();
            string query = "select * from [t_invoices] where id ='" + invoiceID + "';";
            SqlCommand command = new SqlCommand(query, connection);
            command.Connection.Open();
            using (var reader = command.ExecuteReader())
            {
                var list = new List<T_Invoices>();
                if (reader.Read())
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
                invoices = list.ToArray();
                reader.Close();
            }
            treeView1.BeginUpdate();
            treeView1.Nodes.Add(invoices[0].TransId);
            treeView1.Nodes[0].Nodes.Add(invoices[0].FileName);
            treeView1.EndUpdate();
            command.Connection.Close();
        }

        private void LoadLookUpEdit()
        {
            string querystringstatus = "SELECT * FROM [t_invoices] where [id] = '" + invoiceID + "';";
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
            lookUpEdit1.Properties.Columns.Add(new LookUpColumnInfo("fileName", "File Name"));
            lookUpEdit1.Properties.Columns.Add(new LookUpColumnInfo("transId", "Transaction Id"));
            lookUpEdit1.Properties.DataSource = dataSet.Tables[0];
            lookUpEdit1.Properties.DisplayMember = "transId";
            lookUpEdit1.Properties.ValueMember = "id";
            command.Connection.Close();
        }

        private void CreateNewNomenclature()
        {
            try
            {
                string newPathForRenameOld = "";
                string newPathForRenameNew = "";
                var dateTimeOffset = new DateTimeOffset(DateTime.Now);
                var formatDate = dateTimeOffset.ToUnixTimeSeconds();
                if (radioButton1.Checked)
                {
                    newFormat = formatDate + "_" + "Making-Payment-Unsigned" + "_" + lblSelected.Text + ".pdf";
                    newFormat2 = formatDate + "_" + "Bill-Paying" + "_" + lblSelected.Text + ".pdf";
                }
                if (radioButton2.Checked)
                {
                    newFormat = formatDate + "_" + "Making-Payment-Unsigned" + "_" + idTransForQuery2 + ".pdf";
                    newFormat2 = formatDate + "_" + "Bill-Paying" + "_" + idTransForQuery2 + ".pdf";
                }
                newPathForRenameOld = MainViewModel.GetInstance().NewMain.bussinessPath + "making-payment" + "\\" + newFormat;
                newPathForRenameNew = MainViewModel.GetInstance().NewMain.bussinessPath + "making-payment" + "\\" + newFormat2;

                foreach (var item in invoices)
                {
                    string cadena = item.FileName;
                    if (cadena.Contains("Signed"))
                    {
                        string oldRouteSigned = item.Folder + item.FileName;
                        string queryFile = "UPDATE [t_invoices] SET fileName = '" + NewMain.ElementAt(newPathForRenameNew, 1)
                            + "', status_name = 'making-payment', folder='" + MainViewModel.GetInstance().NewMain.bussinessPath + "making-payment\\" + "'," +
                            "date_modified = GETDATE() WHERE [id] = '" + invoiceID + "';";
                        SqlCommand command = new SqlCommand(queryFile, connection);
                        command.Connection.Open();
                        command.ExecuteNonQuery();
                        System.IO.File.Move(oldRouteSigned, newPathForRenameNew);
                        command.Connection.Close();
                    }
                }
                MainViewModel.GetInstance().NewMain.FullRefresh(MainViewModel.GetInstance().NewMain.isRoot);
                MessageBox.Show("Invoice marked as payment in process");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please select a file or: " + ex);
            }
        }

        #endregion Methods

        #region Clicks

        private void button1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            CreateNewNomenclature();
            Cursor.Current = Cursors.Default;
        }

        #endregion Clicks

        #region Events

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
            idTransForQuery2 = lookUpEdit1.GetColumnValue("transId").ToString();
            lblSelected.Text = idTransForQuery2;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode.Parent == null)
            {
                var id = treeView1.SelectedNode.Text;
                lblSelected.Text = id.ToString();
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

        #endregion Events
    }
}