using Payments.Models;
using Payments.Views;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Payments
{
    public partial class Login : Form
    {
        #region Attributes

        private readonly SqlConnection connection;

        #endregion Attributes

        #region Constructor

        public Login()
        {
            InitializeComponent();
            connection = new SqlConnection(DB.cn.Replace(@"\\", @"\"));
            //FirstRun();
        }

        #endregion Constructor

        #region Methods

        private void FirstRun()
        {
            string user = "root";
            string pass = SecurePassword.Hash("root");
            string query = "INSERT INTO [PAYMENTS].[dbo].[t_users] ([id], [user], [password], [type]) VALUES (NEWID(), '" + user + "', '" + pass + "', 'admin')";
            SqlCommand command = new SqlCommand(query, connection);
            command.Connection.Open();
            command.ExecuteNonQuery();
            query = "INSERT INTO [PAYMENTS].[dbo].[t_types] ([id], [content]) VALUES ('1', 'Unsigned Bill'), ('2', 'Signed Bill'), ('3', 'Proof of Payment')";
            command.CommandText = query;
            command.ExecuteNonQuery();
            command.Connection.Close();
        }

        private void UserLogIn(string user, string password)
        {
            string queryString = "SELECT * FROM [PAYMENTS].[dbo].[t_users] WHERE [user] = '" + user + "'";
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                string hash = reader[2].ToString();
                reader.Close();
                if (SecurePassword.Verify(password, hash))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    var instance = MainViewModel.GetInstance().NewMain;
                    if (instance != null) instance.Dispose();
                    instance = MainViewModel.GetInstance().NewMain = new NewMain();
                    instance.Show();
                    Cursor.Current = Cursors.Default;
                    Visible = false;
                    textBoxPass.Clear();
                }
                else
                {
                    MessageBox.Show("The user or the password are incorrect.");
                    textBoxUser.Clear();
                    textBoxPass.Clear();
                }
            }
            else
            {
                reader.Close();
                MessageBox.Show("User not found!");
            }
            command.Connection.Close();
        }

        #endregion Methods

        #region Clicks

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxUser.Text) && !string.IsNullOrEmpty(textBoxPass.Text))
            {
                if (MainViewModel.GetInstance().NewMain != null) MainViewModel.GetInstance().NewMain = null;
                UserLogIn(textBoxUser.Text, textBoxPass.Text);
            }
            else
            {
                MessageBox.Show("There are empty fields!");
            }
        }

        #endregion Clicks
    }
}