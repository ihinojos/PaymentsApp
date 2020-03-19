using Payments.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Payments.Views
{
    public partial class ChangeFileToNewBussiness : Form
    {
        #region Attributes

        private readonly string filePath;
        public readonly string rootPath;
        private readonly List<string> RutasPusibles = new List<string>();

        #endregion Attributes

        #region Constructor

        public ChangeFileToNewBussiness(string path, string root)
        {
            InitializeComponent();
            filePath = path;
            rootPath = root;
            ScanForAvailableBussiness();
        }

        #endregion Constructor

        #region Methods

        private void ScanForAvailableBussiness()
        {
            string[] Bussiness = Directory.GetDirectories(rootPath, "*.*", SearchOption.TopDirectoryOnly);
            foreach (var item in Bussiness)
            {
                RutasPusibles.Add(item);
            }
            foreach (var item in RutasPusibles)
            {
                if (NewMain.ElementAt(item, 1) != NewMain.ElementAt(filePath, 3))
                {
                    treeView1.Nodes.Add(item);
                }
            }
        }

        #endregion Methods

        #region Clicks

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var instance = MainViewModel.GetInstance().NewMain;
                string path = treeView1.SelectedNode.ToString();
                path = path.Replace("TreeNode: ", "");
                path = path + "\\incoming\\" + NewMain.ElementAt(filePath, 1);
                MainViewModel.GetInstance().NewMain.bussinessPath = rootPath + "\\" + NewMain.ElementAt(path, 3)+"\\";
                System.IO.File.Move(filePath, path);
                MessageBox.Show("File moved correctly");
                MainViewModel.GetInstance().NewMain.FullRefresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("There has been an error or business not selected:\n" + ex);
            }
            this.Dispose();
        }

        #endregion Clicks
    }
}