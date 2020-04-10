using DevExpress.Utils;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
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
        private string transId;

        #endregion Attributes

        #region Constructor

        public FinishedTransactions()
        {
            InitializeComponent();
            connection = new SqlConnection(DB.cn.Replace(@"\\", @"\"));
            string query2 = "SELECT * FROM [t_invoices] WHERE status_name = 'payment-captured' AND folder LIKE '" + MainViewModel.GetInstance().NewMain.rootPath + "\\%';";
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
            gridControl1.Update();
            gridControl1.Refresh();
            transId = MainViewModel.GetInstance().NewMain.transId;
            if (!String.IsNullOrEmpty(transId))
            {
                ColumnView view = gridControl1.MainView as ColumnView;
                GridColumn transaction = view.Columns["transId"];
                view.OptionsSelection.MultiSelect = true;
                view.ClearSelection();
                int rowHandle = -1;
                while (rowHandle != GridControl.InvalidRowHandle)
                {
                    rowHandle = view.LocateByDisplayText(rowHandle + 1, transaction, transId);
                    view.FocusedRowHandle = rowHandle;
                    view.SelectRow(rowHandle);
                    ShowTree();
                }
            }
        }

        private void ShowTree()
        {
            treeView1.Nodes.Clear();
            GridView gv = gridView1;
            lblSelected.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "transId").ToString();
            filePath = gv.GetRowCellValue(gv.FocusedRowHandle, "folder").ToString() + gv.GetRowCellValue(gv.FocusedRowHandle, "fileName").ToString();
            treeView1.Nodes.Add(filePath);
        }

        #endregion Methods

        #region Clicks

        private void GridView1_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            ShowTree();
        }

        private void ShowOnDiskButton_Click(object sender, EventArgs e)
        {
            string argument = "/select, \"" + MainViewModel.GetInstance().NewMain.userDic + "\\" + filePath + "\"";
            System.Diagnostics.Process.Start("explorer.exe", argument);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                var instance = MainViewModel.GetInstance().ViewPdf = new ViewPDF(MainViewModel.GetInstance().NewMain.userDic + "\\" + filePath);
                instance.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\nPlease select a transaction.");
            }
        }

        #endregion Clicks
    }
}