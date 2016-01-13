namespace VisualEditor.Logic.Dialogs
{
    partial class LinkDialog
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
            this.linkTargetLabel = new System.Windows.Forms.Label();
            this.linkTextLabel = new System.Windows.Forms.Label();
            this.linkTextTextBox = new System.Windows.Forms.TextBox();
            this.targetTypesPanel = new System.Windows.Forms.Panel();
            this.hyperlinkButton = new System.Windows.Forms.Button();
            this.moduleButton = new System.Windows.Forms.Button();
            this.externalConceptButton = new System.Windows.Forms.Button();
            this.internalConceptButton = new System.Windows.Forms.Button();
            this.bookmarkButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.hyperlinkPanel = new System.Windows.Forms.Panel();
            this.urlTextBox = new System.Windows.Forms.TextBox();
            this.linkTypeComboBox = new System.Windows.Forms.ComboBox();
            this.urlLabel = new System.Windows.Forms.Label();
            this.linkTypeLabel = new System.Windows.Forms.Label();
            this.crosslinkPanel = new System.Windows.Forms.Panel();
            this.linkObjectListBox = new System.Windows.Forms.ListBox();
            this.linkObjectLabel = new System.Windows.Forms.Label();
            this.targetTypesPanel.SuspendLayout();
            this.hyperlinkPanel.SuspendLayout();
            this.crosslinkPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // linkTargetLabel
            // 
            this.linkTargetLabel.AutoSize = true;
            this.linkTargetLabel.Location = new System.Drawing.Point(12, 11);
            this.linkTargetLabel.Name = "linkTargetLabel";
            this.linkTargetLabel.Size = new System.Drawing.Size(61, 13);
            this.linkTargetLabel.TabIndex = 2;
            this.linkTargetLabel.Text = "Связать с:";
            // 
            // linkTextLabel
            // 
            this.linkTextLabel.AutoSize = true;
            this.linkTextLabel.Location = new System.Drawing.Point(112, 11);
            this.linkTextLabel.Name = "linkTextLabel";
            this.linkTextLabel.Size = new System.Drawing.Size(40, 13);
            this.linkTextLabel.TabIndex = 0;
            this.linkTextLabel.Text = "Текст:";
            // 
            // linkTextTextBox
            // 
            this.linkTextTextBox.Location = new System.Drawing.Point(158, 8);
            this.linkTextTextBox.Name = "linkTextTextBox";
            this.linkTextTextBox.Size = new System.Drawing.Size(286, 20);
            this.linkTextTextBox.TabIndex = 1;
            // 
            // targetTypesPanel
            // 
            this.targetTypesPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.targetTypesPanel.Controls.Add(this.hyperlinkButton);
            this.targetTypesPanel.Controls.Add(this.moduleButton);
            this.targetTypesPanel.Controls.Add(this.externalConceptButton);
            this.targetTypesPanel.Controls.Add(this.internalConceptButton);
            this.targetTypesPanel.Controls.Add(this.bookmarkButton);
            this.targetTypesPanel.Location = new System.Drawing.Point(14, 32);
            this.targetTypesPanel.Name = "targetTypesPanel";
            this.targetTypesPanel.Size = new System.Drawing.Size(90, 227);
            this.targetTypesPanel.TabIndex = 3;
            // 
            // hyperlinkButton
            // 
            this.hyperlinkButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.hyperlinkButton.FlatAppearance.BorderSize = 0;
            this.hyperlinkButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.hyperlinkButton.Location = new System.Drawing.Point(0, 180);
            this.hyperlinkButton.Name = "hyperlinkButton";
            this.hyperlinkButton.Size = new System.Drawing.Size(88, 45);
            this.hyperlinkButton.TabIndex = 4;
            this.hyperlinkButton.Text = "файлом, веб- страницей";
            this.hyperlinkButton.UseVisualStyleBackColor = true;
            this.hyperlinkButton.Click += new System.EventHandler(this.hyperlinkButton_Click);
            // 
            // moduleButton
            // 
            this.moduleButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.moduleButton.FlatAppearance.BorderSize = 0;
            this.moduleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.moduleButton.Location = new System.Drawing.Point(0, 135);
            this.moduleButton.Name = "moduleButton";
            this.moduleButton.Size = new System.Drawing.Size(88, 45);
            this.moduleButton.TabIndex = 3;
            this.moduleButton.Text = "модулем";
            this.moduleButton.UseVisualStyleBackColor = true;
            this.moduleButton.Click += new System.EventHandler(this.moduleButton_Click);
            // 
            // externalConceptButton
            // 
            this.externalConceptButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.externalConceptButton.FlatAppearance.BorderSize = 0;
            this.externalConceptButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.externalConceptButton.Location = new System.Drawing.Point(0, 90);
            this.externalConceptButton.Name = "externalConceptButton";
            this.externalConceptButton.Size = new System.Drawing.Size(88, 45);
            this.externalConceptButton.TabIndex = 2;
            this.externalConceptButton.Text = "внешней компетенцией";
            this.externalConceptButton.UseVisualStyleBackColor = true;
            this.externalConceptButton.Click += new System.EventHandler(this.externalConceptButton_Click);
            // 
            // internalConceptButton
            // 
            this.internalConceptButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.internalConceptButton.FlatAppearance.BorderSize = 0;
            this.internalConceptButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.internalConceptButton.Location = new System.Drawing.Point(0, 45);
            this.internalConceptButton.Name = "internalConceptButton";
            this.internalConceptButton.Size = new System.Drawing.Size(88, 45);
            this.internalConceptButton.TabIndex = 1;
            this.internalConceptButton.Text = "внутренней компетенцией";
            this.internalConceptButton.UseVisualStyleBackColor = true;
            this.internalConceptButton.Click += new System.EventHandler(this.internalConceptButton_Click);
            // 
            // bookmarkButton
            // 
            this.bookmarkButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.bookmarkButton.FlatAppearance.BorderSize = 0;
            this.bookmarkButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bookmarkButton.Location = new System.Drawing.Point(0, 0);
            this.bookmarkButton.Name = "bookmarkButton";
            this.bookmarkButton.Size = new System.Drawing.Size(88, 45);
            this.bookmarkButton.TabIndex = 0;
            this.bookmarkButton.Text = "закладкой";
            this.bookmarkButton.UseVisualStyleBackColor = false;
            this.bookmarkButton.Click += new System.EventHandler(this.bookmarkButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(369, 265);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Enabled = false;
            this.okButton.Location = new System.Drawing.Point(288, 265);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 6;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // hyperlinkPanel
            // 
            this.hyperlinkPanel.Controls.Add(this.urlTextBox);
            this.hyperlinkPanel.Controls.Add(this.linkTypeComboBox);
            this.hyperlinkPanel.Controls.Add(this.urlLabel);
            this.hyperlinkPanel.Controls.Add(this.linkTypeLabel);
            this.hyperlinkPanel.Location = new System.Drawing.Point(110, 32);
            this.hyperlinkPanel.Name = "hyperlinkPanel";
            this.hyperlinkPanel.Size = new System.Drawing.Size(341, 227);
            this.hyperlinkPanel.TabIndex = 4;
            this.hyperlinkPanel.Visible = false;
            // 
            // urlTextBox
            // 
            this.urlTextBox.Location = new System.Drawing.Point(48, 41);
            this.urlTextBox.Name = "urlTextBox";
            this.urlTextBox.Size = new System.Drawing.Size(286, 20);
            this.urlTextBox.TabIndex = 3;
            this.urlTextBox.TextChanged += new System.EventHandler(this.urlTextBox_TextChanged);
            // 
            // linkTypeComboBox
            // 
            this.linkTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.linkTypeComboBox.FormattingEnabled = true;
            this.linkTypeComboBox.Items.AddRange(new object[] {
            "file:",
            "ftp:",
            "http:",
            "https:"});
            this.linkTypeComboBox.Location = new System.Drawing.Point(48, 14);
            this.linkTypeComboBox.Name = "linkTypeComboBox";
            this.linkTypeComboBox.Size = new System.Drawing.Size(121, 21);
            this.linkTypeComboBox.TabIndex = 1;
            // 
            // urlLabel
            // 
            this.urlLabel.AutoSize = true;
            this.urlLabel.Location = new System.Drawing.Point(10, 44);
            this.urlLabel.Name = "urlLabel";
            this.urlLabel.Size = new System.Drawing.Size(32, 13);
            this.urlLabel.TabIndex = 2;
            this.urlLabel.Text = "URL:";
            // 
            // linkTypeLabel
            // 
            this.linkTypeLabel.AutoSize = true;
            this.linkTypeLabel.Location = new System.Drawing.Point(13, 17);
            this.linkTypeLabel.Name = "linkTypeLabel";
            this.linkTypeLabel.Size = new System.Drawing.Size(29, 13);
            this.linkTypeLabel.TabIndex = 0;
            this.linkTypeLabel.Text = "Тип:";
            // 
            // crosslinkPanel
            // 
            this.crosslinkPanel.Controls.Add(this.linkObjectListBox);
            this.crosslinkPanel.Controls.Add(this.linkObjectLabel);
            this.crosslinkPanel.Location = new System.Drawing.Point(110, 32);
            this.crosslinkPanel.Name = "crosslinkPanel";
            this.crosslinkPanel.Size = new System.Drawing.Size(341, 227);
            this.crosslinkPanel.TabIndex = 8;
            // 
            // linkObjectListBox
            // 
            this.linkObjectListBox.FormattingEnabled = true;
            this.linkObjectListBox.Location = new System.Drawing.Point(6, 21);
            this.linkObjectListBox.Name = "linkObjectListBox";
            this.linkObjectListBox.Size = new System.Drawing.Size(328, 199);
            this.linkObjectListBox.TabIndex = 1;
            this.linkObjectListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.linkObjectListBox_MouseDoubleClick);
            this.linkObjectListBox.SelectedIndexChanged += new System.EventHandler(this.linkObjectListBox_SelectedIndexChanged);
            // 
            // linkObjectLabel
            // 
            this.linkObjectLabel.AutoSize = true;
            this.linkObjectLabel.Location = new System.Drawing.Point(3, 5);
            this.linkObjectLabel.Name = "linkObjectLabel";
            this.linkObjectLabel.Size = new System.Drawing.Size(89, 13);
            this.linkObjectLabel.TabIndex = 0;
            this.linkObjectLabel.Text = "Объект ссылки:";
            // 
            // LinkDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(455, 299);
            this.Controls.Add(this.crosslinkPanel);
            this.Controls.Add(this.hyperlinkPanel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.targetTypesPanel);
            this.Controls.Add(this.linkTextTextBox);
            this.Controls.Add(this.linkTextLabel);
            this.Controls.Add(this.linkTargetLabel);
            this.Name = "LinkDialog";
            this.Text = "Ссылка";
            this.targetTypesPanel.ResumeLayout(false);
            this.hyperlinkPanel.ResumeLayout(false);
            this.hyperlinkPanel.PerformLayout();
            this.crosslinkPanel.ResumeLayout(false);
            this.crosslinkPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label linkTargetLabel;
        private System.Windows.Forms.Label linkTextLabel;
        private System.Windows.Forms.TextBox linkTextTextBox;
        private System.Windows.Forms.Panel targetTypesPanel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button hyperlinkButton;
        private System.Windows.Forms.Button moduleButton;
        private System.Windows.Forms.Button externalConceptButton;
        private System.Windows.Forms.Button internalConceptButton;
        private System.Windows.Forms.Button bookmarkButton;
        private System.Windows.Forms.Panel hyperlinkPanel;
        private System.Windows.Forms.TextBox urlTextBox;
        private System.Windows.Forms.ComboBox linkTypeComboBox;
        private System.Windows.Forms.Label urlLabel;
        private System.Windows.Forms.Label linkTypeLabel;
        private System.Windows.Forms.Panel crosslinkPanel;
        private System.Windows.Forms.ListBox linkObjectListBox;
        private System.Windows.Forms.Label linkObjectLabel;
    }
}