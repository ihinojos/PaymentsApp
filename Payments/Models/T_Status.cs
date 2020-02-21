namespace Payments.Models
{
    internal class T_Status
    {
        #region Attributes

        private string id;
        private string status;
        private string id_file;

        #endregion Attributes

        #region Properties

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        public string Id_file
        {
            get { return id_file; }
            set { id_file = value; }
        }

        #endregion Properties
    }
}