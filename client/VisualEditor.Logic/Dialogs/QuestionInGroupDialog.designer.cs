using VisualEditor.Utils.Controls;

namespace VisualEditor.Logic.Dialogs
{
    partial class QuestionInGroupDialog
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
            this.questionTypeLabel = new System.Windows.Forms.Label();
            this.questionTypeTextBox = new System.Windows.Forms.TextBox();
            this.responseIsNumberCheckBox = new System.Windows.Forms.CheckBox();
            this.hintButton = new System.Windows.Forms.Button();
            this.resposeVariantsButton = new System.Windows.Forms.Button();
            this.bevel = new Bevel();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(367, 118);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 10;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(286, 118);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 9;
            this.okButton.Text = "ОК";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // questionTypeLabel
            // 
            this.questionTypeLabel.AutoSize = true;
            this.questionTypeLabel.Location = new System.Drawing.Point(9, 14);
            this.questionTypeLabel.Name = "questionTypeLabel";
            this.questionTypeLabel.Size = new System.Drawing.Size(74, 13);
            this.questionTypeLabel.TabIndex = 0;
            this.questionTypeLabel.Text = "Тип вопроса:";
            // 
            // questionTypeTextBox
            // 
            this.questionTypeTextBox.Location = new System.Drawing.Point(89, 11);
            this.questionTypeTextBox.Name = "questionTypeTextBox";
            this.questionTypeTextBox.ReadOnly = true;
            this.questionTypeTextBox.Size = new System.Drawing.Size(160, 20);
            this.questionTypeTextBox.TabIndex = 1;
            // 
            // responseIsNumberCheckBox
            // 
            this.responseIsNumberCheckBox.AutoSize = true;
            this.responseIsNumberCheckBox.Enabled = false;
            this.responseIsNumberCheckBox.Location = new System.Drawing.Point(12, 43);
            this.responseIsNumberCheckBox.Name = "responseIsNumberCheckBox";
            this.responseIsNumberCheckBox.Size = new System.Drawing.Size(114, 17);
            this.responseIsNumberCheckBox.TabIndex = 5;
            this.responseIsNumberCheckBox.Text = "Тип ответа число";
            this.responseIsNumberCheckBox.UseVisualStyleBackColor = true;
            this.responseIsNumberCheckBox.CheckedChanged += new System.EventHandler(this.responseIsNumberCheckBox_CheckedChanged);
            // 
            // hintButton
            // 
            this.hintButton.Location = new System.Drawing.Point(12, 74);
            this.hintButton.Name = "hintButton";
            this.hintButton.Size = new System.Drawing.Size(207, 23);
            this.hintButton.TabIndex = 6;
            this.hintButton.Text = "Редактировать подсказку...";
            this.hintButton.UseVisualStyleBackColor = true;
            this.hintButton.Click += new System.EventHandler(this.hintButton_Click);
            // 
            // resposeVariantsButton
            // 
            this.resposeVariantsButton.Location = new System.Drawing.Point(235, 74);
            this.resposeVariantsButton.Name = "resposeVariantsButton";
            this.resposeVariantsButton.Size = new System.Drawing.Size(207, 23);
            this.resposeVariantsButton.TabIndex = 7;
            this.resposeVariantsButton.Text = "Редактировать варианты ответа...";
            this.resposeVariantsButton.UseVisualStyleBackColor = true;
            this.resposeVariantsButton.Click += new System.EventHandler(this.resposeVariantsButton_Click);
            // 
            // bevel
            // 
            this.bevel.BevelStyle = BevelStyle.Lowered;
            this.bevel.BevelType = BevelType.BottomLine;
            this.bevel.HighlightColor = System.Drawing.SystemColors.ButtonHighlight;
            this.bevel.Location = new System.Drawing.Point(6, 98);
            this.bevel.Name = "bevel";
            this.bevel.ShadowColor = System.Drawing.SystemColors.ButtonShadow;
            this.bevel.Size = new System.Drawing.Size(440, 10);
            this.bevel.TabIndex = 8;
            this.bevel.Text = "bevel1";
            // 
            // QuestionInGroupDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(453, 151);
            this.Controls.Add(this.bevel);
            this.Controls.Add(this.resposeVariantsButton);
            this.Controls.Add(this.hintButton);
            this.Controls.Add(this.responseIsNumberCheckBox);
            this.Controls.Add(this.questionTypeTextBox);
            this.Controls.Add(this.questionTypeLabel);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Name = "QuestionInGroupDialog";
            this.Text = "Параметры вопроса";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label questionTypeLabel;
        private System.Windows.Forms.TextBox questionTypeTextBox;
        private System.Windows.Forms.CheckBox responseIsNumberCheckBox;
        private System.Windows.Forms.Button hintButton;
        private System.Windows.Forms.Button resposeVariantsButton;
        private Bevel bevel;
    }
}