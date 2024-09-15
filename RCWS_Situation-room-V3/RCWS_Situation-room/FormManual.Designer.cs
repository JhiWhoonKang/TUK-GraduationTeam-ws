namespace RCWS_Situation_room
{
    partial class FormManual
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
            this.RTB_MANUAL = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // RTB_MANUAL
            // 
            this.RTB_MANUAL.Location = new System.Drawing.Point(116, 12);
            this.RTB_MANUAL.Name = "RTB_MANUAL";
            this.RTB_MANUAL.ReadOnly = true;
            this.RTB_MANUAL.Size = new System.Drawing.Size(546, 589);
            this.RTB_MANUAL.TabIndex = 5;
            this.RTB_MANUAL.Text = "";
            // 
            // FormManual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 656);
            this.ControlBox = false;
            this.Controls.Add(this.RTB_MANUAL);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormManual";
            this.Text = "FormManual";
            this.Load += new System.EventHandler(this.FormManual_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox RTB_MANUAL;
    }
}