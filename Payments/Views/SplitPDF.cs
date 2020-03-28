using Payments.Models;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Payments.Views
{
    public partial class SplitPDF : Form
    {
        #region Attributes

        private readonly string name;
        private string pathToPlaceFiles;
        private readonly List<int> pages = new List<int>();
        private readonly List<int> pagesComplement = new List<int>();

        #endregion Attributes

        #region Constructor

        public SplitPDF(string pathToPutPDF, string name)
        {
            InitializeComponent();
            this.name = name;
            axAcroPDF1.src = "";
            axAcroPDF1.src = pathToPutPDF;
            pathToPlaceFiles = pathToPutPDF;
            FillComboBox();
        }

        #endregion Constructor

        #region Methods

        private void Split(List<int> pages)
        {
            PdfDocument outputDocument = new PdfDocument
            {
                PageLayout = PdfPageLayout.OneColumn
            };

            PdfDocument inputDocument = PdfReader.Open(pathToPlaceFiles, PdfDocumentOpenMode.Import);
            try
            {
                foreach (var item in pages)
                {
                    PdfPage page = inputDocument.Pages[item];
                    outputDocument.AddPage(page);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pathToPlaceFiles += "_crop";
            outputDocument.Save(pathToPlaceFiles);
            axAcroPDF1.src = pathToPlaceFiles;
            switch (name)
            {
                case "sign":
                    MainViewModel.GetInstance().SignDoc.PutCroppedPdf(pathToPlaceFiles);
                    break;

                case "pay":
                    MainViewModel.GetInstance().CapturePayment.PutCroppedPdf(pathToPlaceFiles);
                    break;
            }
            Dispose();
        }

        private void FillComboBox()
        {
            PdfDocument inputDocument = PdfReader.Open(pathToPlaceFiles, PdfDocumentOpenMode.Import);
            int count = inputDocument.PageCount;
            for (int i = 0; i < count; i++)
            {
                comboBox1.Items.Add(i + 1);
                pagesComplement.Add(i);
            }
        }

        #endregion Methods

        #region Clicks

        private void Button1_Click(object sender, EventArgs e)
        {
            Split(pages);
            MessageBox.Show("File creation was successfull");
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                treeView1.Nodes.Add(comboBox1.SelectedItem.ToString());
                int number = Convert.ToInt32(comboBox1.SelectedItem.ToString());
                comboBox1.Items.Remove(number);
                pages.Add(number - 1);
                pagesComplement.Remove(number - 1);
            }
            catch (Exception)
            {
                MessageBox.Show("You must select one page");
            }
        }

        #endregion Clicks
    }
}