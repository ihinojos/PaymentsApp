namespace Payments.Models
{
    internal class T_ListOfFiles
    {
        #region Attributes

        private string originalFile;
        private string firstFile;
        private string signedFile;
        private string payment;

        #endregion Attributes

        #region Properties

        public string OriginalFile
        {
            get { return originalFile; }
            set { originalFile = value; }
        }

        public string FirstFile
        {
            get { return firstFile; }
            set { firstFile = value; }
        }

        public string SignedFile
        {
            get { return signedFile; }
            set { signedFile = value; }
        }

        public string Payment
        {
            get { return payment; }
            set { payment = value; }
        }

        #endregion Properties
    }
}