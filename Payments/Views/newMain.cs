using DevExpress.XtraGrid.Views.Grid;
using Microsoft.WindowsAPICodePack.Dialogs;
using Payments.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace Payments.Views
{
    public partial class NewMain : Form
    {
        #region Attributes

        public string nameBussiness;
        public String newpath = "";
        public string queryString;
        private T_Files[] allRecords;
        private string idBussiness;
        private readonly SqlConnection connection;

        #endregion Attributes

        #region Constructor

        public NewMain()
        {
            InitializeComponent();
            connection = new SqlConnection(DB.cn.Replace(@"\\", @"\"));
            DeactivateButtons();
        }

        #endregion Constructor

        #region Methods

        public static string LastElement(string splitme)
        {
            string[] strlist = splitme.Split(new char[] { '\\' },
                       20, StringSplitOptions.None);
            return strlist[strlist.Length - 1].ToString();
        }

        public static string SecondLastElement(string splitme)
        {
            string[] strlist = splitme.Split(new char[] { '\\' },
                       20, StringSplitOptions.None);
            return strlist[strlist.Length - 2].ToString();
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
            gridView1.PopulateColumns();
            gridView1.Columns["folder"].Visible = false;
            gridView1.Columns["id"].Visible = false;
            gridView1.Columns["transId"].Visible = false;
            gridView1.Columns["status_name"].Visible = true;
            gridView1.Columns["type"].Visible = false;
            gridView1.Columns["fileName"].Caption = "File Name";
            gridView1.Columns["status_name"].Caption = "Status";
            gridView1.Columns["content"].Caption = "Document Type";
            gridView1.RowCellClick += gridView1_RowCellClick;
            gridControl1.Update();
            gridControl1.Refresh();
        }

        public void ObtainFiles(string path)
        {
            string[] Bussiness = Directory.GetDirectories(path, "*.*", SearchOption.TopDirectoryOnly);
            foreach (var item in Bussiness)
            {
                string[] subBussiness = Directory.GetDirectories(item, "*.*", SearchOption.TopDirectoryOnly);
                foreach (var item2 in subBussiness)
                {
                    string[] files = Directory.GetFiles(item2, "*.*", SearchOption.TopDirectoryOnly);
                    foreach (string file in files)
                    {
                        string fileName = file;
                        string url = fileName.TrimEnd('\\');
                        url = url.Remove(url.LastIndexOf('\\') + 1);
                        string status = url.TrimEnd('\\');
                        status = LastElement(status);
                        string[] strlist = fileName.Split(new char[] { '\\' },
                               20, StringSplitOptions.None);
                        for (int i = 0; i < strlist.Length; i++)
                        {
                            strlist[i] = strlist[i].Replace(((char)39).ToString(), "");
                            if (Path.GetExtension(strlist[i]) == ".pdf")
                            {
                                //Condicion para revisar si ya existe el archivo
                                string queryString = "SELECT [fileName],[folder] FROM[PAYMENTS].[dbo].[t_files] WHERE fileName = '" + strlist[i] + "' AND folder ='" + url + "';";
                                SqlCommand command = new SqlCommand(queryString, connection);
                                command.Connection.Open();
                                SqlDataReader reader = command.ExecuteReader();
                                if (!reader.Read())
                                {
                                    reader.Close();
                                    if (SecondLastElement(file) == "incoming")
                                    {
                                        string queryString2 = "INSERT INTO [PAYMENTS].[dbo].[t_files]([id],[filename],[folder],[status_name],[transId],[type])" +
                                                               " VALUES( NEWID(),'" + strlist[i] + "','" + url + "','" + status + "',NULL,1)";
                                        command.CommandText = queryString2;
                                        command.ExecuteNonQuery();
                                    }
                                }
                                command.Connection.Close();
                            }
                        }
                    }
                }
            }
        }

        private void CheckIfStatesFoldersExists()
        {
            List<string> states = new List<string>();
            string[] dirs = Directory.GetDirectories(newpath, "*", SearchOption.TopDirectoryOnly);
            foreach (var dir in dirs)
            {
                string[] toScanFolders = Directory.GetDirectories(dir.ToString(), "*", SearchOption.TopDirectoryOnly);
                if (toScanFolders.Length != 0)
                {
                    foreach (var item2 in toScanFolders)
                    {
                        string folderToCheck = LastElement(item2);
                        states.Add(folderToCheck);
                    }
                    if (!states.Contains("incomig"))
                    {
                        System.IO.Directory.CreateDirectory(dir + "\\incoming");
                    }
                    if (!states.Contains("waiting-auth"))
                    {
                        System.IO.Directory.CreateDirectory(dir + "\\waiting-auth");
                    }
                    if (!states.Contains("signed"))
                    {
                        System.IO.Directory.CreateDirectory(dir + "\\signed");
                    }
                    if (!states.Contains("making-payment"))
                    {
                        System.IO.Directory.CreateDirectory(dir + "\\making-payment");
                    }
                    if (!states.Contains("payment-captured"))
                    {
                        System.IO.Directory.CreateDirectory(dir + "\\payment-captured");
                    }
                }
                else
                {
                    System.IO.Directory.CreateDirectory(dir + "\\incoming");
                    System.IO.Directory.CreateDirectory(dir + "\\waiting-auth");
                    System.IO.Directory.CreateDirectory(dir + "\\signed");
                    System.IO.Directory.CreateDirectory(dir + "\\making-payment");
                    System.IO.Directory.CreateDirectory(dir + "\\payment-captured");
                }
            }
        }

        private int CountFiles(string path)

        {
            string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            int counter = 0;
            foreach (var item in files)
            {
                if (item.Contains("incoming")) counter += 1;
            }
            return counter;
        }

        private void DeleteRegistersFromFilesThatWasRemoved(string path)
        {
            List<string> allFiles = new List<string>();
            string[] dirs = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
            foreach (var dir in dirs)
            {
                string[] files = Directory.GetFiles(dir, "*", SearchOption.TopDirectoryOnly);
                foreach (var file in files)
                {
                    if (file != path)
                    {
                        allFiles.Add(file);
                    }
                }
            }
            List<string> record = new List<string>();
            List<string> recordId = new List<string>();
            string queryfiles = "SELECT * FROM [PAYMENTS].[dbo].[t_files] WHERE [folder] LIKE '" + nameBussiness + "%';";
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
                if (!allFiles.Contains(record[i]))
                {
                    string querydelete = "DELETE FROM [PAYMENTS].[dbo].[t_files] WHERE [id] = '" + recordId[i] + "';";
                    string querydelete2 = "DELETE FROM [PAYMENTS].[dbo].[t_filesSubs] WHERE [idFile] = '" + recordId[i] + "';";
                    DeleteRegisters(querydelete2);
                    DeleteRegisters(querydelete);
                }
            }
        }

        private void DeleteRegisters(string query)
        {
            SqlCommand commandDelete = new SqlCommand(query, connection);
            lock (commandDelete)
            {
                commandDelete.Connection.Open();
                commandDelete.ExecuteNonQuery();
                commandDelete.Connection.Close();
            }
        }

        private void InitializeComboboxBussines()
        {
            DeleteRemovedBussiness();
            comboBox1.Items.Clear();
            string[] dirs = Directory.GetDirectories(newpath, "*", SearchOption.TopDirectoryOnly);
            foreach (var dir in dirs)
            {
                string[] strlist = dir.Split(new char[] { '\\' }, 20, StringSplitOptions.None);
                string queryString = "SELECT * FROM[PAYMENTS].[dbo].[t_bussiness] WHERE nameBussiness = '"
                    + strlist[strlist.Length - 1] + "' AND pathBussiness = '" + newpath + "';";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    reader.Close();
                    string queryString2 = "INSERT INTO [PAYMENTS].[dbo].[t_bussiness]([id],[nameBussiness], [pathBussiness])" +
                                                       " VALUES( NEWID(),'" + strlist[strlist.Length - 1] + "', '" + newpath + "')";
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

        private void DeleteRemovedBussiness()
        {
            List<string> localBussiness = new List<string>();
            List<string> dataBussiness = new List<string>();
            List<string> idBussiness = new List<string>();
            string[] dirs = Directory.GetDirectories(newpath, "*", SearchOption.TopDirectoryOnly);
            foreach (var dir in dirs) localBussiness.Add(dir);
            string query = "SELECT * FROM [PAYMENTS].[dbo].[t_bussiness]";
            SqlCommand command = new SqlCommand(query, connection);
            command.Connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    dataBussiness.Add(reader[2].ToString() + "\\" + reader[1].ToString());
                    idBussiness.Add(reader[0].ToString());
                }
                reader.Close();
                command.Connection.Close();
            }
            int index = 0;
            foreach (var db in dataBussiness)
            {
                if (!localBussiness.Contains(db))
                {
                    if (DialogResult.OK == MessageBox.Show("The bussiness: " + db + " has not been found, would you like to delete files and sub-bussiness related?", "Bussiness not found", MessageBoxButtons.OKCancel))
                    {
                        string del = "DELETE FROM [PAYMENTS].[dbo].[t_files] WHERE [folder] = '" + db + "%';";
                        DeleteRegisters(del);
                        del = "DELETE FROM [PAYMENTS].[dbo].[t_subbussiness] WHERE [idBussiness] = '" + idBussiness[index] + "';";
                        DeleteRegisters(del);
                        del = "DELETE FROM [PAYMENTS].[dbo].[t_bussiness] WHERE [id] = '" + idBussiness[index] + "';";
                        DeleteRegisters(del);
                    }
                    MessageBox.Show("All records for bussiness \""+db+"\" have been deleted.");
                }
                index++;
            }
        }

        private void UpdateFilesForTransactionId()
        {
            string queryObtainId = "SELECT * FROM [PAYMENTS].[dbo].[t_files] where status_name = 'waiting-auth';";
            SqlCommand command = new SqlCommand(queryObtainId, connection);
            command.Connection.Open();
            using (var reader = command.ExecuteReader())
            {
                var list = new List<T_Files>();
                while (reader.Read())
                    list.Add(new T_Files
                    {
                        Id = reader.GetString(0),
                        Name = reader.GetString(1),
                        Fullroute = reader.GetString(2),
                        Status = reader.GetString(3),
                        TransId = reader.GetString(4)
                    });
                reader.Close();
                allRecords = list.ToArray();
            }
            foreach (T_Files record in allRecords)
            {
                string queryStringStatus = "SELECT * FROM [PAYMENTS].[dbo].[t_transactions] WHERE transactionID LIKE '" + record.TransId + "%';";
                command.CommandText = queryStringStatus;
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string fieldsTableFilesId = reader[1].ToString();
                    reader.Close();
                    string queryUpdate = "UPDATE [PAYMENTS].[dbo].[t_files] SET transId = '" + fieldsTableFilesId + "' WHERE folder ='" + record.Fullroute + "' AND fileName = '" + record.Name + "';";
                    command.CommandText = queryUpdate;
                    command.ExecuteNonQuery();
                }
                else reader.Close();
            }
            command.Connection.Close();
        }

        public void FullRefresh()
        {
            ObtainFiles(newpath);
            CheckIfStatesFoldersExists();
            UpdateFilesForTransactionId();
            DeactivateButtons();
            queryString = "SELECT f.*, t.content FROM [PAYMENTS].[dbo].[t_files] f,[PAYMENTS].[dbo].[t_types] t  WHERE f.folder Like '" + nameBussiness + "%' AND f.type = t.id ORDER BY f.fileName DESC;";
            lblTitleResult.Text = (CountFiles(nameBussiness).ToString());
            LoadTable(queryString);
        }

        private void DeactivateButtons()
        {
            btnMakePayment.Enabled = false;
            btnPaymentCaptured.Enabled = false;
            btnSigned.Enabled = false;
            btnCapture.Enabled = false;
        }

        private bool IsThisRoot(string path)
        {
            List<string> states = new List<string>();
            string[] dirs = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
            foreach (var dir in dirs) states.Add(LastElement(dir));
            return !(states.Contains("incoming") || states.Contains("waiting-auth") || states.Contains("payment-captured") || states.Contains("signed") || states.Contains("waiting-auth"));
        }

        public static DialogResult ShowInputDialog(ref string input)
        {
            System.Drawing.Size size = new System.Drawing.Size(200, 70);
            Form inputBox = new Form
            {
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog,
                ClientSize = size,
                Text = "Enter bussiness name"
            };

            System.Windows.Forms.TextBox textBox = new TextBox
            {
                Size = new System.Drawing.Size(size.Width - 10, 23),
                Location = new System.Drawing.Point(5, 5),
                Text = input
            };
            inputBox.Controls.Add(textBox);

            Button okButton = new Button
            {
                DialogResult = System.Windows.Forms.DialogResult.OK,
                Name = "okButton",
                Size = new System.Drawing.Size(75, 23),
                Text = "&OK",
                Location = new System.Drawing.Point(size.Width - 80 - 80, 39)
            };
            inputBox.Controls.Add(okButton);

            Button cancelButton = new Button
            {
                DialogResult = System.Windows.Forms.DialogResult.Cancel,
                Name = "cancelButton",
                Size = new System.Drawing.Size(75, 23),
                Text = "&Cancel",
                Location = new System.Drawing.Point(size.Width - 80, 39)
            };
            inputBox.Controls.Add(cancelButton);

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            inputBox.StartPosition = FormStartPosition.CenterParent;

            DialogResult result = inputBox.ShowDialog();
            input = textBox.Text;
            return result;
        }

        #endregion Methods

        #region Clicks

        private void gridView1_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            GridView gv = gridView1;
            lblSelectedFile.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "fileName").ToString();
            string status = gv.GetRowCellValue(gv.FocusedRowHandle, "status_name").ToString();
            try
            {
                DeactivateButtons();
                switch (status)
                {
                    case "incoming":
                        btnCapture.Enabled = true;
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
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please select a bussines and a file");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DeactivateButtons();
            nameBussiness = comboBox1.SelectedItem.ToString();
            string queryString = "SELECT * FROM[PAYMENTS].[dbo].[t_bussiness] WHERE nameBussiness = '" + nameBussiness + "';";
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
            nameBussiness = $"{newpath}\\{nameBussiness}\\";
            nameBussiness = nameBussiness.Replace(@"\\", @"\");
            DeleteRegistersFromFilesThatWasRemoved(nameBussiness);
            queryString = "SELECT f.*, t.content FROM [PAYMENTS].[dbo].[t_files] f,[PAYMENTS].[dbo].[t_types] t  WHERE f.folder Like '" + nameBussiness + "%' AND f.type = t.id ORDER BY f.fileName DESC;";
            lblTitleResult.Text = (CountFiles(nameBussiness).ToString());
            LoadTable(queryString);
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            try
            {
                var instance = MainViewModel.GetInstance().AssingSubBussiness;
                if (instance != null) instance.Dispose();
                string name = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "fileName").ToString();
                string path = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "folder").ToString() + name;
                string id = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "id").ToString();
                instance = MainViewModel.GetInstance().AssingSubBussiness = new AssingSubBussines(name, path, idBussiness, id);
                instance.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("You must select a bussines and then a file to continue");
            }
        }

        private void btnPaymentCaptured_Click(object sender, EventArgs e)
        {
            if (lblNameBuss.Text == "")
            {
                MessageBox.Show("Please select a Bussiness");
            }
            else
            {
                var instance = MainViewModel.GetInstance().CapturePayment;
                if (instance != null) instance.Dispose();
                instance = MainViewModel.GetInstance().CapturePayment = new PaymentCaptured(lblNameBuss.Text, gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "transId").ToString());
                instance.Show();
            }
        }

        private void btnSigned_Click_1(object sender, EventArgs e)
        {
            if (lblNameBuss.Text == "")
            {
                MessageBox.Show("Please select a Bussiness");
            }
            else
            {
                var instance = MainViewModel.GetInstance().SignDoc;
                if (instance != null) instance.Dispose();
                instance = MainViewModel.GetInstance().SignDoc = new Signed(lblNameBuss.Text, gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "transId").ToString());
                instance.Show();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                FullRefresh();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception)
            {
                MessageBox.Show("Please select a bussiness");
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            var instance = MainViewModel.GetInstance().FinishedTransaction;
            if (instance != null) instance.Dispose();
            instance = MainViewModel.GetInstance().FinishedTransaction = new FinishedTransactions();
            instance.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (lblNameBuss.Text == "Select a Bussiness.")
            {
                MessageBox.Show("Select a bussines pls");
            }
            else
            {
                var instance = MainViewModel.GetInstance().MakePayment;
                if (instance != null) instance.Dispose();
                instance = MainViewModel.GetInstance().MakePayment = new MakingPayment(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "transId").ToString());
                instance.Show();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                var instance = MainViewModel.GetInstance().ViewPdf;
                string name = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "fileName").ToString();
                string path = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "folder").ToString() + name;
                instance = MainViewModel.GetInstance().ViewPdf = new ViewPDF(path);
                instance.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("Please select a file first");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "status_name").ToString() == "incoming")
                {
                    var instance = MainViewModel.GetInstance().ChangeBussines;
                    if (instance != null) instance.Dispose();
                    string name = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "fileName").ToString();
                    string path = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "folder").ToString() + name;
                    instance = MainViewModel.GetInstance().ChangeBussines = new ChangeFileToNewBussiness(path, newpath);
                    instance.Show();
                }
                else
                {
                    MessageBox.Show("Only new files can be moved");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please select a bussines and a file");
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hope you have a nice day!");
        }

        private void subBussinessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var instance = MainViewModel.GetInstance().AddSubBussiness;
            if (instance != null) instance.Dispose();
            instance = MainViewModel.GetInstance().AddSubBussiness = new SubBussinessAdd();
            instance.Show();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var instance = MainViewModel.GetInstance().AddUser;
            if (instance != null) instance.Dispose();
            instance = MainViewModel.GetInstance().AddUser = new UserAddView();
            instance.Show();
        }

        private void businesssToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string input = "";
            if (String.IsNullOrEmpty(newpath)) MessageBox.Show("Please select a root path.");
            else
            {
                if (ShowInputDialog(ref input) == DialogResult.OK)
                {
                    Directory.CreateDirectory(newpath + "\\" + input);
                    ObtainFiles(newpath);
                    InitializeComboboxBussines();
                    CheckIfStatesFoldersExists();
                    DeactivateButtons();
                    gridControl1.DataSource = null;
                    gridControl1.RefreshDataSource();
                    lblSelectedFile.Text = "Select a file.";
                    lblNameBuss.Text = "Select a business.";
                    MessageBox.Show("Created: " + newpath + "\\" + input);
                }
            }
        }

        private void setRootPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                InitialDirectory = "C:\\Users",
                IsFolderPicker = true
            };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                newpath = dialog.FileName;
                if (IsThisRoot(newpath))
                {
                    lblTitleResult.Text = (CountFiles(newpath).ToString());
                    ObtainFiles(newpath);
                    InitializeComboboxBussines();
                    CheckIfStatesFoldersExists();
                    UpdateFilesForTransactionId();
                    DeactivateButtons();
                    gridControl1.DataSource = null;
                    gridControl1.RefreshDataSource();
                    lblSelectedFile.Text = "Select a file.";
                    lblNameBuss.Text = "Select a business.";
                }
                else MessageBox.Show("This is a bussiness folder.");
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
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

        #endregion Events
    }
}