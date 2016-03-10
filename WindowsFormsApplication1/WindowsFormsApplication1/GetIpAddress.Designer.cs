namespace WindowsFormsApplication1
{
    partial class GetIpAddress
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
            this.tagNOtextBox = new System.Windows.Forms.TextBox();
            this.ResultsrichTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // tagNOtextBox
            // 
            this.tagNOtextBox.Location = new System.Drawing.Point(210, 39);
            this.tagNOtextBox.Name = "tagNOtextBox";
            this.tagNOtextBox.Size = new System.Drawing.Size(100, 20);
            this.tagNOtextBox.TabIndex = 0;
            this.tagNOtextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tagNOtextBox_KeyDown);
            // 
            // ResultsrichTextBox
            // 
            this.ResultsrichTextBox.Location = new System.Drawing.Point(59, 83);
            this.ResultsrichTextBox.Name = "ResultsrichTextBox";
            this.ResultsrichTextBox.Size = new System.Drawing.Size(428, 487);
            this.ResultsrichTextBox.TabIndex = 1;
            this.ResultsrichTextBox.Text = "";
            // 
            // GetIpAddress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(595, 642);
            this.Controls.Add(this.ResultsrichTextBox);
            this.Controls.Add(this.tagNOtextBox);
            this.Name = "GetIpAddress";
            this.Text = "GetIpAddress";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tagNOtextBox;
        private System.Windows.Forms.RichTextBox ResultsrichTextBox;
    }
}