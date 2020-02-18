namespace Payments.Models
{
    public class T_Files
    {
        #region Attributes

        private string id;
        private string status;
        private string name;
        private string fullroute;
        private string transId;

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

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Fullroute
        {
            get { return fullroute; }
            set { fullroute = value; }
        }

        public string TransId
        {
            get { return transId; }
            set { transId = value; }
        }

        #endregion Properties
    }
}