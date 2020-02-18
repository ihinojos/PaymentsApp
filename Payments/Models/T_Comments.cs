namespace Payments.Models
{
    internal class T_Comments
    {
        #region Attributes

        private string id;
        private string comment;
        private string id_status;
        private string comment_date;

        #endregion Attributes

        #region Properties

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        public string Id_status
        {
            get { return id_status; }
            set { id_status = value; }
        }

        public string Comment_date
        {
            get { return comment_date; }
            set { comment_date = value; }
        }

        #endregion Properties
    }
}