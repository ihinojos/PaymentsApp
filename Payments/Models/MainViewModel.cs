using Payments.Views;

namespace Payments.Models
{
    public class MainViewModel
    {
        #region Properties

        private static MainViewModel instance;
        public NewMain NewMain { get; set; }
        public AssingSubBussines AssingSubBussiness { get; set; }
        public ViewPDF ViewPdf { get; set; }
        public Signed SignDoc { get; set; }
        public SplitPDF SplitPdf { get; set; }
        public Login Login { get; set; }
        public MakingPayment MakePayment { get; set; }
        public PaymentCaptured CapturePayment { get; set; }
        public FinishedTransactions FinishedTransaction { get; set; }
        public ChangeFileToNewBussiness ChangeBussines { get; set; }
        public UserAddView AddUser { get; set; }
        public SubBussinessAdd AddSubBussiness { get; set; }

        #endregion Properties

        #region Constructor

        public MainViewModel()
        {
            instance = this;
        }

        #endregion Constructor

        #region Singleton

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