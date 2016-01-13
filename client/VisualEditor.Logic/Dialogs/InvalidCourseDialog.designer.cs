using VisualEditor.Utils.Controls;

namespace VisualEditor.Logic.Dialogs
{
    partial class InvalidCourseDialog
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
            this.neverShowCheckBox = new System.Windows.Forms.CheckBox();
            this.okButton = new System.Windows.Forms.Button();
            this.messageTextBox = new System.Windows.Forms.TextBox();
            this.bevel1 = new Bevel();
            this.SuspendLayout();
            // 
            // neverShowCheckBox
            // 
            this.neverShowCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.neverShowCheckBox.AutoSize = true;
            this.neverShowCheckBox.Location = new System.Drawing.Point(12, 94);
            this.neverShowCheckBox.Name = "neverShowCheckBox";
            this.neverShowCheckBox.Size = new System.Drawing.Size(204, 17);
            this.neverShowCheckBox.TabIndex = 0;
            this.neverShowCheckBox.Text = "Больше не показывать сообщение";
            this.neverShowCheckBox.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(338, 90);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // messageTextBox
            // 
            this.messageTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.messageTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.messageTextBox.Location = new System.Drawing.Point(22, 23);
            this.messageTextBox.Multiline = true;
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.ReadOnly = true;
            this.messageTextBox.Size = new System.Drawing.Size(368, 36);
            this.messageTextBox.TabIndex = 3;
            this.messageTextBox.Text = "В учебном курсе найдены ошибки. Курс будет сохранен, но его невозможно будет в да" +
                "льнейшем загрузить в базу курсов \"Гипертест\".";
            // 
            // bevel1
            // 
            this.bevel1.BevelStyle = BevelStyle.Lowered;
            this.bevel1.BevelType = BevelType.BottomLine;
            this.bevel1.HighlightColor = System.Drawing.SystemColors.ButtonHighlight;
            this.bevel1.Location = new System.Drawing.Point(8, 70);
            this.bevel1.Name = "bevel1";
            this.bevel1.ShadowColor = System.Drawing.SystemColors.ButtonShadow;
            this.bevel1.Size = new System.Drawing.Size(409, 10);
            this.bevel1.TabIndex = 4;
            this.bevel1.Text = "bevel1";
            // 
            // InvalidCourseDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 125);
            this.Controls.Add(this.bevel1);
            this.Controls.Add(this.messageTextBox);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.neverShowCheckBox);
            this.Name = "InvalidCourseDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox neverShowCheckBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TextBox messageTextBox;
        private Bevel bevel1;
    }
}