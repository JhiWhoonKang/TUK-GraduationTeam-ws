namespace RCWS_Situation_room
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.PB_MAP = new System.Windows.Forms.PictureBox();
            this.PNL_MAP_CONTAINER = new System.Windows.Forms.Panel();
            this.RTB_SEND_DISPLAY = new System.Windows.Forms.RichTextBox();
            this.RTB_RECEIVED_DISPLAY = new System.Windows.Forms.RichTextBox();
            this.BTN_RCWS_CONNECT = new System.Windows.Forms.Button();
            this.PB_AZIMUTH = new System.Windows.Forms.PictureBox();
            this.TB_RCWS_AZIMUTH = new System.Windows.Forms.TextBox();
            this.TB_WEAPON_ELEVATION = new System.Windows.Forms.TextBox();
            this.LB_RCWS_AZ = new System.Windows.Forms.Label();
            this.LB_WEAPON_EL = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.LB_OPTICAL_EL = new System.Windows.Forms.Label();
            this.TB_OPTICAL_ELEVATION = new System.Windows.Forms.TextBox();
            this.BTN_PERMISSION = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.GB_OPTICAL = new System.Windows.Forms.GroupBox();
            this.TB_MAGNIFICATION = new System.Windows.Forms.TextBox();
            this.LB_MAGNIFICATION = new System.Windows.Forms.Label();
            this.GB_CONNECTION = new System.Windows.Forms.GroupBox();
            this.BTN_POWER = new System.Windows.Forms.Button();
            this.BTN_CAMERA_CONNECT = new System.Windows.Forms.Button();
            this.BTN_DISCONNECT = new System.Windows.Forms.Button();
            this.GB_OPTION = new System.Windows.Forms.GroupBox();
            this.LB_OPTICAL_ROTATION_VEL = new System.Windows.Forms.Label();
            this.LB_BODY_ROTATION_VEL = new System.Windows.Forms.Label();
            this.LB_OPTICAL_ROTATION = new System.Windows.Forms.Label();
            this.HSB_OPTICAL_VEL = new System.Windows.Forms.HScrollBar();
            this.LB_VEL = new System.Windows.Forms.Label();
            this.LB_BODY_ROTATION = new System.Windows.Forms.Label();
            this.CB_AUTO_TRACKING_ENABLED = new System.Windows.Forms.CheckBox();
            this.HSB_BODY_VEL = new System.Windows.Forms.HScrollBar();
            this.CB_AUTO_AIM_ENABLED = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.GB_WEAPON = new System.Windows.Forms.GroupBox();
            this.BTN_FIRE = new System.Windows.Forms.Button();
            this.BTN_TAKE_AIM = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.GB_RANGE_FOUNDER = new System.Windows.Forms.GroupBox();
            this.TB_DISTANCE = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.TB_SENTRY_AZIMUTH = new System.Windows.Forms.TextBox();
            this.LB_SENTRY_AZ = new System.Windows.Forms.Label();
            this.LB_SENTRY_EL = new System.Windows.Forms.Label();
            this.TB_SENTRY_ELEVATION = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.suspectedEnemyActivityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enemyMovementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enemyConcentrationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PBL_VIDEO = new OpenCvSharp.UserInterface.PictureBoxIpl();
            this.lb_xx = new System.Windows.Forms.Label();
            this.lb_yy = new System.Windows.Forms.Label();
            this.addPinPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deletePinPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BTN_SETTING = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PB_MAP)).BeginInit();
            this.PNL_MAP_CONTAINER.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_AZIMUTH)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.GB_OPTICAL.SuspendLayout();
            this.GB_CONNECTION.SuspendLayout();
            this.GB_OPTION.SuspendLayout();
            this.GB_WEAPON.SuspendLayout();
            this.GB_RANGE_FOUNDER.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBL_VIDEO)).BeginInit();
            this.SuspendLayout();
            // 
            // PB_MAP
            // 
            this.PB_MAP.Image = global::RCWS_Situation_room.Properties.Resources.demomap;
            this.PB_MAP.Location = new System.Drawing.Point(3, 6);
            this.PB_MAP.Name = "PB_MAP";
            this.PB_MAP.Size = new System.Drawing.Size(499, 445);
            this.PB_MAP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.PB_MAP.TabIndex = 0;
            this.PB_MAP.TabStop = false;
            // 
            // PNL_MAP_CONTAINER
            // 
            this.PNL_MAP_CONTAINER.Controls.Add(this.PB_MAP);
            this.PNL_MAP_CONTAINER.Location = new System.Drawing.Point(1402, 577);
            this.PNL_MAP_CONTAINER.Name = "PNL_MAP_CONTAINER";
            this.PNL_MAP_CONTAINER.Size = new System.Drawing.Size(505, 451);
            this.PNL_MAP_CONTAINER.TabIndex = 2;
            // 
            // RTB_SEND_DISPLAY
            // 
            this.RTB_SEND_DISPLAY.Location = new System.Drawing.Point(13, 1084);
            this.RTB_SEND_DISPLAY.Name = "RTB_SEND_DISPLAY";
            this.RTB_SEND_DISPLAY.Size = new System.Drawing.Size(310, 80);
            this.RTB_SEND_DISPLAY.TabIndex = 3;
            this.RTB_SEND_DISPLAY.Text = "";
            // 
            // RTB_RECEIVED_DISPLAY
            // 
            this.RTB_RECEIVED_DISPLAY.Location = new System.Drawing.Point(343, 1084);
            this.RTB_RECEIVED_DISPLAY.Name = "RTB_RECEIVED_DISPLAY";
            this.RTB_RECEIVED_DISPLAY.Size = new System.Drawing.Size(310, 80);
            this.RTB_RECEIVED_DISPLAY.TabIndex = 4;
            this.RTB_RECEIVED_DISPLAY.Text = "";
            // 
            // BTN_RCWS_CONNECT
            // 
            this.BTN_RCWS_CONNECT.FlatAppearance.BorderSize = 0;
            this.BTN_RCWS_CONNECT.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_RCWS_CONNECT.ForeColor = System.Drawing.Color.Black;
            this.BTN_RCWS_CONNECT.Location = new System.Drawing.Point(7, 23);
            this.BTN_RCWS_CONNECT.Margin = new System.Windows.Forms.Padding(0);
            this.BTN_RCWS_CONNECT.Name = "BTN_RCWS_CONNECT";
            this.BTN_RCWS_CONNECT.Size = new System.Drawing.Size(198, 46);
            this.BTN_RCWS_CONNECT.TabIndex = 5;
            this.BTN_RCWS_CONNECT.Text = "RCWS Connect";
            this.BTN_RCWS_CONNECT.UseVisualStyleBackColor = true;
            this.BTN_RCWS_CONNECT.Click += new System.EventHandler(this.BTN_RCWS_CONNECT_Click);
            // 
            // PB_AZIMUTH
            // 
            this.PB_AZIMUTH.Image = ((System.Drawing.Image)(resources.GetObject("PB_AZIMUTH.Image")));
            this.PB_AZIMUTH.Location = new System.Drawing.Point(1402, 53);
            this.PB_AZIMUTH.Name = "PB_AZIMUTH";
            this.PB_AZIMUTH.Size = new System.Drawing.Size(505, 518);
            this.PB_AZIMUTH.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PB_AZIMUTH.TabIndex = 6;
            this.PB_AZIMUTH.TabStop = false;
            this.PB_AZIMUTH.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_azimuth_Paint);
            this.PB_AZIMUTH.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_azimuth_MouseDown);
            // 
            // TB_RCWS_AZIMUTH
            // 
            this.TB_RCWS_AZIMUTH.Location = new System.Drawing.Point(56, 26);
            this.TB_RCWS_AZIMUTH.Name = "TB_RCWS_AZIMUTH";
            this.TB_RCWS_AZIMUTH.Size = new System.Drawing.Size(84, 21);
            this.TB_RCWS_AZIMUTH.TabIndex = 7;
            // 
            // TB_WEAPON_ELEVATION
            // 
            this.TB_WEAPON_ELEVATION.Location = new System.Drawing.Point(43, 14);
            this.TB_WEAPON_ELEVATION.Name = "TB_WEAPON_ELEVATION";
            this.TB_WEAPON_ELEVATION.Size = new System.Drawing.Size(84, 21);
            this.TB_WEAPON_ELEVATION.TabIndex = 8;
            // 
            // LB_RCWS_AZ
            // 
            this.LB_RCWS_AZ.AutoSize = true;
            this.LB_RCWS_AZ.Location = new System.Drawing.Point(6, 29);
            this.LB_RCWS_AZ.Name = "LB_RCWS_AZ";
            this.LB_RCWS_AZ.Size = new System.Drawing.Size(44, 12);
            this.LB_RCWS_AZ.TabIndex = 9;
            this.LB_RCWS_AZ.Text = "방위각";
            // 
            // LB_WEAPON_EL
            // 
            this.LB_WEAPON_EL.AutoSize = true;
            this.LB_WEAPON_EL.Location = new System.Drawing.Point(6, 19);
            this.LB_WEAPON_EL.Name = "LB_WEAPON_EL";
            this.LB_WEAPON_EL.Size = new System.Drawing.Size(31, 12);
            this.LB_WEAPON_EL.TabIndex = 10;
            this.LB_WEAPON_EL.Text = "고각";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.groupBox1.Controls.Add(this.LB_WEAPON_EL);
            this.groupBox1.Controls.Add(this.TB_WEAPON_ELEVATION);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(146, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(140, 44);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Weapon";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.groupBox2.Controls.Add(this.LB_OPTICAL_EL);
            this.groupBox2.Controls.Add(this.TB_OPTICAL_ELEVATION);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(292, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(140, 44);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Optical";
            // 
            // LB_OPTICAL_EL
            // 
            this.LB_OPTICAL_EL.AutoSize = true;
            this.LB_OPTICAL_EL.Location = new System.Drawing.Point(4, 19);
            this.LB_OPTICAL_EL.Name = "LB_OPTICAL_EL";
            this.LB_OPTICAL_EL.Size = new System.Drawing.Size(31, 12);
            this.LB_OPTICAL_EL.TabIndex = 10;
            this.LB_OPTICAL_EL.Text = "고각";
            // 
            // TB_OPTICAL_ELEVATION
            // 
            this.TB_OPTICAL_ELEVATION.Location = new System.Drawing.Point(41, 14);
            this.TB_OPTICAL_ELEVATION.Name = "TB_OPTICAL_ELEVATION";
            this.TB_OPTICAL_ELEVATION.Size = new System.Drawing.Size(84, 21);
            this.TB_OPTICAL_ELEVATION.TabIndex = 8;
            // 
            // BTN_PERMISSION
            // 
            this.BTN_PERMISSION.Location = new System.Drawing.Point(6, 20);
            this.BTN_PERMISSION.Name = "BTN_PERMISSION";
            this.BTN_PERMISSION.Size = new System.Drawing.Size(357, 47);
            this.BTN_PERMISSION.TabIndex = 13;
            this.BTN_PERMISSION.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.groupBox7.Controls.Add(this.GB_OPTICAL);
            this.groupBox7.Controls.Add(this.GB_CONNECTION);
            this.groupBox7.Controls.Add(this.GB_OPTION);
            this.groupBox7.Controls.Add(this.GB_WEAPON);
            this.groupBox7.Controls.Add(this.GB_RANGE_FOUNDER);
            this.groupBox7.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox7.ForeColor = System.Drawing.Color.White;
            this.groupBox7.Location = new System.Drawing.Point(1171, 53);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(225, 975);
            this.groupBox7.TabIndex = 16;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "RCWS Status";
            // 
            // GB_OPTICAL
            // 
            this.GB_OPTICAL.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.GB_OPTICAL.Controls.Add(this.TB_MAGNIFICATION);
            this.GB_OPTICAL.Controls.Add(this.LB_MAGNIFICATION);
            this.GB_OPTICAL.ForeColor = System.Drawing.Color.White;
            this.GB_OPTICAL.Location = new System.Drawing.Point(8, 261);
            this.GB_OPTICAL.Name = "GB_OPTICAL";
            this.GB_OPTICAL.Size = new System.Drawing.Size(211, 70);
            this.GB_OPTICAL.TabIndex = 18;
            this.GB_OPTICAL.TabStop = false;
            this.GB_OPTICAL.Text = "Optical";
            // 
            // TB_MAGNIFICATION
            // 
            this.TB_MAGNIFICATION.Location = new System.Drawing.Point(6, 38);
            this.TB_MAGNIFICATION.Name = "TB_MAGNIFICATION";
            this.TB_MAGNIFICATION.ReadOnly = true;
            this.TB_MAGNIFICATION.Size = new System.Drawing.Size(199, 21);
            this.TB_MAGNIFICATION.TabIndex = 11;
            // 
            // LB_MAGNIFICATION
            // 
            this.LB_MAGNIFICATION.AutoSize = true;
            this.LB_MAGNIFICATION.Location = new System.Drawing.Point(4, 23);
            this.LB_MAGNIFICATION.Name = "LB_MAGNIFICATION";
            this.LB_MAGNIFICATION.Size = new System.Drawing.Size(93, 12);
            this.LB_MAGNIFICATION.TabIndex = 13;
            this.LB_MAGNIFICATION.Text = "Magnification";
            // 
            // GB_CONNECTION
            // 
            this.GB_CONNECTION.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.GB_CONNECTION.Controls.Add(this.BTN_POWER);
            this.GB_CONNECTION.Controls.Add(this.BTN_RCWS_CONNECT);
            this.GB_CONNECTION.Controls.Add(this.BTN_CAMERA_CONNECT);
            this.GB_CONNECTION.Controls.Add(this.BTN_DISCONNECT);
            this.GB_CONNECTION.ForeColor = System.Drawing.Color.White;
            this.GB_CONNECTION.Location = new System.Drawing.Point(8, 23);
            this.GB_CONNECTION.Name = "GB_CONNECTION";
            this.GB_CONNECTION.Size = new System.Drawing.Size(211, 222);
            this.GB_CONNECTION.TabIndex = 18;
            this.GB_CONNECTION.TabStop = false;
            this.GB_CONNECTION.Text = "Button";
            // 
            // BTN_POWER
            // 
            this.BTN_POWER.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_POWER.ForeColor = System.Drawing.Color.Black;
            this.BTN_POWER.Location = new System.Drawing.Point(7, 118);
            this.BTN_POWER.Name = "BTN_POWER";
            this.BTN_POWER.Size = new System.Drawing.Size(198, 46);
            this.BTN_POWER.TabIndex = 27;
            this.BTN_POWER.Text = "Power";
            this.BTN_POWER.UseVisualStyleBackColor = true;
            this.BTN_POWER.Click += new System.EventHandler(this.BTN_POWER_Click);
            // 
            // BTN_CAMERA_CONNECT
            // 
            this.BTN_CAMERA_CONNECT.FlatAppearance.BorderSize = 0;
            this.BTN_CAMERA_CONNECT.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_CAMERA_CONNECT.ForeColor = System.Drawing.Color.Black;
            this.BTN_CAMERA_CONNECT.Location = new System.Drawing.Point(7, 69);
            this.BTN_CAMERA_CONNECT.Margin = new System.Windows.Forms.Padding(0);
            this.BTN_CAMERA_CONNECT.Name = "BTN_CAMERA_CONNECT";
            this.BTN_CAMERA_CONNECT.Size = new System.Drawing.Size(198, 46);
            this.BTN_CAMERA_CONNECT.TabIndex = 20;
            this.BTN_CAMERA_CONNECT.Text = "Camera Connect";
            this.BTN_CAMERA_CONNECT.UseVisualStyleBackColor = true;
            this.BTN_CAMERA_CONNECT.Click += new System.EventHandler(this.BTN_CAMERA_CONNECT_Click);
            // 
            // BTN_DISCONNECT
            // 
            this.BTN_DISCONNECT.FlatAppearance.BorderSize = 0;
            this.BTN_DISCONNECT.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_DISCONNECT.ForeColor = System.Drawing.Color.Black;
            this.BTN_DISCONNECT.Location = new System.Drawing.Point(7, 170);
            this.BTN_DISCONNECT.Name = "BTN_DISCONNECT";
            this.BTN_DISCONNECT.Size = new System.Drawing.Size(198, 46);
            this.BTN_DISCONNECT.TabIndex = 18;
            this.BTN_DISCONNECT.Text = "Disconnect";
            this.BTN_DISCONNECT.UseVisualStyleBackColor = true;
            this.BTN_DISCONNECT.Click += new System.EventHandler(this.BTN_DISCONNECT_Click);
            // 
            // GB_OPTION
            // 
            this.GB_OPTION.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.GB_OPTION.Controls.Add(this.LB_OPTICAL_ROTATION_VEL);
            this.GB_OPTION.Controls.Add(this.LB_BODY_ROTATION_VEL);
            this.GB_OPTION.Controls.Add(this.LB_OPTICAL_ROTATION);
            this.GB_OPTION.Controls.Add(this.HSB_OPTICAL_VEL);
            this.GB_OPTION.Controls.Add(this.LB_VEL);
            this.GB_OPTION.Controls.Add(this.LB_BODY_ROTATION);
            this.GB_OPTION.Controls.Add(this.CB_AUTO_TRACKING_ENABLED);
            this.GB_OPTION.Controls.Add(this.HSB_BODY_VEL);
            this.GB_OPTION.Controls.Add(this.CB_AUTO_AIM_ENABLED);
            this.GB_OPTION.Controls.Add(this.label9);
            this.GB_OPTION.Controls.Add(this.label3);
            this.GB_OPTION.ForeColor = System.Drawing.Color.White;
            this.GB_OPTION.Location = new System.Drawing.Point(8, 537);
            this.GB_OPTION.Name = "GB_OPTION";
            this.GB_OPTION.Size = new System.Drawing.Size(211, 245);
            this.GB_OPTION.TabIndex = 18;
            this.GB_OPTION.TabStop = false;
            this.GB_OPTION.Text = "Option";
            // 
            // LB_OPTICAL_ROTATION_VEL
            // 
            this.LB_OPTICAL_ROTATION_VEL.AutoSize = true;
            this.LB_OPTICAL_ROTATION_VEL.Location = new System.Drawing.Point(177, 181);
            this.LB_OPTICAL_ROTATION_VEL.Name = "LB_OPTICAL_ROTATION_VEL";
            this.LB_OPTICAL_ROTATION_VEL.Size = new System.Drawing.Size(0, 12);
            this.LB_OPTICAL_ROTATION_VEL.TabIndex = 35;
            this.LB_OPTICAL_ROTATION_VEL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LB_BODY_ROTATION_VEL
            // 
            this.LB_BODY_ROTATION_VEL.AutoSize = true;
            this.LB_BODY_ROTATION_VEL.Location = new System.Drawing.Point(177, 123);
            this.LB_BODY_ROTATION_VEL.Name = "LB_BODY_ROTATION_VEL";
            this.LB_BODY_ROTATION_VEL.Size = new System.Drawing.Size(0, 12);
            this.LB_BODY_ROTATION_VEL.TabIndex = 34;
            this.LB_BODY_ROTATION_VEL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LB_OPTICAL_ROTATION
            // 
            this.LB_OPTICAL_ROTATION.AutoSize = true;
            this.LB_OPTICAL_ROTATION.Location = new System.Drawing.Point(4, 181);
            this.LB_OPTICAL_ROTATION.Name = "LB_OPTICAL_ROTATION";
            this.LB_OPTICAL_ROTATION.Size = new System.Drawing.Size(113, 12);
            this.LB_OPTICAL_ROTATION.TabIndex = 33;
            this.LB_OPTICAL_ROTATION.Text = "Optical 회전 속도";
            // 
            // HSB_OPTICAL_VEL
            // 
            this.HSB_OPTICAL_VEL.AllowDrop = true;
            this.HSB_OPTICAL_VEL.LargeChange = 1;
            this.HSB_OPTICAL_VEL.Location = new System.Drawing.Point(5, 202);
            this.HSB_OPTICAL_VEL.Maximum = 50;
            this.HSB_OPTICAL_VEL.Minimum = 1;
            this.HSB_OPTICAL_VEL.Name = "HSB_OPTICAL_VEL";
            this.HSB_OPTICAL_VEL.Size = new System.Drawing.Size(199, 20);
            this.HSB_OPTICAL_VEL.TabIndex = 32;
            this.HSB_OPTICAL_VEL.TabStop = true;
            this.HSB_OPTICAL_VEL.Value = 1;
            this.HSB_OPTICAL_VEL.Scroll += new System.Windows.Forms.ScrollEventHandler(this.HSB_OPTICAL_VEL_Scroll);
            // 
            // LB_VEL
            // 
            this.LB_VEL.AutoSize = true;
            this.LB_VEL.Location = new System.Drawing.Point(72, 112);
            this.LB_VEL.Name = "LB_VEL";
            this.LB_VEL.Size = new System.Drawing.Size(0, 12);
            this.LB_VEL.TabIndex = 28;
            // 
            // LB_BODY_ROTATION
            // 
            this.LB_BODY_ROTATION.AutoSize = true;
            this.LB_BODY_ROTATION.Location = new System.Drawing.Point(3, 123);
            this.LB_BODY_ROTATION.Name = "LB_BODY_ROTATION";
            this.LB_BODY_ROTATION.Size = new System.Drawing.Size(100, 12);
            this.LB_BODY_ROTATION.TabIndex = 31;
            this.LB_BODY_ROTATION.Text = "Body 회전 속도";
            // 
            // CB_AUTO_TRACKING_ENABLED
            // 
            this.CB_AUTO_TRACKING_ENABLED.AutoSize = true;
            this.CB_AUTO_TRACKING_ENABLED.Location = new System.Drawing.Point(7, 93);
            this.CB_AUTO_TRACKING_ENABLED.Name = "CB_AUTO_TRACKING_ENABLED";
            this.CB_AUTO_TRACKING_ENABLED.Size = new System.Drawing.Size(100, 16);
            this.CB_AUTO_TRACKING_ENABLED.TabIndex = 30;
            this.CB_AUTO_TRACKING_ENABLED.Text = "Deactivated";
            this.CB_AUTO_TRACKING_ENABLED.UseVisualStyleBackColor = true;
            this.CB_AUTO_TRACKING_ENABLED.CheckedChanged += new System.EventHandler(this.CB_AUTO_TRACKING_ENABLED_CheckedChanged);
            // 
            // HSB_BODY_VEL
            // 
            this.HSB_BODY_VEL.LargeChange = 1;
            this.HSB_BODY_VEL.Location = new System.Drawing.Point(6, 144);
            this.HSB_BODY_VEL.Maximum = 1000;
            this.HSB_BODY_VEL.Minimum = 1;
            this.HSB_BODY_VEL.Name = "HSB_BODY_VEL";
            this.HSB_BODY_VEL.Size = new System.Drawing.Size(199, 20);
            this.HSB_BODY_VEL.TabIndex = 28;
            this.HSB_BODY_VEL.Value = 1;
            this.HSB_BODY_VEL.Scroll += new System.Windows.Forms.ScrollEventHandler(this.HSB_BODY_VEL_Scroll);
            // 
            // CB_AUTO_AIM_ENABLED
            // 
            this.CB_AUTO_AIM_ENABLED.AutoSize = true;
            this.CB_AUTO_AIM_ENABLED.Location = new System.Drawing.Point(7, 49);
            this.CB_AUTO_AIM_ENABLED.Name = "CB_AUTO_AIM_ENABLED";
            this.CB_AUTO_AIM_ENABLED.Size = new System.Drawing.Size(100, 16);
            this.CB_AUTO_AIM_ENABLED.TabIndex = 29;
            this.CB_AUTO_AIM_ENABLED.Text = "Deactivated";
            this.CB_AUTO_AIM_ENABLED.UseVisualStyleBackColor = true;
            this.CB_AUTO_AIM_ENABLED.CheckedChanged += new System.EventHandler(this.CB_AUTO_AIM_ENABLED_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 78);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(106, 12);
            this.label9.TabIndex = 14;
            this.label9.Text = "사람 자동 트래킹";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 12);
            this.label3.TabIndex = 13;
            this.label3.Text = "자동 조준";
            // 
            // GB_WEAPON
            // 
            this.GB_WEAPON.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.GB_WEAPON.Controls.Add(this.BTN_FIRE);
            this.GB_WEAPON.Controls.Add(this.BTN_TAKE_AIM);
            this.GB_WEAPON.Controls.Add(this.label8);
            this.GB_WEAPON.Controls.Add(this.label13);
            this.GB_WEAPON.ForeColor = System.Drawing.Color.White;
            this.GB_WEAPON.Location = new System.Drawing.Point(8, 413);
            this.GB_WEAPON.Name = "GB_WEAPON";
            this.GB_WEAPON.Size = new System.Drawing.Size(211, 118);
            this.GB_WEAPON.TabIndex = 20;
            this.GB_WEAPON.TabStop = false;
            this.GB_WEAPON.Text = "Weapon";
            // 
            // BTN_FIRE
            // 
            this.BTN_FIRE.Location = new System.Drawing.Point(6, 82);
            this.BTN_FIRE.Name = "BTN_FIRE";
            this.BTN_FIRE.Size = new System.Drawing.Size(199, 23);
            this.BTN_FIRE.TabIndex = 31;
            this.BTN_FIRE.UseVisualStyleBackColor = true;
            // 
            // BTN_TAKE_AIM
            // 
            this.BTN_TAKE_AIM.Location = new System.Drawing.Point(6, 39);
            this.BTN_TAKE_AIM.Name = "BTN_TAKE_AIM";
            this.BTN_TAKE_AIM.Size = new System.Drawing.Size(199, 23);
            this.BTN_TAKE_AIM.TabIndex = 30;
            this.BTN_TAKE_AIM.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 12);
            this.label8.TabIndex = 27;
            this.label8.Text = "Take Aim";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(5, 67);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(30, 12);
            this.label13.TabIndex = 25;
            this.label13.Text = "Fire";
            // 
            // GB_RANGE_FOUNDER
            // 
            this.GB_RANGE_FOUNDER.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.GB_RANGE_FOUNDER.Controls.Add(this.TB_DISTANCE);
            this.GB_RANGE_FOUNDER.Controls.Add(this.label5);
            this.GB_RANGE_FOUNDER.ForeColor = System.Drawing.Color.White;
            this.GB_RANGE_FOUNDER.Location = new System.Drawing.Point(8, 337);
            this.GB_RANGE_FOUNDER.Name = "GB_RANGE_FOUNDER";
            this.GB_RANGE_FOUNDER.Size = new System.Drawing.Size(211, 70);
            this.GB_RANGE_FOUNDER.TabIndex = 17;
            this.GB_RANGE_FOUNDER.TabStop = false;
            this.GB_RANGE_FOUNDER.Text = "Range Founder";
            // 
            // TB_DISTANCE
            // 
            this.TB_DISTANCE.Location = new System.Drawing.Point(6, 38);
            this.TB_DISTANCE.Name = "TB_DISTANCE";
            this.TB_DISTANCE.Size = new System.Drawing.Size(199, 21);
            this.TB_DISTANCE.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "Distance";
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.groupBox6.Controls.Add(this.BTN_PERMISSION);
            this.groupBox6.ForeColor = System.Drawing.Color.White;
            this.groupBox6.Location = new System.Drawing.Point(777, 20);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(369, 73);
            this.groupBox6.TabIndex = 18;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Control Authority Status";
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.groupBox5.Controls.Add(this.groupBox9);
            this.groupBox5.Controls.Add(this.groupBox6);
            this.groupBox5.Controls.Add(this.groupBox3);
            this.groupBox5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox5.ForeColor = System.Drawing.Color.White;
            this.groupBox5.Location = new System.Drawing.Point(13, 53);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(1152, 105);
            this.groupBox5.TabIndex = 17;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "AZEL";
            // 
            // groupBox9
            // 
            this.groupBox9.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.groupBox9.Controls.Add(this.groupBox2);
            this.groupBox9.Controls.Add(this.TB_RCWS_AZIMUTH);
            this.groupBox9.Controls.Add(this.groupBox1);
            this.groupBox9.Controls.Add(this.LB_RCWS_AZ);
            this.groupBox9.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox9.ForeColor = System.Drawing.Color.White;
            this.groupBox9.Location = new System.Drawing.Point(6, 20);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(455, 73);
            this.groupBox9.TabIndex = 27;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "RCWS";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.groupBox3.Controls.Add(this.TB_SENTRY_AZIMUTH);
            this.groupBox3.Controls.Add(this.LB_SENTRY_AZ);
            this.groupBox3.Controls.Add(this.LB_SENTRY_EL);
            this.groupBox3.Controls.Add(this.TB_SENTRY_ELEVATION);
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(476, 20);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(295, 73);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Sentry";
            // 
            // TB_SENTRY_AZIMUTH
            // 
            this.TB_SENTRY_AZIMUTH.Location = new System.Drawing.Point(58, 26);
            this.TB_SENTRY_AZIMUTH.Name = "TB_SENTRY_AZIMUTH";
            this.TB_SENTRY_AZIMUTH.Size = new System.Drawing.Size(84, 21);
            this.TB_SENTRY_AZIMUTH.TabIndex = 7;
            // 
            // LB_SENTRY_AZ
            // 
            this.LB_SENTRY_AZ.AutoSize = true;
            this.LB_SENTRY_AZ.Location = new System.Drawing.Point(8, 31);
            this.LB_SENTRY_AZ.Name = "LB_SENTRY_AZ";
            this.LB_SENTRY_AZ.Size = new System.Drawing.Size(44, 12);
            this.LB_SENTRY_AZ.TabIndex = 9;
            this.LB_SENTRY_AZ.Text = "방위각";
            // 
            // LB_SENTRY_EL
            // 
            this.LB_SENTRY_EL.AutoSize = true;
            this.LB_SENTRY_EL.Location = new System.Drawing.Point(163, 31);
            this.LB_SENTRY_EL.Name = "LB_SENTRY_EL";
            this.LB_SENTRY_EL.Size = new System.Drawing.Size(31, 12);
            this.LB_SENTRY_EL.TabIndex = 10;
            this.LB_SENTRY_EL.Text = "고각";
            // 
            // TB_SENTRY_ELEVATION
            // 
            this.TB_SENTRY_ELEVATION.Location = new System.Drawing.Point(200, 26);
            this.TB_SENTRY_ELEVATION.Name = "TB_SENTRY_ELEVATION";
            this.TB_SENTRY_ELEVATION.Size = new System.Drawing.Size(84, 21);
            this.TB_SENTRY_ELEVATION.TabIndex = 8;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.suspectedEnemyActivityToolStripMenuItem,
            this.enemyMovementToolStripMenuItem,
            this.enemyConcentrationToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(214, 70);
            // 
            // suspectedEnemyActivityToolStripMenuItem
            // 
            this.suspectedEnemyActivityToolStripMenuItem.Name = "suspectedEnemyActivityToolStripMenuItem";
            this.suspectedEnemyActivityToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.suspectedEnemyActivityToolStripMenuItem.Text = "Suspected Enemy Activity";
            this.suspectedEnemyActivityToolStripMenuItem.Click += new System.EventHandler(this.suspectedEnemyActivityToolStripMenuItem_Click);
            // 
            // enemyMovementToolStripMenuItem
            // 
            this.enemyMovementToolStripMenuItem.Name = "enemyMovementToolStripMenuItem";
            this.enemyMovementToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.enemyMovementToolStripMenuItem.Text = "Enemy Movement";
            this.enemyMovementToolStripMenuItem.Click += new System.EventHandler(this.enemyMovementToolStripMenuItem_Click);
            // 
            // enemyConcentrationToolStripMenuItem
            // 
            this.enemyConcentrationToolStripMenuItem.Name = "enemyConcentrationToolStripMenuItem";
            this.enemyConcentrationToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.enemyConcentrationToolStripMenuItem.Text = "Enemy Concentration";
            this.enemyConcentrationToolStripMenuItem.Click += new System.EventHandler(this.enemyConcentrationToolStripMenuItem_Click);
            // 
            // PBL_VIDEO
            // 
            this.PBL_VIDEO.Location = new System.Drawing.Point(13, 164);
            this.PBL_VIDEO.Name = "PBL_VIDEO";
            this.PBL_VIDEO.Size = new System.Drawing.Size(1152, 864);
            this.PBL_VIDEO.TabIndex = 21;
            this.PBL_VIDEO.TabStop = false;
            this.PBL_VIDEO.Paint += new System.Windows.Forms.PaintEventHandler(this.PBI_VIDEO_Paint);
            this.PBL_VIDEO.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PBI_Video_MouseClick);
            this.PBL_VIDEO.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PBL_VIDEO_MouseDown);
            this.PBL_VIDEO.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PBL_VIDEO_MouseMove);
            this.PBL_VIDEO.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PBL_VIDEO_MouseUp);
            // 
            // lb_xx
            // 
            this.lb_xx.AutoSize = true;
            this.lb_xx.Location = new System.Drawing.Point(1625, 704);
            this.lb_xx.Name = "lb_xx";
            this.lb_xx.Size = new System.Drawing.Size(0, 12);
            this.lb_xx.TabIndex = 24;
            // 
            // lb_yy
            // 
            this.lb_yy.AutoSize = true;
            this.lb_yy.Location = new System.Drawing.Point(1625, 738);
            this.lb_yy.Name = "lb_yy";
            this.lb_yy.Size = new System.Drawing.Size(0, 12);
            this.lb_yy.TabIndex = 25;
            // 
            // addPinPointToolStripMenuItem
            // 
            this.addPinPointToolStripMenuItem.Name = "addPinPointToolStripMenuItem";
            this.addPinPointToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.addPinPointToolStripMenuItem.Text = "Add Pin Point";
            this.addPinPointToolStripMenuItem.Click += new System.EventHandler(this.addPinPointToolStripMenuItem_Click);
            // 
            // deletePinPointToolStripMenuItem
            // 
            this.deletePinPointToolStripMenuItem.Name = "deletePinPointToolStripMenuItem";
            this.deletePinPointToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.deletePinPointToolStripMenuItem.Text = "Delete Pin Point";
            this.deletePinPointToolStripMenuItem.Click += new System.EventHandler(this.deletePinPointToolStripMenuItem_Click);
            // 
            // BTN_SETTING
            // 
            this.BTN_SETTING.FlatAppearance.BorderSize = 0;
            this.BTN_SETTING.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_SETTING.Location = new System.Drawing.Point(1815, 13);
            this.BTN_SETTING.Margin = new System.Windows.Forms.Padding(0);
            this.BTN_SETTING.Name = "BTN_SETTING";
            this.BTN_SETTING.Size = new System.Drawing.Size(92, 46);
            this.BTN_SETTING.TabIndex = 26;
            this.BTN_SETTING.Text = "Setting";
            this.BTN_SETTING.UseVisualStyleBackColor = true;
            this.BTN_SETTING.Click += new System.EventHandler(this.Setting_Click);
            // 
            // FormMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1920, 1209);
            this.Controls.Add(this.BTN_SETTING);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.lb_yy);
            this.Controls.Add(this.lb_xx);
            this.Controls.Add(this.PBL_VIDEO);
            this.Controls.Add(this.PB_AZIMUTH);
            this.Controls.Add(this.RTB_RECEIVED_DISPLAY);
            this.Controls.Add(this.RTB_SEND_DISPLAY);
            this.Controls.Add(this.PNL_MAP_CONTAINER);
            this.Controls.Add(this.groupBox5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "FormMain";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.GUI_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GUI_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GUI_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.PB_MAP)).EndInit();
            this.PNL_MAP_CONTAINER.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PB_AZIMUTH)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.GB_OPTICAL.ResumeLayout(false);
            this.GB_OPTICAL.PerformLayout();
            this.GB_CONNECTION.ResumeLayout(false);
            this.GB_OPTION.ResumeLayout(false);
            this.GB_OPTION.PerformLayout();
            this.GB_WEAPON.ResumeLayout(false);
            this.GB_WEAPON.PerformLayout();
            this.GB_RANGE_FOUNDER.ResumeLayout(false);
            this.GB_RANGE_FOUNDER.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PBL_VIDEO)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BTN_RCWS_CONNECT;
        private System.Windows.Forms.Button BTN_PERMISSION;
        private System.Windows.Forms.Button BTN_DISCONNECT;

        private System.Windows.Forms.Panel PNL_MAP_CONTAINER;

        private System.Windows.Forms.RichTextBox RTB_SEND_DISPLAY;
        private System.Windows.Forms.RichTextBox RTB_RECEIVED_DISPLAY;

        private System.Windows.Forms.PictureBox PB_AZIMUTH;
        private System.Windows.Forms.PictureBox PB_MAP;

        private System.Windows.Forms.TextBox TB_RCWS_AZIMUTH;
        private System.Windows.Forms.TextBox TB_WEAPON_ELEVATION;
        private System.Windows.Forms.TextBox TB_OPTICAL_ELEVATION;
        private System.Windows.Forms.TextBox TB_DISTANCE;

        private System.Windows.Forms.Label LB_RCWS_AZ;
        private System.Windows.Forms.Label LB_WEAPON_EL;
        private System.Windows.Forms.Label LB_OPTICAL_EL;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label13;

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox GB_WEAPON;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.GroupBox GB_RANGE_FOUNDER;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem suspectedEnemyActivityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enemyMovementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enemyConcentrationToolStripMenuItem;
        private System.Windows.Forms.Button BTN_FIRE;
        private System.Windows.Forms.Button BTN_TAKE_AIM;
        private System.Windows.Forms.Button BTN_CAMERA_CONNECT;
        private OpenCvSharp.UserInterface.PictureBoxIpl PBL_VIDEO;
        private System.Windows.Forms.Label lb_xx;
        private System.Windows.Forms.Label lb_yy;
        private System.Windows.Forms.ToolStripMenuItem addPinPointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deletePinPointToolStripMenuItem;
        private System.Windows.Forms.Button BTN_SETTING;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox TB_SENTRY_AZIMUTH;
        private System.Windows.Forms.Label LB_SENTRY_AZ;
        private System.Windows.Forms.Label LB_SENTRY_EL;
        private System.Windows.Forms.TextBox TB_SENTRY_ELEVATION;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Button BTN_POWER;
        private System.Windows.Forms.CheckBox CB_AUTO_AIM_ENABLED;
        private System.Windows.Forms.GroupBox GB_OPTION;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox CB_AUTO_TRACKING_ENABLED;
        private System.Windows.Forms.HScrollBar HSB_BODY_VEL;
        private System.Windows.Forms.Label LB_BODY_ROTATION;
        private System.Windows.Forms.Label LB_VEL;
        private System.Windows.Forms.GroupBox GB_CONNECTION;
        private System.Windows.Forms.GroupBox GB_OPTICAL;
        private System.Windows.Forms.TextBox TB_MAGNIFICATION;
        private System.Windows.Forms.Label LB_MAGNIFICATION;
        private System.Windows.Forms.Label LB_OPTICAL_ROTATION;
        private System.Windows.Forms.HScrollBar HSB_OPTICAL_VEL;
        private System.Windows.Forms.Label LB_BODY_ROTATION_VEL;
        private System.Windows.Forms.Label LB_OPTICAL_ROTATION_VEL;
    }
}