using VisualEditor.Utils;
using VisualEditor.Utils.Controls;

namespace VisualEditor.Logic.Dialogs
{
    partial class GroupDialog
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
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.questionsCountTextBox = new System.Windows.Forms.TextBox();
            this.questionsCountLabel = new System.Windows.Forms.Label();
            this.secondsLabel = new System.Windows.Forms.Label();
            this.minutesLabel = new System.Windows.Forms.Label();
            this.secondsUpDown = new UpDown();
            this.minutesUpDown = new UpDown();
            this.timeRestrictionCheckBox = new System.Windows.Forms.CheckBox();
            this.profileComboBox = new System.Windows.Forms.ComboBox();
            this.profileLabel = new System.Windows.Forms.Label();
            this.marksUpDown = new System.Windows.Forms.NumericUpDown();
            this.marksLabel = new System.Windows.Forms.Label();
            this.questionCountGroupBox = new System.Windows.Forms.GroupBox();
            this.percentUpDown = new UpDown();
            this.questionsCountUpDown = new UpDown();
            this.percentRadioButton = new System.Windows.Forms.RadioButton();
            this.questionsCountRadioButton = new System.Windows.Forms.RadioButton();
            this.questionOptionsGroupBox = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.secondsUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minutesUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.marksUpDown)).BeginInit();
            this.questionCountGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.percentUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.questionsCountUpDown)).BeginInit();
            this.questionOptionsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Enabled = false;
            this.okButton.Location = new System.Drawing.Point(284, 244);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(76, 23);
            this.okButton.TabIndex = 4;
            this.okButton.Text = "ОК";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(366, 244);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(76, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // questionsCountTextBox
            // 
            this.questionsCountTextBox.Location = new System.Drawing.Point(181, 11);
            this.questionsCountTextBox.Name = "questionsCountTextBox";
            this.questionsCountTextBox.ReadOnly = true;
            this.questionsCountTextBox.Size = new System.Drawing.Size(60, 20);
            this.questionsCountTextBox.TabIndex = 1;
            // 
            // questionsCountLabel
            // 
            this.questionsCountLabel.AutoSize = true;
            this.questionsCountLabel.Location = new System.Drawing.Point(9, 14);
            this.questionsCountLabel.Name = "questionsCountLabel";
            this.questionsCountLabel.Size = new System.Drawing.Size(166, 13);
            this.questionsCountLabel.TabIndex = 0;
            this.questionsCountLabel.Text = "Количество вопросов в группе:";
            // 
            // secondsLabel
            // 
            this.secondsLabel.AutoSize = true;
            this.secondsLabel.Location = new System.Drawing.Point(357, 23);
            this.secondsLabel.Name = "secondsLabel";
            this.secondsLabel.Size = new System.Drawing.Size(25, 13);
            this.secondsLabel.TabIndex = 4;
            this.secondsLabel.Text = "сек";
            // 
            // minutesLabel
            // 
            this.minutesLabel.AutoSize = true;
            this.minutesLabel.Location = new System.Drawing.Point(258, 23);
            this.minutesLabel.Name = "minutesLabel";
            this.minutesLabel.Size = new System.Drawing.Size(27, 13);
            this.minutesLabel.TabIndex = 2;
            this.minutesLabel.Text = "мин";
            // 
            // secondsUpDown
            // 
            this.secondsUpDown.Enabled = false;
            this.secondsUpDown.Increment = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.secondsUpDown.Location = new System.Drawing.Point(291, 21);
            this.secondsUpDown.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.secondsUpDown.Name = "secondsUpDown";
            this.secondsUpDown.Size = new System.Drawing.Size(60, 20);
            this.secondsUpDown.TabIndex = 3;
            this.secondsUpDown.DownButtonClicked += new System.EventHandler(this.secondsUpDown_DownButtonClicked);
            this.secondsUpDown.UpButtonClicked += new System.EventHandler(this.secondsUpDown_UpButtonClicked);
            // 
            // minutesUpDown
            // 
            this.minutesUpDown.Enabled = false;
            this.minutesUpDown.Increment = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.minutesUpDown.Location = new System.Drawing.Point(192, 21);
            this.minutesUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.minutesUpDown.Name = "minutesUpDown";
            this.minutesUpDown.Size = new System.Drawing.Size(60, 20);
            this.minutesUpDown.TabIndex = 1;
            this.minutesUpDown.DownButtonClicked += new System.EventHandler(this.minutesUpDown_DownButtonClicked);
            this.minutesUpDown.UpButtonClicked += new System.EventHandler(this.minutesUpDown_UpButtonClicked);
            // 
            // timeRestrictionCheckBox
            // 
            this.timeRestrictionCheckBox.AutoSize = true;
            this.timeRestrictionCheckBox.Location = new System.Drawing.Point(14, 22);
            this.timeRestrictionCheckBox.Name = "timeRestrictionCheckBox";
            this.timeRestrictionCheckBox.Size = new System.Drawing.Size(157, 17);
            this.timeRestrictionCheckBox.TabIndex = 0;
            this.timeRestrictionCheckBox.Text = "Ограничение по времени:";
            this.timeRestrictionCheckBox.UseVisualStyleBackColor = true;
            this.timeRestrictionCheckBox.CheckedChanged += new System.EventHandler(this.timeRestrictionCheckBox_CheckedChanged);
            // 
            // profileComboBox
            // 
            this.profileComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.profileComboBox.FormattingEnabled = true;
            this.profileComboBox.Location = new System.Drawing.Point(192, 48);
            this.profileComboBox.Name = "profileComboBox";
            this.profileComboBox.Size = new System.Drawing.Size(224, 21);
            this.profileComboBox.TabIndex = 6;
            this.profileComboBox.SelectedIndexChanged += new System.EventHandler(this.profileComboBox_SelectedIndexChanged);
            // 
            // profileLabel
            // 
            this.profileLabel.AutoSize = true;
            this.profileLabel.Location = new System.Drawing.Point(11, 51);
            this.profileLabel.Name = "profileLabel";
            this.profileLabel.Size = new System.Drawing.Size(175, 13);
            this.profileLabel.TabIndex = 5;
            this.profileLabel.Text = "Оцениваемый элемент профиля:";
            // 
            // marksUpDown
            // 
            this.marksUpDown.Location = new System.Drawing.Point(192, 76);
            this.marksUpDown.Name = "marksUpDown";
            this.marksUpDown.Size = new System.Drawing.Size(60, 20);
            this.marksUpDown.TabIndex = 8;
            // 
            // marksLabel
            // 
            this.marksLabel.AutoSize = true;
            this.marksLabel.Location = new System.Drawing.Point(78, 78);
            this.marksLabel.Name = "marksLabel";
            this.marksLabel.Size = new System.Drawing.Size(108, 13);
            this.marksLabel.TabIndex = 7;
            this.marksLabel.Text = "Количество баллов:";
            // 
            // questionCountGroupBox
            // 
            this.questionCountGroupBox.Controls.Add(this.percentUpDown);
            this.questionCountGroupBox.Controls.Add(this.questionsCountUpDown);
            this.questionCountGroupBox.Controls.Add(this.percentRadioButton);
            this.questionCountGroupBox.Controls.Add(this.questionsCountRadioButton);
            this.questionCountGroupBox.Location = new System.Drawing.Point(12, 155);
            this.questionCountGroupBox.Name = "questionCountGroupBox";
            this.questionCountGroupBox.Size = new System.Drawing.Size(430, 78);
            this.questionCountGroupBox.TabIndex = 3;
            this.questionCountGroupBox.TabStop = false;
            this.questionCountGroupBox.Text = "Количество выбираемых вопросов";
            // 
            // percentUpDown
            // 
            this.percentUpDown.Enabled = false;
            this.percentUpDown.Increment = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.percentUpDown.Location = new System.Drawing.Point(195, 46);
            this.percentUpDown.Name = "percentUpDown";
            this.percentUpDown.Size = new System.Drawing.Size(60, 20);
            this.percentUpDown.TabIndex = 3;
            this.percentUpDown.DownButtonClicked += new System.EventHandler(this.percentUpDown_DownButtonClicked);
            this.percentUpDown.UpButtonClicked += new System.EventHandler(this.percentUpDown_UpButtonClicked);
            // 
            // questionsCountUpDown
            // 
            this.questionsCountUpDown.Increment = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.questionsCountUpDown.Location = new System.Drawing.Point(195, 19);
            this.questionsCountUpDown.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.questionsCountUpDown.Name = "questionsCountUpDown";
            this.questionsCountUpDown.Size = new System.Drawing.Size(60, 20);
            this.questionsCountUpDown.TabIndex = 1;
            this.questionsCountUpDown.DownButtonClicked += new System.EventHandler(this.questionsCountUpDown_DownButtonClicked);
            this.questionsCountUpDown.UpButtonClicked += new System.EventHandler(this.questionsCountUpDown_UpButtonClicked);
            // 
            // percentRadioButton
            // 
            this.percentRadioButton.AutoSize = true;
            this.percentRadioButton.Location = new System.Drawing.Point(17, 46);
            this.percentRadioButton.Name = "percentRadioButton";
            this.percentRadioButton.Size = new System.Drawing.Size(164, 17);
            this.percentRadioButton.TabIndex = 2;
            this.percentRadioButton.TabStop = true;
            this.percentRadioButton.Text = "Количество вопросов (в %):";
            this.percentRadioButton.UseVisualStyleBackColor = true;
            this.percentRadioButton.CheckedChanged += new System.EventHandler(this.percentRadioButton_CheckedChanged);
            // 
            // questionsCountRadioButton
            // 
            this.questionsCountRadioButton.AutoSize = true;
            this.questionsCountRadioButton.Checked = true;
            this.questionsCountRadioButton.Location = new System.Drawing.Point(17, 19);
            this.questionsCountRadioButton.Name = "questionsCountRadioButton";
            this.questionsCountRadioButton.Size = new System.Drawing.Size(172, 17);
            this.questionsCountRadioButton.TabIndex = 0;
            this.questionsCountRadioButton.TabStop = true;
            this.questionsCountRadioButton.Text = "Количество вопросов (в шт.):";
            this.questionsCountRadioButton.UseVisualStyleBackColor = true;
            this.questionsCountRadioButton.CheckedChanged += new System.EventHandler(this.questionsCountRadioButton_CheckedChanged);
            // 
            // questionOptionsGroupBox
            // 
            this.questionOptionsGroupBox.Controls.Add(this.timeRestrictionCheckBox);
            this.questionOptionsGroupBox.Controls.Add(this.minutesUpDown);
            this.questionOptionsGroupBox.Controls.Add(this.marksUpDown);
            this.questionOptionsGroupBox.Controls.Add(this.secondsUpDown);
            this.questionOptionsGroupBox.Controls.Add(this.marksLabel);
            this.questionOptionsGroupBox.Controls.Add(this.minutesLabel);
            this.questionOptionsGroupBox.Controls.Add(this.profileComboBox);
            this.questionOptionsGroupBox.Controls.Add(this.secondsLabel);
            this.questionOptionsGroupBox.Controls.Add(this.profileLabel);
            this.questionOptionsGroupBox.Location = new System.Drawing.Point(12, 37);
            this.questionOptionsGroupBox.Name = "questionOptionsGroupBox";
            this.questionOptionsGroupBox.Size = new System.Drawing.Size(430, 112);
            this.questionOptionsGroupBox.TabIndex = 2;
            this.questionOptionsGroupBox.TabStop = false;
            this.questionOptionsGroupBox.Text = "Параметры вопросов";
            // 
            // GroupDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(454, 278);
            this.Controls.Add(this.questionOptionsGroupBox);
            this.Controls.Add(this.questionCountGroupBox);
            this.Controls.Add(this.questionsCountTextBox);
            this.Controls.Add(this.questionsCountLabel);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Name = "GroupDialog";
            this.Text = "Параметры группы";
            ((System.ComponentModel.ISupportInitialize)(this.secondsUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minutesUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.marksUpDown)).EndInit();
            this.questionCountGroupBox.ResumeLayout(false);
            this.questionCountGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.percentUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.questionsCountUpDown)).EndInit();
            this.questionOptionsGroupBox.ResumeLayout(false);
            this.questionOptionsGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TextBox questionsCountTextBox;
        private System.Windows.Forms.Label questionsCountLabel;
        private System.Windows.Forms.Label secondsLabel;
        private System.Windows.Forms.Label minutesLabel;
        private UpDown secondsUpDown;
        private UpDown minutesUpDown;
        private System.Windows.Forms.CheckBox timeRestrictionCheckBox;
        private System.Windows.Forms.ComboBox profileComboBox;
        private System.Windows.Forms.Label profileLabel;
        private System.Windows.Forms.NumericUpDown marksUpDown;
        private System.Windows.Forms.Label marksLabel;
        private System.Windows.Forms.GroupBox questionCountGroupBox;
        private UpDown percentUpDown;
        private UpDown questionsCountUpDown;
        private System.Windows.Forms.RadioButton percentRadioButton;
        private System.Windows.Forms.RadioButton questionsCountRadioButton;
        private System.Windows.Forms.GroupBox questionOptionsGroupBox;
    }
}