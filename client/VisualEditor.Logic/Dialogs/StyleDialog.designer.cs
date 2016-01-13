using VisualEditor.Utils.Controls;

namespace VisualEditor.Logic.Dialogs
{
    partial class StyleDialog
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
            this.bevel = new Bevel();
            this.styleNameComboBox = new System.Windows.Forms.ComboBox();
            this.styleNameLabel = new System.Windows.Forms.Label();
            this.hintTextTextBox = new System.Windows.Forms.TextBox();
            this.hintTextLabel = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bevel
            // 
            this.bevel.BevelStyle = BevelStyle.Lowered;
            this.bevel.BevelType = BevelType.BottomLine;
            this.bevel.HighlightColor = System.Drawing.SystemColors.ButtonHighlight;
            this.bevel.Location = new System.Drawing.Point(8, 60);
            this.bevel.Name = "bevel";
            this.bevel.ShadowColor = System.Drawing.SystemColors.ButtonShadow;
            this.bevel.Size = new System.Drawing.Size(358, 10);
            this.bevel.TabIndex = 4;
            this.bevel.Text = "bevel1";
            // 
            // styleNameComboBox
            // 
            this.styleNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.styleNameComboBox.FormattingEnabled = true;
            this.styleNameComboBox.Items.AddRange(new object[] {
            "нет",
            "удалить стиль",
            "внимание",
            "выводы",
            "определение",
            "ключевые слова",
            "рамка",
            "текст примера",
            "формула",
            "подсказка",
            "информация",
            "задание"});
            this.styleNameComboBox.Location = new System.Drawing.Point(81, 11);
            this.styleNameComboBox.Name = "styleNameComboBox";
            this.styleNameComboBox.Size = new System.Drawing.Size(119, 21);
            this.styleNameComboBox.TabIndex = 1;
            this.styleNameComboBox.SelectedIndexChanged += new System.EventHandler(this.styleComboBox_SelectedIndexChanged);
            // 
            // styleNameLabel
            // 
            this.styleNameLabel.AutoSize = true;
            this.styleNameLabel.Location = new System.Drawing.Point(9, 14);
            this.styleNameLabel.Name = "styleNameLabel";
            this.styleNameLabel.Size = new System.Drawing.Size(64, 13);
            this.styleNameLabel.TabIndex = 0;
            this.styleNameLabel.Text = "Имя стиля:";
            // 
            // hintTextTextBox
            // 
            this.hintTextTextBox.Enabled = false;
            this.hintTextTextBox.Location = new System.Drawing.Point(81, 38);
            this.hintTextTextBox.Name = "hintTextTextBox";
            this.hintTextTextBox.Size = new System.Drawing.Size(282, 20);
            this.hintTextTextBox.TabIndex = 3;
            // 
            // hintTextLabel
            // 
            this.hintTextLabel.AutoSize = true;
            this.hintTextLabel.Location = new System.Drawing.Point(9, 41);
            this.hintTextLabel.Name = "hintTextLabel";
            this.hintTextLabel.Size = new System.Drawing.Size(66, 13);
            this.hintTextLabel.TabIndex = 2;
            this.hintTextLabel.Text = "Подсказка:";
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(288, 80);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Enabled = false;
            this.okButton.Location = new System.Drawing.Point(207, 80);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 5;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // StyleDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(375, 114);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.hintTextTextBox);
            this.Controls.Add(this.hintTextLabel);
            this.Controls.Add(this.styleNameComboBox);
            this.Controls.Add(this.styleNameLabel);
            this.Controls.Add(this.bevel);
            this.Name = "StyleDialog";
            this.Text = "Стиль";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Bevel bevel;
        private System.Windows.Forms.ComboBox styleNameComboBox;
        private System.Windows.Forms.Label styleNameLabel;
        private System.Windows.Forms.TextBox hintTextTextBox;
        private System.Windows.Forms.Label hintTextLabel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
    }
}