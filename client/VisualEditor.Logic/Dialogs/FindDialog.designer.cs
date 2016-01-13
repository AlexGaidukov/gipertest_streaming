using VisualEditor.Utils.Controls;

namespace VisualEditor.Logic.Dialogs
{
    partial class FindDialog
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
            this.findNextButton = new System.Windows.Forms.Button();
            this.directionGroupBox = new System.Windows.Forms.GroupBox();
            this.forwardRadioButton = new System.Windows.Forms.RadioButton();
            this.backwardRadioButton = new System.Windows.Forms.RadioButton();
            this.wholeWordCheckBox = new System.Windows.Forms.CheckBox();
            this.caseCheckBox = new System.Windows.Forms.CheckBox();
            this.findWhatTextBox = new System.Windows.Forms.TextBox();
            this.findWhatLabel = new System.Windows.Forms.Label();
            this.bevel1 = new Bevel();
            this.directionGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(209, 131);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // findNextButton
            // 
            this.findNextButton.Enabled = false;
            this.findNextButton.Location = new System.Drawing.Point(109, 131);
            this.findNextButton.Name = "findNextButton";
            this.findNextButton.Size = new System.Drawing.Size(94, 23);
            this.findNextButton.TabIndex = 6;
            this.findNextButton.Text = "Искать далее";
            this.findNextButton.UseVisualStyleBackColor = true;
            this.findNextButton.Click += new System.EventHandler(this.findNextButton_Click);
            // 
            // directionGroupBox
            // 
            this.directionGroupBox.Controls.Add(this.forwardRadioButton);
            this.directionGroupBox.Controls.Add(this.backwardRadioButton);
            this.directionGroupBox.Location = new System.Drawing.Point(190, 37);
            this.directionGroupBox.Name = "directionGroupBox";
            this.directionGroupBox.Size = new System.Drawing.Size(92, 72);
            this.directionGroupBox.TabIndex = 4;
            this.directionGroupBox.TabStop = false;
            this.directionGroupBox.Text = "Направление";
            // 
            // forwardRadioButton
            // 
            this.forwardRadioButton.AutoSize = true;
            this.forwardRadioButton.Checked = true;
            this.forwardRadioButton.Location = new System.Drawing.Point(14, 42);
            this.forwardRadioButton.Name = "forwardRadioButton";
            this.forwardRadioButton.Size = new System.Drawing.Size(50, 17);
            this.forwardRadioButton.TabIndex = 1;
            this.forwardRadioButton.TabStop = true;
            this.forwardRadioButton.Text = "Вниз";
            this.forwardRadioButton.UseVisualStyleBackColor = true;
            // 
            // backwardRadioButton
            // 
            this.backwardRadioButton.AutoSize = true;
            this.backwardRadioButton.Location = new System.Drawing.Point(14, 20);
            this.backwardRadioButton.Name = "backwardRadioButton";
            this.backwardRadioButton.Size = new System.Drawing.Size(55, 17);
            this.backwardRadioButton.TabIndex = 0;
            this.backwardRadioButton.Text = "Вверх";
            this.backwardRadioButton.UseVisualStyleBackColor = true;
            // 
            // wholeWordCheckBox
            // 
            this.wholeWordCheckBox.AutoSize = true;
            this.wholeWordCheckBox.Location = new System.Drawing.Point(11, 79);
            this.wholeWordCheckBox.Name = "wholeWordCheckBox";
            this.wholeWordCheckBox.Size = new System.Drawing.Size(131, 17);
            this.wholeWordCheckBox.TabIndex = 3;
            this.wholeWordCheckBox.Text = "Только целые слова";
            this.wholeWordCheckBox.UseVisualStyleBackColor = true;
            // 
            // caseCheckBox
            // 
            this.caseCheckBox.AutoSize = true;
            this.caseCheckBox.Location = new System.Drawing.Point(11, 56);
            this.caseCheckBox.Name = "caseCheckBox";
            this.caseCheckBox.Size = new System.Drawing.Size(124, 17);
            this.caseCheckBox.TabIndex = 2;
            this.caseCheckBox.Text = "Учитывать регистр";
            this.caseCheckBox.UseVisualStyleBackColor = true;
            // 
            // findWhatTextBox
            // 
            this.findWhatTextBox.Location = new System.Drawing.Point(82, 11);
            this.findWhatTextBox.Name = "findWhatTextBox";
            this.findWhatTextBox.Size = new System.Drawing.Size(200, 20);
            this.findWhatTextBox.TabIndex = 1;
            this.findWhatTextBox.TextChanged += new System.EventHandler(this.findWhatTextBox_TextChanged);
            // 
            // findWhatLabel
            // 
            this.findWhatLabel.AutoSize = true;
            this.findWhatLabel.Location = new System.Drawing.Point(9, 14);
            this.findWhatLabel.Name = "findWhatLabel";
            this.findWhatLabel.Size = new System.Drawing.Size(67, 13);
            this.findWhatLabel.TabIndex = 0;
            this.findWhatLabel.Text = "Что искать:";
            // 
            // bevel1
            // 
            this.bevel1.BevelStyle = BevelStyle.Lowered;
            this.bevel1.BevelType = BevelType.BottomLine;
            this.bevel1.HighlightColor = System.Drawing.SystemColors.ButtonHighlight;
            this.bevel1.Location = new System.Drawing.Point(8, 112);
            this.bevel1.Name = "bevel1";
            this.bevel1.ShadowColor = System.Drawing.SystemColors.ButtonShadow;
            this.bevel1.Size = new System.Drawing.Size(278, 10);
            this.bevel1.TabIndex = 5;
            this.bevel1.Text = "bevel1";
            // 
            // FindReplaceDialog
            // 
            this.AcceptButton = this.findNextButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(294, 165);
            this.Controls.Add(this.bevel1);
            this.Controls.Add(this.directionGroupBox);
            this.Controls.Add(this.wholeWordCheckBox);
            this.Controls.Add(this.caseCheckBox);
            this.Controls.Add(this.findWhatTextBox);
            this.Controls.Add(this.findWhatLabel);
            this.Controls.Add(this.findNextButton);
            this.Controls.Add(this.cancelButton);
            this.Name = "FindReplaceDialog";
            this.Text = "Найти";
            this.directionGroupBox.ResumeLayout(false);
            this.directionGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button findNextButton;
        private System.Windows.Forms.GroupBox directionGroupBox;
        private System.Windows.Forms.RadioButton forwardRadioButton;
        private System.Windows.Forms.RadioButton backwardRadioButton;
        private System.Windows.Forms.CheckBox wholeWordCheckBox;
        private System.Windows.Forms.CheckBox caseCheckBox;
        private System.Windows.Forms.TextBox findWhatTextBox;
        private System.Windows.Forms.Label findWhatLabel;
        private Bevel bevel1;
    }
}