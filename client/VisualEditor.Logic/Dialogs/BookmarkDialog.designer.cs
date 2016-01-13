using System;
using VisualEditor.Logic.Controls.Trees;
using VisualEditor.Utils.Controls;

namespace VisualEditor.Logic.Dialogs
{
    partial class BookmarkDialog
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
            this.bookmarkNameLabel = new System.Windows.Forms.Label();
            this.bookmarkNameTextBox = new System.Windows.Forms.TextBox();
            this.addButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.navigateButton = new System.Windows.Forms.Button();
            this.wholeCourseRadioButton = new System.Windows.Forms.RadioButton();
            this.currentModuleRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.bevel = new Bevel();
            this.bookmarkTree = new VisualEditor.Logic.Controls.Trees.BookmarkTree();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(274, 312);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // bookmarkNameLabel
            // 
            this.bookmarkNameLabel.AutoSize = true;
            this.bookmarkNameLabel.Location = new System.Drawing.Point(9, 12);
            this.bookmarkNameLabel.Name = "bookmarkNameLabel";
            this.bookmarkNameLabel.Size = new System.Drawing.Size(32, 13);
            this.bookmarkNameLabel.TabIndex = 0;
            this.bookmarkNameLabel.Text = "Имя:";
            // 
            // bookmarkNameTextBox
            // 
            this.bookmarkNameTextBox.Location = new System.Drawing.Point(12, 28);
            this.bookmarkNameTextBox.Name = "bookmarkNameTextBox";
            this.bookmarkNameTextBox.Size = new System.Drawing.Size(249, 20);
            this.bookmarkNameTextBox.TabIndex = 1;
            this.bookmarkNameTextBox.TextChanged += new System.EventHandler(this.bookmarkNameTextBox_TextChanged);
            // 
            // addButton
            // 
            this.addButton.Enabled = false;
            this.addButton.Location = new System.Drawing.Point(274, 28);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 3;
            this.addButton.Text = "Добавить";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Enabled = false;
            this.deleteButton.Location = new System.Drawing.Point(274, 57);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 23);
            this.deleteButton.TabIndex = 4;
            this.deleteButton.Text = "Удалить";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // navigateButton
            // 
            this.navigateButton.Enabled = false;
            this.navigateButton.Location = new System.Drawing.Point(274, 86);
            this.navigateButton.Name = "navigateButton";
            this.navigateButton.Size = new System.Drawing.Size(75, 23);
            this.navigateButton.TabIndex = 5;
            this.navigateButton.Text = "Перейти";
            this.navigateButton.UseVisualStyleBackColor = true;
            this.navigateButton.Click += new System.EventHandler(this.navigateButton_Click);
            // 
            // wholeCourseRadioButton
            // 
            this.wholeCourseRadioButton.AutoSize = true;
            this.wholeCourseRadioButton.Checked = true;
            this.wholeCourseRadioButton.Location = new System.Drawing.Point(15, 19);
            this.wholeCourseRadioButton.Name = "wholeCourseRadioButton";
            this.wholeCourseRadioButton.Size = new System.Drawing.Size(143, 17);
            this.wholeCourseRadioButton.TabIndex = 0;
            this.wholeCourseRadioButton.TabStop = true;
            this.wholeCourseRadioButton.Text = "во всем учебном курсе";
            this.wholeCourseRadioButton.UseVisualStyleBackColor = true;
            this.wholeCourseRadioButton.CheckedChanged += new System.EventHandler(this.wholeCourseRadioButton_CheckedChanged);
            // 
            // currentModuleRadioButton
            // 
            this.currentModuleRadioButton.AutoSize = true;
            this.currentModuleRadioButton.Location = new System.Drawing.Point(15, 42);
            this.currentModuleRadioButton.Name = "currentModuleRadioButton";
            this.currentModuleRadioButton.Size = new System.Drawing.Size(119, 17);
            this.currentModuleRadioButton.TabIndex = 1;
            this.currentModuleRadioButton.Text = "в текущем модуле";
            this.currentModuleRadioButton.UseVisualStyleBackColor = true;
            this.currentModuleRadioButton.CheckedChanged += new System.EventHandler(this.currentModuleRadioButton_CheckedChanged);
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.wholeCourseRadioButton);
            this.groupBox.Controls.Add(this.currentModuleRadioButton);
            this.groupBox.Location = new System.Drawing.Point(12, 228);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(166, 73);
            this.groupBox.TabIndex = 6;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Расположение";
            // 
            // bevel
            // 
            this.bevel.BevelStyle = VisualEditor.Utils.Controls.BevelStyle.Lowered;
            this.bevel.BevelType = VisualEditor.Utils.Controls.BevelType.BottomLine;
            this.bevel.HighlightColor = System.Drawing.SystemColors.ButtonHighlight;
            this.bevel.Location = new System.Drawing.Point(184, 291);
            this.bevel.Name = "bevel";
            this.bevel.ShadowColor = System.Drawing.SystemColors.ButtonShadow;
            this.bevel.Size = new System.Drawing.Size(171, 10);
            this.bevel.TabIndex = 7;
            this.bevel.Text = "bevel1";
            // 
            // bookmarksTree
            // 
            this.bookmarkTree.CurrentNode = null;
            this.bookmarkTree.FullRowSelect = true;
            this.bookmarkTree.HideSelection = false;
            this.bookmarkTree.Location = new System.Drawing.Point(12, 49);
            this.bookmarkTree.Name = "bookmarksTree";
            this.bookmarkTree.ShowLines = false;
            this.bookmarkTree.ShowRootLines = false;
            this.bookmarkTree.Size = new System.Drawing.Size(249, 173);
            this.bookmarkTree.Sorted = true;
            this.bookmarkTree.TabIndex = 2;
            this.bookmarkTree.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.bookmarksTree_MouseDoubleClick);
            this.bookmarkTree.CurrentNodeChanged += new System.EventHandler(this.bookmarksTree_CurrentNodeChanged);
            this.bookmarkTree.KeyDown += new System.Windows.Forms.KeyEventHandler(this.bookmarksTree_KeyDown);
            // 
            // BookmarkDialog
            // 
            this.AcceptButton = this.addButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(362, 347);
            this.Controls.Add(this.bookmarkTree);
            this.Controls.Add(this.bevel);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.navigateButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.bookmarkNameTextBox);
            this.Controls.Add(this.bookmarkNameLabel);
            this.Controls.Add(this.cancelButton);
            this.Name = "BookmarkDialog";
            this.Text = "Закладка";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BookmarkDialog_FormClosing);
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label bookmarkNameLabel;
        private System.Windows.Forms.TextBox bookmarkNameTextBox;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button navigateButton;
        private System.Windows.Forms.RadioButton wholeCourseRadioButton;
        private System.Windows.Forms.RadioButton currentModuleRadioButton;
        private System.Windows.Forms.GroupBox groupBox;
        private Bevel bevel;
        private BookmarkTree bookmarkTree;
    }
}