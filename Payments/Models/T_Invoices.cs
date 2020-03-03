using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.Models
{
    class T_Invoices
    {
        #region Attributes
        private string id;
        private string fileName;
        private string folder;
        private string status_name;
        private string date_modified;
        private string transID;
        private double amount;
        #endregion

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public string Folder
        {
            get { return folder; }
            set { folder = value; }
        }

        public string Status
        {
            get { return status_name; }
            set { status_name = value; }
        }

        public string Date
        {
            get { return date_modified; }
            set { date_modified = value; }
        }

        public string TransId
        {
            get { return transID; }
            set { transID = value; }
        }

        public double Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        
    }
}
