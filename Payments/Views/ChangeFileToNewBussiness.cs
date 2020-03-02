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

        private readonly string fileDirectory;
        public readonly string newpath;
        private readonly List<string> RutasPusibles = new List<string>();

        #endregion Attributes

        #region Constructor

        public ChangeFileToNewBussiness(string path, string root)
        {
            InitializeComponent();
            fileDirectory = path;
            newpath = root;
            ScanForAvailableBussiness();
        }

        #endregion Constructor

        #region Methods

        private void ScanForAvailableBussiness()
        {
            string[] Bussiness = Directory.GetDirectories(newpath, "*.*", SearchOption.TopDirectoryOnly);
            foreach (var item in Bussiness)
            {
                RutasPusibles.Add(item);
            }
            foreach (var item in RutasPusibles)
            {
                if (NewMain.LastElement(item) != FileBussiness(fileDirectory))
                {
                    treeView1.Nodes.Add(item);
                }
            }
        }


        public string FileBussiness(string splitme)
        {
            string[] strlist = splitme.Split(new char[] { '\\'  },
                       20, StringSplitOptions.None);
            return strlist[strlist.Length - 3].ToString();
        }

        #endregion Methods

        #region Clicks

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string path = treeView1.SelectedNode.ToString();
                path = path.Replace("TreeNode: ", "");
                path = path + "\\incoming\\" + NewMain.LastElement(fileDirectory);
                System.IO.File.Move(fileDirectory, path);
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