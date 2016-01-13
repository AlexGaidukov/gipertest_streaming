namespace VisualEditor.Logic.Dialogs
{
    partial class AppSettingsDialog
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.savingTabPage = new System.Windows.Forms.TabPage();
            this.autosavingLabel = new System.Windows.Forms.Label();
            this.autosavingUpDown = new System.Windows.Forms.NumericUpDown();
            this.autosavingCheckBox = new System.Windows.Forms.CheckBox();
            this.invalidCourseCheckBox = new System.Windows.Forms.CheckBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.savingTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.autosavingUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.savingTabPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(546, 208);
            this.tabControl.TabIndex = 0;
            // 
            // savingTabPage
            // 
            this.savingTabPage.Controls.Add(this.autosavingLabel);
            this.savingTabPage.Controls.Add(this.autosavingUpDown);
            this.savingTabPage.Controls.Add(this.autosavingCheckBox);
            this.savingTabPage.Controls.Add(this.invalidCourseCheckBox);
            this.savingTabPage.Location = new System.Drawing.Point(4, 22);
            this.savingTabPage.Name = "savingTabPage";
            this.savingTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.savingTabPage.Size = new System.Drawing.Size(538, 182);
            this.savingTabPage.TabIndex = 1;
            this.savingTabPage.Text = "Сохранение курса";
            this.savingTabPage.UseVisualStyleBackColor = true;
            // 
            // autosavingLabel
            // 
            this.autosavingLabel.AutoSize = true;
            this.autosavingLabel.Location = new System.Drawing.Point(92, 57);
            this.autosavingLabel.Name = "autosavingLabel";
            this.autosavingLabel.Size = new System.Drawing.Size(37, 13);
            this.autosavingLabel.TabIndex = 4;
            this.autosavingLabel.Text = "минут";
            // 
            // autosavingUpDown
            // 
            this.autosavingUpDown.Location = new System.Drawing.Point(26, 55);
            this.autosavingUpDown.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.autosavingUpDown.Name = "autosavingUpDown";
            this.autosavingUpDown.Size = new System.Drawing.Size(60, 20);
            this.autosavingUpDown.TabIndex = 3;
            this.autosavingUpDown.ValueChanged += new System.EventHandler(this.autosavingUpDown_ValueChanged);
            // 
            // autosavingCheckBox
            // 
            this.autosavingCheckBox.AutoSize = true;
            this.autosavingCheckBox.Location = new System.Drawing.Point(7, 32);
            this.autosavingCheckBox.Name = "autosavingCheckBox";
            this.autosavingCheckBox.Size = new System.Drawing.Size(152, 17);
            this.autosavingCheckBox.TabIndex = 2;
            this.autosavingCheckBox.Text = "Автосохранение каждые";
            this.autosavingCheckBox.UseVisualStyleBackColor = true;
            this.autosavingCheckBox.CheckedChanged += new System.EventHandler(this.autosavingCheckBox_CheckedChanged);
            // 
            // invalidCourseCheckBox
            // 
            this.invalidCourseCheckBox.AutoSize = true;
            this.invalidCourseCheckBox.Location = new System.Drawing.Point(7, 8);
            this.invalidCourseCheckBox.Name = "invalidCourseCheckBox";
            this.invalidCourseCheckBox.Size = new System.Drawing.Size(272, 17);
            this.invalidCourseCheckBox.TabIndex = 0;
            this.invalidCourseCheckBox.Text = "Показывать диалог нарушения структуры курса";
            this.invalidCourseCheckBox.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(378, 219);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "ОК";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(459, 219);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // AppSettingsDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(546, 254);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.tabControl);
            this.Name = "AppSettingsDialog";
            this.Text = "Параметры редактора";
            this.tabControl.ResumeLayout(false);
            this.savingTabPage.ResumeLayout(false);
            this.savingTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.autosavingUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage savingTabPage;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.CheckBox invalidCourseCheckBox;
        private System.Windows.Forms.Label autosavingLabel;
        private System.Windows.Forms.NumericUpDown autosavingUpDown;
        private System.Windows.Forms.CheckBox autosavingCheckBox;
    }
}