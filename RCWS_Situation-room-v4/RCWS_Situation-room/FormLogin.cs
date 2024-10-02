using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace RCWS_Situation_room
{
    public partial class FormLogin : Form
    {
        FormDataSetting formDataSetting = new FormDataSetting();

        public FormLogin()
        {
            InitializeComponent();
            TB_PW.UseSystemPasswordChar = true;
            TB_PW.PasswordChar = '*';
            TB_ID.Focus();
        }

        StreamWriter streamWriter;

        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
            Application.Exit();
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            //this.Visible = false;
            //FormMain gui = new FormMain(streamWriter, formDataSetting);
            //gui.Show();

            if (TB_ID.Text == "까불면 메카 약이다" && TB_PW.Text == "mecha")
            {
                MessageBox.Show("Successfully Login");
                this.Visible = false;
                FormMain gui = new FormMain(streamWriter, formDataSetting);
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
            {
                MessageBox.Show("Invalid User Name or Password");
            }
        }

        private void TB_ID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TB_PW.Focus();
            }              
        }

        private void TB_PW_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter && TB_ID.Text=="까불면 메카 약이다" && TB_PW.Text == "mecha")
            {
                MessageBox.Show("Successfully Login");
                this.Visible = false;
                FormMain gui = new FormMain(streamWriter, formDataSetting);
                gui.Show();
            }
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            TB_ID.Text = "까불면 메카 약이다";
            TB_PW.Text = "mecha";
        }
    }
}