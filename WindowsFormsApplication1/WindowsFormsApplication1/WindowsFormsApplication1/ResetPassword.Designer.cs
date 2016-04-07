namespace WindowsFormsApplication1
{
    partial class ResetPassword
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
            this.aCheckPasswordMustChange = new System.Windows.Forms.CheckBox();
            this.aLabelDescription = new System.Windows.Forms.Label();
            this.aCheckBoxTest1 = new System.Windows.Forms.CheckBox();
            this.aCheckBoxTest2 = new System.Windows.Forms.CheckBox();
            this.aPasswordResetButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // aCheckPasswordMustChange
            // 
            this.aCheckPasswordMustChange.AutoSize = true;
            this.aCheckPasswordMustChange.Font = new System.Drawing.Font("Lucida Console", 12F);
            this.aCheckPasswordMustChange.Location = new System.Drawing.Point(12, 62);
            this.aCheckPasswordMustChange.Name = "aCheckPasswordMustChange";
            this.aCheckPasswordMustChange.Size = new System.Drawing.Size(427, 20);
            this.aCheckPasswordMustChange.TabIndex = 35;
            this.aCheckPasswordMustChange.Text = "User must change password at next logon!";
            this.aCheckPasswordMustChange.UseVisualStyleBackColor = true;
            this.aCheckPasswordMustChange.MouseClick += new System.Windows.Forms.MouseEventHandler(this.aCheckPasswordMustChange_MouseClick);
            // 
            // aLabelDescription
            // 
            this.aLabelDescription.AutoSize = true;
            this.aLabelDescription.Font = new System.Drawing.Font("Lucida Console", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aLabelDescription.Location = new System.Drawing.Point(8, 9);
            this.aLabelDescription.Name = "aLabelDescription";
            this.aLabelDescription.Size = new System.Drawing.Size(493, 19);
            this.aLabelDescription.TabIndex = 36;
            this.aLabelDescription.Text = "Some settings before you reset the password:";
            // 
            // aCheckBoxTest1
            // 
            this.aCheckBoxTest1.AutoSize = true;
            this.aCheckBoxTest1.Font = new System.Drawing.Font("Lucida Console", 12F);
            this.aCheckBoxTest1.Location = new System.Drawing.Point(12, 88);
            this.aCheckBoxTest1.Name = "aCheckBoxTest1";
            this.aCheckBoxTest1.Size = new System.Drawing.Size(337, 20);
            this.aCheckBoxTest1.TabIndex = 37;
            this.aCheckBoxTest1.Text = "User must receive a bitch slap!";
            this.aCheckBoxTest1.UseVisualStyleBackColor = true;
            // 
            // aCheckBoxTest2
            // 
            this.aCheckBoxTest2.AutoSize = true;
            this.aCheckBoxTest2.Font = new System.Drawing.Font("Lucida Console", 12F);
            this.aCheckBoxTest2.Location = new System.Drawing.Point(12, 114);
            this.aCheckBoxTest2.Name = "aCheckBoxTest2";
            this.aCheckBoxTest2.Size = new System.Drawing.Size(297, 20);
            this.aCheckBoxTest2.TabIndex = 38;
            this.aCheckBoxTest2.Text = "Send regards to his mother!";
            this.aCheckBoxTest2.UseVisualStyleBackColor = true;
            // 
            // aPasswordResetButton
            // 
            this.aPasswordResetButton.Font = new System.Drawing.Font("Lucida Console", 12F);
            this.aPasswordResetButton.Location = new System.Drawing.Point(97, 181);
            this.aPasswordResetButton.Name = "aPasswordResetButton";
            this.aPasswordResetButton.Size = new System.Drawing.Size(135, 54);
            this.aPasswordResetButton.TabIndex = 39;
            this.aPasswordResetButton.Text = "Password Reset";
            this.aPasswordResetButton.UseVisualStyleBackColor = true;
            this.aPasswordResetButton.Click += new System.EventHandler(this.aPasswordResetButton_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Lucida Console", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(238, 181);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(135, 54);
            this.button2.TabIndex = 40;
            this.button2.Text = "Abortion!";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ResetPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 280);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.aPasswordResetButton);
            this.Controls.Add(this.aCheckBoxTest2);
            this.Controls.Add(this.aCheckBoxTest1);
            this.Controls.Add(this.aLabelDescription);
            this.Controls.Add(this.aCheckPasswordMustChange);
            this.Name = "ResetPassword";
            this.Text = "ResetPassword";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox aCheckPasswordMustChange;
        private System.Windows.Forms.Label aLabelDescription;
        private System.Windows.Forms.CheckBox aCheckBoxTest1;
        private System.Windows.Forms.CheckBox aCheckBoxTest2;
        private System.Windows.Forms.Button aPasswordResetButton;
        private System.Windows.Forms.Button button2;
    }
}