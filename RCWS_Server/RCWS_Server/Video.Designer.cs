namespace RCWS_Server
{
    partial class Video
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
            this.pictureBox_Display = new System.Windows.Forms.PictureBox();
            this.richUdpConnectionStatus = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Display)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox_Display
            // 
            this.pictureBox_Display.Location = new System.Drawing.Point(12, 12);
            this.pictureBox_Display.Name = "pictureBox_Display";
            this.pictureBox_Display.Size = new System.Drawing.Size(776, 259);
            this.pictureBox_Display.TabIndex = 0;
            this.pictureBox_Display.TabStop = false;
            // 
            // richUdpConnectionStatus
            // 
            this.richUdpConnectionStatus.Location = new System.Drawing.Point(12, 296);
            this.richUdpConnectionStatus.Name = "richUdpConnectionStatus";
            this.richUdpConnectionStatus.Size = new System.Drawing.Size(258, 96);
            this.richUdpConnectionStatus.TabIndex = 1;
            this.richUdpConnectionStatus.Text = "";
            // 
            // Video
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.richUdpConnectionStatus);
            this.Controls.Add(this.pictureBox_Display);
            this.Name = "Video";
            this.Text = "Video";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Display)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_Display;
        private System.Windows.Forms.RichTextBox richUdpConnectionStatus;
    }
}