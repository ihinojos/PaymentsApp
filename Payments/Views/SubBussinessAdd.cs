using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Payments.Views
{
    public partial class SubBussinessAdd : Form
    {
        #region Attributes

        private readonly SqlConnection connection;

        #endregion Attributes

        #region Constructor

        public SubBussinessAdd()
        {
            InitializeComponent();
            connection = new SqlConnection(DB.cn.Replace(@"\\", @"\"));
            LoadCombo();
        }

        #endregion Constructor

        #region Methods

        private void LoadCombo()
        {
            string query = "SELECT * FROM [PRUEBA1].[dbo].[t_bussiness]";
            SqlCommand command = new SqlCommand(query, connection);
            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                bussinessBox.Items.Add(reader[1].ToString());
            }
            reader.Close();
            command.Connection.Close();
        }

        private void AddSubBussiness()
        {
            try
            {
                string name = nameTextBox.Text;
                string addr = addTextBox.Text;
                string buss = bussinessBox.SelectedItem.ToString();
                if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(buss))
                {
                    MessageBox.Show("One or more invalid parameters!");
                }
                else
                {
                    string query = "INSERT INTO [PRUEBA1].[dbo].[t_subbussiness] ([id], [nameSub], [address], [idBussiness]) " +
                        "VALUES ( NEWID(), '" + name + "', '" + addr + "', '" + buss + "')";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Connection.Open();
                    int res = command.ExecuteNonQuery();
                    command.Connection.Close();
                    _ = res != 0 ? MessageBox.Show("Sub Bussiness Created") : MessageBox.Show("There was an error");
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion Methods

        #region Clicks

        private void confirmBtn_Click(object sender, EventArgs e)
        {
            AddSubBussiness();
        }

        #endregion Clicks
    }
}