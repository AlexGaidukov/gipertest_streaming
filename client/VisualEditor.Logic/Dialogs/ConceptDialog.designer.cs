using VisualEditor.Utils.Controls;

namespace VisualEditor.Logic.Dialogs
{
    partial class ConceptDialog
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
            this.conceptNameLabel = new System.Windows.Forms.Label();
            this.conceptNameTextBox = new System.Windows.Forms.TextBox();
            this.externalConceptCheckBox = new System.Windows.Forms.CheckBox();
            this.bevel = new Bevel();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(297, 82);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Enabled = false;
            this.okButton.Location = new System.Drawing.Point(216, 82);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 4;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // conceptNameLabel
            // 
            this.conceptNameLabel.AutoSize = true;
            this.conceptNameLabel.Location = new System.Drawing.Point(9, 14);
            this.conceptNameLabel.Name = "conceptNameLabel";
            this.conceptNameLabel.Size = new System.Drawing.Size(32, 13);
            this.conceptNameLabel.TabIndex = 0;
            this.conceptNameLabel.Text = "Имя:";
            // 
            // conceptNameTextBox
            // 
            this.conceptNameTextBox.Location = new System.Drawing.Point(47, 11);
            this.conceptNameTextBox.Name = "conceptNameTextBox";
            this.conceptNameTextBox.Size = new System.Drawing.Size(325, 20);
            this.conceptNameTextBox.TabIndex = 1;
            this.conceptNameTextBox.TextChanged += new System.EventHandler(this.conceptNameTextBox_TextChanged);
            // 
            // externalConceptCheckBox
            // 
            this.externalConceptCheckBox.AutoSize = true;
            this.externalConceptCheckBox.Location = new System.Drawing.Point(12, 46);
            this.externalConceptCheckBox.Name = "externalConceptCheckBox";
            this.externalConceptCheckBox.Size = new System.Drawing.Size(141, 17);
            this.externalConceptCheckBox.TabIndex = 2;
            this.externalConceptCheckBox.Text = "Внешняя компетенция";
            this.externalConceptCheckBox.UseVisualStyleBackColor = true;
            // 
            // bevel
            // 
            this.bevel.BevelStyle = VisualEditor.Utils.Controls.BevelStyle.Lowered;
            this.bevel.BevelType = VisualEditor.Utils.Controls.BevelType.BottomLine;
            this.bevel.HighlightColor = System.Drawing.SystemColors.ButtonHighlight;
            this.bevel.Location = new System.Drawing.Point(8, 62);
            this.bevel.Name = "bevel";
            this.bevel.ShadowColor = System.Drawing.SystemColors.ButtonShadow;
            this.bevel.Size = new System.Drawing.Size(366, 10);
            this.bevel.TabIndex = 3;
            this.bevel.Text = "bevel";
            // 
            // ConceptDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(384, 116);
            this.Controls.Add(this.bevel);
            this.Controls.Add(this.externalConceptCheckBox);
            this.Controls.Add(this.conceptNameTextBox);
            this.Controls.Add(this.conceptNameLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Name = "ConceptDialog";
            this.Text = "Компетенция";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label conceptNameLabel;
        private System.Windows.Forms.TextBox conceptNameTextBox;
        private System.Windows.Forms.CheckBox externalConceptCheckBox;
        private Bevel bevel;
    }
}