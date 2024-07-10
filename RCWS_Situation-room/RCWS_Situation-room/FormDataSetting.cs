using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RCWS_Situation_room
{
    public partial class FormDataSetting : Form
    {
        public FormDataSetting()
        {
            InitializeComponent();
        }

        private void FormDataSetting_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
        }

        public void DisplayReceivedData(string data)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(DisplayReceivedData), new object[] { data });
                rtb_receivetcp.AppendText(data + "\r\n");
            }
            else
            {
                rtb_receivetcp.AppendText(data + "\r\n");
            }
        }
    }
}