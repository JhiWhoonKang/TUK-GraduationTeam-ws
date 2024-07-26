namespace RCWS_Situation_room
{
    partial class FormSettingMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Button BTN_OPTICLA_DATA;
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.BTN_MENU = new System.Windows.Forms.PictureBox();
            this.PNL_SIDE_BAR = new System.Windows.Forms.FlowLayoutPanel();
            this.PNL_MANUAL_CONTAINER = new System.Windows.Forms.Panel();
            this.BTN_MANUAL = new System.Windows.Forms.Button();
            this.PNL_OPTICAL_CONTAINER = new System.Windows.Forms.Panel();
            this.BTN_OPTICAL = new System.Windows.Forms.Button();
            this.BTN_RETICLE = new System.Windows.Forms.Button();
            this.PNL_WEAPON_CONTAINER = new System.Windows.Forms.Panel();
            this.BTN_WEAPON = new System.Windows.Forms.Button();
            this.BTN_WEAPON_DATA = new System.Windows.Forms.Button();
            this.PNL_MOTOR_CONTAINER = new System.Windows.Forms.Panel();
            this.BTN_MOTOR_DATA = new System.Windows.Forms.Button();
            this.BTN_MOTOR = new System.Windows.Forms.Button();
            this.PNL_OVERALL_CONTAINER = new System.Windows.Forms.Panel();
            this.BTN_DATA = new System.Windows.Forms.Button();
            this.PNL_CLOSE_CONTAINER = new System.Windows.Forms.Panel();
            this.BTN_CLOSE = new System.Windows.Forms.Button();
            this.opticalTransition = new System.Windows.Forms.Timer(this.components);
            this.sidebarTransition = new System.Windows.Forms.Timer(this.components);
            this.weaponTransition = new System.Windows.Forms.Timer(this.components);
            this.motorTransition = new System.Windows.Forms.Timer(this.components);
            BTN_OPTICLA_DATA = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BTN_MENU)).BeginInit();
            this.PNL_SIDE_BAR.SuspendLayout();
            this.PNL_MANUAL_CONTAINER.SuspendLayout();
            this.PNL_OPTICAL_CONTAINER.SuspendLayout();
            this.PNL_WEAPON_CONTAINER.SuspendLayout();
            this.PNL_MOTOR_CONTAINER.SuspendLayout();
            this.PNL_OVERALL_CONTAINER.SuspendLayout();
            this.PNL_CLOSE_CONTAINER.SuspendLayout();
            this.SuspendLayout();
            // 
            // BTN_OPTICLA_DATA
            // 
            BTN_OPTICLA_DATA.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            BTN_OPTICLA_DATA.Dock = System.Windows.Forms.DockStyle.Bottom;
            BTN_OPTICLA_DATA.FlatAppearance.BorderSize = 0;
            BTN_OPTICLA_DATA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            BTN_OPTICLA_DATA.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            BTN_OPTICLA_DATA.ForeColor = System.Drawing.Color.White;
            BTN_OPTICLA_DATA.Location = new System.Drawing.Point(0, 122);
            BTN_OPTICLA_DATA.Margin = new System.Windows.Forms.Padding(0);
            BTN_OPTICLA_DATA.Name = "BTN_OPTICLA_DATA";
            BTN_OPTICLA_DATA.Size = new System.Drawing.Size(181, 60);
            BTN_OPTICLA_DATA.TabIndex = 6;
            BTN_OPTICLA_DATA.Text = "   Optical Data";
            BTN_OPTICLA_DATA.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            BTN_OPTICLA_DATA.UseVisualStyleBackColor = false;
            BTN_OPTICLA_DATA.Click += new System.EventHandler(this.BTN_OPTICLA_DATA_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.BTN_MENU);
            this.panel1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1000, 50);
            this.panel1.TabIndex = 0;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Emoji", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(44, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(244, 26);
            this.label1.TabIndex = 2;
            this.label1.Text = "RCWS | CONFIGURATION";
            // 
            // BTN_MENU
            // 
            this.BTN_MENU.Image = global::RCWS_Situation_room.Properties.Resources.list_icon;
            this.BTN_MENU.Location = new System.Drawing.Point(12, 12);
            this.BTN_MENU.Name = "BTN_MENU";
            this.BTN_MENU.Size = new System.Drawing.Size(26, 26);
            this.BTN_MENU.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.BTN_MENU.TabIndex = 2;
            this.BTN_MENU.TabStop = false;
            this.BTN_MENU.Click += new System.EventHandler(this.BTN_MENU_Click);
            // 
            // PNL_SIDE_BAR
            // 
            this.PNL_SIDE_BAR.BackColor = System.Drawing.SystemColors.ControlDark;
            this.PNL_SIDE_BAR.Controls.Add(this.PNL_MANUAL_CONTAINER);
            this.PNL_SIDE_BAR.Controls.Add(this.PNL_OPTICAL_CONTAINER);
            this.PNL_SIDE_BAR.Controls.Add(this.PNL_WEAPON_CONTAINER);
            this.PNL_SIDE_BAR.Controls.Add(this.PNL_MOTOR_CONTAINER);
            this.PNL_SIDE_BAR.Controls.Add(this.PNL_OVERALL_CONTAINER);
            this.PNL_SIDE_BAR.Controls.Add(this.PNL_CLOSE_CONTAINER);
            this.PNL_SIDE_BAR.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.PNL_SIDE_BAR.Dock = System.Windows.Forms.DockStyle.Left;
            this.PNL_SIDE_BAR.Location = new System.Drawing.Point(0, 50);
            this.PNL_SIDE_BAR.Name = "PNL_SIDE_BAR";
            this.PNL_SIDE_BAR.Size = new System.Drawing.Size(339, 732);
            this.PNL_SIDE_BAR.TabIndex = 2;
            // 
            // PNL_MANUAL_CONTAINER
            // 
            this.PNL_MANUAL_CONTAINER.Controls.Add(this.BTN_MANUAL);
            this.PNL_MANUAL_CONTAINER.Dock = System.Windows.Forms.DockStyle.Top;
            this.PNL_MANUAL_CONTAINER.Location = new System.Drawing.Point(0, 0);
            this.PNL_MANUAL_CONTAINER.Margin = new System.Windows.Forms.Padding(0);
            this.PNL_MANUAL_CONTAINER.Name = "PNL_MANUAL_CONTAINER";
            this.PNL_MANUAL_CONTAINER.Size = new System.Drawing.Size(181, 60);
            this.PNL_MANUAL_CONTAINER.TabIndex = 4;
            // 
            // BTN_MANUAL
            // 
            this.BTN_MANUAL.BackColor = System.Drawing.SystemColors.ControlDark;
            this.BTN_MANUAL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BTN_MANUAL.FlatAppearance.BorderSize = 0;
            this.BTN_MANUAL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_MANUAL.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_MANUAL.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BTN_MANUAL.Location = new System.Drawing.Point(0, 0);
            this.BTN_MANUAL.Margin = new System.Windows.Forms.Padding(0);
            this.BTN_MANUAL.Name = "BTN_MANUAL";
            this.BTN_MANUAL.Size = new System.Drawing.Size(181, 60);
            this.BTN_MANUAL.TabIndex = 23;
            this.BTN_MANUAL.Text = "   Manual";
            this.BTN_MANUAL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BTN_MANUAL.UseVisualStyleBackColor = false;
            this.BTN_MANUAL.Click += new System.EventHandler(this.BTN_MANUAL_Click);
            // 
            // PNL_OPTICAL_CONTAINER
            // 
            this.PNL_OPTICAL_CONTAINER.Controls.Add(this.BTN_OPTICAL);
            this.PNL_OPTICAL_CONTAINER.Controls.Add(this.BTN_RETICLE);
            this.PNL_OPTICAL_CONTAINER.Controls.Add(BTN_OPTICLA_DATA);
            this.PNL_OPTICAL_CONTAINER.Location = new System.Drawing.Point(0, 60);
            this.PNL_OPTICAL_CONTAINER.Margin = new System.Windows.Forms.Padding(0);
            this.PNL_OPTICAL_CONTAINER.Name = "PNL_OPTICAL_CONTAINER";
            this.PNL_OPTICAL_CONTAINER.Size = new System.Drawing.Size(181, 182);
            this.PNL_OPTICAL_CONTAINER.TabIndex = 5;
            // 
            // BTN_OPTICAL
            // 
            this.BTN_OPTICAL.BackColor = System.Drawing.SystemColors.ControlDark;
            this.BTN_OPTICAL.Dock = System.Windows.Forms.DockStyle.Top;
            this.BTN_OPTICAL.FlatAppearance.BorderSize = 0;
            this.BTN_OPTICAL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_OPTICAL.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_OPTICAL.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BTN_OPTICAL.Location = new System.Drawing.Point(0, 0);
            this.BTN_OPTICAL.Margin = new System.Windows.Forms.Padding(0);
            this.BTN_OPTICAL.Name = "BTN_OPTICAL";
            this.BTN_OPTICAL.Size = new System.Drawing.Size(181, 60);
            this.BTN_OPTICAL.TabIndex = 4;
            this.BTN_OPTICAL.Text = "   Optical";
            this.BTN_OPTICAL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BTN_OPTICAL.UseVisualStyleBackColor = false;
            this.BTN_OPTICAL.Click += new System.EventHandler(this.BTN_OPTICAL_Click);
            // 
            // BTN_RETICLE
            // 
            this.BTN_RETICLE.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.BTN_RETICLE.FlatAppearance.BorderSize = 0;
            this.BTN_RETICLE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_RETICLE.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_RETICLE.ForeColor = System.Drawing.Color.White;
            this.BTN_RETICLE.Location = new System.Drawing.Point(0, 60);
            this.BTN_RETICLE.Margin = new System.Windows.Forms.Padding(0);
            this.BTN_RETICLE.Name = "BTN_RETICLE";
            this.BTN_RETICLE.Size = new System.Drawing.Size(181, 60);
            this.BTN_RETICLE.TabIndex = 5;
            this.BTN_RETICLE.Text = "   Reticle";
            this.BTN_RETICLE.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BTN_RETICLE.UseVisualStyleBackColor = false;
            this.BTN_RETICLE.Click += new System.EventHandler(this.BTN_RETICLE_Click);
            // 
            // PNL_WEAPON_CONTAINER
            // 
            this.PNL_WEAPON_CONTAINER.Controls.Add(this.BTN_WEAPON);
            this.PNL_WEAPON_CONTAINER.Controls.Add(this.BTN_WEAPON_DATA);
            this.PNL_WEAPON_CONTAINER.Location = new System.Drawing.Point(0, 242);
            this.PNL_WEAPON_CONTAINER.Margin = new System.Windows.Forms.Padding(0);
            this.PNL_WEAPON_CONTAINER.Name = "PNL_WEAPON_CONTAINER";
            this.PNL_WEAPON_CONTAINER.Size = new System.Drawing.Size(181, 120);
            this.PNL_WEAPON_CONTAINER.TabIndex = 3;
            // 
            // BTN_WEAPON
            // 
            this.BTN_WEAPON.BackColor = System.Drawing.SystemColors.ControlDark;
            this.BTN_WEAPON.Dock = System.Windows.Forms.DockStyle.Top;
            this.BTN_WEAPON.FlatAppearance.BorderSize = 0;
            this.BTN_WEAPON.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_WEAPON.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_WEAPON.Location = new System.Drawing.Point(0, 0);
            this.BTN_WEAPON.Margin = new System.Windows.Forms.Padding(0);
            this.BTN_WEAPON.Name = "BTN_WEAPON";
            this.BTN_WEAPON.Size = new System.Drawing.Size(181, 60);
            this.BTN_WEAPON.TabIndex = 4;
            this.BTN_WEAPON.Text = "   Weapon";
            this.BTN_WEAPON.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BTN_WEAPON.UseVisualStyleBackColor = false;
            this.BTN_WEAPON.Click += new System.EventHandler(this.BTN_WEAPON_Click);
            // 
            // BTN_WEAPON_DATA
            // 
            this.BTN_WEAPON_DATA.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.BTN_WEAPON_DATA.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BTN_WEAPON_DATA.FlatAppearance.BorderSize = 0;
            this.BTN_WEAPON_DATA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_WEAPON_DATA.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_WEAPON_DATA.ForeColor = System.Drawing.Color.White;
            this.BTN_WEAPON_DATA.Location = new System.Drawing.Point(0, 60);
            this.BTN_WEAPON_DATA.Margin = new System.Windows.Forms.Padding(0);
            this.BTN_WEAPON_DATA.Name = "BTN_WEAPON_DATA";
            this.BTN_WEAPON_DATA.Size = new System.Drawing.Size(181, 60);
            this.BTN_WEAPON_DATA.TabIndex = 5;
            this.BTN_WEAPON_DATA.Text = "   Weapon Data";
            this.BTN_WEAPON_DATA.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BTN_WEAPON_DATA.UseVisualStyleBackColor = false;
            this.BTN_WEAPON_DATA.Click += new System.EventHandler(this.BTN_WEAPON_DATA_Click);
            // 
            // PNL_MOTOR_CONTAINER
            // 
            this.PNL_MOTOR_CONTAINER.Controls.Add(this.BTN_MOTOR_DATA);
            this.PNL_MOTOR_CONTAINER.Controls.Add(this.BTN_MOTOR);
            this.PNL_MOTOR_CONTAINER.Location = new System.Drawing.Point(0, 362);
            this.PNL_MOTOR_CONTAINER.Margin = new System.Windows.Forms.Padding(0);
            this.PNL_MOTOR_CONTAINER.Name = "PNL_MOTOR_CONTAINER";
            this.PNL_MOTOR_CONTAINER.Size = new System.Drawing.Size(181, 120);
            this.PNL_MOTOR_CONTAINER.TabIndex = 10;
            // 
            // BTN_MOTOR_DATA
            // 
            this.BTN_MOTOR_DATA.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.BTN_MOTOR_DATA.Dock = System.Windows.Forms.DockStyle.Top;
            this.BTN_MOTOR_DATA.FlatAppearance.BorderSize = 0;
            this.BTN_MOTOR_DATA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_MOTOR_DATA.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_MOTOR_DATA.ForeColor = System.Drawing.Color.White;
            this.BTN_MOTOR_DATA.Location = new System.Drawing.Point(0, 60);
            this.BTN_MOTOR_DATA.Margin = new System.Windows.Forms.Padding(0);
            this.BTN_MOTOR_DATA.Name = "BTN_MOTOR_DATA";
            this.BTN_MOTOR_DATA.Size = new System.Drawing.Size(181, 60);
            this.BTN_MOTOR_DATA.TabIndex = 16;
            this.BTN_MOTOR_DATA.Text = "   Motor Data";
            this.BTN_MOTOR_DATA.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BTN_MOTOR_DATA.UseVisualStyleBackColor = false;
            this.BTN_MOTOR_DATA.Click += new System.EventHandler(this.BTN_MOTOR_DATA_Click);
            // 
            // BTN_MOTOR
            // 
            this.BTN_MOTOR.BackColor = System.Drawing.SystemColors.ControlDark;
            this.BTN_MOTOR.Dock = System.Windows.Forms.DockStyle.Top;
            this.BTN_MOTOR.FlatAppearance.BorderSize = 0;
            this.BTN_MOTOR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_MOTOR.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_MOTOR.Location = new System.Drawing.Point(0, 0);
            this.BTN_MOTOR.Margin = new System.Windows.Forms.Padding(0);
            this.BTN_MOTOR.Name = "BTN_MOTOR";
            this.BTN_MOTOR.Size = new System.Drawing.Size(181, 60);
            this.BTN_MOTOR.TabIndex = 15;
            this.BTN_MOTOR.Text = "   Motor";
            this.BTN_MOTOR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BTN_MOTOR.UseVisualStyleBackColor = false;
            this.BTN_MOTOR.Click += new System.EventHandler(this.BTN_MOTOR_Click);
            // 
            // PNL_OVERALL_CONTAINER
            // 
            this.PNL_OVERALL_CONTAINER.Controls.Add(this.BTN_DATA);
            this.PNL_OVERALL_CONTAINER.Location = new System.Drawing.Point(0, 482);
            this.PNL_OVERALL_CONTAINER.Margin = new System.Windows.Forms.Padding(0);
            this.PNL_OVERALL_CONTAINER.Name = "PNL_OVERALL_CONTAINER";
            this.PNL_OVERALL_CONTAINER.Size = new System.Drawing.Size(181, 60);
            this.PNL_OVERALL_CONTAINER.TabIndex = 3;
            // 
            // BTN_DATA
            // 
            this.BTN_DATA.BackColor = System.Drawing.SystemColors.ControlDark;
            this.BTN_DATA.Dock = System.Windows.Forms.DockStyle.Top;
            this.BTN_DATA.FlatAppearance.BorderSize = 0;
            this.BTN_DATA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_DATA.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_DATA.Location = new System.Drawing.Point(0, 0);
            this.BTN_DATA.Margin = new System.Windows.Forms.Padding(0);
            this.BTN_DATA.Name = "BTN_DATA";
            this.BTN_DATA.Size = new System.Drawing.Size(181, 60);
            this.BTN_DATA.TabIndex = 17;
            this.BTN_DATA.Text = "   Overall Data";
            this.BTN_DATA.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BTN_DATA.UseVisualStyleBackColor = false;
            this.BTN_DATA.Click += new System.EventHandler(this.BTN_DATA_Click);
            // 
            // PNL_CLOSE_CONTAINER
            // 
            this.PNL_CLOSE_CONTAINER.Controls.Add(this.BTN_CLOSE);
            this.PNL_CLOSE_CONTAINER.Location = new System.Drawing.Point(0, 542);
            this.PNL_CLOSE_CONTAINER.Margin = new System.Windows.Forms.Padding(0);
            this.PNL_CLOSE_CONTAINER.Name = "PNL_CLOSE_CONTAINER";
            this.PNL_CLOSE_CONTAINER.Size = new System.Drawing.Size(181, 60);
            this.PNL_CLOSE_CONTAINER.TabIndex = 22;
            // 
            // BTN_CLOSE
            // 
            this.BTN_CLOSE.BackColor = System.Drawing.SystemColors.ControlDark;
            this.BTN_CLOSE.Dock = System.Windows.Forms.DockStyle.Top;
            this.BTN_CLOSE.FlatAppearance.BorderSize = 0;
            this.BTN_CLOSE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_CLOSE.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_CLOSE.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BTN_CLOSE.Location = new System.Drawing.Point(0, 0);
            this.BTN_CLOSE.Margin = new System.Windows.Forms.Padding(0);
            this.BTN_CLOSE.Name = "BTN_CLOSE";
            this.BTN_CLOSE.Size = new System.Drawing.Size(181, 60);
            this.BTN_CLOSE.TabIndex = 5;
            this.BTN_CLOSE.Text = "   Close";
            this.BTN_CLOSE.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BTN_CLOSE.UseVisualStyleBackColor = false;
            this.BTN_CLOSE.Click += new System.EventHandler(this.BTN_CLOSE_Click);
            // 
            // opticalTransition
            // 
            this.opticalTransition.Interval = 10;
            this.opticalTransition.Tick += new System.EventHandler(this.opticalTransition_Tick);
            // 
            // sidebarTransition
            // 
            this.sidebarTransition.Interval = 10;
            this.sidebarTransition.Tick += new System.EventHandler(this.sidebarTransition_Tick);
            // 
            // weaponTransition
            // 
            this.weaponTransition.Interval = 10;
            this.weaponTransition.Tick += new System.EventHandler(this.weaponTransition_Tick);
            // 
            // motorTransition
            // 
            this.motorTransition.Interval = 10;
            this.motorTransition.Tick += new System.EventHandler(this.motorTransition_Tick);
            // 
            // FormSettingMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1000, 782);
            this.Controls.Add(this.PNL_SIDE_BAR);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.IsMdiContainer = true;
            this.Name = "FormSettingMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Setting";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Setting_FormClosed);
            this.Load += new System.EventHandler(this.FormSettingMain_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BTN_MENU)).EndInit();
            this.PNL_SIDE_BAR.ResumeLayout(false);
            this.PNL_MANUAL_CONTAINER.ResumeLayout(false);
            this.PNL_OPTICAL_CONTAINER.ResumeLayout(false);
            this.PNL_WEAPON_CONTAINER.ResumeLayout(false);
            this.PNL_MOTOR_CONTAINER.ResumeLayout(false);
            this.PNL_OVERALL_CONTAINER.ResumeLayout(false);
            this.PNL_CLOSE_CONTAINER.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox BTN_MENU;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel PNL_SIDE_BAR;
        private System.Windows.Forms.Panel PNL_OVERALL_CONTAINER;
        private System.Windows.Forms.Button BTN_OPTICAL;
        private System.Windows.Forms.Panel PNL_OPTICAL_CONTAINER;
        private System.Windows.Forms.Button BTN_CLOSE;
        private System.Windows.Forms.Button BTN_RETICLE;
        private System.Windows.Forms.Timer opticalTransition;
        private System.Windows.Forms.Timer sidebarTransition;
        private System.Windows.Forms.Panel PNL_WEAPON_CONTAINER;
        private System.Windows.Forms.Button BTN_WEAPON;
        private System.Windows.Forms.Button BTN_WEAPON_DATA;
        private System.Windows.Forms.Panel PNL_MOTOR_CONTAINER;
        private System.Windows.Forms.Button BTN_MOTOR;
        private System.Windows.Forms.Button BTN_MOTOR_DATA;
        private System.Windows.Forms.Button BTN_DATA;
        private System.Windows.Forms.Panel PNL_CLOSE_CONTAINER;
        private System.Windows.Forms.Timer weaponTransition;
        private System.Windows.Forms.Timer motorTransition;
        private System.Windows.Forms.Button BTN_MANUAL;
        private System.Windows.Forms.Panel PNL_MANUAL_CONTAINER;
    }
}