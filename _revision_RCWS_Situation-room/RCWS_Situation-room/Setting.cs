using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace RCWS_Situation_room
{
    public partial class Setting : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;

        FormOpticalSetting opticalSettingboard;
        FormWeaponSetting weaponSettingboard;
        FormMotorSetting motorSettingboard;
        FormDataSetting dataSettingboard;

        FormReticleSetting reticleSettingboard;
        FormOpticalDataSetting opticaldataSettingboard;
        FormWeaponDataSetting weapondataSettingboard;
        FormMotorDataSetting motordataSettingboard;

        public Setting()
        {
            InitializeComponent();

            sidebar.Width = 52;

            pnl_optical_setting_container.Height = 60;
            pnl_weapon_setting_container.Height = 60;
            pnl_motor_setting_container.Height = 60;
            pnl_overalldata_container.Height = 60;
            pnl_exit_container.Height = 60;
        }        

        private void btn_optical_setting_Click(object sender, EventArgs e)
        {
            opticalTransition.Start();
            if(opticalSettingboard ==  null)
            {
                opticalSettingboard = new FormOpticalSetting();
                opticalSettingboard.FormClosed += Setting_FormClosed;
                opticalSettingboard.MdiParent = this;
                opticalSettingboard.Dock = DockStyle.Fill;
                opticalSettingboard.Show();
            }
            else
            {
                opticalSettingboard.Activate();
            }
        }

        private void btn_reticle_setting_Click(object sender, EventArgs e)
        {
            if (reticleSettingboard == null)
            {
                reticleSettingboard = new FormReticleSetting();
                reticleSettingboard.FormClosed += Setting_FormClosed;
                reticleSettingboard.MdiParent = this;
                reticleSettingboard.Dock = DockStyle.Fill;
                reticleSettingboard.Show();
            }
            else
            {
                reticleSettingboard.Activate();
            }
        }

        private void btn_optical_data_setting_Click(object sender, EventArgs e)
        {
            if (opticaldataSettingboard == null)
            {
                opticaldataSettingboard = new FormOpticalDataSetting();
                opticaldataSettingboard.FormClosed += Setting_FormClosed;
                opticaldataSettingboard.MdiParent = this;
                opticaldataSettingboard.Dock = DockStyle.Fill;
                opticaldataSettingboard.Show();
            }
            else
            {
                opticaldataSettingboard.Activate();
            }
        }

        private void btn_weapon_setting_Click(object sender, EventArgs e)
        {
            weaponTransition.Start();
            if (weaponSettingboard == null)
            {
                weaponSettingboard = new FormWeaponSetting();
                weaponSettingboard.FormClosed += Setting_FormClosed;
                weaponSettingboard.MdiParent = this;
                weaponSettingboard.Dock = DockStyle.Fill;
                weaponSettingboard.Show();
            }
            else
            {
                weaponSettingboard.Activate();
            }
        }

        private void btn_weapon_data_setting_Click(object sender, EventArgs e)
        {
            if (weapondataSettingboard == null)
            {
                weapondataSettingboard = new FormWeaponDataSetting();
                weapondataSettingboard.FormClosed += Setting_FormClosed;
                weapondataSettingboard.MdiParent = this;
                weapondataSettingboard.Dock = DockStyle.Fill;
                weapondataSettingboard.Show();
            }
            else
            {
                weapondataSettingboard.Activate();
            }
        }

        private void btn_motor_setting_Click(object sender, EventArgs e)
        {
            motorTransition.Start();
            if (motorSettingboard == null)
            {
                motorSettingboard = new FormMotorSetting();
                motorSettingboard.FormClosed += Setting_FormClosed;
                motorSettingboard.MdiParent = this;
                motorSettingboard.Dock = DockStyle.Fill;
                motorSettingboard.Show();
            }
            else
            {
                motorSettingboard.Activate();
            }
        }

        private void btn_motor_data_setting_Click(object sender, EventArgs e)
        {
            if (motordataSettingboard == null)
            {
                motordataSettingboard = new FormMotorDataSetting();
                motordataSettingboard.FormClosed += Setting_FormClosed;
                motordataSettingboard.MdiParent = this;
                motordataSettingboard.Dock = DockStyle.Fill;
                motordataSettingboard.Show();
            }
            else
            {
                motordataSettingboard.Activate();
            }
        }

        private void btn_overall_data_setting_Click(object sender, EventArgs e)
        {
            if (dataSettingboard == null)
            {
                dataSettingboard = new FormDataSetting();
                dataSettingboard.FormClosed += Setting_FormClosed;
                dataSettingboard.MdiParent = this;
                dataSettingboard.Dock = DockStyle.Fill;
                dataSettingboard.Show();
            }
            else
            {
                dataSettingboard.Activate();
            }
        }

        private void btn_logout_Click(object sender, EventArgs e)
        {
            Close();
        }

        bool sidebarExpand = false;
        private void sidebarTransition_Tick(object sender, EventArgs e)
        {
            if (sidebarExpand)
            {
                sidebar.Width -= 5;
                if (sidebar.Width <= 52)
                {
                    sidebarExpand = false;
                    sidebarTransition.Stop();

                    pnl_optical_setting_container.Width = sidebar.Width;
                    pnl_weapon_setting_container.Width = sidebar.Width;
                    pnl_motor_setting_container.Width = sidebar.Width;
                    pnl_overalldata_container.Width = sidebar.Width;
                    pnl_exit_container.Width = sidebar.Width;                    
                }
            }
            else
            {
                sidebar.Width += 5;
                if (sidebar.Width >= 181)
                {
                    sidebarExpand = true;
                    sidebarTransition.Stop();

                    pnl_optical_setting_container.Width = sidebar.Width;
                    pnl_weapon_setting_container.Width = sidebar.Width;
                    pnl_motor_setting_container.Width = sidebar.Width;
                    pnl_overalldata_container.Width = sidebar.Width;
                    pnl_exit_container.Width = sidebar.Width;
                }
            }
        }

        bool optical_menu_Expand = false;
        private void opticalTransition_Tick(object sender, EventArgs e)
        {
            if (optical_menu_Expand == false)
            {
                pnl_optical_setting_container.Height += 5;
                if (pnl_optical_setting_container.Height >= 180)
                {
                    opticalTransition.Stop();
                    optical_menu_Expand = true;
                }
            }
            else
            {
                pnl_optical_setting_container.Height -= 5;
                if (pnl_optical_setting_container.Height <= 60)
                {
                    opticalTransition.Stop();
                    optical_menu_Expand = false;
                }
            }
        }

        bool weapon_menu_Expand = false;
        private void weaponTransition_Tick(object sender, EventArgs e)
        {
            if (weapon_menu_Expand == false)
            {
                pnl_weapon_setting_container.Height += 5;
                if (pnl_weapon_setting_container.Height >= 120)
                {
                    weaponTransition.Stop();
                    weapon_menu_Expand = true;
                }
            }
            else
            {
                pnl_weapon_setting_container.Height -= 5;
                if (pnl_weapon_setting_container.Height <= 60)
                {
                    weaponTransition.Stop();
                    weapon_menu_Expand = false;
                }
            }
        }

        bool motor_menu_Expand = false;
        private void motorTransition_Tick(object sender, EventArgs e)
        {
            if (motor_menu_Expand == false)
            {
                pnl_motor_setting_container.Height += 5;
                if (pnl_motor_setting_container.Height >= 120)
                {
                    motorTransition.Stop();
                    motor_menu_Expand = true;
                }
            }
            else
            {
                pnl_motor_setting_container.Height -= 5;
                if (pnl_motor_setting_container.Height <= 60)
                {
                    motorTransition.Stop();
                    motor_menu_Expand = false;
                }
            }
        }

        private void btn_menu_Click(object sender, EventArgs e)
        {
            sidebarTransition.Start();
        }

        private void pb_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Setting_FormClosed(object sender, FormClosedEventArgs e)
        {
            opticalSettingboard = null;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }        
    }
}
