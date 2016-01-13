using VisualEditor.Utils.Controls;

namespace VisualEditor.Logic.Dialogs
{
    partial class ResponseVariantDialog
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
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.variantsTabControl = new System.Windows.Forms.TabControl();
            this.weightNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.weightLabel = new System.Windows.Forms.Label();
            this.deleteVariantButton = new System.Windows.Forms.Button();
            this.addVariantButton = new System.Windows.Forms.Button();
            this.hintButton = new System.Windows.Forms.Button();
            this.viewVariantButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.weightNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // bevel
            // 
            this.bevel.BevelStyle = BevelStyle.Lowered;
            this.bevel.BevelType = BevelType.BottomLine;
            this.bevel.HighlightColor = System.Drawing.SystemColors.ButtonHighlight;
            this.bevel.Location = new System.Drawing.Point(8, 320);
            this.bevel.Name = "bevel";
            this.bevel.ShadowColor = System.Drawing.SystemColors.ButtonShadow;
            this.bevel.Size = new System.Drawing.Size(487, 10);
            this.bevel.TabIndex = 7;
            this.bevel.Text = "bevel";
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(414, 340);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Enabled = false;
            this.okButton.Location = new System.Drawing.Point(333, 340);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 8;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // variantsTabControl
            // 
            this.variantsTabControl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.variantsTabControl.Location = new System.Drawing.Point(10, 9);
            this.variantsTabControl.Name = "variantsTabControl";
            this.variantsTabControl.SelectedIndex = 0;
            this.variantsTabControl.Size = new System.Drawing.Size(354, 255);
            this.variantsTabControl.TabIndex = 0;
            this.variantsTabControl.SelectedIndexChanged += new System.EventHandler(this.variantsTabControl_SelectedIndexChanged);
            // 
            // weightNumericUpDown
            // 
            this.weightNumericUpDown.DecimalPlaces = 1;
            this.weightNumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.weightNumericUpDown.Location = new System.Drawing.Point(211, 269);
            this.weightNumericUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.weightNumericUpDown.Name = "weightNumericUpDown";
            this.weightNumericUpDown.Size = new System.Drawing.Size(60, 20);
            this.weightNumericUpDown.TabIndex = 4;
            this.weightNumericUpDown.ValueChanged += new System.EventHandler(this.weightNumericUpDown_ValueChanged);
            // 
            // weightLabel
            // 
            this.weightLabel.AutoSize = true;
            this.weightLabel.Location = new System.Drawing.Point(7, 271);
            this.weightLabel.Name = "weightLabel";
            this.weightLabel.Size = new System.Drawing.Size(198, 13);
            this.weightLabel.TabIndex = 3;
            this.weightLabel.Text = "Мера правильности варианта ответа:";
            // 
            // deleteVariantButton
            // 
            this.deleteVariantButton.Location = new System.Drawing.Point(375, 39);
            this.deleteVariantButton.Name = "deleteVariantButton";
            this.deleteVariantButton.Size = new System.Drawing.Size(114, 23);
            this.deleteVariantButton.TabIndex = 2;
            this.deleteVariantButton.Text = "Удалить вариант";
            this.deleteVariantButton.UseVisualStyleBackColor = true;
            this.deleteVariantButton.Click += new System.EventHandler(this.deleteVariantButton_Click);
            // 
            // addVariantButton
            // 
            this.addVariantButton.Location = new System.Drawing.Point(375, 10);
            this.addVariantButton.Name = "addVariantButton";
            this.addVariantButton.Size = new System.Drawing.Size(114, 23);
            this.addVariantButton.TabIndex = 1;
            this.addVariantButton.Text = "Добавить вариант";
            this.addVariantButton.UseVisualStyleBackColor = true;
            this.addVariantButton.Click += new System.EventHandler(this.addVariantButton_Click);
            // 
            // hintButton
            // 
            this.hintButton.Location = new System.Drawing.Point(10, 296);
            this.hintButton.Name = "hintButton";
            this.hintButton.Size = new System.Drawing.Size(164, 23);
            this.hintButton.TabIndex = 5;
            this.hintButton.Text = "Редактировать подсказку...";
            this.hintButton.UseVisualStyleBackColor = true;
            this.hintButton.Click += new System.EventHandler(this.hintButton_Click);
            // 
            // viewVariantButton
            // 
            this.viewVariantButton.Location = new System.Drawing.Point(180, 296);
            this.viewVariantButton.Name = "viewVariantButton";
            this.viewVariantButton.Size = new System.Drawing.Size(184, 23);
            this.viewVariantButton.TabIndex = 6;
            this.viewVariantButton.Text = "Просмотреть вариант ответа...";
            this.viewVariantButton.UseVisualStyleBackColor = true;
            this.viewVariantButton.Click += new System.EventHandler(this.viewVariantButton_Click);
            // 
            // ResponseVariantDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(502, 375);
            this.Controls.Add(this.viewVariantButton);
            this.Controls.Add(this.hintButton);
            this.Controls.Add(this.deleteVariantButton);
            this.Controls.Add(this.addVariantButton);
            this.Controls.Add(this.weightNumericUpDown);
            this.Controls.Add(this.weightLabel);
            this.Controls.Add(this.variantsTabControl);
            this.Controls.Add(this.bevel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Name = "ResponseVariantDialog";
            this.Text = "Варианты ответа";
            this.Load += new System.EventHandler(this.ResponseVariantDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.weightNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Bevel bevel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TabControl variantsTabControl;
        private System.Windows.Forms.NumericUpDown weightNumericUpDown;
        private System.Windows.Forms.Label weightLabel;
        private System.Windows.Forms.Button deleteVariantButton;
        private System.Windows.Forms.Button addVariantButton;
        private System.Windows.Forms.Button hintButton;
        private System.Windows.Forms.Button viewVariantButton;
    }
}