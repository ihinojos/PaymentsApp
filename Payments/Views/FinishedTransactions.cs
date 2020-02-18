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

        private SqlConnection cn;
        public DataTable FullDT;

        #endregion Attributes

        #region Constructor

        public FinishedTransactions()
        {
            InitializeComponent();
            cn = new SqlConnection(DB.cn.Replace(@"\\", @"\"));
            string query2 = "Select transactionId,t.id from [PRUEBA1].[dbo].[t_transactions] t inner join [PRUEBA1].[dbo].[t_files] f on f.transId = t.id where f.type = '3';";
            loadTable(query2);
        }

        #endregion Constructor

        #region Methods

        public void loadTable(string queryString)
        {
            SqlCommand command = new SqlCommand(queryString, cn);
            if (command.Connection.State != ConnectionState.Open)
            {
                command.Connection.Close();
                command.Connection.Open();
            }
            int row = command.ExecuteNonQuery();
            FullDT = new DataTable();
            using (SqlDataAdapter DA = new SqlDataAdapter(command))
            {
                DA.Fill(FullDT);
            }
            gridControl1.RefreshDataSource();
            GridView gv = gridView1;
            gridControl1.DataSource = null;
            gridControl1.DataSource = FullDT;
            gridView1.PopulateColumns();
            gridView1.Columns["id"].Visible = false;
            gridView1.RowCellClick += gridView1_RowCellClick;
            gridControl1.Update();
            gridControl1.Refresh();
            command.Connection.Close();
        }

        private void gridView1_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            treeView1.Nodes.Clear();
            GridView gv = gridView1;

            string status = gv.GetRowCellValue(gv.FocusedRowHandle, "id").ToString();
            lblSelected.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "transactionId").ToString();

            string query3 = "Select * from [PRUEBA1].[dbo].[t_files] f inner join [PRUEBA1].[dbo].[t_transactions] t on f.transId = t.id where t.id = '" + status + "';";
            SqlCommand command = new SqlCommand(query3, cn);
            if (command.Connection.State != ConnectionState.Open)
            {
                command.Connection.Close();
                command.Connection.Open();
            }
            int row = command.ExecuteNonQuery();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                treeView1.Nodes.Add(reader[2].ToString() + reader[1].ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (MainViewModel.GetInstance().ViewPdf == null)
                {
                    MainViewModel.GetInstance().ViewPdf = new ViewPDF(treeView1.SelectedNode.Text);
                    MainViewModel.GetInstance().ViewPdf.FormClosed += MainViewModel.GetInstance().NewMain.FormClosed;
                    MainViewModel.GetInstance().ViewPdf.Show();
                }
                else MainViewModel.GetInstance().ViewPdf.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        #endregion Methods


   
    }
}