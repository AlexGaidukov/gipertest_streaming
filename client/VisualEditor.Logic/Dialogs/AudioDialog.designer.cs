using VisualEditor.Utils.Controls;

namespace VisualEditor.Logic.Dialogs
{
    partial class AudioDialog
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
            this.sourceButton = new System.Windows.Forms.Button();
            this.sourceTextBox = new System.Windows.Forms.TextBox();
            this.sourceLabel = new System.Windows.Forms.Label();
            this.bevel = new VisualEditor.Utils.Controls.Bevel();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.linkTextTextBox = new System.Windows.Forms.TextBox();
            this.linkTextLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // sourceButton
            // 
            this.sourceButton.Location = new System.Drawing.Point(466, 9);
            this.sourceButton.Name = "sourceButton";
            this.sourceButton.Size = new System.Drawing.Size(107, 23);
            this.sourceButton.TabIndex = 2;
            this.sourceButton.Text = "Выбрать файл...";
            this.sourceButton.UseVisualStyleBackColor = true;
            this.sourceButton.Click += new System.EventHandler(this.sourceButton_Click);
            // 
            // sourceTextBox
            // 
            this.sourceTextBox.Location = new System.Drawing.Point(73, 11);
            this.sourceTextBox.Name = "sourceTextBox";
            this.sourceTextBox.ReadOnly = true;
            this.sourceTextBox.Size = new System.Drawing.Size(387, 20);
            this.sourceTextBox.TabIndex = 1;
            this.sourceTextBox.TextChanged += new System.EventHandler(this.sourceTextBox_TextChanged);
            // 
            // sourceLabel
            // 
            this.sourceLabel.AutoSize = true;
            this.sourceLabel.Location = new System.Drawing.Point(9, 14);
            this.sourceLabel.Name = "sourceLabel";
            this.sourceLabel.Size = new System.Drawing.Size(58, 13);
            this.sourceLabel.TabIndex = 0;
            this.sourceLabel.Text = "Источник:";
            // 
            // bevel
            // 
            this.bevel.BevelStyle = VisualEditor.Utils.Controls.BevelStyle.Lowered;
            this.bevel.BevelType = VisualEditor.Utils.Controls.BevelType.BottomLine;
            this.bevel.HighlightColor = System.Drawing.SystemColors.ButtonHighlight;
            this.bevel.Location = new System.Drawing.Point(8, 62);
            this.bevel.Name = "bevel";
            this.bevel.ShadowColor = System.Drawing.SystemColors.ButtonShadow;
            this.bevel.Size = new System.Drawing.Size(568, 10);
            this.bevel.TabIndex = 5;
            this.bevel.Text = "bevel";
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(498, 82);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Enabled = false;
            this.okButton.Location = new System.Drawing.Point(417, 82);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 6;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // linkTextTextBox
            // 
            this.linkTextTextBox.Location = new System.Drawing.Point(96, 40);
            this.linkTextTextBox.Name = "linkTextTextBox";
            this.linkTextTextBox.Size = new System.Drawing.Size(364, 20);
            this.linkTextTextBox.TabIndex = 4;
            // 
            // linkTextLabel
            // 
            this.linkTextLabel.AutoSize = true;
            this.linkTextLabel.Location = new System.Drawing.Point(9, 43);
            this.linkTextLabel.Name = "linkTextLabel";
            this.linkTextLabel.Size = new System.Drawing.Size(81, 13);
            this.linkTextLabel.TabIndex = 3;
            this.linkTextLabel.Text = "Текст ссылки:";
            // 
            // AudioDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(585, 116);
            this.Controls.Add(this.linkTextTextBox);
            this.Controls.Add(this.linkTextLabel);
            this.Controls.Add(this.bevel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.sourceButton);
            this.Controls.Add(this.sourceTextBox);
            this.Controls.Add(this.sourceLabel);
            this.Name = "AudioDialog";
            this.Text = "Аудио";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button sourceButton;
        private System.Windows.Forms.TextBox sourceTextBox;
        private System.Windows.Forms.Label sourceLabel;
        private Bevel bevel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TextBox linkTextTextBox;
        private System.Windows.Forms.Label linkTextLabel;
    }
}