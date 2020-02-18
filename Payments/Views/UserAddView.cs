using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Payments.Views
{
    public partial class UserAddView : Form
    {
        private SqlConnection con;

        public UserAddView()
        {
            InitializeComponent();
            typeBox.Items.Add("admin");
            typeBox.Items.Add("capture");
            con = new SqlConnection(DB.cn.Replace(@"\\", @"\"));
            MessageBox.Show("Password requirements: \n1.- At least 8 digits.\n2.- At least 1 uppercase letter\n3.- At least 1 number");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (emptyCheck() && passwordCheck(passTextBox.Text, passConfirm.Text))
            {
                saveUser();
            }
        }

        private bool passwordCheck(string p1, string p2)
        {
            return p1.Equals(p2);
        }

        private bool emptyCheck()
        {
            string box;
            try
            {
                box = typeBox.SelectedItem.ToString();
            }
            catch (Exception)
            {
                box = null;
            }
            if (string.IsNullOrEmpty(userTextBox.Text) || string.IsNullOrEmpty(passTextBox.Text)
                || string.IsNullOrEmpty(passConfirm.Text) || string.IsNullOrEmpty(box))
            {
                MessageBox.Show("Invalid parameters");
                return false;
            }
            return true;
        }

        private void saveUser()
        {
            string user = userTextBox.Text;
            string pass = passTextBox.Text;
            string type = typeBox.SelectedItem.ToString();
            if (SecurePassword.IsSecure(pass))
            {
                pass = SecurePassword.Hash(pass);
                string query = "INSERT INTO [PRUEBA1].[dbo].[t_users] ([id], [user], [password], [type]) VALUES (NEWID(), '" + user + "', '" + pass + "', '" + type + "')";

                SqlCommand command = new SqlCommand(query, con);
                command.Connection.Open();
                try
                {
                    int res = command.ExecuteNonQuery();
                    if (res != 0)
                    {
                        MessageBox.Show("User created successfully.");
                    }
                    else
                    {
                        MessageBox.Show("There was a problem.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("User already exists. \n" + ex.Message);
                }
                command.Connection.Close();
                this.Dispose();
            }
        }
    }
}