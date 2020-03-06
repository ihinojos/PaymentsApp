using DevExpress.Utils;
using DevExpress.Utils.Filtering.Internal;
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
        private string filePath;

        #endregion Attributes

        #region Constructor

        public FinishedTransactions()
        {
            InitializeComponent();
            connection = new SqlConnection(DB.cn.Replace(@"\\", @"\"));
            string query2 = "SELECT * FROM [t_invoices] WHERE status_name = 'payment-captured' AND folder LIKE '" + MainViewModel.GetInstance().NewMain.newpath + "%';";
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
            gv.PopulateColumns();
            gv.Columns["id"].Visible = false;
            gv.Columns["transId"].VisibleIndex = 0;
            gv.Columns["folder"].Visible = false;
            gv.Columns["fileName"].Visible = false;
            gv.Columns["status_name"].Visible = false;
            gv.Columns["idSubBussiness"].Visible = false;
            gv.Columns["date_modified"].DisplayFormat.FormatType = FormatType.DateTime;
            gv.Columns["date_modified"].DisplayFormat.FormatString = "g";
            gv.Columns["amount"].DisplayFormat.FormatType = FormatType.Numeric;
            gv.Columns["amount"].DisplayFormat.FormatString = "c2";
            gv.RowCellClick += gridView1_RowCellClick;
            gridControl1.Update();
            gridControl1.Refresh();



        }

        #endregion Methods

        #region Clicks

        private void gridView1_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            treeView1.Nodes.Clear();
            GridView gv = gridView1;
            lblSelected.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "transId").ToString();
            filePath = gv.GetRowCellValue(gv.FocusedRowHandle, "folder").ToString() + gv.GetRowCellValue(gv.FocusedRowHandle, "fileName").ToString();
            treeView1.Nodes.Add(filePath);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var instance = MainViewModel.GetInstance().ViewPdf = new ViewPDF(filePath);
                instance.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\nPlease select a transaction.");
            }
        }

        #endregion Clicks

        private void showOnDiskButton_Click(object sender, EventArgs e)
        {
            string argument = "/select, \"" + filePath + "\"";
            System.Diagnostics.Process.Start("explorer.exe", argument);
        }
    }
}