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
    public partial class FormSettingMain : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;

        FormManual FORM_MANUAL;
        FormOpticalSetting FORM_OPTICAL;
        FormWeaponSetting FORM_WEAPON;
        FormMotorSetting FORM_MOTOR;
        FormDataSetting FORM_DATA;

        FormReticleSetting FORM_RETICLE;
        FormOpticalDataSetting FORM_OPTICAL_DATA;
        FormWeaponDataSetting FORM_WEAPON_DATA;
        FormMotorDataSetting FORM_MOTOR_DATA;

        public FormSettingMain()
        {
            InitializeComponent();

            PNL_SIDE_BAR.Width = 52;

            PNL_OPTICAL_CONTAINER.Height = 60;
            PNL_WEAPON_CONTAINER.Height = 60;
            PNL_MOTOR_CONTAINER.Height = 60;
            PNL_OVERALL_CONTAINER.Height = 60;
            PNL_CLOSE_CONTAINER.Height = 60;
        }

        private void FormSettingMain_Load(object sender, EventArgs e)
        {
            
        }
        #region MANUAL
        private void BTN_MANUAL_Click(object sender, EventArgs e)
        {
            if(side_bar_flag)
            {
                if (FORM_MANUAL == null)
                {
                    FORM_MANUAL = new FormManual();
                    FORM_MANUAL.FormClosed += Setting_FormClosed;
                    FORM_MANUAL.MdiParent = this;
                    FORM_MANUAL.Dock = DockStyle.Fill;
                    FORM_MANUAL.Show();
                }
                else
                {
                    FORM_MANUAL.Activate();
                }
            }            
        }
        #endregion

        #region optical

        private void BTN_OPTICAL_Click(object sender, EventArgs e)
        {
            if(side_bar_flag)
            {
                opticalTransition.Start();
                if (FORM_OPTICAL == null)
                {
                    FORM_OPTICAL = new FormOpticalSetting();
                    FORM_OPTICAL.FormClosed += Setting_FormClosed;
                    FORM_OPTICAL.MdiParent = this;
                    FORM_OPTICAL.Dock = DockStyle.Fill;
                    FORM_OPTICAL.Show();
                }
                else
                {
                    FORM_OPTICAL.Activate();
                }
            }            
        }

        private void BTN_RETICLE_Click(object sender, EventArgs e)
        {            
            if (FORM_RETICLE == null)
            {
                FORM_RETICLE = new FormReticleSetting();
                FORM_RETICLE.FormClosed += Setting_FormClosed;
                FORM_RETICLE.MdiParent = this;
                FORM_RETICLE.Dock = DockStyle.Fill;
                FORM_RETICLE.Show();
            }
            else
            {
                FORM_RETICLE.Activate();
            }
        }

        private void BTN_OPTICLA_DATA_Click(object sender, EventArgs e)
        {
            if (FORM_OPTICAL_DATA == null)
            {
                FORM_OPTICAL_DATA = new FormOpticalDataSetting();
                FORM_OPTICAL_DATA.FormClosed += Setting_FormClosed;
                FORM_OPTICAL_DATA.MdiParent = this;
                FORM_OPTICAL_DATA.Dock = DockStyle.Fill;
                FORM_OPTICAL_DATA.Show();
            }
            else
            {
                FORM_OPTICAL_DATA.Activate();
            }
        }

        private void opticalTransition_Tick(object sender, EventArgs e)
        {
            if (optical_menu_Expand == false)
            {
                PNL_OPTICAL_CONTAINER.Height += 5;
                if (PNL_OPTICAL_CONTAINER.Height >= 180)
                {
                    opticalTransition.Stop();
                    optical_menu_Expand = true;
                }
            }
            else
            {
                PNL_OPTICAL_CONTAINER.Height -= 5;
                if (PNL_OPTICAL_CONTAINER.Height <= 60)
                {
                    opticalTransition.Stop();
                    optical_menu_Expand = false;
                }
            }
        }

        #endregion

        #region weapon
        private void BTN_WEAPON_Click(object sender, EventArgs e)
        {
            if(side_bar_flag)
            {
                weaponTransition.Start();
                if (FORM_WEAPON == null)
                {
                    FORM_WEAPON = new FormWeaponSetting();
                    FORM_WEAPON.FormClosed += Setting_FormClosed;
                    FORM_WEAPON.MdiParent = this;
                    FORM_WEAPON.Dock = DockStyle.Fill;
                    FORM_WEAPON.Show();
                }
                else
                {
                    FORM_WEAPON.Activate();
                }
            }            
        }

        private void BTN_WEAPON_DATA_Click(object sender, EventArgs e)
        {
            if (FORM_WEAPON_DATA == null)
            {
                FORM_WEAPON_DATA = new FormWeaponDataSetting();
                FORM_WEAPON_DATA.FormClosed += Setting_FormClosed;
                FORM_WEAPON_DATA.MdiParent = this;
                FORM_WEAPON_DATA.Dock = DockStyle.Fill;
                FORM_WEAPON_DATA.Show();
            }
            else
            {
                FORM_WEAPON_DATA.Activate();
            }
        }

        private void weaponTransition_Tick(object sender, EventArgs e)
        {
            if (weapon_menu_Expand == false)
            {
                PNL_WEAPON_CONTAINER.Height += 5;
                if (PNL_WEAPON_CONTAINER.Height >= 120)
                {
                    weaponTransition.Stop();
                    weapon_menu_Expand = true;
                }
            }
            else
            {
                PNL_WEAPON_CONTAINER.Height -= 5;
                if (PNL_WEAPON_CONTAINER.Height <= 60)
                {
                    weaponTransition.Stop();
                    weapon_menu_Expand = false;
                }
            }
        }

        #endregion

        #region motor
        private void BTN_MOTOR_Click(object sender, EventArgs e)
        {
            if(side_bar_flag)
            {
                motorTransition.Start();
                if (FORM_MOTOR == null)
                {
                    FORM_MOTOR = new FormMotorSetting();
                    FORM_MOTOR.FormClosed += Setting_FormClosed;
                    FORM_MOTOR.MdiParent = this;
                    FORM_MOTOR.Dock = DockStyle.Fill;
                    FORM_MOTOR.Show();
                }
                else
                {
                    FORM_MOTOR.Activate();
                }
            }            
        }

        private void BTN_MOTOR_DATA_Click(object sender, EventArgs e)
        {
            if (FORM_MOTOR_DATA == null)
            {
                FORM_MOTOR_DATA = new FormMotorDataSetting();
                FORM_MOTOR_DATA.FormClosed += Setting_FormClosed;
                FORM_MOTOR_DATA.MdiParent = this;
                FORM_MOTOR_DATA.Dock = DockStyle.Fill;
                FORM_MOTOR_DATA.Show();
            }
            else
            {
                FORM_MOTOR_DATA.Activate();
            }
        }

        private void motorTransition_Tick(object sender, EventArgs e)
        {
            if (motor_menu_Expand == false)
            {
                PNL_MOTOR_CONTAINER.Height += 5;
                if (PNL_MOTOR_CONTAINER.Height >= 120)
                {
                    motorTransition.Stop();
                    motor_menu_Expand = true;
                }
            }
            else
            {
                PNL_MOTOR_CONTAINER.Height -= 5;
                if (PNL_MOTOR_CONTAINER.Height <= 60)
                {
                    motorTransition.Stop();
                    motor_menu_Expand = false;
                }
            }
        }

        #endregion

        #region data
        private void BTN_DATA_Click(object sender, EventArgs e)
        {
            if(side_bar_flag)
            {
                if (FORM_DATA == null)
                {
                    FORM_DATA = new FormDataSetting();
                    FORM_DATA.FormClosed += Setting_FormClosed;
                    FORM_DATA.MdiParent = this;
                    FORM_DATA.Dock = DockStyle.Fill;
                    FORM_DATA.Show();
                }
                else
                {
                    FORM_DATA.Activate();
                }
            }            
        }
        #endregion

        private void BTN_CLOSE_Click(object sender, EventArgs e)
        {
            if(side_bar_flag)
            {
                Close();
            }            
        }

        bool sidebarExpand = false;
        private void sidebarTransition_Tick(object sender, EventArgs e)
        {
            if (sidebarExpand)
            {
                PNL_SIDE_BAR.Width -= 5;
                if (PNL_SIDE_BAR.Width <= 52)
                {
                    sidebarExpand = false;
                    sidebarTransition.Stop();
                }
            }
            else
            {
                PNL_SIDE_BAR.Width += 5;
                if (PNL_SIDE_BAR.Width >= 181)
                {
                    sidebarExpand = true;
                    sidebarTransition.Stop();
                }
            }
        }

        bool optical_menu_Expand = false;
        bool weapon_menu_Expand = false;
        bool motor_menu_Expand = false;
        bool side_bar_flag = false;
        private void BTN_MENU_Click(object sender, EventArgs e)
        {
            side_bar_flag = !side_bar_flag;
            sidebarTransition.Start();
        }

        private void Setting_FormClosed(object sender, FormClosedEventArgs e)
        {
            FORM_OPTICAL = null;
            FORM_MANUAL = null;
            FORM_OPTICAL = null;
            FORM_WEAPON = null;
            FORM_MOTOR = null;
            FORM_DATA = null;

            FORM_RETICLE = null;
            FORM_OPTICAL_DATA = null;
            FORM_WEAPON_DATA = null;
            FORM_MOTOR_DATA = null;
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