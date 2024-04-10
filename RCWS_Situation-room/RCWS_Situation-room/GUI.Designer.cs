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
            this.pictureBox_Map = new System.Windows.Forms.PictureBox();
            this.btn_close = new System.Windows.Forms.Button();
            this.pn_mapcontainer = new System.Windows.Forms.Panel();
            this.rtb_sendtcp = new System.Windows.Forms.RichTextBox();
            this.rtb_receivetcp = new System.Windows.Forms.RichTextBox();
            this.btn_RCWS_connect = new System.Windows.Forms.Button();
            this.pictureBox_azimuth = new System.Windows.Forms.PictureBox();
            this.tb_body_azimuth = new System.Windows.Forms.TextBox();
            this.tb_body_elevation = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tb_optical_azimuth = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_optical_elevation = new System.Windows.Forms.TextBox();
            this.btn_Permission = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btn_fire = new System.Windows.Forms.Button();
            this.btn_takeaim = new System.Windows.Forms.Button();
            this.tb_gunvoltage = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tb_RemainingBullets = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.tb_Magnification = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tb_Pointdistance = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_Distance = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btn_disconnect = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.suspectedEnemyActivityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enemyMovementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enemyConcentrationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_Camera_connect = new System.Windows.Forms.Button();
            this.pbI_Video = new OpenCvSharp.UserInterface.PictureBoxIpl();
            this.lb_xx = new System.Windows.Forms.Label();
            this.lb_yy = new System.Windows.Forms.Label();
            this.lb_y = new System.Windows.Forms.Label();
            this.lb_x = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Map)).BeginInit();
            this.pn_mapcontainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_azimuth)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbI_Video)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox_Map
            // 
            this.pictureBox_Map.Image = global::RCWS_Situation_room.Properties.Resources.demomap;
            this.pictureBox_Map.Location = new System.Drawing.Point(3, 3);
            this.pictureBox_Map.Name = "pictureBox_Map";
            this.pictureBox_Map.Size = new System.Drawing.Size(537, 674);
            this.pictureBox_Map.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox_Map.TabIndex = 0;
            this.pictureBox_Map.TabStop = false;
            // 
            // btn_close
            // 
            this.btn_close.BackColor = System.Drawing.Color.White;
            this.btn_close.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_close.BackgroundImage")));
            this.btn_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_close.Location = new System.Drawing.Point(2067, 13);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(27, 27);
            this.btn_close.TabIndex = 1;
            this.btn_close.UseVisualStyleBackColor = false;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // pn_mapcontainer
            // 
            this.pn_mapcontainer.Controls.Add(this.pictureBox_Map);
            this.pn_mapcontainer.Location = new System.Drawing.Point(1551, 86);
            this.pn_mapcontainer.Name = "pn_mapcontainer";
            this.pn_mapcontainer.Size = new System.Drawing.Size(543, 683);
            this.pn_mapcontainer.TabIndex = 2;
            // 
            // rtb_sendtcp
            // 
            this.rtb_sendtcp.Location = new System.Drawing.Point(13, 683);
            this.rtb_sendtcp.Name = "rtb_sendtcp";
            this.rtb_sendtcp.Size = new System.Drawing.Size(310, 80);
            this.rtb_sendtcp.TabIndex = 3;
            this.rtb_sendtcp.Text = "";
            // 
            // rtb_receivetcp
            // 
            this.rtb_receivetcp.Location = new System.Drawing.Point(343, 683);
            this.rtb_receivetcp.Name = "rtb_receivetcp";
            this.rtb_receivetcp.Size = new System.Drawing.Size(310, 80);
            this.rtb_receivetcp.TabIndex = 4;
            this.rtb_receivetcp.Text = "";
            // 
            // btn_RCWS_connect
            // 
            this.btn_RCWS_connect.Location = new System.Drawing.Point(13, 13);
            this.btn_RCWS_connect.Name = "btn_RCWS_connect";
            this.btn_RCWS_connect.Size = new System.Drawing.Size(92, 46);
            this.btn_RCWS_connect.TabIndex = 5;
            this.btn_RCWS_connect.Text = "RCWS Connect";
            this.btn_RCWS_connect.UseVisualStyleBackColor = true;
            this.btn_RCWS_connect.Click += new System.EventHandler(this.btn_RCWS_Connect_Click);
            // 
            // pictureBox_azimuth
            // 
            this.pictureBox_azimuth.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_azimuth.Image")));
            this.pictureBox_azimuth.Location = new System.Drawing.Point(798, 86);
            this.pictureBox_azimuth.Name = "pictureBox_azimuth";
            this.pictureBox_azimuth.Size = new System.Drawing.Size(747, 677);
            this.pictureBox_azimuth.TabIndex = 6;
            this.pictureBox_azimuth.TabStop = false;
            this.pictureBox_azimuth.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_azimuth_Paint);
            this.pictureBox_azimuth.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_azimuth_MouseDown);
            // 
            // tb_body_azimuth
            // 
            this.tb_body_azimuth.Location = new System.Drawing.Point(6, 38);
            this.tb_body_azimuth.Name = "tb_body_azimuth";
            this.tb_body_azimuth.Size = new System.Drawing.Size(84, 28);
            this.tb_body_azimuth.TabIndex = 7;
            // 
            // tb_body_elevation
            // 
            this.tb_body_elevation.Location = new System.Drawing.Point(96, 38);
            this.tb_body_elevation.Name = "tb_body_elevation";
            this.tb_body_elevation.Size = new System.Drawing.Size(84, 28);
            this.tb_body_elevation.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 18);
            this.label1.TabIndex = 9;
            this.label1.Text = "방위각";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(94, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 18);
            this.label2.TabIndex = 10;
            this.label2.Text = "고각";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.groupBox1.Controls.Add(this.tb_body_azimuth);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tb_body_elevation);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(6, 29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(186, 70);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "RCWS Body";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.groupBox2.Controls.Add(this.tb_optical_azimuth);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.tb_optical_elevation);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(198, 29);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(186, 70);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Optical Body";
            // 
            // tb_optical_azimuth
            // 
            this.tb_optical_azimuth.Location = new System.Drawing.Point(6, 38);
            this.tb_optical_azimuth.Name = "tb_optical_azimuth";
            this.tb_optical_azimuth.Size = new System.Drawing.Size(84, 28);
            this.tb_optical_azimuth.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 18);
            this.label3.TabIndex = 9;
            this.label3.Text = "방위각";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(94, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 18);
            this.label4.TabIndex = 10;
            this.label4.Text = "고각";
            // 
            // tb_optical_elevation
            // 
            this.tb_optical_elevation.Location = new System.Drawing.Point(96, 38);
            this.tb_optical_elevation.Name = "tb_optical_elevation";
            this.tb_optical_elevation.Size = new System.Drawing.Size(84, 28);
            this.tb_optical_elevation.TabIndex = 8;
            // 
            // btn_Permission
            // 
            this.btn_Permission.Location = new System.Drawing.Point(6, 29);
            this.btn_Permission.Name = "btn_Permission";
            this.btn_Permission.Size = new System.Drawing.Size(231, 70);
            this.btn_Permission.TabIndex = 13;
            this.btn_Permission.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.groupBox3.Controls.Add(this.btn_Permission);
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(410, 86);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(243, 105);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Control Authority Status";
            // 
            // groupBox7
            // 
            this.groupBox7.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.groupBox7.Controls.Add(this.groupBox4);
            this.groupBox7.Controls.Add(this.groupBox8);
            this.groupBox7.ForeColor = System.Drawing.Color.White;
            this.groupBox7.Location = new System.Drawing.Point(659, 86);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(131, 677);
            this.groupBox7.TabIndex = 16;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "RCWS Status";
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.groupBox4.Controls.Add(this.btn_fire);
            this.groupBox4.Controls.Add(this.btn_takeaim);
            this.groupBox4.Controls.Add(this.tb_gunvoltage);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.tb_RemainingBullets);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.ForeColor = System.Drawing.Color.White;
            this.groupBox4.Location = new System.Drawing.Point(6, 196);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(118, 255);
            this.groupBox4.TabIndex = 20;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Weapon";
            // 
            // btn_fire
            // 
            this.btn_fire.Location = new System.Drawing.Point(6, 82);
            this.btn_fire.Name = "btn_fire";
            this.btn_fire.Size = new System.Drawing.Size(105, 23);
            this.btn_fire.TabIndex = 31;
            this.btn_fire.Text = "Fire";
            this.btn_fire.UseVisualStyleBackColor = true;
            // 
            // btn_takeaim
            // 
            this.btn_takeaim.Location = new System.Drawing.Point(6, 39);
            this.btn_takeaim.Name = "btn_takeaim";
            this.btn_takeaim.Size = new System.Drawing.Size(105, 23);
            this.btn_takeaim.TabIndex = 30;
            this.btn_takeaim.Text = "Take Aim";
            this.btn_takeaim.UseVisualStyleBackColor = true;
            // 
            // tb_gunvoltage
            // 
            this.tb_gunvoltage.Location = new System.Drawing.Point(6, 174);
            this.tb_gunvoltage.Name = "tb_gunvoltage";
            this.tb_gunvoltage.Size = new System.Drawing.Size(105, 28);
            this.tb_gunvoltage.TabIndex = 20;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 159);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(106, 18);
            this.label7.TabIndex = 29;
            this.label7.Text = "Gun Voltage";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 18);
            this.label8.TabIndex = 27;
            this.label8.Text = "Take Aim";
            // 
            // tb_RemainingBullets
            // 
            this.tb_RemainingBullets.Location = new System.Drawing.Point(6, 126);
            this.tb_RemainingBullets.Name = "tb_RemainingBullets";
            this.tb_RemainingBullets.Size = new System.Drawing.Size(105, 28);
            this.tb_RemainingBullets.TabIndex = 24;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(4, 111);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(148, 18);
            this.label11.TabIndex = 26;
            this.label11.Text = "Remaining Bullets";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(5, 67);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(36, 18);
            this.label13.TabIndex = 25;
            this.label13.Text = "Fire";
            // 
            // groupBox8
            // 
            this.groupBox8.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.groupBox8.Controls.Add(this.tb_Magnification);
            this.groupBox8.Controls.Add(this.label12);
            this.groupBox8.Controls.Add(this.tb_Pointdistance);
            this.groupBox8.Controls.Add(this.label6);
            this.groupBox8.Controls.Add(this.tb_Distance);
            this.groupBox8.Controls.Add(this.label5);
            this.groupBox8.ForeColor = System.Drawing.Color.White;
            this.groupBox8.Location = new System.Drawing.Point(6, 29);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(118, 161);
            this.groupBox8.TabIndex = 17;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Range Founder";
            // 
            // tb_Magnification
            // 
            this.tb_Magnification.Location = new System.Drawing.Point(6, 39);
            this.tb_Magnification.Name = "tb_Magnification";
            this.tb_Magnification.Size = new System.Drawing.Size(105, 28);
            this.tb_Magnification.TabIndex = 22;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 24);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(113, 18);
            this.label12.TabIndex = 21;
            this.label12.Text = "Magnification";
            // 
            // tb_Pointdistance
            // 
            this.tb_Pointdistance.Location = new System.Drawing.Point(6, 126);
            this.tb_Pointdistance.Name = "tb_Pointdistance";
            this.tb_Pointdistance.Size = new System.Drawing.Size(105, 28);
            this.tb_Pointdistance.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 111);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(123, 18);
            this.label6.TabIndex = 14;
            this.label6.Text = "Point Distance";
            // 
            // tb_Distance
            // 
            this.tb_Distance.Location = new System.Drawing.Point(6, 82);
            this.tb_Distance.Name = "tb_Distance";
            this.tb_Distance.Size = new System.Drawing.Size(105, 28);
            this.tb_Distance.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 18);
            this.label5.TabIndex = 13;
            this.label5.Text = "Distance";
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.groupBox5.Controls.Add(this.groupBox2);
            this.groupBox5.Controls.Add(this.groupBox1);
            this.groupBox5.ForeColor = System.Drawing.Color.White;
            this.groupBox5.Location = new System.Drawing.Point(13, 86);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(391, 105);
            this.groupBox5.TabIndex = 17;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "AZEL";
            // 
            // btn_disconnect
            // 
            this.btn_disconnect.Location = new System.Drawing.Point(209, 13);
            this.btn_disconnect.Name = "btn_disconnect";
            this.btn_disconnect.Size = new System.Drawing.Size(122, 46);
            this.btn_disconnect.TabIndex = 18;
            this.btn_disconnect.Text = "Disconnect";
            this.btn_disconnect.UseVisualStyleBackColor = true;
            this.btn_disconnect.Click += new System.EventHandler(this.btn_disconnect_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.suspectedEnemyActivityToolStripMenuItem,
            this.enemyMovementToolStripMenuItem,
            this.enemyConcentrationToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(295, 100);
            // 
            // suspectedEnemyActivityToolStripMenuItem
            // 
            this.suspectedEnemyActivityToolStripMenuItem.Name = "suspectedEnemyActivityToolStripMenuItem";
            this.suspectedEnemyActivityToolStripMenuItem.Size = new System.Drawing.Size(294, 32);
            this.suspectedEnemyActivityToolStripMenuItem.Text = "Suspected Enemy Activity";
            this.suspectedEnemyActivityToolStripMenuItem.Click += new System.EventHandler(this.suspectedEnemyActivityToolStripMenuItem_Click);
            // 
            // enemyMovementToolStripMenuItem
            // 
            this.enemyMovementToolStripMenuItem.Name = "enemyMovementToolStripMenuItem";
            this.enemyMovementToolStripMenuItem.Size = new System.Drawing.Size(294, 32);
            this.enemyMovementToolStripMenuItem.Text = "Enemy Movement";
            this.enemyMovementToolStripMenuItem.Click += new System.EventHandler(this.enemyMovementToolStripMenuItem_Click);
            // 
            // enemyConcentrationToolStripMenuItem
            // 
            this.enemyConcentrationToolStripMenuItem.Name = "enemyConcentrationToolStripMenuItem";
            this.enemyConcentrationToolStripMenuItem.Size = new System.Drawing.Size(294, 32);
            this.enemyConcentrationToolStripMenuItem.Text = "Enemy Concentration";
            this.enemyConcentrationToolStripMenuItem.Click += new System.EventHandler(this.enemyConcentrationToolStripMenuItem_Click);
            // 
            // btn_Camera_connect
            // 
            this.btn_Camera_connect.Location = new System.Drawing.Point(111, 13);
            this.btn_Camera_connect.Name = "btn_Camera_connect";
            this.btn_Camera_connect.Size = new System.Drawing.Size(92, 46);
            this.btn_Camera_connect.TabIndex = 20;
            this.btn_Camera_connect.Text = "Camera Connect";
            this.btn_Camera_connect.UseVisualStyleBackColor = true;
            this.btn_Camera_connect.Click += new System.EventHandler(this.btn_Camera_Connect_Click);
            // 
            // pbI_Video
            // 
            this.pbI_Video.Location = new System.Drawing.Point(13, 197);
            this.pbI_Video.Name = "pbI_Video";
            this.pbI_Video.Size = new System.Drawing.Size(640, 480);
            this.pbI_Video.TabIndex = 21;
            this.pbI_Video.TabStop = false;
            this.pbI_Video.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbI_Video_MouseClick);
            // 
            // lb_xx
            // 
            this.lb_xx.AutoSize = true;
            this.lb_xx.Location = new System.Drawing.Point(1630, 704);
            this.lb_xx.Name = "lb_xx";
            this.lb_xx.Size = new System.Drawing.Size(0, 18);
            this.lb_xx.TabIndex = 24;
            // 
            // lb_yy
            // 
            this.lb_yy.AutoSize = true;
            this.lb_yy.Location = new System.Drawing.Point(1630, 738);
            this.lb_yy.Name = "lb_yy";
            this.lb_yy.Size = new System.Drawing.Size(0, 18);
            this.lb_yy.TabIndex = 25;
            // 
            // lb_y
            // 
            this.lb_y.AutoSize = true;
            this.lb_y.Location = new System.Drawing.Point(721, 27);
            this.lb_y.Name = "lb_y";
            this.lb_y.Size = new System.Drawing.Size(0, 18);
            this.lb_y.TabIndex = 23;
            // 
            // lb_x
            // 
            this.lb_x.AutoSize = true;
            this.lb_x.Location = new System.Drawing.Point(616, 27);
            this.lb_x.Name = "lb_x";
            this.lb_x.Size = new System.Drawing.Size(0, 18);
            this.lb_x.TabIndex = 22;
            // 
            // GUI
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(2105, 782);
            this.Controls.Add(this.lb_yy);
            this.Controls.Add(this.lb_xx);
            this.Controls.Add(this.lb_y);
            this.Controls.Add(this.lb_x);
            this.Controls.Add(this.pbI_Video);
            this.Controls.Add(this.btn_Camera_connect);
            this.Controls.Add(this.btn_disconnect);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.pictureBox_azimuth);
            this.Controls.Add(this.btn_RCWS_connect);
            this.Controls.Add(this.rtb_receivetcp);
            this.Controls.Add(this.rtb_sendtcp);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.pn_mapcontainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "GUI";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MotionControl";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GUI_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GUI_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Map)).EndInit();
            this.pn_mapcontainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_azimuth)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbI_Video)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Button btn_RCWS_connect;
        private System.Windows.Forms.Button btn_Permission;
        private System.Windows.Forms.Button btn_disconnect;

        private System.Windows.Forms.Panel pn_mapcontainer;

        private System.Windows.Forms.RichTextBox rtb_sendtcp;
        private System.Windows.Forms.RichTextBox rtb_receivetcp;

        private System.Windows.Forms.PictureBox pictureBox_azimuth;
        private System.Windows.Forms.PictureBox pictureBox_Map;

        private System.Windows.Forms.TextBox tb_body_azimuth;
        private System.Windows.Forms.TextBox tb_body_elevation;
        private System.Windows.Forms.TextBox tb_optical_azimuth;
        private System.Windows.Forms.TextBox tb_optical_elevation;
        private System.Windows.Forms.TextBox tb_Distance;
        private System.Windows.Forms.TextBox tb_Pointdistance;
        private System.Windows.Forms.TextBox tb_Magnification;
        private System.Windows.Forms.TextBox tb_RemainingBullets;

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox tb_gunvoltage;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem suspectedEnemyActivityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enemyMovementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enemyConcentrationToolStripMenuItem;
        private System.Windows.Forms.Button btn_fire;
        private System.Windows.Forms.Button btn_takeaim;
        private System.Windows.Forms.Button btn_Camera_connect;
        private OpenCvSharp.UserInterface.PictureBoxIpl pbI_Video;
        private System.Windows.Forms.Label lb_xx;
        private System.Windows.Forms.Label lb_yy;
        private System.Windows.Forms.Label lb_y;
        private System.Windows.Forms.Label lb_x;
    }
}