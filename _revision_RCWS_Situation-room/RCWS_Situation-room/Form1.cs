using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RCWS_Situation_room
{
    public partial class Form1 : Form
    {
        static Process RCWSCam;
        FormDataSetting formDataSetting = new FormDataSetting();
        public Form1()
        {
            InitializeComponent();
        }

        StreamWriter streamWriter;

        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
            Application.Exit();
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            if (TB_ID.Text == "까불면메카약이다" && TB_PW.Text == "메카트로닉스")
            {
                MessageBox.Show("Successfully Login");
                this.Visible = false;
                GUI gui = new GUI(streamWriter, formDataSetting);
                gui.Show();
            }

            else if (TB_ID.Text == "")
            {
                MessageBox.Show("Invalid User Name");
                TB_ID.Focus();
                return;
            }

            else if (TB_PW.Text == "")
            {
                MessageBox.Show("Invalid Password");
                TB_PW.Focus();
                return;
            }

            else
                MessageBox.Show("Invalid User Name or Password");
        }

        private void TB_ID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                TB_PW.Focus();
        }

        private void TB_PW_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter && TB_ID.Text=="까불면 메카 약이다" && TB_PW.Text == "mecha")
            {
                MessageBox.Show("Successfully Login");
                this.Visible = false;
                GUI gui = new GUI(streamWriter, formDataSetting);
                gui.Show();
            }
        }
    }
}