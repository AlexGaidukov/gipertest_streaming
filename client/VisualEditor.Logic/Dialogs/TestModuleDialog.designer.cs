using VisualEditor.Utils;
using VisualEditor.Utils.Controls;

namespace VisualEditor.Logic.Dialogs
{
    partial class TestModuleDialog
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
            this.mistakesNumberCheckBox = new System.Windows.Forms.CheckBox();
            this.timeRestrictionCheckBox = new System.Windows.Forms.CheckBox();
            this.trainerCheckBox = new System.Windows.Forms.CheckBox();
            this.questionSequenceLabel = new System.Windows.Forms.Label();
            this.mistakesNumberUpDown = new System.Windows.Forms.NumericUpDown();
            this.minutesUpDown = new UpDown();
            this.secondsUpDown = new UpDown();
            this.minutesLabel = new System.Windows.Forms.Label();
            this.secondsLabel = new System.Windows.Forms.Label();
            this.questionSequenceComboBox = new System.Windows.Forms.ComboBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.bevel = new Bevel();
            ((System.ComponentModel.ISupportInitialize)(this.mistakesNumberUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minutesUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.secondsUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // mistakesNumberCheckBox
            // 
            this.mistakesNumberCheckBox.AutoSize = true;
            this.mistakesNumberCheckBox.Location = new System.Drawing.Point(12, 12);
            this.mistakesNumberCheckBox.Name = "mistakesNumberCheckBox";
            this.mistakesNumberCheckBox.Size = new System.Drawing.Size(197, 17);
            this.mistakesNumberCheckBox.TabIndex = 0;
            this.mistakesNumberCheckBox.Text = "Ограничение количества ошибок:";
            this.mistakesNumberCheckBox.UseVisualStyleBackColor = true;
            this.mistakesNumberCheckBox.CheckedChanged += new System.EventHandler(this.mistakesNumberCheckBox_CheckedChanged);
            // 
            // timeRestrictionCheckBox
            // 
            this.timeRestrictionCheckBox.AutoSize = true;
            this.timeRestrictionCheckBox.Location = new System.Drawing.Point(12, 39);
            this.timeRestrictionCheckBox.Name = "timeRestrictionCheckBox";
            this.timeRestrictionCheckBox.Size = new System.Drawing.Size(157, 17);
            this.timeRestrictionCheckBox.TabIndex = 2;
            this.timeRestrictionCheckBox.Text = "Ограничение по времени:";
            this.timeRestrictionCheckBox.UseVisualStyleBackColor = true;
            this.timeRestrictionCheckBox.CheckedChanged += new System.EventHandler(this.timeRestrictionCheckBox_CheckedChanged);
            // 
            // trainerCheckBox
            // 
            this.trainerCheckBox.AutoSize = true;
            this.trainerCheckBox.Location = new System.Drawing.Point(12, 66);
            this.trainerCheckBox.Name = "trainerCheckBox";
            this.trainerCheckBox.Size = new System.Drawing.Size(77, 17);
            this.trainerCheckBox.TabIndex = 7;
            this.trainerCheckBox.Text = "Тренажер";
            this.trainerCheckBox.UseVisualStyleBackColor = true;
            // 
            // questionSequenceLabel
            // 
            this.questionSequenceLabel.AutoSize = true;
            this.questionSequenceLabel.Location = new System.Drawing.Point(9, 93);
            this.questionSequenceLabel.Name = "questionSequenceLabel";
            this.questionSequenceLabel.Size = new System.Drawing.Size(169, 13);
            this.questionSequenceLabel.TabIndex = 8;
            this.questionSequenceLabel.Text = "Последовательность вопросов:";
            // 
            // mistakesNumberUpDown
            // 
            this.mistakesNumberUpDown.Enabled = false;
            this.mistakesNumberUpDown.Location = new System.Drawing.Point(212, 11);
            this.mistakesNumberUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.mistakesNumberUpDown.Name = "mistakesNumberUpDown";
            this.mistakesNumberUpDown.Size = new System.Drawing.Size(60, 20);
            this.mistakesNumberUpDown.TabIndex = 1;
            // 
            // minutesUpDown
            // 
            this.minutesUpDown.Enabled = false;
            this.minutesUpDown.Increment = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.minutesUpDown.Location = new System.Drawing.Point(212, 38);
            this.minutesUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.minutesUpDown.Name = "minutesUpDown";
            this.minutesUpDown.Size = new System.Drawing.Size(60, 20);
            this.minutesUpDown.TabIndex = 3;
            this.minutesUpDown.DownButtonClicked += new System.EventHandler(this.minutesUpDown_DownButtonClicked);
            this.minutesUpDown.UpButtonClicked += new System.EventHandler(this.minutesUpDown_UpButtonClicked);
            // 
            // secondsUpDown
            // 
            this.secondsUpDown.Enabled = false;
            this.secondsUpDown.Increment = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.secondsUpDown.Location = new System.Drawing.Point(311, 38);
            this.secondsUpDown.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.secondsUpDown.Name = "secondsUpDown";
            this.secondsUpDown.Size = new System.Drawing.Size(60, 20);
            this.secondsUpDown.TabIndex = 5;
            this.secondsUpDown.DownButtonClicked += new System.EventHandler(this.secondsUpDown_DownButtonClicked);
            this.secondsUpDown.UpButtonClicked += new System.EventHandler(this.secondsUpDown_UpButtonClicked);
            // 
            // minutesLabel
            // 
            this.minutesLabel.AutoSize = true;
            this.minutesLabel.Location = new System.Drawing.Point(278, 43);
            this.minutesLabel.Name = "minutesLabel";
            this.minutesLabel.Size = new System.Drawing.Size(27, 13);
            this.minutesLabel.TabIndex = 4;
            this.minutesLabel.Text = "мин";
            // 
            // secondsLabel
            // 
            this.secondsLabel.AutoSize = true;
            this.secondsLabel.Location = new System.Drawing.Point(377, 43);
            this.secondsLabel.Name = "secondsLabel";
            this.secondsLabel.Size = new System.Drawing.Size(25, 13);
            this.secondsLabel.TabIndex = 6;
            this.secondsLabel.Text = "сек";
            // 
            // questionSequenceComboBox
            // 
            this.questionSequenceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.questionSequenceComboBox.FormattingEnabled = true;
            this.questionSequenceComboBox.Items.AddRange(new object[] {
            "естественная",
            "случайная",
            "сетевая"});
            this.questionSequenceComboBox.Location = new System.Drawing.Point(212, 90);
            this.questionSequenceComboBox.Name = "questionSequenceComboBox";
            this.questionSequenceComboBox.Size = new System.Drawing.Size(115, 21);
            this.questionSequenceComboBox.TabIndex = 9;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(322, 133);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 12;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(241, 133);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 11;
            this.okButton.Text = "ОК";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // bevel
            // 
            this.bevel.BevelStyle = BevelStyle.Lowered;
            this.bevel.BevelType = BevelType.BottomLine;
            this.bevel.HighlightColor = System.Drawing.SystemColors.ButtonHighlight;
            this.bevel.Location = new System.Drawing.Point(7, 113);
            this.bevel.Name = "bevel";
            this.bevel.ShadowColor = System.Drawing.SystemColors.ButtonShadow;
            this.bevel.Size = new System.Drawing.Size(393, 10);
            this.bevel.TabIndex = 10;
            this.bevel.Text = "bevel1";
            // 
            // TestModuleDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(408, 166);
            this.Controls.Add(this.bevel);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.questionSequenceComboBox);
            this.Controls.Add(this.secondsLabel);
            this.Controls.Add(this.minutesLabel);
            this.Controls.Add(this.secondsUpDown);
            this.Controls.Add(this.minutesUpDown);
            this.Controls.Add(this.mistakesNumberUpDown);
            this.Controls.Add(this.questionSequenceLabel);
            this.Controls.Add(this.trainerCheckBox);
            this.Controls.Add(this.timeRestrictionCheckBox);
            this.Controls.Add(this.mistakesNumberCheckBox);
            this.Name = "TestModuleDialog";
            this.Text = "Параметры контроля";
            ((System.ComponentModel.ISupportInitialize)(this.mistakesNumberUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minutesUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.secondsUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox mistakesNumberCheckBox;
        private System.Windows.Forms.CheckBox timeRestrictionCheckBox;
        private System.Windows.Forms.CheckBox trainerCheckBox;
        private System.Windows.Forms.Label questionSequenceLabel;
        private System.Windows.Forms.NumericUpDown mistakesNumberUpDown;
        private UpDown minutesUpDown;
        private UpDown secondsUpDown;
        private System.Windows.Forms.Label minutesLabel;
        private System.Windows.Forms.Label secondsLabel;
        private System.Windows.Forms.ComboBox questionSequenceComboBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private Bevel bevel;
    }
}