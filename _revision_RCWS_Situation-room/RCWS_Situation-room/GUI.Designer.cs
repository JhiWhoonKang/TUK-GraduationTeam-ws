namespace RCWS_Situation_room
{
    partial class GUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GUI));
            this.PB_MAP = new System.Windows.Forms.PictureBox();
            this.pn_mapcontainer = new System.Windows.Forms.Panel();
            this.RTB_SEND_DISPLAY = new System.Windows.Forms.RichTextBox();
            this.RTB_RECEIVED_DISPLAY = new System.Windows.Forms.RichTextBox();
            this.BTN_RCWS_CONNECT = new System.Windows.Forms.Button();
            this.PB_AZIMUTH = new System.Windows.Forms.PictureBox();
            this.TB_RCWS_AZIMUTH = new System.Windows.Forms.TextBox();
            this.TB_WEAPON_ELEVATION = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TB_OPTICAL_ELEVATION = new System.Windows.Forms.TextBox();
            this.BTN_PERMISSION = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.CB_AUTO_TRACKING_ENABLED = new System.Windows.Forms.CheckBox();
            this.HSB_Vel = new System.Windows.Forms.HScrollBar();
            this.CB_AUTO_AIM_ENABLED = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.BTN_FIRE = new System.Windows.Forms.Button();
            this.BTN_TAKE_AIM = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.TB_DISTANCE = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.TB_SENTRY_AZIMUTH = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.TB_SENTRY_ELEVATION = new System.Windows.Forms.TextBox();
            this.BTN_DISCONNECT = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.suspectedEnemyActivityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enemyMovementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enemyConcentrationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BTN_CAMERA_CONNECT = new System.Windows.Forms.Button();
            this.PBL_VIDEO = new OpenCvSharp.UserInterface.PictureBoxIpl();
            this.lb_xx = new System.Windows.Forms.Label();
            this.lb_yy = new System.Windows.Forms.Label();
            this.addPinPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deletePinPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BTN_SETTING = new System.Windows.Forms.Button();
            this.TIM_ALARM = new System.Windows.Forms.Timer(this.components);
            this.BTN_POWER = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PB_MAP)).BeginInit();
            this.pn_mapcontainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_AZIMUTH)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox8.SuspendLayout();
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
            this.PB_MAP.Location = new System.Drawing.Point(3, 3);
            this.PB_MAP.Name = "PB_MAP";
            this.PB_MAP.Size = new System.Drawing.Size(537, 674);
            this.PB_MAP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.PB_MAP.TabIndex = 0;
            this.PB_MAP.TabStop = false;
            // 
            // pn_mapcontainer
            // 
            this.pn_mapcontainer.Controls.Add(this.PB_MAP);
            this.pn_mapcontainer.Location = new System.Drawing.Point(1260, 86);
            this.pn_mapcontainer.Name = "pn_mapcontainer";
            this.pn_mapcontainer.Size = new System.Drawing.Size(557, 683);
            this.pn_mapcontainer.TabIndex = 2;
            // 
            // RTB_SEND_DISPLAY
            // 
            this.RTB_SEND_DISPLAY.Location = new System.Drawing.Point(13, 683);
            this.RTB_SEND_DISPLAY.Name = "RTB_SEND_DISPLAY";
            this.RTB_SEND_DISPLAY.Size = new System.Drawing.Size(310, 80);
            this.RTB_SEND_DISPLAY.TabIndex = 3;
            this.RTB_SEND_DISPLAY.Text = "";
            // 
            // RTB_RECEIVED_DISPLAY
            // 
            this.RTB_RECEIVED_DISPLAY.Location = new System.Drawing.Point(343, 683);
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
            this.BTN_RCWS_CONNECT.Location = new System.Drawing.Point(13, 13);
            this.BTN_RCWS_CONNECT.Margin = new System.Windows.Forms.Padding(0);
            this.BTN_RCWS_CONNECT.Name = "BTN_RCWS_CONNECT";
            this.BTN_RCWS_CONNECT.Size = new System.Drawing.Size(92, 46);
            this.BTN_RCWS_CONNECT.TabIndex = 5;
            this.BTN_RCWS_CONNECT.Text = "RCWS Connect";
            this.BTN_RCWS_CONNECT.UseVisualStyleBackColor = true;
            this.BTN_RCWS_CONNECT.Click += new System.EventHandler(this.btn_RCWS_Connect_Click);
            // 
            // PB_AZIMUTH
            // 
            this.PB_AZIMUTH.Image = ((System.Drawing.Image)(resources.GetObject("PB_AZIMUTH.Image")));
            this.PB_AZIMUTH.Location = new System.Drawing.Point(798, 86);
            this.PB_AZIMUTH.Name = "PB_AZIMUTH";
            this.PB_AZIMUTH.Size = new System.Drawing.Size(456, 677);
            this.PB_AZIMUTH.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PB_AZIMUTH.TabIndex = 6;
            this.PB_AZIMUTH.TabStop = false;
            this.PB_AZIMUTH.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_azimuth_Paint);
            this.PB_AZIMUTH.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_azimuth_MouseDown);
            // 
            // TB_RCWS_AZIMUTH
            // 
            this.TB_RCWS_AZIMUTH.Location = new System.Drawing.Point(54, 29);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "방위각";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "고각";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.TB_WEAPON_ELEVATION);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(152, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(140, 44);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Weapon";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.TB_OPTICAL_ELEVATION);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(307, 15);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(140, 44);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Optical";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "고각";
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
            this.BTN_PERMISSION.Location = new System.Drawing.Point(6, 38);
            this.BTN_PERMISSION.Name = "BTN_PERMISSION";
            this.BTN_PERMISSION.Size = new System.Drawing.Size(106, 87);
            this.BTN_PERMISSION.TabIndex = 13;
            this.BTN_PERMISSION.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.groupBox7.Controls.Add(this.groupBox10);
            this.groupBox7.Controls.Add(this.groupBox6);
            this.groupBox7.Controls.Add(this.groupBox4);
            this.groupBox7.Controls.Add(this.groupBox8);
            this.groupBox7.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox7.ForeColor = System.Drawing.Color.White;
            this.groupBox7.Location = new System.Drawing.Point(659, 179);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(131, 584);
            this.groupBox7.TabIndex = 16;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "RCWS Status";
            // 
            // groupBox10
            // 
            this.groupBox10.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.groupBox10.Controls.Add(this.label10);
            this.groupBox10.Controls.Add(this.CB_AUTO_TRACKING_ENABLED);
            this.groupBox10.Controls.Add(this.HSB_Vel);
            this.groupBox10.Controls.Add(this.CB_AUTO_AIM_ENABLED);
            this.groupBox10.Controls.Add(this.label9);
            this.groupBox10.Controls.Add(this.label3);
            this.groupBox10.ForeColor = System.Drawing.Color.White;
            this.groupBox10.Location = new System.Drawing.Point(7, 366);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(118, 171);
            this.groupBox10.TabIndex = 18;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Option";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(4, 112);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(62, 12);
            this.label10.TabIndex = 31;
            this.label10.Text = "회전 속도";
            // 
            // CB_AUTO_TRACKING_ENABLED
            // 
            this.CB_AUTO_TRACKING_ENABLED.AutoSize = true;
            this.CB_AUTO_TRACKING_ENABLED.Location = new System.Drawing.Point(7, 83);
            this.CB_AUTO_TRACKING_ENABLED.Name = "CB_AUTO_TRACKING_ENABLED";
            this.CB_AUTO_TRACKING_ENABLED.Size = new System.Drawing.Size(100, 16);
            this.CB_AUTO_TRACKING_ENABLED.TabIndex = 30;
            this.CB_AUTO_TRACKING_ENABLED.Text = "Deactivated";
            this.CB_AUTO_TRACKING_ENABLED.UseVisualStyleBackColor = true;
            this.CB_AUTO_TRACKING_ENABLED.CheckedChanged += new System.EventHandler(this.CB_AUTO_TRACKING_ENABLED_CheckedChanged);
            // 
            // HSB_Vel
            // 
            this.HSB_Vel.LargeChange = 1;
            this.HSB_Vel.Location = new System.Drawing.Point(7, 133);
            this.HSB_Vel.Maximum = 1000;
            this.HSB_Vel.Name = "HSB_Vel";
            this.HSB_Vel.Size = new System.Drawing.Size(106, 16);
            this.HSB_Vel.TabIndex = 28;
            this.HSB_Vel.Scroll += new System.Windows.Forms.ScrollEventHandler(this.HSB_Vel_Scroll);
            // 
            // CB_AUTO_AIM_ENABLED
            // 
            this.CB_AUTO_AIM_ENABLED.AutoSize = true;
            this.CB_AUTO_AIM_ENABLED.Location = new System.Drawing.Point(7, 38);
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
            this.label9.Location = new System.Drawing.Point(4, 68);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(106, 12);
            this.label9.TabIndex = 14;
            this.label9.Text = "사람 자동 트래킹";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 12);
            this.label3.TabIndex = 13;
            this.label3.Text = "자동 조준";
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.groupBox6.Controls.Add(this.BTN_PERMISSION);
            this.groupBox6.ForeColor = System.Drawing.Color.White;
            this.groupBox6.Location = new System.Drawing.Point(7, 29);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(118, 131);
            this.groupBox6.TabIndex = 18;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Control Authority Status";
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.groupBox4.Controls.Add(this.BTN_FIRE);
            this.groupBox4.Controls.Add(this.BTN_TAKE_AIM);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.ForeColor = System.Drawing.Color.White;
            this.groupBox4.Location = new System.Drawing.Point(7, 242);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(118, 118);
            this.groupBox4.TabIndex = 20;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Weapon";
            // 
            // BTN_FIRE
            // 
            this.BTN_FIRE.Location = new System.Drawing.Point(6, 82);
            this.BTN_FIRE.Name = "BTN_FIRE";
            this.BTN_FIRE.Size = new System.Drawing.Size(105, 23);
            this.BTN_FIRE.TabIndex = 31;
            this.BTN_FIRE.UseVisualStyleBackColor = true;
            // 
            // BTN_TAKE_AIM
            // 
            this.BTN_TAKE_AIM.Location = new System.Drawing.Point(6, 39);
            this.BTN_TAKE_AIM.Name = "BTN_TAKE_AIM";
            this.BTN_TAKE_AIM.Size = new System.Drawing.Size(105, 23);
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
            // groupBox8
            // 
            this.groupBox8.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.groupBox8.Controls.Add(this.TB_DISTANCE);
            this.groupBox8.Controls.Add(this.label5);
            this.groupBox8.ForeColor = System.Drawing.Color.White;
            this.groupBox8.Location = new System.Drawing.Point(7, 166);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(118, 70);
            this.groupBox8.TabIndex = 17;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Range Founder";
            // 
            // TB_DISTANCE
            // 
            this.TB_DISTANCE.Location = new System.Drawing.Point(6, 38);
            this.TB_DISTANCE.Name = "TB_DISTANCE";
            this.TB_DISTANCE.Size = new System.Drawing.Size(105, 21);
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
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.groupBox5.Controls.Add(this.groupBox9);
            this.groupBox5.Controls.Add(this.groupBox3);
            this.groupBox5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox5.ForeColor = System.Drawing.Color.White;
            this.groupBox5.Location = new System.Drawing.Point(13, 86);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(777, 105);
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
            this.groupBox9.Controls.Add(this.label1);
            this.groupBox9.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox9.ForeColor = System.Drawing.Color.White;
            this.groupBox9.Location = new System.Drawing.Point(6, 20);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(455, 67);
            this.groupBox9.TabIndex = 27;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "RCWS";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.groupBox3.Controls.Add(this.TB_SENTRY_AZIMUTH);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.TB_SENTRY_ELEVATION);
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(476, 20);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(295, 67);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Sentry";
            // 
            // TB_SENTRY_AZIMUTH
            // 
            this.TB_SENTRY_AZIMUTH.Location = new System.Drawing.Point(56, 29);
            this.TB_SENTRY_AZIMUTH.Name = "TB_SENTRY_AZIMUTH";
            this.TB_SENTRY_AZIMUTH.Size = new System.Drawing.Size(84, 21);
            this.TB_SENTRY_AZIMUTH.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 34);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "방위각";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(161, 34);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 12);
            this.label7.TabIndex = 10;
            this.label7.Text = "고각";
            // 
            // TB_SENTRY_ELEVATION
            // 
            this.TB_SENTRY_ELEVATION.Location = new System.Drawing.Point(198, 29);
            this.TB_SENTRY_ELEVATION.Name = "TB_SENTRY_ELEVATION";
            this.TB_SENTRY_ELEVATION.Size = new System.Drawing.Size(84, 21);
            this.TB_SENTRY_ELEVATION.TabIndex = 8;
            // 
            // BTN_DISCONNECT
            // 
            this.BTN_DISCONNECT.FlatAppearance.BorderSize = 0;
            this.BTN_DISCONNECT.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_DISCONNECT.Location = new System.Drawing.Point(211, 13);
            this.BTN_DISCONNECT.Name = "BTN_DISCONNECT";
            this.BTN_DISCONNECT.Size = new System.Drawing.Size(92, 46);
            this.BTN_DISCONNECT.TabIndex = 18;
            this.BTN_DISCONNECT.Text = "Disconnect";
            this.BTN_DISCONNECT.UseVisualStyleBackColor = true;
            this.BTN_DISCONNECT.Click += new System.EventHandler(this.BTN_DISCONNECT_Click);
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
            // BTN_CAMERA_CONNECT
            // 
            this.BTN_CAMERA_CONNECT.FlatAppearance.BorderSize = 0;
            this.BTN_CAMERA_CONNECT.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BTN_CAMERA_CONNECT.ForeColor = System.Drawing.Color.Black;
            this.BTN_CAMERA_CONNECT.Location = new System.Drawing.Point(111, 13);
            this.BTN_CAMERA_CONNECT.Margin = new System.Windows.Forms.Padding(0);
            this.BTN_CAMERA_CONNECT.Name = "BTN_CAMERA_CONNECT";
            this.BTN_CAMERA_CONNECT.Size = new System.Drawing.Size(92, 46);
            this.BTN_CAMERA_CONNECT.TabIndex = 20;
            this.BTN_CAMERA_CONNECT.Text = "Camera Connect";
            this.BTN_CAMERA_CONNECT.UseVisualStyleBackColor = true;
            this.BTN_CAMERA_CONNECT.Click += new System.EventHandler(this.btn_Camera_Connect_Click);
            // 
            // PBL_VIDEO
            // 
            this.PBL_VIDEO.Location = new System.Drawing.Point(13, 197);
            this.PBL_VIDEO.Name = "PBL_VIDEO";
            this.PBL_VIDEO.Size = new System.Drawing.Size(640, 480);
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
            this.lb_xx.Location = new System.Drawing.Point(1630, 704);
            this.lb_xx.Name = "lb_xx";
            this.lb_xx.Size = new System.Drawing.Size(0, 12);
            this.lb_xx.TabIndex = 24;
            // 
            // lb_yy
            // 
            this.lb_yy.AutoSize = true;
            this.lb_yy.Location = new System.Drawing.Point(1630, 738);
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
            this.BTN_SETTING.Location = new System.Drawing.Point(1725, 13);
            this.BTN_SETTING.Margin = new System.Windows.Forms.Padding(0);
            this.BTN_SETTING.Name = "BTN_SETTING";
            this.BTN_SETTING.Size = new System.Drawing.Size(92, 46);
            this.BTN_SETTING.TabIndex = 26;
            this.BTN_SETTING.Text = "Setting";
            this.BTN_SETTING.UseVisualStyleBackColor = true;
            this.BTN_SETTING.Click += new System.EventHandler(this.Setting_Click);
            // 
            // TIM_ALARM
            // 
            this.TIM_ALARM.Tick += new System.EventHandler(this.ALARM_Tick);
            // 
            // BTN_POWER
            // 
            this.BTN_POWER.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.BTN_POWER.Location = new System.Drawing.Point(312, 13);
            this.BTN_POWER.Name = "BTN_POWER";
            this.BTN_POWER.Size = new System.Drawing.Size(92, 46);
            this.BTN_POWER.TabIndex = 27;
            this.BTN_POWER.Text = "Power";
            this.BTN_POWER.UseVisualStyleBackColor = true;
            this.BTN_POWER.Click += new System.EventHandler(this.BTN_POWER_Click);
            // 
            // GUI
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1830, 782);
            this.Controls.Add(this.BTN_POWER);
            this.Controls.Add(this.BTN_SETTING);
            this.Controls.Add(this.lb_yy);
            this.Controls.Add(this.lb_xx);
            this.Controls.Add(this.PBL_VIDEO);
            this.Controls.Add(this.BTN_CAMERA_CONNECT);
            this.Controls.Add(this.BTN_DISCONNECT);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.PB_AZIMUTH);
            this.Controls.Add(this.BTN_RCWS_CONNECT);
            this.Controls.Add(this.RTB_RECEIVED_DISPLAY);
            this.Controls.Add(this.RTB_SEND_DISPLAY);
            this.Controls.Add(this.pn_mapcontainer);
            this.Controls.Add(this.groupBox5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "GUI";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MotionControl";
            this.Load += new System.EventHandler(this.GUI_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GUI_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GUI_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.PB_MAP)).EndInit();
            this.pn_mapcontainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PB_AZIMUTH)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
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

        private System.Windows.Forms.Panel pn_mapcontainer;

        private System.Windows.Forms.RichTextBox RTB_SEND_DISPLAY;
        private System.Windows.Forms.RichTextBox RTB_RECEIVED_DISPLAY;

        private System.Windows.Forms.PictureBox PB_AZIMUTH;
        private System.Windows.Forms.PictureBox PB_MAP;

        private System.Windows.Forms.TextBox TB_RCWS_AZIMUTH;
        private System.Windows.Forms.TextBox TB_WEAPON_ELEVATION;
        private System.Windows.Forms.TextBox TB_OPTICAL_ELEVATION;
        private System.Windows.Forms.TextBox TB_DISTANCE;

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label13;

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.GroupBox groupBox8;
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
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox TB_SENTRY_ELEVATION;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Timer TIM_ALARM;
        private System.Windows.Forms.Button BTN_POWER;
        private System.Windows.Forms.CheckBox CB_AUTO_AIM_ENABLED;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox CB_AUTO_TRACKING_ENABLED;
        private System.Windows.Forms.HScrollBar HSB_Vel;
        private System.Windows.Forms.Label label10;
    }
}