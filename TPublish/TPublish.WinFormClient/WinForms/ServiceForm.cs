using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TPublish.WinFormClient.WinForms
{
    public partial class ServiceForm : HZH_Controls.Forms.FrmWithOKCancel2
    {
        public ServiceForm()
        {
            InitializeComponent();

            List<KeyValuePair<string, string>> lstCom = new List<KeyValuePair<string, string>>();
            for (int i = 0; i < 5; i++)
            {
                lstCom.Add(new KeyValuePair<string, string>(i.ToString(), "IIS" + i));
            }

            ucCombox1.Source = lstCom;
            ucCombox2.Source = lstCom;
        }
    }
}
