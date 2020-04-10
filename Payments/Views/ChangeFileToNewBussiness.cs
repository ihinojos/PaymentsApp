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
            string[] Bussiness = Directory.GetDirectories(MainViewModel.GetInstance().NewMain.userDic + "\\" + rootPath, "*.*", SearchOption.TopDirectoryOnly);
            foreach (var item in Bussiness)
            {
                RutasPusibles.Add(item);
            }
            foreach (var item in RutasPusibles)
            {
                if (NewMain.ElementAt(item, 1) != NewMain.ElementAt(filePath, 3))
                {
                    treeView1.Nodes.Add(item.Replace(MainViewModel.GetInstance().NewMain.userDic + "\\", ""));
                }
            }
        }

        #endregion Methods

        #region Clicks

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                var instance = MainViewModel.GetInstance().NewMain;
                string path = treeView1.SelectedNode.ToString();
                path = path.Replace("TreeNode: ", "");
                path += "\\incoming\\" + NewMain.ElementAt(filePath, 1);
                instance.bussinessPath = rootPath + "\\" + NewMain.ElementAt(path, 3) + "\\";
                File.Move(instance.userDic + "\\" + filePath, instance.userDic + "\\" + path);

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