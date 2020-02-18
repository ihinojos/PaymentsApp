using DevExpress.XtraGrid.Views.Grid;
using Payments.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Payments.Views
{
    public partial class AssingSubBussines : Form
    {
        #region Attributes

        public string isActive = "1";
        private SqlConnection cn;
        private int hi;
        public DataTable FullDT;
        private string idFileSelected;
        private string pathfileSelected;
        private string queryString;
        private string selectedBussines;
        private string idTansaction;
        private string queryStringSubBussinesFiles;
        private string pathToThisBussinesWaitingAuth;
        private int currentNumberOfSubsOftheFile;

        #endregion Attributes

        #region Constructor

        public AssingSubBussines(string fileSelected, string pathToFile, string bussinessSelected, string id)
        {
            InitializeComponent();
            cn = new SqlConnection(DB.cn.Replace(@"\\", @"\"));
            selectedBussines = bussinessSelected;
            queryString = "SELECT * FROM [PRUEBA1].[dbo].[t_subbussiness] WHERE idBussiness = '" + bussinessSelected + "';";
            lblFileSelected.Text = fileSelected;
            loadCombo(queryString);
            idFileSelected = id;
            pathfileSelected = pathToFile;
            axAcroPDF1.src = pathToFile;
            try
            {
                queryStringSubBussinesFiles = "SELECT * FROM [PRUEBA1].[dbo].[t_filesSubs] WHERE idFile = '" + idFileSelected + "';";
                loadTable(queryStringSubBussinesFiles);
            }
            catch (Exception ex)
            {
                MessageBox.Show("The query had a problem:" + ex);
            }
        }

        #endregion Constructor

        #region Methods

        #region Clicks

        private void gridView1_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            GridView gv = gridView1;
            lblSubSelected.Text = gv.GetRowCellValue(gv.FocusedRowHandle, "idSubBussiness").ToString();
        }

        private void btnAssingTo_Click(object sender, EventArgs e)
        {
            try
            {
                string queryStringNew = "SELECT * FROM[PRUEBA1].[dbo].[t_filesSubs] WHERE idFile = '" + idFileSelected + "' AND idSubBussiness ='" + comboBoxSubBussiness.SelectedItem.ToString() + "';";
                SqlCommand command = new SqlCommand(queryStringNew, cn);
                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Close();
                    command.Connection.Open();
                }
                int row = command.ExecuteNonQuery();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    MessageBox.Show("The sub-bussiness already exists");
                    command.Connection.Close();
                }
                else
                {
                    command.Connection.Close();
                    string newSub = lblSubSelected.Text;
                    string queryString2 = "INSERT INTO [PRUEBA1].[dbo].[t_filesSubs]([id],[idFile],[idSubBussiness])" +
                                                           " VALUES( NEWID(),'" + idFileSelected
                                                           + "','" + comboBoxSubBussiness.SelectedItem.ToString()
                                                                   + "')";
                    SqlCommand command2 = new SqlCommand(queryString2, cn);
                    command2.Connection.Open();
                    command2.ExecuteNonQuery();
                    command2.Connection.Close();
                    loadTable(queryStringSubBussinesFiles);
                    MessageBox.Show("Assignation sucessful for the file: " + lblFileSelected.Text + " for the sub-bussiness: " + comboBoxSubBussiness.SelectedItem.ToString());
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion Clicks

        private void loadTable(string queryString)
        {
            gridControl1.DataSource = null;
            SqlCommand command = new SqlCommand(queryString, cn);
            command.Connection.Open();
            currentNumberOfSubsOftheFile = command.ExecuteNonQuery();
            FullDT = new DataTable();
            using (SqlDataAdapter DA = new SqlDataAdapter(command))
            {
                DA.Fill(FullDT);
            }
            gridControl1.RefreshDataSource();
            GridView gv = gridView1;
            gridControl1.DataSource = FullDT;
            gridView1.PopulateColumns();
            gridView1.Columns["idFile"].Visible = false;
            gridView1.Columns["id"].Visible = false;
            gridView1.RowCellClick += gridView1_RowCellClick;
            gridControl1.Update();
            gridControl1.Refresh();
            command.Connection.Close();
        }

        private void loadCombo(string queryString)
        {
            comboBoxSubBussiness.Items.Clear();
            SqlCommand command = new SqlCommand(queryString, cn);
            command.Connection.Open();
            command.ExecuteNonQuery();
            SqlDataReader reader = command.ExecuteReader();
            do
            {
                while (reader.Read())
                {
                    comboBoxSubBussiness.Items.Add(reader.GetValue(1).ToString());
                }
            } while (reader.NextResult());
            command.Connection.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        #region Split array

        public string lastElement(string splitme)
        {
            string[] strlist = splitme.Split(new char[] { '\\' },
                       20, StringSplitOptions.None);
            return strlist[strlist.Length - 1].ToString();
        }

        public string secondlastElement(string splitme)
        {
            string[] strlist = splitme.Split(new char[] { '\\' },
                       20, StringSplitOptions.None);
            return strlist[strlist.Length - 2].ToString();
        }

        #endregion Split array

        #endregion Methods

        private void btnCapture_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            queryStringSubBussinesFiles = "SELECT COUNT (id) FROM [PRUEBA1].[dbo].[t_filesSubs] WHERE idFile = '" + idFileSelected + "';";
            SqlCommand commandFormat1 = new SqlCommand(queryStringSubBussinesFiles, cn);
            if (commandFormat1.Connection.State != ConnectionState.Open)
            {
                commandFormat1.Connection.Close();
                commandFormat1.Connection.Open();
            }
            currentNumberOfSubsOftheFile = commandFormat1.ExecuteNonQuery();
            SqlDataReader reader = commandFormat1.ExecuteReader();
            do
            {
                while (reader.Read())
                {
                    hi = Convert.ToInt32(reader.GetValue(0));
                }
            } while (reader.NextResult());

            commandFormat1.Connection.Close();
            if (textBoxTransaction.Text == "" || hi <= 0)
            {
                MessageBox.Show("Verify if file has sub_bussiness or there is a transacion Id");
            }
            else
            {
                string newPathForRename = "";
                var dateTimeOffset = new DateTimeOffset(DateTime.Now);
                var formatDate = dateTimeOffset.ToUnixTimeSeconds();
                string newFormat = formatDate + "_" + "waitingAuth" + "_" + textBoxTransaction.Text.ToString() + ".pdf";
                char[] spearator = { '\\' };
                Int32 count = 20;

                // Using the Method
                String[] strlist = pathfileSelected.Split(spearator,
                       count, StringSplitOptions.None);
                for (int i = 0; i < strlist.Length - 2; i++)
                {
                    if (i == 0)
                    {
                        newPathForRename = strlist[i];
                    }
                    else
                    {
                        newPathForRename = newPathForRename + "\\" + strlist[i];
                        pathToThisBussinesWaitingAuth = newPathForRename;
                    }
                }
                newPathForRename = newPathForRename + "\\" + "waiting-auth" + "\\" + newFormat;
                axAcroPDF1.src = "";

                try
                {

                    //Insertar nueva transaccion
                    string queryStringformat1 = "INSERT INTO [PRUEBA1].[dbo].[t_transactions]([id],[transactionId])" +
                                                               " VALUES( NEWID(),'" + textBoxTransaction.Text
                                                               + "')";
                    SqlCommand commandFormat12 = new SqlCommand(queryStringformat1, cn);
                    commandFormat12.Connection.Open();
                    int rowsAffected = commandFormat12.ExecuteNonQuery();
                    commandFormat12.Connection.Close();
                    //Fin Insertar nueva transaccion

                    //Obtener el id creado en el paso anterior para posteriormente indexarlo a la tabla t_files
                    string queryObtainNewId = "select * from [PRUEBA1].[dbo].[t_transactions] where transactionId ='" + textBoxTransaction.Text + "';";
                    SqlCommand commandObtainNewId = new SqlCommand(queryObtainNewId, cn);
                    commandObtainNewId.Connection.Open();
                    int row = commandObtainNewId.ExecuteNonQuery();
                    SqlDataReader readerNewId = commandObtainNewId.ExecuteReader();
                    if (readerNewId.Read())
                    {
                        idTansaction = readerNewId[1].ToString();
                        string transactionNumber = readerNewId[0].ToString();
                    }
                    commandObtainNewId.Connection.Close();
                    //Fin obtener el id creado en el paso anterior para posteriormente indexarlo a la tabla t_files

                    //Hacer update de los cambios recientes, de nomenclatura, nuevo estado y nuevo id de transaccion
                    string queryStringDelete1 = "UPDATE [PRUEBA1].[dbo].[t_files] SET " +
                        "fileName = '" + lastElement(newPathForRename) + "', " +
                        "folder= '" + pathToThisBussinesWaitingAuth + "\\" + "waiting-auth\\" + "'," +
                        "transId = '" + idTansaction + "', " +
                        "status_name = 'waiting-auth'" +
                        " WHERE id = '" + idFileSelected + "';";
                    SqlCommand commandDelete1 = new SqlCommand(queryStringDelete1, cn);
                    commandDelete1.Connection.Open();
                    int row3 = commandDelete1.ExecuteNonQuery();
                    commandDelete1.Connection.Close();

                    System.IO.File.Move(pathfileSelected, newPathForRename);

                    //Hacer update de los cambios recientes, de nomenclatura, nuevo estado y nuevo id de transaccion
                    MessageBox.Show("Invoice captured correctly");
                    MainViewModel.GetInstance().NewMain.fullRefresh();
                    MainViewModel.GetInstance().NewMain.LoadTable(MainViewModel.GetInstance().NewMain.queryString);
                    MainViewModel.GetInstance().NewMain.gridControl1.Update();
                }
                catch (Exception ex)
                {
                    MessageBox.Show( ex.Message);
                }
                
                Cursor.Current = Cursors.Default;
                this.Close();
            }
        }
    }
}