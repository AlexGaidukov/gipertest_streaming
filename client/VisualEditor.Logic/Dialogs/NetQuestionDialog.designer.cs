using VisualEditor.Utils.Controls;

namespace VisualEditor.Logic.Dialogs
{
    partial class NetQuestionDialog
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
            this.questionOptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.timeRestrictionCheckBox = new System.Windows.Forms.CheckBox();
            this.minutesUpDown = new UpDown();
            this.marksUpDown = new System.Windows.Forms.NumericUpDown();
            this.secondsUpDown = new UpDown();
            this.marksLabel = new System.Windows.Forms.Label();
            this.minutesLabel = new System.Windows.Forms.Label();
            this.profileComboBox = new System.Windows.Forms.ComboBox();
            this.secondsLabel = new System.Windows.Forms.Label();
            this.profileLabel = new System.Windows.Forms.Label();
            this.nextQuestionComboBox = new System.Windows.Forms.ComboBox();
            this.nextQuestionLabel = new System.Windows.Forms.Label();
            this.nextQuestionButton = new System.Windows.Forms.Button();
            this.viewNextQuestionButton = new System.Windows.Forms.Button();
            this.questionOptionsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minutesUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.marksUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.secondsUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(367, 271);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 10;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Enabled = false;
            this.okButton.Location = new System.Drawing.Point(286, 271);
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
            this.responseIsNumberCheckBox.Location = new System.Drawing.Point(12, 196);
            this.responseIsNumberCheckBox.Name = "responseIsNumberCheckBox";
            this.responseIsNumberCheckBox.Size = new System.Drawing.Size(114, 17);
            this.responseIsNumberCheckBox.TabIndex = 5;
            this.responseIsNumberCheckBox.Text = "Тип ответа число";
            this.responseIsNumberCheckBox.UseVisualStyleBackColor = true;
            this.responseIsNumberCheckBox.CheckedChanged += new System.EventHandler(this.responseIsNumberCheckBox_CheckedChanged);
            // 
            // hintButton
            // 
            this.hintButton.Location = new System.Drawing.Point(12, 227);
            this.hintButton.Name = "hintButton";
            this.hintButton.Size = new System.Drawing.Size(207, 23);
            this.hintButton.TabIndex = 6;
            this.hintButton.Text = "Редактировать подсказку...";
            this.hintButton.UseVisualStyleBackColor = true;
            this.hintButton.Click += new System.EventHandler(this.hintButton_Click);
            // 
            // resposeVariantsButton
            // 
            this.resposeVariantsButton.Location = new System.Drawing.Point(235, 227);
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
            this.bevel.Location = new System.Drawing.Point(6, 251);
            this.bevel.Name = "bevel";
            this.bevel.ShadowColor = System.Drawing.SystemColors.ButtonShadow;
            this.bevel.Size = new System.Drawing.Size(440, 10);
            this.bevel.TabIndex = 8;
            this.bevel.Text = "bevel1";
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
            this.questionOptionsGroupBox.Text = "Параметры";
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
            // marksUpDown
            // 
            this.marksUpDown.Location = new System.Drawing.Point(192, 76);
            this.marksUpDown.Name = "marksUpDown";
            this.marksUpDown.Size = new System.Drawing.Size(60, 20);
            this.marksUpDown.TabIndex = 8;
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
            // marksLabel
            // 
            this.marksLabel.AutoSize = true;
            this.marksLabel.Location = new System.Drawing.Point(78, 78);
            this.marksLabel.Name = "marksLabel";
            this.marksLabel.Size = new System.Drawing.Size(108, 13);
            this.marksLabel.TabIndex = 7;
            this.marksLabel.Text = "Количество баллов:";
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
            // secondsLabel
            // 
            this.secondsLabel.AutoSize = true;
            this.secondsLabel.Location = new System.Drawing.Point(357, 23);
            this.secondsLabel.Name = "secondsLabel";
            this.secondsLabel.Size = new System.Drawing.Size(25, 13);
            this.secondsLabel.TabIndex = 4;
            this.secondsLabel.Text = "сек";
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
            // nextQuestionComboBox
            // 
            this.nextQuestionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.nextQuestionComboBox.FormattingEnabled = true;
            this.nextQuestionComboBox.Location = new System.Drawing.Point(123, 165);
            this.nextQuestionComboBox.Name = "nextQuestionComboBox";
            this.nextQuestionComboBox.Size = new System.Drawing.Size(231, 21);
            this.nextQuestionComboBox.TabIndex = 42;
            this.nextQuestionComboBox.SelectedIndexChanged += new System.EventHandler(this.nextQuestionComboBox_SelectedIndexChanged);
            // 
            // nextQuestionLabel
            // 
            this.nextQuestionLabel.AutoSize = true;
            this.nextQuestionLabel.Location = new System.Drawing.Point(9, 168);
            this.nextQuestionLabel.Name = "nextQuestionLabel";
            this.nextQuestionLabel.Size = new System.Drawing.Size(108, 13);
            this.nextQuestionLabel.TabIndex = 41;
            this.nextQuestionLabel.Text = "Следующий вопрос:";
            // 
            // nextQuestionButton
            // 
            this.nextQuestionButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.nextQuestionButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.nextQuestionButton.FlatAppearance.BorderSize = 0;
            this.nextQuestionButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.nextQuestionButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.nextQuestionButton.Image = global::VisualEditor.Logic.Properties.Resources.NextQuestion;
            this.nextQuestionButton.Location = new System.Drawing.Point(360, 155);
            this.nextQuestionButton.Name = "nextQuestionButton";
            this.nextQuestionButton.Size = new System.Drawing.Size(38, 38);
            this.nextQuestionButton.TabIndex = 39;
            this.nextQuestionButton.UseVisualStyleBackColor = true;
            this.nextQuestionButton.Click += new System.EventHandler(this.nextQuestionButton_Click);
            // 
            // viewNextQuestionButton
            // 
            this.viewNextQuestionButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.viewNextQuestionButton.FlatAppearance.BorderSize = 0;
            this.viewNextQuestionButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.viewNextQuestionButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.viewNextQuestionButton.Image = global::VisualEditor.Logic.Properties.Resources.ViewNextQuestion;
            this.viewNextQuestionButton.Location = new System.Drawing.Point(404, 155);
            this.viewNextQuestionButton.Name = "viewNextQuestionButton";
            this.viewNextQuestionButton.Size = new System.Drawing.Size(38, 38);
            this.viewNextQuestionButton.TabIndex = 40;
            this.viewNextQuestionButton.UseVisualStyleBackColor = true;
            this.viewNextQuestionButton.Click += new System.EventHandler(this.viewNextQuestionButton_Click);
            // 
            // NetQuestionDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(453, 305);
            this.Controls.Add(this.nextQuestionComboBox);
            this.Controls.Add(this.nextQuestionLabel);
            this.Controls.Add(this.nextQuestionButton);
            this.Controls.Add(this.viewNextQuestionButton);
            this.Controls.Add(this.questionOptionsGroupBox);
            this.Controls.Add(this.bevel);
            this.Controls.Add(this.resposeVariantsButton);
            this.Controls.Add(this.hintButton);
            this.Controls.Add(this.responseIsNumberCheckBox);
            this.Controls.Add(this.questionTypeTextBox);
            this.Controls.Add(this.questionTypeLabel);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Name = "NetQuestionDialog";
            this.Text = "Параметры вопроса";
            this.questionOptionsGroupBox.ResumeLayout(false);
            this.questionOptionsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minutesUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.marksUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.secondsUpDown)).EndInit();
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
        private System.Windows.Forms.GroupBox questionOptionsGroupBox;
        private System.Windows.Forms.CheckBox timeRestrictionCheckBox;
        private UpDown minutesUpDown;
        private System.Windows.Forms.NumericUpDown marksUpDown;
        private UpDown secondsUpDown;
        private System.Windows.Forms.Label marksLabel;
        private System.Windows.Forms.Label minutesLabel;
        private System.Windows.Forms.ComboBox profileComboBox;
        private System.Windows.Forms.Label secondsLabel;
        private System.Windows.Forms.Label profileLabel;
        private System.Windows.Forms.ComboBox nextQuestionComboBox;
        private System.Windows.Forms.Label nextQuestionLabel;
        private System.Windows.Forms.Button nextQuestionButton;
        private System.Windows.Forms.Button viewNextQuestionButton;
    }
}