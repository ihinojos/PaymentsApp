using DevExpress.XtraGrid.Views.Grid;
using Payments.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Payments.Views
{
    public partial class FinishedTransactions : Form
    {
        #region Attributes

        private readonly SqlConnection connection;

        #endregion Attributes

        #region Constructor

        public FinishedTransactions()
        {
            InitializeComponent();
            connection = new SqlConnection(DB.cn.Replace(@"\\", @"\"));
            string query2 = "Select t.transactionId,t.id from [TESTPAY].[dbo].[t_transactions] t inner join [TESTPAY].[dbo].[t_files] f on f.transId = t.id where f.status_name = 'payment-captured';";
            LoadTable(query2);
        }

        #endregion Constructor

        #region Methods

        public void LoadTable(string queryString)
        {
            SqlCommand command = new SqlCommand(queryString, connection);
            DataTable FullDT = new DataTable();
            command.Connection.Open();
            using (SqlDataAdapter DA = new SqlDataAdapter(command))
            {
                DA.Fill(FullDT);
            }
            command.Connection.Close();
            gridControl1.RefreshDataSource();
            GridView gv = gridView1;
            gridControl1.DataSource = null;
            gridControl1.DataSource = FullDT;
            gridView1.PopulateColumns();
            gridView1.RowCellClick += gridView1_RowCellClick;
            gridControl1.Update();
            gridControl1.Refresh();
        }

        #endregion Methods

        #region Clicks

        private void gridView1_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            treeView1.Nodes.Clear();
            GridView gv = gridView1;

            string status = gv.GetRowCellValue(gv.FocusedRowHandle, "id").ToString();
            lblSelected.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "transactionId").ToString();

            string query3 = "Select * from [TESTPAY].[dbo].[t_files] f inner join [TESTPAY].[dbo].[t_transactions] t on f.transId = t.id where t.id = '" + status + "';";
            SqlCommand command = new SqlCommand(query3, connection);
            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                treeView1.Nodes.Add(reader[2].ToString() + reader[1].ToString());
            }
            reader.Close();
            command.Connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var instance = MainViewModel.GetInstance().ViewPdf = new ViewPDF(treeView1.SelectedNode.Text);
                instance.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion Clicks
    }
}