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
            this.LV_RX = new System.Windows.Forms.ListView();
            this.LV_TX = new System.Windows.Forms.ListView();
            this.LB_RX = new System.Windows.Forms.Label();
            this.LB_TX = new System.Windows.Forms.Label();
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
            // LV_RX
            // 
            this.LV_RX.HideSelection = false;
            this.LV_RX.Location = new System.Drawing.Point(17, 84);
            this.LV_RX.Name = "LV_RX";
            this.LV_RX.Scrollable = false;
            this.LV_RX.Size = new System.Drawing.Size(771, 280);
            this.LV_RX.TabIndex = 8;
            this.LV_RX.UseCompatibleStateImageBehavior = false;
            // 
            // LV_TX
            // 
            this.LV_TX.HideSelection = false;
            this.LV_TX.Location = new System.Drawing.Point(16, 407);
            this.LV_TX.Name = "LV_TX";
            this.LV_TX.Scrollable = false;
            this.LV_TX.Size = new System.Drawing.Size(771, 280);
            this.LV_TX.TabIndex = 9;
            this.LV_TX.UseCompatibleStateImageBehavior = false;
            // 
            // LB_RX
            // 
            this.LB_RX.AutoSize = true;
            this.LB_RX.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LB_RX.Location = new System.Drawing.Point(13, 62);
            this.LB_RX.Name = "LB_RX";
            this.LB_RX.Size = new System.Drawing.Size(79, 19);
            this.LB_RX.TabIndex = 10;
            this.LB_RX.Text = "Rx Data";
            // 
            // LB_TX
            // 
            this.LB_TX.AutoSize = true;
            this.LB_TX.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LB_TX.Location = new System.Drawing.Point(13, 385);
            this.LB_TX.Name = "LB_TX";
            this.LB_TX.Size = new System.Drawing.Size(78, 19);
            this.LB_TX.TabIndex = 11;
            this.LB_TX.Text = "Tx Data";
            // 
            // FormDataSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 871);
            this.Controls.Add(this.LB_TX);
            this.Controls.Add(this.LB_RX);
            this.Controls.Add(this.LV_TX);
            this.Controls.Add(this.LV_RX);
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
        private System.Windows.Forms.ListView LV_RX;
        private System.Windows.Forms.ListView LV_TX;
        private System.Windows.Forms.Label LB_RX;
        private System.Windows.Forms.Label LB_TX;
    }
}