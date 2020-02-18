using Payments.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Payments.Views
{
    public partial class ChangeFileToNewBussiness : Form
    {
        private string oldDirectory;
        public String newpath = "C:\\TestFiles";
        private int count;
        private List<string> RutasPusibles = new List<string>();

        public ChangeFileToNewBussiness(string path)
        {
            InitializeComponent();
            oldDirectory = path;
            count = 0;
            ScanForAvailableBussiness();
        }

        private void ScanForAvailableBussiness()
        {
            string[] Bussiness = Directory.GetDirectories(newpath, "*.*", SearchOption.TopDirectoryOnly);
            foreach (var item in Bussiness)
            {
                count = count + 1;
                RutasPusibles.Add(item);
            }
            foreach (var item in RutasPusibles)
            {
                if (lastElement(item) != secondlastElement(oldDirectory))
                {
                    treeView1.Nodes.Add(item);
                }
            }
        }

        public string lastElement(string splitme)
        {
            string[] strlist = splitme.Split(new char[] { '\\' },
                       20, StringSplitOptions.None);
            return strlist[strlist.Length - 1].ToString();
        }

        public string secondlastElement(string splitme)
        {
            string[] strlist = splitme.Split(new char[] { '\\' },
                       20, StringSplitOptions.None);
            return strlist[strlist.Length - 3].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            
            try
            {
                string path = treeView1.SelectedNode.ToString();
                path = path.Replace("TreeNode: ", "");
                path = path + "\\incoming\\" + lastElement(oldDirectory);
                System.IO.File.Move(oldDirectory, path);
                MessageBox.Show("File moved correctly");
                MainViewModel.GetInstance().newmain.fullRefresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("There has been an error or business not selected:\n" + ex);
            }
            this.Dispose();
        }
    }
}