using Payments.Views;

namespace Payments.Models
{
    public class MainViewModel
    {
        #region Properties

        public NewMain newmain { get; set; }
        public AssingSubBussines assing { get; set; }
        public ViewPDF pdf { get; set; }
        public Signed signed { get; set; }
        public SplitPDF splitPDF { get; set; }
        public Login login { get; set; }
        public MakingPayment payment { get; set; }
        public PaymentCaptured captured { get; set; }
        public FinishedTransactions transaction { get; set; }
        public ChangeFileToNewBussiness changeBussines { get; set; }
        public UserAddView addUser { get; set; }
        public SubBussinessAdd addSub { get; set; }

        #endregion Properties

        #region Constructor

        public MainViewModel()
        {
            instance = this;
        }

        #endregion Constructor

        #region Singleton

        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }
            return instance;
        }

        #endregion Singleton
    }
}