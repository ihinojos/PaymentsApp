namespace Payments.Models
{
    internal class Transactions
    {
        #region Attributes

        private string id;
        private string transactionQB;

        #endregion Attributes

        #region Properties

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string TransactionQB
        {
            get { return transactionQB; }
            set { transactionQB = value; }
        }

        #endregion Properties
    }
}