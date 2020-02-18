namespace Payments.Models
{
    internal class T_SubBussines
    {
        #region Attributes

        private string id;
        private string idFile;
        private string idSubBussiness;

        #endregion Attributes

        #region Properties

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string IdFile
        {
            get { return idFile; }
            set { idFile = value; }
        }

        public string IdSubBussiness
        {
            get { return idSubBussiness; }
            set { idSubBussiness = value; }
        }

        #endregion Properties
    }
}