using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.Models
{
    class T_Status
    {
        #region Attributes
        string id;
        string status;
        string id_file;
        #endregion

        #region Properties
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        public string Id_file
        {
            get { return id_file; }
            set { id_file = value; }
        }
        #endregion

    }
}
