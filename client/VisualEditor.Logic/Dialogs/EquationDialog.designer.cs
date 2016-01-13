using VisualEditor.Utils.Controls;

namespace VisualEditor.Logic.Dialogs
{
    partial class EquationDialog
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
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.equationTextBox = new System.Windows.Forms.TextBox();
            this.equationPictureBox = new System.Windows.Forms.PictureBox();
            this.insertButton = new System.Windows.Forms.Button();
            this.examplesButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.bevel = new Bevel();
            this.helpLabel = new System.Windows.Forms.LinkLabel();
            this.onlineEditorLabel = new System.Windows.Forms.LinkLabel();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.equationPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Location = new System.Drawing.Point(9, 14);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.equationTextBox);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.equationPictureBox);
            this.splitContainer.Size = new System.Drawing.Size(426, 249);
            this.splitContainer.SplitterDistance = 126;
            this.splitContainer.SplitterWidth = 2;
            this.splitContainer.TabIndex = 0;
            // 
            // equationTextBox
            // 
            this.equationTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.equationTextBox.Location = new System.Drawing.Point(0, 0);
            this.equationTextBox.Multiline = true;
            this.equationTextBox.Name = "equationTextBox";
            this.equationTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.equationTextBox.Size = new System.Drawing.Size(426, 126);
            this.equationTextBox.TabIndex = 0;
            this.equationTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.equationTextBox.TextChanged += new System.EventHandler(this.equationTextBox_TextChanged);
            // 
            // equationPictureBox
            // 
            this.equationPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.equationPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.equationPictureBox.Location = new System.Drawing.Point(0, 0);
            this.equationPictureBox.Name = "equationPictureBox";
            this.equationPictureBox.Size = new System.Drawing.Size(426, 121);
            this.equationPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.equationPictureBox.TabIndex = 0;
            this.equationPictureBox.TabStop = false;
            // 
            // insertButton
            // 
            this.insertButton.Enabled = false;
            this.insertButton.Location = new System.Drawing.Point(450, 14);
            this.insertButton.Name = "insertButton";
            this.insertButton.Size = new System.Drawing.Size(75, 23);
            this.insertButton.TabIndex = 1;
            this.insertButton.Text = "Вставить";
            this.insertButton.UseVisualStyleBackColor = true;
            this.insertButton.Click += new System.EventHandler(this.insertButton_Click);
            // 
            // examplesButton
            // 
            this.examplesButton.Location = new System.Drawing.Point(450, 43);
            this.examplesButton.Name = "examplesButton";
            this.examplesButton.Size = new System.Drawing.Size(75, 23);
            this.examplesButton.TabIndex = 2;
            this.examplesButton.Text = "Примеры";
            this.examplesButton.UseVisualStyleBackColor = true;
            this.examplesButton.Click += new System.EventHandler(this.examplesButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(450, 275);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // bevel
            // 
            this.bevel.BevelStyle = BevelStyle.Lowered;
            this.bevel.BevelType = BevelType.BottomLine;
            this.bevel.HighlightColor = System.Drawing.SystemColors.ButtonHighlight;
            this.bevel.Location = new System.Drawing.Point(441, 253);
            this.bevel.Name = "bevel";
            this.bevel.ShadowColor = System.Drawing.SystemColors.ButtonShadow;
            this.bevel.Size = new System.Drawing.Size(90, 11);
            this.bevel.TabIndex = 3;
            // 
            // helpLabel
            // 
            this.helpLabel.AutoSize = true;
            this.helpLabel.Location = new System.Drawing.Point(6, 280);
            this.helpLabel.Name = "helpLabel";
            this.helpLabel.Size = new System.Drawing.Size(88, 13);
            this.helpLabel.TabIndex = 4;
            this.helpLabel.TabStop = true;
            this.helpLabel.Text = "Справка по TeX";
            this.helpLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.helpLabel_LinkClicked);
            // 
            // onlineEditorLabel
            // 
            this.onlineEditorLabel.AutoSize = true;
            this.onlineEditorLabel.Location = new System.Drawing.Point(100, 280);
            this.onlineEditorLabel.Name = "onlineEditorLabel";
            this.onlineEditorLabel.Size = new System.Drawing.Size(98, 13);
            this.onlineEditorLabel.TabIndex = 5;
            this.onlineEditorLabel.TabStop = true;
            this.onlineEditorLabel.Text = "Он-лайн редактор";
            this.onlineEditorLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.onlineEditorLabel_LinkClicked);
            // 
            // EquationDialog
            // 
            this.AcceptButton = this.insertButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(538, 309);
            this.Controls.Add(this.onlineEditorLabel);
            this.Controls.Add(this.helpLabel);
            this.Controls.Add(this.bevel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.examplesButton);
            this.Controls.Add(this.insertButton);
            this.Controls.Add(this.splitContainer);
            this.Name = "EquationDialog";
            this.Text = "Формула";
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.equationPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TextBox equationTextBox;
        private System.Windows.Forms.PictureBox equationPictureBox;
        private System.Windows.Forms.Button insertButton;
        private System.Windows.Forms.Button examplesButton;
        private System.Windows.Forms.Button cancelButton;
        private Bevel bevel;
        private System.Windows.Forms.LinkLabel helpLabel;
        private System.Windows.Forms.LinkLabel onlineEditorLabel;

    }
}