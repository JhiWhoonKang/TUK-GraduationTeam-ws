namespace RCWS_Situation_room
{
    partial class FormDataSetting
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
            this.label1 = new System.Windows.Forms.Label();
            this.rtb_sendtcp = new System.Windows.Forms.RichTextBox();
            this.rtb_receivetcp = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("휴먼둥근헤드라인", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(238, 25);
            this.label1.TabIndex = 3;
            this.label1.Text = "상황실 데이터 확인";
            // 
            // rtb_sendtcp
            // 
            this.rtb_sendtcp.Location = new System.Drawing.Point(12, 105);
            this.rtb_sendtcp.Name = "rtb_sendtcp";
            this.rtb_sendtcp.Size = new System.Drawing.Size(776, 137);
            this.rtb_sendtcp.TabIndex = 4;
            this.rtb_sendtcp.Text = "";
            // 
            // rtb_receivetcp
            // 
            this.rtb_receivetcp.Location = new System.Drawing.Point(12, 301);
            this.rtb_receivetcp.Name = "rtb_receivetcp";
            this.rtb_receivetcp.Size = new System.Drawing.Size(776, 137);
            this.rtb_receivetcp.TabIndex = 5;
            this.rtb_receivetcp.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("새굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(12, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(228, 24);
            this.label2.TabIndex = 6;
            this.label2.Text = "상황실 송신 데이터";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(12, 274);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(228, 24);
            this.label3.TabIndex = 7;
            this.label3.Text = "상황실 수신 데이터";
            // 
            // FormDataSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rtb_receivetcp);
            this.Controls.Add(this.rtb_sendtcp);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormDataSetting";
            this.Text = "FormDataSetting";
            this.Load += new System.EventHandler(this.FormDataSetting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox rtb_sendtcp;
        private System.Windows.Forms.RichTextBox rtb_receivetcp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}