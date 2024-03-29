﻿using DevExpress.XtraGrid.Views.Grid;
using Payments.Models;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace Payments.Views
{
    public partial class AssingSubBussines : Form
    {
        #region Attributes

        private readonly SqlConnection connection;
        private readonly string idFile;
        private readonly string idBussiness;
        private readonly string selectedFilePath;
        private readonly string queryStringSubBussinesFiles;
        private readonly string userDic = MainViewModel.GetInstance().NewMain.userDic;

        #endregion Attributes

        #region Constructor

        public AssingSubBussines(string pathToFile, string idBussiness, string id)
        {
            InitializeComponent();
            connection = new SqlConnection(DB.cn.Replace(@"\\", @"\"));
            lblFileSelected.Text = Path.GetFileName(pathToFile);
            this.idBussiness = idBussiness;
            LoadCombo("SELECT * FROM [t_subBussiness] WHERE idBussiness = '" + idBussiness + "';");
            idFile = id;
            selectedFilePath = userDic + "\\" + pathToFile;
            axAcroPDF1.src = userDic + "\\" + pathToFile;
            try
            {
                queryStringSubBussinesFiles = "SELECT s.nameSub FROM [t_invoices] i, [t_subBussiness] s  WHERE i.[id] = '" + idFile + "' AND s.id = i.idSubBussiness;";
                LoadTable(queryStringSubBussinesFiles);
            }
            catch (Exception ex)
            {
                MessageBox.Show("The query had a problem:" + ex);
            }
        }

        #endregion Constructor

        #region Methods

        private void LoadTable(string queryString)
        {
            gridControl1.DataSource = null;
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Connection.Open();
            command.ExecuteNonQuery();
            DataTable FullDT = new DataTable();
            using (SqlDataAdapter DA = new SqlDataAdapter(command))
            {
                DA.Fill(FullDT);
            }
            gridControl1.RefreshDataSource();
            GridView gv = gridView1;
            gridControl1.DataSource = FullDT;
            gridView1.PopulateColumns();
            gridView1.Columns["nameSub"].Caption = "Sub_Bussiness Name";
            var name = gv.GetRowCellValue(gv.FocusedRowHandle, "nameSub");
            if (name == null)
            {
                lblSubSelected.Text = "Not assigned";
            }
            else
            {
                lblSubSelected.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "nameSub").ToString();
            }
            gridControl1.Update();
            gridControl1.Refresh();
            command.Connection.Close();
        }

        private void LoadCombo(string queryString)
        {
            comboBoxSubBussiness.Items.Clear();
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                comboBoxSubBussiness.Items.Add(reader.GetValue(1).ToString());
            }
            reader.Close();
            command.Connection.Close();
        }

        #endregion Methods

        #region Clicks

        private void BtnCapture_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxTransaction.Text) || String.IsNullOrEmpty(textBoxAmount.Text))
            {
                MessageBox.Show("Verify the inserted information and try again.");
            }
            else
            {
                try
                {
                    double amount = Double.Parse(textBoxAmount.Text);
                    var dateTimeOffset = new DateTimeOffset(DateTime.Now);
                    var formatDate = dateTimeOffset.ToUnixTimeSeconds();
                    string newFormat = formatDate + "_" + "waitingAuth" + "_" + textBoxTransaction.Text.ToString() + ".pdf";

                    string query = "UPDATE [t_invoices] SET " +
                        "fileName = '" + newFormat + "', " +
                        "folder= '" + MainViewModel.GetInstance().NewMain.bussinessPath + "waiting-auth\\" + "'," +
                        "status_name = 'waiting-auth'," +
                        "date_modified = GETDATE()," +
                        "transId = '" + textBoxTransaction.Text + "'," +
                        "amount = " + amount +
                        " WHERE id = '" + idFile + "';";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();

                    PdfDocument file = NewMain.AddWaterMark(PdfReader.Open(selectedFilePath, PdfDocumentOpenMode.Modify), "Unsigned invoice");
                    file.Save(userDic + "\\" + MainViewModel.GetInstance().NewMain.bussinessPath + "waiting-auth\\" + newFormat);
                    File.Delete(selectedFilePath);
                    MessageBox.Show("Invoice captured correctly");
                    MainViewModel.GetInstance().NewMain.FullRefresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                this.Close();
            }
        }

        private void BtnAssingTo_Click(object sender, EventArgs e)
        {
            try
            {
                string subBussiness = comboBoxSubBussiness.SelectedItem.ToString();
                string idSubBussiness = "";
                string query = "SELECT * FROM [t_subBussiness] WHERE [nameSub] = '" + subBussiness + "' AND [idBussiness] = '" + idBussiness + "';";
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    idSubBussiness = reader[0].ToString();
                }
                reader.Close();
                string queryStringNew = "SELECT * FROM [t_invoices] WHERE [id] = '" + idFile + "' AND [idSubBussiness] ='" + idSubBussiness + "';";
                command.CommandText = queryStringNew;
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    MessageBox.Show("The sub-bussiness already exists");
                    reader.Close();
                    command.Connection.Close();
                }
                else
                {
                    reader.Close();
                    string newSub = lblSubSelected.Text;
                    string queryString2 = "UPDATE [t_invoices] SET [idSubBussiness] = '" + idSubBussiness + "' WHERE [id] = '" + idFile + "';";
                    command.CommandText = queryString2;
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                    LoadTable(queryStringSubBussinesFiles);
                    MessageBox.Show("Assignation sucessful");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion Clicks
    }
}