using System;
using System.Windows.Forms;

namespace Payments.Views
{
    public partial class ViewPDF : Form
    {
        #region Constructor

        public ViewPDF(String InitialTextBoxValue)
        {
            InitializeComponent();
            axAcroPDF1.src = InitialTextBoxValue;
        }

        #endregion Constructor
    }
}