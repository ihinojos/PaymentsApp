using DevExpress.LookAndFeel;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using Microsoft.WindowsAPICodePack.Dialogs;
using Payments.Models;
using Payments.Properties;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Payments.Views
{
    public partial class NewMain : Form
    {
        #region Attributes

        public string bussinessPath;
        public bool isRoot;
        public string queryString;
        public string rootPath = "";
        public string transId;
        public readonly string userDic = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        private readonly SqlConnection connection;
        private string idBussiness;

        #endregion Attributes

        #region Constructor

        public NewMain()
        {
            InitializeComponent();
            UserLookAndFeel.Default.SetSkinStyle(SkinStyle.MetropolisDark);
            connection = new SqlConnection(DB.cn.Replace(@"\\", @"\"));
            DeactivateButtons();
            rootButton.Enabled = false;

            if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Dropbox (IE)"))
                rootPath = Settings.Default.root_path;

            if (!String.IsNullOrEmpty(rootPath))
            {
                ObtainFiles();
                lblSelectedFile.Text = "Select an invoice.";
                lblNameBuss.Text = "Select a bussiness.";
                queryString = "EXEC [GetAllInvoiceInfo] @location = '" + rootPath + "\\';";
                LoadTable(queryString);
                idBussiness = "";
                lblTitleResult.Text = (CountFiles().ToString());
                InitializeComboboxBussines();
                CheckIfStatesFoldersExists();
                rootButton.Enabled = true;
            }
            else MessageBox.Show("There is no root path selected.");
        }

        #endregion Constructor

        #region Methods

        public static PdfDocument AddWaterMark(PdfDocument doc, string text)
        {
            PdfPage page = doc.Pages[0];
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Helvetica", 14, XFontStyle.Italic);
            gfx.DrawString(text, font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.TopLeft);
            return doc;
        }

        public static PdfDocument Combine(PdfDocument doc1, PdfDocument doc2)
        {
            PdfDocument outPdf = new PdfDocument();
            for (int i = 0; i < doc1.PageCount; i++)
            {
                outPdf.AddPage(doc1.Pages[i]);
            }
            for (int i = 0; i < doc2.PageCount; i++)
            {
                outPdf.AddPage(doc2.Pages[i]);
            }
            doc1.Close();
            doc2.Close();
            return outPdf;
        }

        public static string ElementAt(string str, int i)
        {
            string[] strlist = str.Split(new char[] { '\\' });
            return strlist[strlist.Length - i];
        }

        public static DialogResult ShowInputDialog(ref string input)
        {

            Size size = new Size(300, 75);
            Form inputBox = new Form
            {
                FormBorderStyle = FormBorderStyle.FixedDialog,
                ClientSize = size,
                Text = "Enter Bussiness name",
                Icon = Resources._32
            };

            TextBox textBox = new TextBox
            {
                Size = new Size(size.Width - 10, 23),
                Location = new Point(5, 5),
                Text = input
            };
            inputBox.Controls.Add(textBox);

            Button okButton = new Button
            {
                DialogResult = DialogResult.OK,
                Name = "okButton",
                Size = new Size(75, 23),
                Text = "&OK",
                Location = new Point(size.Width - 80 - 80, 39)
            };
            inputBox.Controls.Add(okButton);

            Button cancelButton = new Button
            {
                DialogResult = DialogResult.Cancel,
                Name = "cancelButton",
                Size = new Size(75, 23),
                Text = "&Cancel",
                Location = new Point(size.Width - 80, 39)
            };
            inputBox.Controls.Add(cancelButton);

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            inputBox.StartPosition = FormStartPosition.CenterParent;

            DialogResult result = inputBox.ShowDialog();
            input = textBox.Text;
            return result;
        }

        public void FullRefresh()
        {
            ObtainFiles();
            CheckIfStatesFoldersExists();
            DeactivateButtons();
            switch (isRoot)
            {
                case true:
                    queryString = "EXEC [GetAllInvoiceInfo] @location = '" + rootPath + "\\';";
                    break;

                case false:
                    queryString = "EXEC [GetAllInvoiceInfo] @location = '" + bussinessPath + "';";
                    lblTitleResult.Text = (CountFiles().ToString());
                    break;

                default:
                    return;
            }
            DeleteMissingRegisters();
            LoadTable(queryString);
        }

        public void LoadTable(string queryString)
        {
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Connection.Open();
            DataTable FullDT = new DataTable();
            using (SqlDataAdapter DA = new SqlDataAdapter(command))
            {
                DA.Fill(FullDT);
            }
            command.Connection.Close();
            GridView gv = gridView1;
            gridControl1.DataSource = null;
            gridControl1.DataSource = FullDT;
            gv.PopulateColumns();
            gv.Columns["id"].Visible = false;
            gv.Columns["folder"].Visible = false;
            gv.Columns["idSubBussiness"].Visible = false;
            gv.Columns["idBussiness"].Visible = false;
            gv.Columns["nameBussiness"].Visible = false;
            gv.Columns["nameSub"].Visible = false;
            gv.Columns["date_modified"].DisplayFormat.FormatType = FormatType.DateTime;
            gv.Columns["date_modified"].DisplayFormat.FormatString = "g";
            gv.Columns["amount"].DisplayFormat.FormatType = FormatType.Numeric;
            gv.Columns["amount"].DisplayFormat.FormatString = "c2";
            gv.Columns["transId"].VisibleIndex = 0;
            gv.BestFitColumns(true);
            gridControl1.Update();
            gridControl1.Refresh();
        }

        public void ObtainFiles()
        {
            List<T_Invoices> cloudFiles = new List<T_Invoices>();
            string query = "SELECT * FROM [t_invoices] WHERE folder LIKE '" + rootPath + "\\%';";
            SqlCommand command = new SqlCommand(query, connection);
            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                cloudFiles.Add(new T_Invoices
                {
                    Id = reader[0].ToString(),
                    FileName = reader[1].ToString(),
                    Folder = reader[2].ToString()
                });
            }
            reader.Close();
            string[] files = Directory.GetFiles(userDic + "\\" + rootPath, "*.pdf", SearchOption.AllDirectories);
            List<String> localFiles = new List<string>(files);

            foreach (var file in cloudFiles)
            {
                string fullPath = file.Folder + file.FileName;
                if (localFiles.Contains(userDic + "\\" + fullPath))
                {
                    localFiles.Remove(userDic + "\\" + fullPath);
                }
            }

            foreach (var file in localFiles)
            {
                string name = Path.GetFileName(file);
                string path = file.Replace(userDic + "\\", "").Replace(name, "");
                string state = "";
                if (path == rootPath + "\\") state = "unassigned";
                else if (path.Contains("incoming")) state = "incoming";
                if (!String.IsNullOrEmpty(state))
                {
                    command.CommandText = "INSERT INTO [t_invoices]([id],[filename],[folder],[status_name],[date_modified],[transId],[amount],[idSubBussiness])" +
                                        " VALUES( NEWID()," +
                                        "'" + name + "'," +
                                        "'" + path + "'," +
                                        "'" + state + "'," +
                                        "GETDATE()," +
                                        "NULL," +
                                        "NULL," +
                                        "NULL)";

                    command.ExecuteNonQuery();
                }
            }
            command.Connection.Close();
        }

        private void CheckIfStatesFoldersExists()
        {
            List<string> states = new List<string>();
            string[] dirs = Directory.GetDirectories(userDic + "\\" + rootPath, "*", SearchOption.TopDirectoryOnly);
            foreach (var dir in dirs)
            {
                string[] toScanFolders = Directory.GetDirectories(dir.ToString(), "*", SearchOption.TopDirectoryOnly);
                if (toScanFolders.Length != 0)
                {
                    foreach (var item2 in toScanFolders)
                    {
                        string folderToCheck = ElementAt(item2, 1);
                        states.Add(folderToCheck);
                    }
                    if (!states.Contains("incomig"))
                    {
                        Directory.CreateDirectory(dir + "\\incoming");
                    }
                    if (!states.Contains("waiting-auth"))
                    {
                        Directory.CreateDirectory(dir + "\\waiting-auth");
                    }
                    if (!states.Contains("signed"))
                    {
                        Directory.CreateDirectory(dir + "\\signed");
                    }
                    if (!states.Contains("making-payment"))
                    {
                        Directory.CreateDirectory(dir + "\\making-payment");
                    }
                    if (!states.Contains("payment-captured"))
                    {
                        Directory.CreateDirectory(dir + "\\payment-captured");
                    }
                }
                else
                {
                    Directory.CreateDirectory(dir + "\\incoming");
                    Directory.CreateDirectory(dir + "\\waiting-auth");
                    Directory.CreateDirectory(dir + "\\signed");
                    Directory.CreateDirectory(dir + "\\making-payment");
                    Directory.CreateDirectory(dir + "\\payment-captured");
                }
            }
        }

        private int CountFiles()
        {
            string[] files = Directory.GetFiles(userDic + "\\" + rootPath, "*.*", SearchOption.AllDirectories);
            int counter = 0;
            foreach (var item in files)
            {
                if (item.Contains("incoming")) counter += 1;
            }
            return counter;
        }

        private void DeactivateButtons()
        {
            btnMakePayment.Enabled = false;
            btnPaymentCaptured.Enabled = false;
            btnSigned.Enabled = false;
            btnCapture.Enabled = false;
            btnChangeFileOfBussiness.Enabled = false;
        }

        private void DeleteMissingRegisters()
        {
            List<string> allFiles = new List<string>();
            string[] files = Directory.GetFiles(userDic + "\\" + rootPath, "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                allFiles.Add(file);
            }

            List<string> record = new List<string>();
            List<string> recordId = new List<string>();
            string queryfiles = "SELECT * FROM [t_invoices] WHERE [folder] LIKE '" + rootPath + "\\%' and status_name = 'incoming' or status_name = 'unassigned';";
            SqlCommand command = new SqlCommand(queryfiles, connection);
            command.Connection.Open();
            SqlDataReader read = command.ExecuteReader();
            while (read.Read())
            {
                string r = read[2].ToString() + read[1].ToString();
                record.Add(r);
                recordId.Add(read[0].ToString());
            }
            read.Close();
            command.Connection.Close();
            for (int i = 0; i < record.Count; i++)
            {
                if (!allFiles.Contains(userDic + "\\" + record[i]))
                {
                    string querydelete = "DELETE FROM [t_invoices] WHERE [id] = '" + recordId[i] + "';";
                    SqlCommand commandDelete = new SqlCommand(querydelete, connection);
                    lock (commandDelete)
                    {
                        commandDelete.Connection.Open();
                        commandDelete.ExecuteNonQuery();
                        commandDelete.Connection.Close();
                    }
                }
            }
        }

        private void EmptyLabels()
        {
            lblSelectedFile.Text = "";
            lblAmuont.Text = "";
            subBussinessLabel.Text = "";
            lblDateModified.Text = "";
            filePathLabel.Text = "";
        }

        private void InitializeComboboxBussines()
        {
            comboBox1.Items.Clear();
            string[] dirs = Directory.GetDirectories(userDic + "\\" + rootPath, "*", SearchOption.TopDirectoryOnly);
            foreach (var dir in dirs)
            {
                string[] strlist = dir.Split(new char[] { '\\' }, 20, StringSplitOptions.None);
                string queryString = "SELECT * FROM [t_bussiness] WHERE nameBussiness = '"
                    + strlist[strlist.Length - 1] + "' AND pathBussiness = '" + rootPath + "\\';";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    reader.Close();
                    string queryString2 = "INSERT INTO [t_bussiness]([id],[nameBussiness], [pathBussiness])" +
                                                       " VALUES( NEWID(),'" + strlist[strlist.Length - 1] + "', '" + rootPath + "\\')";
                    command.CommandText = queryString2;
                    command.ExecuteNonQuery();
                }
                else reader.Close();
                for (int i = strlist.Length - 1; i < strlist.Length; i++)
                {
                    comboBox1.Items.Add(strlist[i]);
                }
                command.Connection.Close();
            }
        }

        private bool IsThisRoot(string path)
        {
            if (path.Contains(rootPath) && path != rootPath)
            {
                List<string> states = new List<string>();
                string[] dirs = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
                foreach (var dir in dirs) states.Add(ElementAt(dir, 1));
                return !(states.Contains("incoming") ||
                    states.Contains("waiting-auth") ||
                    states.Contains("payment-captured") ||
                    states.Contains("signed") ||
                    states.Contains("waiting-auth"));
            }
            return false;
        }

        #endregion Methods

        #region Clicks

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                MessageBox.Show("Intelogix México © 2020\n\nCurrent Version: " + System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion);
            }
            else
            {
                MessageBox.Show("Not currently deployed.");
            }
        }

        private void BtnCapture_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(idBussiness))
            {
                MessageBox.Show("Please select a Bussiness");
            }
            else
            {
                try
                {
                    var instance = MainViewModel.GetInstance().AssingSubBussiness;
                    if (instance != null) instance.Dispose();
                    string name = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "fileName").ToString();
                    string id = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "id").ToString();
                    string path = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "folder").ToString() + name;
                    instance = MainViewModel.GetInstance().AssingSubBussiness = new AssingSubBussines(path, idBussiness, id);
                    instance.Show();
                }
                catch (Exception)
                {
                    MessageBox.Show("You must select a bussines and then a file to continue");
                }
            }
        }

        private void BtnPaymentCaptured_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(idBussiness))
            {
                MessageBox.Show("Please select a Bussiness");
            }
            else
            {
                var instance = MainViewModel.GetInstance().CapturePayment;
                if (instance != null) instance.Dispose();
                instance = MainViewModel.GetInstance().CapturePayment = new PaymentCaptured(lblNameBuss.Text, gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "id").ToString());
                instance.Show();
            }
        }

        private void BtnSigned_Click_1(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(idBussiness))
            {
                MessageBox.Show("Please select a Bussiness");
            }
            else
            {
                var instance = MainViewModel.GetInstance().SignDoc;
                if (instance != null) instance.Dispose();
                instance = MainViewModel.GetInstance().SignDoc = new Signed(lblNameBuss.Text, gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "id").ToString());
                instance.Show();
            }
        }

        private void BusinesssToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string input = "";
            if (String.IsNullOrEmpty(rootPath)) MessageBox.Show("Please select a root path.");
            else
            {
                if (ShowInputDialog(ref input) == DialogResult.OK)
                {
                    Directory.CreateDirectory(userDic + "\\" + rootPath + "\\" + input);
                    ObtainFiles();
                    InitializeComboboxBussines();
                    CheckIfStatesFoldersExists();
                    DeactivateButtons();
                    gridControl1.DataSource = null;
                    gridControl1.RefreshDataSource();
                    lblSelectedFile.Text = "Select a file.";
                    lblNameBuss.Text = "Select a business.";
                    MessageBox.Show("Created: " + rootPath + "\\" + input);
                }
            }
        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                string name = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "fileName").ToString();
                string filePath = userDic + "\\" + gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "folder").ToString() + name;
                string argument = "/select, \"" + filePath + "\"";
                System.Diagnostics.Process.Start("explorer.exe", argument);
            }
            catch
            {
                MessageBox.Show("Please select an invoice.");
            }
        }

        private void Button1_Click_2(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(rootPath))
            {
                MessageBox.Show("Please select a root path.");
                return;
            }
            var instance = MainViewModel.GetInstance().FinishedTransaction;
            if (instance != null) instance.Dispose();
            instance = MainViewModel.GetInstance().FinishedTransaction = new FinishedTransactions();
            instance.Show();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(idBussiness))
            {
                MessageBox.Show("Please select a Bussiness");
            }
            else
            {
                var instance = MainViewModel.GetInstance().MakePayment;
                if (instance != null) instance.Dispose();
                instance = MainViewModel.GetInstance().MakePayment = new MakingPayment(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "id").ToString());
                instance.Show();
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            try
            {
                var instance = MainViewModel.GetInstance().ViewPdf;
                string name = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "fileName").ToString();
                string path = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "folder").ToString() + name;
                instance = MainViewModel.GetInstance().ViewPdf = new ViewPDF(userDic + "\\" + path);
                instance.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("Please select a file first");
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(bussinessPath)) MessageBox.Show("Please select a Bussiness.");
                else
                {
                    string stat = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "status_name").ToString();
                    if (stat == "incoming" || stat == "unassigned")
                    {
                        var instance = MainViewModel.GetInstance().ChangeBussines;
                        if (instance != null) instance.Dispose();
                        string name = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "fileName").ToString();
                        string path = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "folder").ToString() + name;
                        instance = MainViewModel.GetInstance().ChangeBussines = new ChangeFileToNewBussiness(path, rootPath);
                        instance.Show();
                    }
                    else
                    {
                        MessageBox.Show("Only new files can be moved");
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please select a bussines and a file");
            }
        }

        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            string status = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "status_name").ToString();
            switch (status)
            {
                case "incoming":
                    btnCapture.PerformClick();
                    break;

                case "waiting-auth":
                    btnSigned.PerformClick();
                    break;

                case "signed":
                    btnMakePayment.PerformClick();
                    break;

                case "making-payment":
                    btnPaymentCaptured.PerformClick();
                    break;

                case "payment-captured":
                    transId = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "transId").ToString();
                    button1.PerformClick();
                    break;

                case "unassigned":
                    btnChangeFileOfBussiness.PerformClick();
                    break;
            }
        }

        private void GridView1_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            GridView gv = gridView1;
            lblSelectedFile.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "fileName").ToString();
            bussinessPath = lblNameBuss.Text = ElementAt(gv.GetRowCellValue(gv.FocusedRowHandle, "folder").ToString(), 3);
            filePathLabel.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "folder").ToString();
            transId = gv.GetRowCellValue(gv.FocusedRowHandle, "transId").ToString();

            subBussinessLabel.Text = String.IsNullOrEmpty(gv.GetRowCellValue(gv.FocusedRowHandle, "nameSub").ToString()) ? "Not yet assigned" : gv.GetRowCellValue(gv.FocusedRowHandle, "nameSub").ToString();
            bussinessPath = $"{rootPath}\\{lblNameBuss.Text}\\";
            bussinessPath = bussinessPath.Replace(@"\\", @"\");
            decimal amount = String.IsNullOrEmpty(gv.GetRowCellValue(gv.FocusedRowHandle, "amount").ToString()) ? 0 : Decimal.Parse(gv.GetRowCellValue(gv.FocusedRowHandle, "amount").ToString());
            string txtAmnt = String.Format("{0:C}", amount);
            lblAmuont.Text = txtAmnt;
            var date1 = DateTime.Parse(gv.GetRowCellValue(gv.FocusedRowHandle, "date_modified").ToString());
            lblDateModified.Text = date1.ToString("F");
            string queryString = "SELECT * FROM [t_bussiness] WHERE nameBussiness = '" + lblNameBuss.Text + "' AND pathBussiness = '" + rootPath + "\\';";
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Connection.Open();
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                idBussiness = reader[0].ToString();
                reader.Close();
            }
            command.Connection.Close();
            string status = gv.GetRowCellValue(gv.FocusedRowHandle, "status_name").ToString();
            try
            {
                DeactivateButtons();
                switch (status)
                {
                    case "incoming":
                        btnCapture.Enabled = true;
                        btnChangeFileOfBussiness.Enabled = true;
                        break;

                    case "waiting-auth":
                        btnSigned.Enabled = true;
                        break;

                    case "signed":
                        btnMakePayment.Enabled = true;
                        break;

                    case "making-payment":
                        btnPaymentCaptured.Enabled = true;
                        break;

                    case "unassigned":
                        btnChangeFileOfBussiness.Enabled = true;
                        break;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please select a bussines and a file");
            }
        }

        private void RootButton_Click(object sender, EventArgs e)
        {
            isRoot = true;
            ObtainFiles();
            DeleteMissingRegisters();
            CheckIfStatesFoldersExists();
            DeactivateButtons();
            EmptyLabels();
            queryString = "EXEC [GetAllInvoiceInfo] @location = '" + rootPath + "\\';";
            lblTitleResult.Text = (CountFiles().ToString());
            LoadTable(queryString);
        }

        private void SetRootPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                InitialDirectory = userDic + "\\" + rootPath,
                IsFolderPicker = true
            };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                if (IsThisRoot(dialog.FileName))
                {
                    Settings.Default.root_path = rootPath = dialog.FileName.Replace(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\", "");
                    Settings.Default.Save();
                    isRoot = true;
                    lblTitleResult.Text = (CountFiles().ToString());
                    ObtainFiles();
                    InitializeComboboxBussines();
                    CheckIfStatesFoldersExists();
                    DeactivateButtons();
                    gridControl1.DataSource = null;
                    gridControl1.RefreshDataSource();
                    lblSelectedFile.Text = "Select an invoice.";
                    lblNameBuss.Text = "Select a bussiness.";
                    queryString = "EXEC [GetAllInvoiceInfo] @location = '" + rootPath + "\\';";
                    LoadTable(queryString);
                    idBussiness = "";
                    rootButton.Enabled = true;
                }
                else MessageBox.Show("This is a bussiness folder.");
            }
        }

        private void SubBussinessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var instance = MainViewModel.GetInstance().AddSubBussiness;
            if (instance != null) instance.Dispose();
            instance = MainViewModel.GetInstance().AddSubBussiness = new SubBussinessAdd();
            instance.Show();
        }

        private void UsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var instance = MainViewModel.GetInstance().AddUser;
            if (instance != null) instance.Dispose();
            instance = MainViewModel.GetInstance().AddUser = new UserAddView();
            instance.Show();
        }

        #endregion Clicks

        #region Events

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                dynamic mboxResult = MessageBox.Show("Do you want to exit the app?", "Payments", MessageBoxButtons.OKCancel);
                if (mboxResult == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
                else if (mboxResult == DialogResult.OK)
                {
                    e.Cancel = false;
                    MainViewModel.GetInstance().Login.Visible = true;
                }
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            isRoot = false;
            ObtainFiles();
            DeactivateButtons();
            string queryString = "SELECT * FROM [t_bussiness] WHERE nameBussiness = '" + comboBox1.SelectedItem.ToString() + "' AND pathBussiness = '" + rootPath + "\\';";
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Connection.Open();
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                idBussiness = reader[0].ToString();
                reader.Close();
            }
            command.Connection.Close();
            lblNameBuss.Text = comboBox1.SelectedItem.ToString();

            bussinessPath = $"{rootPath}\\{comboBox1.SelectedItem}\\".Replace(@"\\", @"\");
            queryString = "EXEC [GetAllInvoiceInfo] @location = '" + bussinessPath + "'";
            EmptyLabels();
            DeleteMissingRegisters();
            lblTitleResult.Text = (CountFiles().ToString());
            LoadTable(queryString);
        }

        private void GridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.RowHandle == view.FocusedRowHandle)
            {
                e.Appearance.BackColor = Color.MidnightBlue; //Midnight Blues - Snowy White
                e.Appearance.ForeColor = Color.White;
            }
        }

        private void GridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string status = View.GetRowCellDisplayText(e.RowHandle, View.Columns["status_name"]);
                switch (status)
                {
                    case "incoming":
                        e.Appearance.BackColor = ColorTranslator.FromHtml("#b2dcef"); // Soap Bubble
                        e.Appearance.BackColor2 = ColorTranslator.FromHtml("#87cefa "); // Transparent Blue
                        break;

                    case "waiting-auth":
                        e.Appearance.BackColor = ColorTranslator.FromHtml("#fffe7a"); // Light Yellow
                        e.Appearance.BackColor2 = ColorTranslator.FromHtml("#fada5f"); // Napples Yellow
                        break;

                    case "signed":
                        e.Appearance.BackColor = ColorTranslator.FromHtml("#ffcc99"); // Peach Orange
                        e.Appearance.BackColor2 = ColorTranslator.FromHtml("#ffb07c"); // Peach
                        break;

                    case "making-payment":
                        e.Appearance.BackColor = ColorTranslator.FromHtml("#ffc0cb"); //Pink
                        e.Appearance.BackColor2 = ColorTranslator.FromHtml("#f2a0a1"); //Plum Blossom
                        break;

                    case "payment-captured":
                        e.Appearance.BackColor = ColorTranslator.FromHtml("#c4fe82"); //Light Pea Green
                        e.Appearance.BackColor2 = ColorTranslator.FromHtml("#98d98e"); //Leek
                        break;
                }
            }
        }

        #endregion Events
    }
}