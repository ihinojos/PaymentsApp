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

        public DataTable FullDT;
        public string nameBussiness;
        public String newpath = "";
        public string queryString;
        private T_Files[] allRecords;
        private T_Status[] allRecordsStatus;
        private readonly SqlConnection connection;
        private string status;

        #endregion Attributes

        #region Constructor

        public NewMain(string userType)
        {
            InitializeComponent();
            if (userType == "capture")
            {
                createToolStripMenuItem.Enabled = false;
            }
            connection = new SqlConnection(DB.cn.Replace(@"\\", @"\"));
            DeactivateButtons();
        }

        #endregion Constructor

        #region Methods

        public string LastElement(string splitme)
        {
            string[] strlist = splitme.Split(new char[] { '\\' },
                       20, StringSplitOptions.None);
            return strlist[strlist.Length - 1].ToString();
        }

        public string SecondlastElement(string splitme)
        {
            string[] strlist = splitme.Split(new char[] { '\\' },
                       20, StringSplitOptions.None);
            return strlist[strlist.Length - 2].ToString();
        }

        public void LoadTable(string queryString)
        {
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Connection.Open();
            command.ExecuteNonQuery();
            FullDT = new DataTable();
            using (SqlDataAdapter DA = new SqlDataAdapter(command))
            {
                DA.Fill(FullDT);
            }
            GridView gv = gridView1;
            gridControl1.DataSource = null;
            gridControl1.DataSource = FullDT;
            gridView1.PopulateColumns();
            gridView1.Columns["folder"].Visible = false;
            gridView1.Columns["id"].Visible = false;
            gridView1.Columns["transId"].Visible = false;
            gridView1.Columns["idstatus"].Visible = false;
            gridView1.Columns["status_name"].Visible = true;
            gridView1.Columns["type"].Visible = false;
            gridView1.RowCellClick += gridView1_RowCellClick;
            gridControl1.Update();
            gridControl1.Refresh();
            command.Connection.Close();
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
                        status = url.TrimEnd('\\');
                        status = LastElement(status);
                        string[] strlist = fileName.Split(new char[] { '\\' },
                               20, StringSplitOptions.None);
                        for (int i = 0; i < strlist.Length; i++)
                        {
                            strlist[i] = strlist[i].Replace(((char)39).ToString(), "");
                            if (Path.GetExtension(strlist[i]) == ".pdf")
                            {
                                //Condicion para revisar si ya existe el archivo
                                string queryString = "SELECT [fileName],[folder] FROM[PRUEBA1].[dbo].[t_files] WHERE fileName = '" + strlist[i] + "' AND folder ='" + url + "';";
                                SqlCommand command = new SqlCommand(queryString, connection);
                                command.Connection.Open();
                                command.ExecuteNonQuery();
                                SqlDataReader reader = command.ExecuteReader();
                                if (!reader.Read())
                                {
                                    reader.Close();
                                    if (SecondlastElement(file) == "incoming")
                                    {
                                        string queryString2 = "INSERT INTO [PRUEBA1].[dbo].[t_files]([id],[filename],[folder],[idstatus],[transId],[type])" +
                                                               " VALUES( NEWID(),'" + strlist[i] + "','" + url + "','" + status + "','" + "Not assigned yet',1)";
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
            string[] dirs = Directory.GetDirectories(newpath, "*", SearchOption.TopDirectoryOnly);
            foreach (var dir in dirs)
            {
                string[] toScanFolders = Directory.GetDirectories(dir.ToString(), "*", SearchOption.TopDirectoryOnly);
                if (toScanFolders.Length > 0)
                {
                    foreach (var item2 in toScanFolders)
                    {
                        string folderToCheck = item2.ToString();
                        folderToCheck = LastElement(folderToCheck);
                        if (!(folderToCheck == "incoming"))
                        {
                            System.IO.Directory.CreateDirectory(dir + "\\incoming");
                        }
                        if (!(folderToCheck == "waiting-auth"))
                        {
                            System.IO.Directory.CreateDirectory(dir + "\\waiting-auth");
                        }
                        if (!(folderToCheck == "signed"))
                        {
                            System.IO.Directory.CreateDirectory(dir + "\\signed");
                        }
                        if (!(folderToCheck == "making-payment"))
                        {
                            System.IO.Directory.CreateDirectory(dir + "\\making-payment");
                        }
                        if (!(folderToCheck == "payment-captured"))
                        {
                            System.IO.Directory.CreateDirectory(dir + "\\payment-captured");
                        }
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

        private int countFiles(string path)

        {
            string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            int counter = 0;
            foreach (var item in files)
            {
                if (item.Contains("incoming"))
                {
                    counter += 1;
                }
            }
            return counter;
        }

        private void deleteRegistersFromFilesThatWasRemoved(string path)
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
            string queryfiles = "SELECT * FROM [PRUEBA1].[dbo].[t_files] WHERE [folder] LIKE '" + nameBussiness + "%';";
            SqlCommand command = new SqlCommand(queryfiles, connection);
            command.Connection.Open();
            command.ExecuteNonQuery();
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
                    string querydelete = "DELETE FROM [PRUEBA1].[dbo].[t_files] WHERE [id] = '" + recordId[i] + "';";
                    string querydelete2 = "DELETE FROM [PRUEBA1].[dbo].[t_filesSubs] WHERE [idFile] = '" + recordId[i] + "';";
                    string querydelete3 = "DELETE FROM [PRUEBA1].[dbo].[t_status] WHERE [id_file] = '" + recordId[i] + "';";
                    DeleteRegisters(querydelete);
                    DeleteRegisters(querydelete2);
                    DeleteRegisters(querydelete3);
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

        private void FillStatusTable()
        {
            allRecords = null;
            string query = "select * from [prueba1].[dbo].[t_files];";
            using (var command = new SqlCommand(query, connection))
            {
                command.Connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    var list = new List<T_Files>();
                    while (reader.Read())
                        list.Add(new T_Files { Id = reader.GetString(0), Name = reader.GetString(1), Status = reader.GetString(3) });
                    allRecords = list.ToArray();
                    reader.Close();
                }
                foreach (T_Files record in allRecords)
                {
                    string queryObtainId = "select * from [prueba1].[dbo].[t_status] where id_file = '" + record.Id + "';";
                    command.CommandText = queryObtainId;
                    SqlDataReader reader = command.ExecuteReader();
                    if (!reader.Read())
                    {
                        reader.Close();
                        var timeStamp = DateTime.Now;
                        string queryStringStatus = "insert into [prueba1].[dbo].[t_status]([id],[name_status],[id_file],[name_file],[date])" +
                                           " values( newid(),'" + record.Status + "','" + record.Id + "','" + record.Name + "','" + timeStamp + "')";
                        command.CommandText = queryStringStatus;
                        command.ExecuteNonQuery();
                    }
                    else reader.Close();
                }
                command.Connection.Close();
            }
        }

        private void InitializeComboboxBussines()
        {
            comboBox1.Items.Clear();
            string[] dirs = Directory.GetDirectories(newpath, "*", SearchOption.TopDirectoryOnly);
            foreach (var dir in dirs)
            {
                string[] strlist = dir.Split(new char[] { '\\' }, 20, StringSplitOptions.None);
                string queryString = "SELECT * FROM[PRUEBA1].[dbo].[t_bussiness] WHERE nameBussiness = '"
                    + strlist[strlist.Length - 1] + "';";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
                SqlDataReader reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    reader.Close();
                    string queryString2 = "INSERT INTO [PRUEBA1].[dbo].[t_bussiness]([id],[nameBussiness])" +
                                                       " VALUES( NEWID(),'" + strlist[strlist.Length - 1] + "')";
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

        private void UpdateFilesForTransactionId(string path)
        {
            string queryObtainId = "SELECT * FROM [PRUEBA1].[dbo].[t_files] where status_name = 'waiting-auth';";
            SqlCommand command = new SqlCommand(queryObtainId, connection);
            command.Connection.Open();
            command.ExecuteNonQuery();
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
                string queryStringStatus = "SELECT * FROM [PRUEBA1].[dbo].[t_transactions] WHERE transactionID LIKE '" + record.TransId + "%';";
                command.CommandText = queryStringStatus;
                command.ExecuteNonQuery();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string fieldsTableFilesId = reader[1].ToString();
                    string fieldsTableFilesName = reader[0].ToString();
                    reader.Close();
                    string queryUpdate = "UPDATE [PRUEBA1].[dbo].[t_files] SET transId = '" + fieldsTableFilesId + "' WHERE folder ='" + record.Fullroute + "' AND fileName = '" + record.Name + "';";
                    command.CommandText = queryUpdate;
                    command.ExecuteNonQuery();
                }
                else reader.Close();
            }
            command.Connection.Close();
        }

        private void ObtainFolders(string newpath)
        {
            try
            {
                string[] dirs = Directory.GetDirectories(newpath, "*", SearchOption.TopDirectoryOnly);
                foreach (string dir in dirs)
                {
                    string[] strlist = dir.Split(new char[] { '\\' }, 20, StringSplitOptions.None);
                    string queryString = "SELECT * FROM[PRUEBA1].[dbo].[t_folders] WHERE folderName = '"
                        + strlist[strlist.Length - 1] + "' AND path ='" + newpath + "\\';";
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    SqlDataReader reader = command.ExecuteReader();
                    if (!reader.Read())
                    {
                        reader.Close();
                        string queryString3 = "INSERT INTO [PRUEBA1].[dbo].[t_folders]([id],[folderName],[path],[fullpath])" +
                                                   " VALUES( NEWID(),'" + strlist[strlist.Length - 1]
                                                   + "','" + newpath + "\\','" + newpath + "\\" + strlist[strlist.Length - 1]
                                                   + "\\')";
                        command.CommandText = queryString3;
                        command.ExecuteNonQuery();
                        command.Connection.Close();
                        ObtainFolders(dir);
                    }
                    else reader.Close();
                    command.Connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }

        private void UpdateFilesForIdStatus(string path)
        {
            string queryObtainId = "SELECT id,id_file,name_status FROM [PRUEBA1].[dbo].[t_status];";
            SqlCommand command = new SqlCommand(queryObtainId, connection);
            command.Connection.Open();
            command.ExecuteNonQuery();
            using (var reader = command.ExecuteReader())
            {
                var list = new List<T_Status>();
                while (reader.Read())
                    list.Add(new T_Status { Id = reader.GetString(0), Id_file = reader.GetString(1), Status = reader.GetString(2) });
                allRecordsStatus = list.ToArray();
                reader.Close();
            }
            foreach (T_Status record in allRecordsStatus)
            {
                string queryStringStatus = "SELECT * FROM [PRUEBA1].[dbo].[t_files] WHERE id LIKE '" + record.Id_file + "%';";
                command.CommandText = queryStringStatus;
                command.ExecuteNonQuery();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (reader[3].ToString() == "incoming")
                    {
                        reader.Close();
                        string queryUpdate = "UPDATE [PRUEBA1].[dbo].[t_files] SET idstatus = '" + record.Id_file + "', status_name='incoming' WHERE id LIKE '" + record.Id_file + "';";
                        command.CommandText = queryUpdate;
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        reader.Close();
                        string queryUpdate = "UPDATE [PRUEBA1].[dbo].[t_files] SET idstatus = '" + record.Id_file + "' WHERE id LIKE '" + record.Id_file + "';";
                        command.CommandText = queryUpdate;
                        command.ExecuteNonQuery();
                    }
                }
            }
            command.Connection.Close();
        }

        public void fullRefresh()
        {
            ObtainFiles(newpath);
            ObtainFolders(newpath);
            CheckIfStatesFoldersExists();
            FillStatusTable();
            UpdateFilesForIdStatus(newpath);
            UpdateFilesForTransactionId(newpath);
            queryString = "SELECT f.*, t.content FROM [PRUEBA1].[dbo].[t_files] f,[PRUEBA1].[dbo].[t_types] t  WHERE f.folder Like '" + nameBussiness + "%' AND f.type = t.id ORDER BY f.idstatus DESC;";
            lblTitleResult.Text = (countFiles(nameBussiness).ToString());
            LoadTable(queryString);
        }

        private void DeactivateButtons()
        {
            btnMakePayment.Enabled = false;
            btnPaymentCaptured.Enabled = false;
            btnSigned.Enabled = false;
            btnCapture.Enabled = false;
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
                if (status == "incoming")
                {
                    DeactivateButtons();
                    btnCapture.Enabled = true;
                }
                if (status == "waiting-auth")
                {
                    DeactivateButtons();
                    btnSigned.Enabled = true;
                }
                if (status == "signed")
                {
                    DeactivateButtons();
                    btnMakePayment.Enabled = true;
                }
                if (status == "making-payment")
                {
                    DeactivateButtons();
                    btnPaymentCaptured.Enabled = true;
                }
                if (status == "payment-captured")
                {
                    DeactivateButtons();
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
            lblNameBuss.Text = comboBox1.SelectedItem.ToString();
            nameBussiness = $"{newpath}\\{nameBussiness}\\";
            nameBussiness = nameBussiness.Replace(@"\\", @"\");
            deleteRegistersFromFilesThatWasRemoved(nameBussiness);
            queryString = "SELECT f.*, t.content FROM [PRUEBA1].[dbo].[t_files] f,[PRUEBA1].[dbo].[t_types] t  WHERE f.folder Like '" + nameBussiness + "%' AND f.type = t.id ORDER BY f.idstatus DESC;";
            lblTitleResult.Text = (countFiles(nameBussiness).ToString());
            LoadTable(queryString);
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            try
            {
                if (MainViewModel.GetInstance().assing == null)
                {
                    GridView gv = gridView1;
                    string path = gv.GetRowCellValue(gv.FocusedRowHandle, "folder").ToString();
                    string name = gv.GetRowCellValue(gv.FocusedRowHandle, "fileName").ToString();
                    string id = gv.GetRowCellValue(gv.FocusedRowHandle, "id").ToString();
                    path += name;
                    MainViewModel.GetInstance().assing = new AssingSubBussines(name, path, comboBox1.SelectedItem.ToString(), id);
                    MainViewModel.GetInstance().assing.FormClosed += FormClosed;
                    MainViewModel.GetInstance().assing.Show();
                    MainViewModel.GetInstance().assing.BringToFront();
                }
                else MainViewModel.GetInstance().assing.BringToFront();
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
                if (MainViewModel.GetInstance().captured == null)
                {
                    GridView gv = gridView1;
                    string name = lblNameBuss.Text;
                    string id = gv.GetRowCellValue(gv.FocusedRowHandle, "transId").ToString();

                    MainViewModel.GetInstance().captured = new PaymentCaptured(name, id);
                    MainViewModel.GetInstance().captured.FormClosed += FormClosed;
                    MainViewModel.GetInstance().captured.Show();
                }
                else MainViewModel.GetInstance().captured.BringToFront();
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
                if (MainViewModel.GetInstance().signed == null)
                {
                    GridView gv = gridView1;
                    string name = lblNameBuss.Text;
                    string id = gv.GetRowCellValue(gv.FocusedRowHandle, "transId").ToString();
                    MainViewModel.GetInstance().signed = new Signed(name, id);
                    MainViewModel.GetInstance().signed.FormClosed += FormClosed;
                    MainViewModel.GetInstance().signed.Show();
                }
                else MainViewModel.GetInstance().signed.BringToFront();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                fullRefresh();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception)
            {
                MessageBox.Show("Please select a bussiness");
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            if (MainViewModel.GetInstance().transaction == null)
            {
                MainViewModel.GetInstance().transaction = new FinishedTransactions();
                MainViewModel.GetInstance().transaction.FormClosed += FormClosed;
                MainViewModel.GetInstance().transaction.Show();
            }
            else MainViewModel.GetInstance().transaction.BringToFront();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (lblNameBuss.Text == "Select a Bussiness.")
            {
                MessageBox.Show("Select a bussines pls");
            }
            else
            {
                if (MainViewModel.GetInstance().payment == null)
                {
                    GridView gv = gridView1;
                    string transId = gv.GetRowCellValue(gv.FocusedRowHandle, "transId").ToString();
                    MainViewModel.GetInstance().payment = new MakingPayment(transId);
                    MainViewModel.GetInstance().payment.FormClosed += FormClosed;
                    MainViewModel.GetInstance().payment.Show();
                }
                else MainViewModel.GetInstance().payment.BringToFront();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (MainViewModel.GetInstance().pdf == null)
                {
                    GridView gv = gridView1;
                    string path = gv.GetRowCellValue(gv.FocusedRowHandle, "folder").ToString();
                    string name = gv.GetRowCellValue(gv.FocusedRowHandle, "fileName").ToString();
                    path = path + name;
                    MainViewModel.GetInstance().pdf = new ViewPDF(path);
                    MainViewModel.GetInstance().pdf.FormClosed += FormClosed;
                    MainViewModel.GetInstance().pdf.Show();
                }
                else MainViewModel.GetInstance().pdf.BringToFront();
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
                GridView gv = gridView1;
                string status = gv.GetRowCellValue(gv.FocusedRowHandle, "status_name").ToString();
                if (status == "incoming")
                {
                    if (MainViewModel.GetInstance().changeBussines == null)
                    {
                        string path = gv.GetRowCellValue(gv.FocusedRowHandle, "folder").ToString();
                        string name = gv.GetRowCellValue(gv.FocusedRowHandle, "fileName").ToString();
                        path = path + name;
                        MainViewModel.GetInstance().changeBussines = new ChangeFileToNewBussiness(path);
                        MainViewModel.GetInstance().changeBussines.FormClosed += FormClosed;
                        MainViewModel.GetInstance().changeBussines.Show();
                    }
                    else MainViewModel.GetInstance().changeBussines.BringToFront();
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
            if (MainViewModel.GetInstance().addSub == null)
            {
                MainViewModel.GetInstance().addSub = new SubBussinessAdd();
                MainViewModel.GetInstance().addSub.FormClosed += FormClosed;
                MainViewModel.GetInstance().addSub.Show();
            }
            else MainViewModel.GetInstance().addSub.BringToFront();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MainViewModel.GetInstance().addUser == null)
            {
                MainViewModel.GetInstance().addUser = new UserAddView();
                MainViewModel.GetInstance().addUser.FormClosed += FormClosed;
                MainViewModel.GetInstance().addUser.Show();
            }
            else MainViewModel.GetInstance().addUser.BringToFront();
        }

        private void businesssToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please create a folder with the desired Business name in the root path:" +
                "\n" + newpath);
        }

        private void setRootPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\Users";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                newpath = dialog.FileName;
                lblTitleResult.Text = (countFiles(newpath).ToString());
                ObtainFiles(newpath);
                ObtainFolders(newpath);
                InitializeComboboxBussines();
                CheckIfStatesFoldersExists();
                FillStatusTable();
                UpdateFilesForIdStatus(newpath);
                UpdateFilesForTransactionId(newpath);
                DeactivateButtons();
                gridControl1.DataSource = null;
                gridControl1.RefreshDataSource();
                lblSelectedFile.Text = "Select a file.";
                lblNameBuss.Text = "Select a business.";
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                MessageBox.Show("Current Version: " + System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion);
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
                    MainViewModel.GetInstance().login.Visible = true;
                }
            }
        }

        public new void FormClosed(object sender, FormClosedEventArgs e)
        {
            switch (sender.GetType().ToString())
            {
                case "Payments.Views.AssingSubBussines":
                    MainViewModel.GetInstance().assing = null;
                    break;

                case "Payments.Views.Signed":
                    MainViewModel.GetInstance().signed = null;
                    break;

                case "Payments.Views.MakingPayment":
                    MainViewModel.GetInstance().payment = null;
                    break;

                case "Payments.Views.PaymentCaptured":
                    MainViewModel.GetInstance().captured = null;
                    break;

                case "Payments.Views.ChangeFileToNewBussiness":
                    MainViewModel.GetInstance().changeBussines = null;
                    break;

                case "Payments.Views.ViewPDF":
                    MainViewModel.GetInstance().pdf = null;
                    break;

                case "Payments.Views.FinishedTransactions":
                    MainViewModel.GetInstance().transaction = null;
                    break;

                case "Payments.Views.SubBussinessAdd":
                    MainViewModel.GetInstance().addSub = null;
                    break;

                case "Payments.Views.UserAddView":
                    MainViewModel.GetInstance().addUser = null;
                    break;

                case "Payments.Views.SplitPDF":
                    MainViewModel.GetInstance().splitPDF = null;
                    break;
            }
        }

        #endregion Events
    }
}