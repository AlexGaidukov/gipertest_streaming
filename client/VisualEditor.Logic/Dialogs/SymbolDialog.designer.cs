namespace VisualEditor.Logic.Dialogs
{
    partial class SymbolDialog
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
            this.symbolsPanel = new VisualEditor.Logic.Dialogs.SymbolPanel();
            this.closeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // symbolsPanel
            // 
            this.symbolsPanel.AutoScroll = true;
            this.symbolsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.symbolsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.symbolsPanel.Location = new System.Drawing.Point(0, 0);
            this.symbolsPanel.Name = "symbolsPanel";
            this.symbolsPanel.Size = new System.Drawing.Size(488, 248);
            this.symbolsPanel.TabIndex = 0;
            // 
            // closeButton
            // 
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(401, 259);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "Закрыть";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // SymbolDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(488, 293);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.symbolsPanel);
            this.Name = "SymbolDialog";
            this.Text = "Символ";
            this.ResumeLayout(false);

        }

        #endregion

        private SymbolPanel symbolsPanel;
        private System.Windows.Forms.Button closeButton;
    }
}