namespace RCWS_Situation_room
{
    partial class Setting
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
            System.Windows.Forms.Button btn_optical_data_setting;
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.sidebar = new System.Windows.Forms.FlowLayoutPanel();
            this.pnl_optical_setting_container = new System.Windows.Forms.Panel();
            this.btn_reticle_setting = new System.Windows.Forms.Button();
            this.btn_optical_setting = new System.Windows.Forms.Button();
            this.pnl_overalldata_container = new System.Windows.Forms.Panel();
            this.btn_logout = new System.Windows.Forms.Button();
            this.opticalTransition = new System.Windows.Forms.Timer(this.components);
            this.sidebarTransition = new System.Windows.Forms.Timer(this.components);
            this.pnl_weapon_setting_container = new System.Windows.Forms.Panel();
            this.btn_weapon_setting = new System.Windows.Forms.Button();
            this.btn_weapon_data_setting = new System.Windows.Forms.Button();
            this.pnl_motor_setting_container = new System.Windows.Forms.Panel();
            this.btn_motor_setting = new System.Windows.Forms.Button();
            this.btn_motor_data_setting = new System.Windows.Forms.Button();
            this.btn_overall_data_setting = new System.Windows.Forms.Button();
            this.pnl_exit_container = new System.Windows.Forms.Panel();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.pictureBox10 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox11 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox12 = new System.Windows.Forms.PictureBox();
            this.pictureBox13 = new System.Windows.Forms.PictureBox();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.pictureBox9 = new System.Windows.Forms.PictureBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.pb_close = new System.Windows.Forms.PictureBox();
            this.btn_menu = new System.Windows.Forms.PictureBox();
            this.weaponTransition = new System.Windows.Forms.Timer(this.components);
            this.motorTransition = new System.Windows.Forms.Timer(this.components);
            btn_optical_data_setting = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.sidebar.SuspendLayout();
            this.pnl_optical_setting_container.SuspendLayout();
            this.pnl_overalldata_container.SuspendLayout();
            this.pnl_weapon_setting_container.SuspendLayout();
            this.pnl_motor_setting_container.SuspendLayout();
            this.pnl_exit_container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_close)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_menu)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.pb_close);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btn_menu);
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
            // sidebar
            // 
            this.sidebar.BackColor = System.Drawing.SystemColors.ControlDark;
            this.sidebar.Controls.Add(this.pnl_optical_setting_container);
            this.sidebar.Controls.Add(this.pnl_weapon_setting_container);
            this.sidebar.Controls.Add(this.pnl_motor_setting_container);
            this.sidebar.Controls.Add(this.pnl_overalldata_container);
            this.sidebar.Controls.Add(this.pnl_exit_container);
            this.sidebar.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.sidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebar.Location = new System.Drawing.Point(0, 50);
            this.sidebar.Name = "sidebar";
            this.sidebar.Size = new System.Drawing.Size(339, 732);
            this.sidebar.TabIndex = 2;
            // 
            // pnl_optical_setting_container
            // 
            this.pnl_optical_setting_container.Controls.Add(this.pictureBox5);
            this.pnl_optical_setting_container.Controls.Add(this.pictureBox10);
            this.pnl_optical_setting_container.Controls.Add(this.pictureBox4);
            this.pnl_optical_setting_container.Controls.Add(this.pictureBox11);
            this.pnl_optical_setting_container.Controls.Add(this.pictureBox3);
            this.pnl_optical_setting_container.Controls.Add(this.btn_optical_setting);
            this.pnl_optical_setting_container.Controls.Add(btn_optical_data_setting);
            this.pnl_optical_setting_container.Controls.Add(this.btn_reticle_setting);
            this.pnl_optical_setting_container.Location = new System.Drawing.Point(0, 0);
            this.pnl_optical_setting_container.Margin = new System.Windows.Forms.Padding(0);
            this.pnl_optical_setting_container.Name = "pnl_optical_setting_container";
            this.pnl_optical_setting_container.Size = new System.Drawing.Size(181, 182);
            this.pnl_optical_setting_container.TabIndex = 5;
            // 
            // btn_reticle_setting
            // 
            this.btn_reticle_setting.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btn_reticle_setting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_reticle_setting.FlatAppearance.BorderSize = 0;
            this.btn_reticle_setting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_reticle_setting.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_reticle_setting.Location = new System.Drawing.Point(0, 0);
            this.btn_reticle_setting.Margin = new System.Windows.Forms.Padding(0);
            this.btn_reticle_setting.Name = "btn_reticle_setting";
            this.btn_reticle_setting.Size = new System.Drawing.Size(181, 182);
            this.btn_reticle_setting.TabIndex = 5;
            this.btn_reticle_setting.Text = "                    Reticle";
            this.btn_reticle_setting.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_reticle_setting.UseVisualStyleBackColor = false;
            this.btn_reticle_setting.Click += new System.EventHandler(this.btn_reticle_setting_Click);
            // 
            // btn_optical_data_setting
            // 
            btn_optical_data_setting.BackColor = System.Drawing.SystemColors.ControlDark;
            btn_optical_data_setting.Dock = System.Windows.Forms.DockStyle.Bottom;
            btn_optical_data_setting.FlatAppearance.BorderSize = 0;
            btn_optical_data_setting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn_optical_data_setting.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            btn_optical_data_setting.Location = new System.Drawing.Point(0, 122);
            btn_optical_data_setting.Margin = new System.Windows.Forms.Padding(0);
            btn_optical_data_setting.Name = "btn_optical_data_setting";
            btn_optical_data_setting.Size = new System.Drawing.Size(181, 60);
            btn_optical_data_setting.TabIndex = 6;
            btn_optical_data_setting.Text = "                    Optical Data";
            btn_optical_data_setting.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btn_optical_data_setting.UseVisualStyleBackColor = false;
            btn_optical_data_setting.Click += new System.EventHandler(this.btn_optical_data_setting_Click);
            // 
            // btn_optical_setting
            // 
            this.btn_optical_setting.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btn_optical_setting.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_optical_setting.FlatAppearance.BorderSize = 0;
            this.btn_optical_setting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_optical_setting.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_optical_setting.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_optical_setting.Location = new System.Drawing.Point(0, 0);
            this.btn_optical_setting.Margin = new System.Windows.Forms.Padding(0);
            this.btn_optical_setting.Name = "btn_optical_setting";
            this.btn_optical_setting.Size = new System.Drawing.Size(181, 60);
            this.btn_optical_setting.TabIndex = 4;
            this.btn_optical_setting.Text = "            Optical";
            this.btn_optical_setting.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_optical_setting.UseVisualStyleBackColor = false;
            this.btn_optical_setting.Click += new System.EventHandler(this.btn_optical_setting_Click);
            // 
            // pnl_overalldata_container
            // 
            this.pnl_overalldata_container.Controls.Add(this.pictureBox9);
            this.pnl_overalldata_container.Controls.Add(this.btn_overall_data_setting);
            this.pnl_overalldata_container.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_overalldata_container.Location = new System.Drawing.Point(0, 423);
            this.pnl_overalldata_container.Margin = new System.Windows.Forms.Padding(0);
            this.pnl_overalldata_container.Name = "pnl_overalldata_container";
            this.pnl_overalldata_container.Size = new System.Drawing.Size(181, 60);
            this.pnl_overalldata_container.TabIndex = 3;
            // 
            // btn_logout
            // 
            this.btn_logout.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btn_logout.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_logout.FlatAppearance.BorderSize = 0;
            this.btn_logout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_logout.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_logout.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_logout.Location = new System.Drawing.Point(0, 0);
            this.btn_logout.Margin = new System.Windows.Forms.Padding(0);
            this.btn_logout.Name = "btn_logout";
            this.btn_logout.Size = new System.Drawing.Size(181, 60);
            this.btn_logout.TabIndex = 5;
            this.btn_logout.Text = "            Close";
            this.btn_logout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_logout.UseVisualStyleBackColor = false;
            this.btn_logout.Click += new System.EventHandler(this.btn_logout_Click);
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
            // pnl_weapon_setting_container
            // 
            this.pnl_weapon_setting_container.Controls.Add(this.pictureBox1);
            this.pnl_weapon_setting_container.Controls.Add(this.pictureBox2);
            this.pnl_weapon_setting_container.Controls.Add(this.pictureBox12);
            this.pnl_weapon_setting_container.Controls.Add(this.btn_weapon_setting);
            this.pnl_weapon_setting_container.Controls.Add(this.btn_weapon_data_setting);
            this.pnl_weapon_setting_container.Location = new System.Drawing.Point(0, 182);
            this.pnl_weapon_setting_container.Margin = new System.Windows.Forms.Padding(0);
            this.pnl_weapon_setting_container.Name = "pnl_weapon_setting_container";
            this.pnl_weapon_setting_container.Size = new System.Drawing.Size(181, 120);
            this.pnl_weapon_setting_container.TabIndex = 3;
            // 
            // btn_weapon_setting
            // 
            this.btn_weapon_setting.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btn_weapon_setting.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_weapon_setting.FlatAppearance.BorderSize = 0;
            this.btn_weapon_setting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_weapon_setting.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_weapon_setting.Location = new System.Drawing.Point(0, 0);
            this.btn_weapon_setting.Margin = new System.Windows.Forms.Padding(0);
            this.btn_weapon_setting.Name = "btn_weapon_setting";
            this.btn_weapon_setting.Size = new System.Drawing.Size(181, 60);
            this.btn_weapon_setting.TabIndex = 4;
            this.btn_weapon_setting.Text = "            Weapon";
            this.btn_weapon_setting.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_weapon_setting.UseVisualStyleBackColor = false;
            this.btn_weapon_setting.Click += new System.EventHandler(this.btn_weapon_setting_Click);
            // 
            // btn_weapon_data_setting
            // 
            this.btn_weapon_data_setting.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btn_weapon_data_setting.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btn_weapon_data_setting.FlatAppearance.BorderSize = 0;
            this.btn_weapon_data_setting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_weapon_data_setting.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_weapon_data_setting.Location = new System.Drawing.Point(0, 60);
            this.btn_weapon_data_setting.Margin = new System.Windows.Forms.Padding(0);
            this.btn_weapon_data_setting.Name = "btn_weapon_data_setting";
            this.btn_weapon_data_setting.Size = new System.Drawing.Size(181, 60);
            this.btn_weapon_data_setting.TabIndex = 5;
            this.btn_weapon_data_setting.Text = "                    Weapon Data";
            this.btn_weapon_data_setting.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_weapon_data_setting.UseVisualStyleBackColor = false;
            this.btn_weapon_data_setting.Click += new System.EventHandler(this.btn_weapon_data_setting_Click);
            // 
            // pnl_motor_setting_container
            // 
            this.pnl_motor_setting_container.Controls.Add(this.pictureBox13);
            this.pnl_motor_setting_container.Controls.Add(this.pictureBox7);
            this.pnl_motor_setting_container.Controls.Add(this.pictureBox8);
            this.pnl_motor_setting_container.Controls.Add(this.btn_motor_setting);
            this.pnl_motor_setting_container.Controls.Add(this.btn_motor_data_setting);
            this.pnl_motor_setting_container.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_motor_setting_container.Location = new System.Drawing.Point(0, 302);
            this.pnl_motor_setting_container.Margin = new System.Windows.Forms.Padding(0);
            this.pnl_motor_setting_container.Name = "pnl_motor_setting_container";
            this.pnl_motor_setting_container.Size = new System.Drawing.Size(181, 121);
            this.pnl_motor_setting_container.TabIndex = 10;
            // 
            // btn_motor_setting
            // 
            this.btn_motor_setting.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btn_motor_setting.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_motor_setting.FlatAppearance.BorderSize = 0;
            this.btn_motor_setting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_motor_setting.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_motor_setting.Location = new System.Drawing.Point(0, 0);
            this.btn_motor_setting.Margin = new System.Windows.Forms.Padding(0);
            this.btn_motor_setting.Name = "btn_motor_setting";
            this.btn_motor_setting.Size = new System.Drawing.Size(181, 60);
            this.btn_motor_setting.TabIndex = 15;
            this.btn_motor_setting.Text = "            Motor";
            this.btn_motor_setting.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_motor_setting.UseVisualStyleBackColor = false;
            this.btn_motor_setting.Click += new System.EventHandler(this.btn_motor_setting_Click);
            // 
            // btn_motor_data_setting
            // 
            this.btn_motor_data_setting.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btn_motor_data_setting.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btn_motor_data_setting.FlatAppearance.BorderSize = 0;
            this.btn_motor_data_setting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_motor_data_setting.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_motor_data_setting.Location = new System.Drawing.Point(0, 61);
            this.btn_motor_data_setting.Margin = new System.Windows.Forms.Padding(0);
            this.btn_motor_data_setting.Name = "btn_motor_data_setting";
            this.btn_motor_data_setting.Size = new System.Drawing.Size(181, 60);
            this.btn_motor_data_setting.TabIndex = 16;
            this.btn_motor_data_setting.Text = "                    Motor Data";
            this.btn_motor_data_setting.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_motor_data_setting.UseVisualStyleBackColor = false;
            this.btn_motor_data_setting.Click += new System.EventHandler(this.btn_motor_data_setting_Click);
            // 
            // btn_overall_data_setting
            // 
            this.btn_overall_data_setting.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btn_overall_data_setting.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_overall_data_setting.FlatAppearance.BorderSize = 0;
            this.btn_overall_data_setting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_overall_data_setting.Font = new System.Drawing.Font("Segoe UI Emoji", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_overall_data_setting.Location = new System.Drawing.Point(0, 0);
            this.btn_overall_data_setting.Margin = new System.Windows.Forms.Padding(0);
            this.btn_overall_data_setting.Name = "btn_overall_data_setting";
            this.btn_overall_data_setting.Size = new System.Drawing.Size(181, 60);
            this.btn_overall_data_setting.TabIndex = 17;
            this.btn_overall_data_setting.Text = "            Overall Data";
            this.btn_overall_data_setting.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_overall_data_setting.UseVisualStyleBackColor = false;
            this.btn_overall_data_setting.Click += new System.EventHandler(this.btn_overall_data_setting_Click);
            // 
            // pnl_exit_container
            // 
            this.pnl_exit_container.Controls.Add(this.pictureBox6);
            this.pnl_exit_container.Controls.Add(this.btn_logout);
            this.pnl_exit_container.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_exit_container.Location = new System.Drawing.Point(0, 483);
            this.pnl_exit_container.Margin = new System.Windows.Forms.Padding(0);
            this.pnl_exit_container.Name = "pnl_exit_container";
            this.pnl_exit_container.Size = new System.Drawing.Size(181, 60);
            this.pnl_exit_container.TabIndex = 22;
            // 
            // pictureBox5
            // 
            this.pictureBox5.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox5.Image = global::RCWS_Situation_room.Properties.Resources.database_icon;
            this.pictureBox5.Location = new System.Drawing.Point(38, 135);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(35, 35);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox5.TabIndex = 8;
            this.pictureBox5.TabStop = false;
            // 
            // pictureBox10
            // 
            this.pictureBox10.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox10.Image = global::RCWS_Situation_room.Properties.Resources.dot_icon;
            this.pictureBox10.Location = new System.Drawing.Point(12, 143);
            this.pictureBox10.Name = "pictureBox10";
            this.pictureBox10.Size = new System.Drawing.Size(20, 20);
            this.pictureBox10.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox10.TabIndex = 21;
            this.pictureBox10.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox4.Image = global::RCWS_Situation_room.Properties.Resources.optical_icon;
            this.pictureBox4.Location = new System.Drawing.Point(12, 15);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(35, 35);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox4.TabIndex = 8;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox11
            // 
            this.pictureBox11.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox11.Image = global::RCWS_Situation_room.Properties.Resources.dot_icon;
            this.pictureBox11.Location = new System.Drawing.Point(12, 82);
            this.pictureBox11.Name = "pictureBox11";
            this.pictureBox11.Size = new System.Drawing.Size(20, 20);
            this.pictureBox11.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox11.TabIndex = 22;
            this.pictureBox11.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox3.Image = global::RCWS_Situation_room.Properties.Resources.reticle_icon;
            this.pictureBox3.Location = new System.Drawing.Point(38, 75);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(35, 35);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 8;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox1.Image = global::RCWS_Situation_room.Properties.Resources.database_icon;
            this.pictureBox1.Location = new System.Drawing.Point(38, 73);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(35, 35);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox2.Image = global::RCWS_Situation_room.Properties.Resources.weapon_icon;
            this.pictureBox2.Location = new System.Drawing.Point(12, 14);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(35, 35);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 14;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox12
            // 
            this.pictureBox12.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox12.Image = global::RCWS_Situation_room.Properties.Resources.dot_icon;
            this.pictureBox12.Location = new System.Drawing.Point(12, 81);
            this.pictureBox12.Name = "pictureBox12";
            this.pictureBox12.Size = new System.Drawing.Size(20, 20);
            this.pictureBox12.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox12.TabIndex = 23;
            this.pictureBox12.TabStop = false;
            // 
            // pictureBox13
            // 
            this.pictureBox13.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox13.Image = global::RCWS_Situation_room.Properties.Resources.dot_icon;
            this.pictureBox13.Location = new System.Drawing.Point(12, 82);
            this.pictureBox13.Name = "pictureBox13";
            this.pictureBox13.Size = new System.Drawing.Size(20, 20);
            this.pictureBox13.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox13.TabIndex = 24;
            this.pictureBox13.TabStop = false;
            // 
            // pictureBox7
            // 
            this.pictureBox7.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox7.Image = global::RCWS_Situation_room.Properties.Resources.database_icon;
            this.pictureBox7.Location = new System.Drawing.Point(38, 75);
            this.pictureBox7.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(35, 35);
            this.pictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox7.TabIndex = 18;
            this.pictureBox7.TabStop = false;
            // 
            // pictureBox8
            // 
            this.pictureBox8.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox8.Image = global::RCWS_Situation_room.Properties.Resources.motor_icon;
            this.pictureBox8.Location = new System.Drawing.Point(12, 14);
            this.pictureBox8.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(35, 35);
            this.pictureBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox8.TabIndex = 19;
            this.pictureBox8.TabStop = false;
            // 
            // pictureBox9
            // 
            this.pictureBox9.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox9.Image = global::RCWS_Situation_room.Properties.Resources.database_icon;
            this.pictureBox9.Location = new System.Drawing.Point(12, 12);
            this.pictureBox9.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox9.Name = "pictureBox9";
            this.pictureBox9.Size = new System.Drawing.Size(35, 35);
            this.pictureBox9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox9.TabIndex = 20;
            this.pictureBox9.TabStop = false;
            // 
            // pictureBox6
            // 
            this.pictureBox6.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox6.Image = global::RCWS_Situation_room.Properties.Resources.logout_icon;
            this.pictureBox6.Location = new System.Drawing.Point(12, 13);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(35, 35);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox6.TabIndex = 8;
            this.pictureBox6.TabStop = false;
            // 
            // pb_close
            // 
            this.pb_close.Image = global::RCWS_Situation_room.Properties.Resources.close_icon;
            this.pb_close.Location = new System.Drawing.Point(952, 7);
            this.pb_close.Name = "pb_close";
            this.pb_close.Size = new System.Drawing.Size(36, 36);
            this.pb_close.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_close.TabIndex = 2;
            this.pb_close.TabStop = false;
            this.pb_close.Click += new System.EventHandler(this.pb_close_Click);
            // 
            // btn_menu
            // 
            this.btn_menu.Image = global::RCWS_Situation_room.Properties.Resources.list_icon;
            this.btn_menu.Location = new System.Drawing.Point(12, 12);
            this.btn_menu.Name = "btn_menu";
            this.btn_menu.Size = new System.Drawing.Size(26, 26);
            this.btn_menu.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btn_menu.TabIndex = 2;
            this.btn_menu.TabStop = false;
            this.btn_menu.Click += new System.EventHandler(this.btn_menu_Click);
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
            // Setting
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1000, 782);
            this.Controls.Add(this.sidebar);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.IsMdiContainer = true;
            this.Name = "Setting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Setting";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Setting_FormClosed);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.sidebar.ResumeLayout(false);
            this.pnl_optical_setting_container.ResumeLayout(false);
            this.pnl_overalldata_container.ResumeLayout(false);
            this.pnl_weapon_setting_container.ResumeLayout(false);
            this.pnl_motor_setting_container.ResumeLayout(false);
            this.pnl_exit_container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_close)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_menu)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox btn_menu;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pb_close;
        private System.Windows.Forms.FlowLayoutPanel sidebar;
        private System.Windows.Forms.Panel pnl_overalldata_container;
        private System.Windows.Forms.Button btn_optical_setting;
        private System.Windows.Forms.Panel pnl_optical_setting_container;
        private System.Windows.Forms.Button btn_logout;
        private System.Windows.Forms.Button btn_reticle_setting;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.Timer opticalTransition;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.Timer sidebarTransition;
        private System.Windows.Forms.Panel pnl_weapon_setting_container;
        private System.Windows.Forms.Button btn_weapon_setting;
        private System.Windows.Forms.Button btn_weapon_data_setting;
        private System.Windows.Forms.Panel pnl_motor_setting_container;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btn_motor_setting;
        private System.Windows.Forms.Button btn_motor_data_setting;
        private System.Windows.Forms.Button btn_overall_data_setting;
        private System.Windows.Forms.PictureBox pictureBox7;
        private System.Windows.Forms.PictureBox pictureBox8;
        private System.Windows.Forms.PictureBox pictureBox9;
        private System.Windows.Forms.PictureBox pictureBox10;
        private System.Windows.Forms.PictureBox pictureBox11;
        private System.Windows.Forms.PictureBox pictureBox12;
        private System.Windows.Forms.PictureBox pictureBox13;
        private System.Windows.Forms.Panel pnl_exit_container;
        private System.Windows.Forms.Timer weaponTransition;
        private System.Windows.Forms.Timer motorTransition;
    }
}