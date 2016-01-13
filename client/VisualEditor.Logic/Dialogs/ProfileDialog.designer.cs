using VisualEditor.Utils.Controls;

namespace VisualEditor.Logic.Dialogs
{
    partial class ProfileDialog
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
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.lowerBoundUpDown = new System.Windows.Forms.NumericUpDown();
            this.lowerBoundLabel = new System.Windows.Forms.Label();
            this.bevel = new Bevel();
            ((System.ComponentModel.ISupportInitialize)(this.lowerBoundUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(133, 54);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(52, 54);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // lowerBoundUpDown
            // 
            this.lowerBoundUpDown.DecimalPlaces = 1;
            this.lowerBoundUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.lowerBoundUpDown.Location = new System.Drawing.Point(148, 12);
            this.lowerBoundUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.lowerBoundUpDown.Name = "lowerBoundUpDown";
            this.lowerBoundUpDown.Size = new System.Drawing.Size(60, 20);
            this.lowerBoundUpDown.TabIndex = 6;
            this.lowerBoundUpDown.ThousandsSeparator = true;
            this.lowerBoundUpDown.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lowerBoundUpDown_KeyDown);
            // 
            // lowerBoundLabel
            // 
            this.lowerBoundLabel.AutoSize = true;
            this.lowerBoundLabel.Location = new System.Drawing.Point(9, 14);
            this.lowerBoundLabel.Name = "lowerBoundLabel";
            this.lowerBoundLabel.Size = new System.Drawing.Size(133, 13);
            this.lowerBoundLabel.TabIndex = 5;
            this.lowerBoundLabel.Text = "Нижняя граница оценки:";
            // 
            // bevel
            // 
            this.bevel.BevelStyle = BevelStyle.Lowered;
            this.bevel.BevelType = BevelType.BottomLine;
            this.bevel.HighlightColor = System.Drawing.SystemColors.ButtonHighlight;
            this.bevel.Location = new System.Drawing.Point(8, 34);
            this.bevel.Name = "bevel";
            this.bevel.ShadowColor = System.Drawing.SystemColors.ButtonShadow;
            this.bevel.Size = new System.Drawing.Size(203, 10);
            this.bevel.TabIndex = 7;
            this.bevel.Text = "bevel1";
            // 
            // ProfileDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(220, 88);
            this.Controls.Add(this.bevel);
            this.Controls.Add(this.lowerBoundUpDown);
            this.Controls.Add(this.lowerBoundLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Name = "ProfileDialog";
            this.Text = "Свойства элемента профиля";
            ((System.ComponentModel.ISupportInitialize)(this.lowerBoundUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.NumericUpDown lowerBoundUpDown;
        private System.Windows.Forms.Label lowerBoundLabel;
        private Bevel bevel;
    }
}