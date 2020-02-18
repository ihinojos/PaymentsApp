using Payments.Models;
using Payments.Views;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Payments
{
    public partial class Login : Form
    {
        private SqlConnection cn;

        public Login()
        {
            InitializeComponent();
            cn = new SqlConnection(DB.cn.Replace(@"\\", @"\"));
        }

        //Checks if user/password entered exist and match in the database.
        private void UserLogIn(string user, string password)
        {
            string queryString = "SELECT * FROM [PRUEBA1].[dbo].[t_users] WHERE [user] = '" + user + "'";
            SqlCommand command = new SqlCommand(queryString, cn);
            command.Connection.Open();
            command.ExecuteNonQuery();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                string hash = reader[2].ToString();
                if (SecurePassword.Verify(password, hash))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    MainViewModel.GetInstance().newmain = new NewMain(reader[3].ToString());
                    MainViewModel.GetInstance().newmain.Show();
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
                MessageBox.Show("User not found!");
            }
            command.Connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxUser.Text) && !string.IsNullOrEmpty(textBoxPass.Text))
            {
                if (MainViewModel.GetInstance().newmain != null) MainViewModel.GetInstance().newmain = null;
                UserLogIn(textBoxUser.Text, textBoxPass.Text);
            }
            else
            {
                MessageBox.Show("There are empty fields!");
            }
        }
    }
}
