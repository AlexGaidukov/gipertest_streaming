namespace VisualEditor.Logic.Dialogs
{
    partial class TableDialog
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.dimensionPage = new System.Windows.Forms.TabPage();
            this.tableHeightUpDown = new System.Windows.Forms.NumericUpDown();
            this.tableWidthUpDown = new System.Windows.Forms.NumericUpDown();
            this.rowsNumberUpDown = new System.Windows.Forms.NumericUpDown();
            this.columnsNumberUpDown = new System.Windows.Forms.NumericUpDown();
            this.tableHeightUnitComboBox = new System.Windows.Forms.ComboBox();
            this.tableWidthUnitComboBox = new System.Windows.Forms.ComboBox();
            this.columnsNumberLabel = new System.Windows.Forms.Label();
            this.rowsNumberLabel = new System.Windows.Forms.Label();
            this.tableWidthLabel = new System.Windows.Forms.Label();
            this.tableHeightLabel = new System.Windows.Forms.Label();
            this.appearancePage = new System.Windows.Forms.TabPage();
            this.tableJustifyComboBox = new System.Windows.Forms.ComboBox();
            this.tableColorButton = new System.Windows.Forms.Button();
            this.colorLabel = new System.Windows.Forms.Label();
            this.tableJustifyLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.marginUpDown = new System.Windows.Forms.NumericUpDown();
            this.borderUpDown = new System.Windows.Forms.NumericUpDown();
            this.innerMarginUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.innerMarginLabel = new System.Windows.Forms.Label();
            this.marginLabel = new System.Windows.Forms.Label();
            this.frameLabel = new System.Windows.Forms.Label();
            this.titlePage = new System.Windows.Forms.TabPage();
            this.tableTitleTextBox = new System.Windows.Forms.TextBox();
            this.tableTitleLocationComboBox = new System.Windows.Forms.ComboBox();
            this.tableTitleLocationLabel = new System.Windows.Forms.Label();
            this.tableTitleLabel = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.dimensionPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tableHeightUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableWidthUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rowsNumberUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.columnsNumberUpDown)).BeginInit();
            this.appearancePage.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.marginUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.borderUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.innerMarginUpDown)).BeginInit();
            this.titlePage.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(401, 219);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(320, 219);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.dimensionPage);
            this.tabControl.Controls.Add(this.appearancePage);
            this.tabControl.Controls.Add(this.titlePage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(488, 208);
            this.tabControl.TabIndex = 0;
            // 
            // dimensionPage
            // 
            this.dimensionPage.Controls.Add(this.tableHeightUpDown);
            this.dimensionPage.Controls.Add(this.tableWidthUpDown);
            this.dimensionPage.Controls.Add(this.rowsNumberUpDown);
            this.dimensionPage.Controls.Add(this.columnsNumberUpDown);
            this.dimensionPage.Controls.Add(this.tableHeightUnitComboBox);
            this.dimensionPage.Controls.Add(this.tableWidthUnitComboBox);
            this.dimensionPage.Controls.Add(this.columnsNumberLabel);
            this.dimensionPage.Controls.Add(this.rowsNumberLabel);
            this.dimensionPage.Controls.Add(this.tableWidthLabel);
            this.dimensionPage.Controls.Add(this.tableHeightLabel);
            this.dimensionPage.Location = new System.Drawing.Point(4, 22);
            this.dimensionPage.Name = "dimensionPage";
            this.dimensionPage.Padding = new System.Windows.Forms.Padding(3);
            this.dimensionPage.Size = new System.Drawing.Size(480, 182);
            this.dimensionPage.TabIndex = 0;
            this.dimensionPage.Text = "Размер";
            this.dimensionPage.UseVisualStyleBackColor = true;
            // 
            // tableHeightUpDown
            // 
            this.tableHeightUpDown.Location = new System.Drawing.Point(201, 37);
            this.tableHeightUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.tableHeightUpDown.Name = "tableHeightUpDown";
            this.tableHeightUpDown.Size = new System.Drawing.Size(60, 20);
            this.tableHeightUpDown.TabIndex = 8;
            this.tableHeightUpDown.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // tableWidthUpDown
            // 
            this.tableWidthUpDown.Location = new System.Drawing.Point(201, 11);
            this.tableWidthUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.tableWidthUpDown.Name = "tableWidthUpDown";
            this.tableWidthUpDown.Size = new System.Drawing.Size(60, 20);
            this.tableWidthUpDown.TabIndex = 5;
            this.tableWidthUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // rowsNumberUpDown
            // 
            this.rowsNumberUpDown.Location = new System.Drawing.Point(68, 37);
            this.rowsNumberUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.rowsNumberUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.rowsNumberUpDown.Name = "rowsNumberUpDown";
            this.rowsNumberUpDown.Size = new System.Drawing.Size(60, 20);
            this.rowsNumberUpDown.TabIndex = 3;
            this.rowsNumberUpDown.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // columnsNumberUpDown
            // 
            this.columnsNumberUpDown.Location = new System.Drawing.Point(68, 11);
            this.columnsNumberUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.columnsNumberUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.columnsNumberUpDown.Name = "columnsNumberUpDown";
            this.columnsNumberUpDown.Size = new System.Drawing.Size(60, 20);
            this.columnsNumberUpDown.TabIndex = 1;
            this.columnsNumberUpDown.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // tableHeightUnitComboBox
            // 
            this.tableHeightUnitComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tableHeightUnitComboBox.FormattingEnabled = true;
            this.tableHeightUnitComboBox.Items.AddRange(new object[] {
            "пикселов",
            "% от размера окна"});
            this.tableHeightUnitComboBox.Location = new System.Drawing.Point(267, 37);
            this.tableHeightUnitComboBox.Name = "tableHeightUnitComboBox";
            this.tableHeightUnitComboBox.Size = new System.Drawing.Size(121, 21);
            this.tableHeightUnitComboBox.TabIndex = 9;
            this.tableHeightUnitComboBox.SelectedIndexChanged += new System.EventHandler(this.tableHeightUnitComboBox_SelectedIndexChanged);
            // 
            // tableWidthUnitComboBox
            // 
            this.tableWidthUnitComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tableWidthUnitComboBox.FormattingEnabled = true;
            this.tableWidthUnitComboBox.Items.AddRange(new object[] {
            "пикселов",
            "% от размера окна"});
            this.tableWidthUnitComboBox.Location = new System.Drawing.Point(267, 10);
            this.tableWidthUnitComboBox.Name = "tableWidthUnitComboBox";
            this.tableWidthUnitComboBox.Size = new System.Drawing.Size(121, 21);
            this.tableWidthUnitComboBox.TabIndex = 6;
            this.tableWidthUnitComboBox.SelectedIndexChanged += new System.EventHandler(this.tableWidthUnitComboBox_SelectedIndexChanged);
            // 
            // columnsNumberLabel
            // 
            this.columnsNumberLabel.AutoSize = true;
            this.columnsNumberLabel.Location = new System.Drawing.Point(8, 13);
            this.columnsNumberLabel.Name = "columnsNumberLabel";
            this.columnsNumberLabel.Size = new System.Drawing.Size(54, 13);
            this.columnsNumberLabel.TabIndex = 0;
            this.columnsNumberLabel.Text = "Столбцы:";
            // 
            // rowsNumberLabel
            // 
            this.rowsNumberLabel.AutoSize = true;
            this.rowsNumberLabel.Location = new System.Drawing.Point(16, 39);
            this.rowsNumberLabel.Name = "rowsNumberLabel";
            this.rowsNumberLabel.Size = new System.Drawing.Size(46, 13);
            this.rowsNumberLabel.TabIndex = 2;
            this.rowsNumberLabel.Text = "Строки:";
            // 
            // tableWidthLabel
            // 
            this.tableWidthLabel.AutoSize = true;
            this.tableWidthLabel.Location = new System.Drawing.Point(146, 13);
            this.tableWidthLabel.Name = "tableWidthLabel";
            this.tableWidthLabel.Size = new System.Drawing.Size(49, 13);
            this.tableWidthLabel.TabIndex = 4;
            this.tableWidthLabel.Text = "Ширина:";
            // 
            // tableHeightLabel
            // 
            this.tableHeightLabel.AutoSize = true;
            this.tableHeightLabel.Location = new System.Drawing.Point(147, 39);
            this.tableHeightLabel.Name = "tableHeightLabel";
            this.tableHeightLabel.Size = new System.Drawing.Size(48, 13);
            this.tableHeightLabel.TabIndex = 7;
            this.tableHeightLabel.Text = "Высота:";
            // 
            // appearancePage
            // 
            this.appearancePage.Controls.Add(this.tableJustifyComboBox);
            this.appearancePage.Controls.Add(this.tableColorButton);
            this.appearancePage.Controls.Add(this.colorLabel);
            this.appearancePage.Controls.Add(this.tableJustifyLabel);
            this.appearancePage.Controls.Add(this.groupBox1);
            this.appearancePage.Location = new System.Drawing.Point(4, 22);
            this.appearancePage.Name = "appearancePage";
            this.appearancePage.Size = new System.Drawing.Size(480, 182);
            this.appearancePage.TabIndex = 1;
            this.appearancePage.Text = "Внешний вид";
            this.appearancePage.UseVisualStyleBackColor = true;
            // 
            // tableJustifyComboBox
            // 
            this.tableJustifyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tableJustifyComboBox.FormattingEnabled = true;
            this.tableJustifyComboBox.Items.AddRange(new object[] {
            "влево",
            "по центру",
            "вправо"});
            this.tableJustifyComboBox.Location = new System.Drawing.Point(145, 121);
            this.tableJustifyComboBox.Name = "tableJustifyComboBox";
            this.tableJustifyComboBox.Size = new System.Drawing.Size(75, 21);
            this.tableJustifyComboBox.TabIndex = 2;
            // 
            // tableColorButton
            // 
            this.tableColorButton.BackColor = System.Drawing.SystemColors.Control;
            this.tableColorButton.Location = new System.Drawing.Point(145, 148);
            this.tableColorButton.Name = "tableColorButton";
            this.tableColorButton.Size = new System.Drawing.Size(27, 23);
            this.tableColorButton.TabIndex = 4;
            this.tableColorButton.UseVisualStyleBackColor = false;
            this.tableColorButton.Click += new System.EventHandler(this.tableColorButton_Click);
            // 
            // colorLabel
            // 
            this.colorLabel.AutoSize = true;
            this.colorLabel.Location = new System.Drawing.Point(75, 153);
            this.colorLabel.Name = "colorLabel";
            this.colorLabel.Size = new System.Drawing.Size(64, 13);
            this.colorLabel.TabIndex = 3;
            this.colorLabel.Text = "Цвет фона:";
            // 
            // tableJustifyLabel
            // 
            this.tableJustifyLabel.AutoSize = true;
            this.tableJustifyLabel.Location = new System.Drawing.Point(8, 124);
            this.tableJustifyLabel.Name = "tableJustifyLabel";
            this.tableJustifyLabel.Size = new System.Drawing.Size(131, 13);
            this.tableJustifyLabel.TabIndex = 1;
            this.tableJustifyLabel.Text = "Выравнивание таблицы:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.marginUpDown);
            this.groupBox1.Controls.Add(this.borderUpDown);
            this.groupBox1.Controls.Add(this.innerMarginUpDown);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.innerMarginLabel);
            this.groupBox1.Controls.Add(this.marginLabel);
            this.groupBox1.Controls.Add(this.frameLabel);
            this.groupBox1.Location = new System.Drawing.Point(8, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(464, 102);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Обрамление и поля";
            // 
            // marginUpDown
            // 
            this.marginUpDown.Location = new System.Drawing.Point(126, 43);
            this.marginUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.marginUpDown.Name = "marginUpDown";
            this.marginUpDown.Size = new System.Drawing.Size(60, 20);
            this.marginUpDown.TabIndex = 4;
            // 
            // borderUpDown
            // 
            this.borderUpDown.Location = new System.Drawing.Point(126, 17);
            this.borderUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.borderUpDown.Name = "borderUpDown";
            this.borderUpDown.Size = new System.Drawing.Size(60, 20);
            this.borderUpDown.TabIndex = 1;
            this.borderUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // innerMarginUpDown
            // 
            this.innerMarginUpDown.Location = new System.Drawing.Point(126, 70);
            this.innerMarginUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.innerMarginUpDown.Name = "innerMarginUpDown";
            this.innerMarginUpDown.Size = new System.Drawing.Size(60, 20);
            this.innerMarginUpDown.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(192, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(265, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "пикселов между рамкой ячейки и ее содержимым";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(192, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "пикселов между ячейками";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(192, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "пикселов";
            // 
            // innerMarginLabel
            // 
            this.innerMarginLabel.AutoSize = true;
            this.innerMarginLabel.Location = new System.Drawing.Point(9, 72);
            this.innerMarginLabel.Name = "innerMarginLabel";
            this.innerMarginLabel.Size = new System.Drawing.Size(111, 13);
            this.innerMarginLabel.TabIndex = 6;
            this.innerMarginLabel.Text = "Поля внутри ячейки:";
            // 
            // marginLabel
            // 
            this.marginLabel.AutoSize = true;
            this.marginLabel.Location = new System.Drawing.Point(84, 45);
            this.marginLabel.Name = "marginLabel";
            this.marginLabel.Size = new System.Drawing.Size(36, 13);
            this.marginLabel.TabIndex = 3;
            this.marginLabel.Text = "Поля:";
            // 
            // frameLabel
            // 
            this.frameLabel.AutoSize = true;
            this.frameLabel.Location = new System.Drawing.Point(77, 19);
            this.frameLabel.Name = "frameLabel";
            this.frameLabel.Size = new System.Drawing.Size(43, 13);
            this.frameLabel.TabIndex = 0;
            this.frameLabel.Text = "Рамка:";
            // 
            // titlePage
            // 
            this.titlePage.Controls.Add(this.tableTitleTextBox);
            this.titlePage.Controls.Add(this.tableTitleLocationComboBox);
            this.titlePage.Controls.Add(this.tableTitleLocationLabel);
            this.titlePage.Controls.Add(this.tableTitleLabel);
            this.titlePage.Location = new System.Drawing.Point(4, 22);
            this.titlePage.Name = "titlePage";
            this.titlePage.Size = new System.Drawing.Size(480, 182);
            this.titlePage.TabIndex = 2;
            this.titlePage.Text = "Заголовок";
            this.titlePage.UseVisualStyleBackColor = true;
            // 
            // tableTitleTextBox
            // 
            this.tableTitleTextBox.Location = new System.Drawing.Point(99, 10);
            this.tableTitleTextBox.Name = "tableTitleTextBox";
            this.tableTitleTextBox.Size = new System.Drawing.Size(373, 20);
            this.tableTitleTextBox.TabIndex = 1;
            // 
            // tableTitleLocationComboBox
            // 
            this.tableTitleLocationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tableTitleLocationComboBox.FormattingEnabled = true;
            this.tableTitleLocationComboBox.Items.AddRange(new object[] {
            "слева",
            "по центру",
            "справа"});
            this.tableTitleLocationComboBox.Location = new System.Drawing.Point(99, 36);
            this.tableTitleLocationComboBox.Name = "tableTitleLocationComboBox";
            this.tableTitleLocationComboBox.Size = new System.Drawing.Size(121, 21);
            this.tableTitleLocationComboBox.TabIndex = 3;
            // 
            // tableTitleLocationLabel
            // 
            this.tableTitleLocationLabel.AutoSize = true;
            this.tableTitleLocationLabel.Location = new System.Drawing.Point(8, 39);
            this.tableTitleLocationLabel.Name = "tableTitleLocationLabel";
            this.tableTitleLocationLabel.Size = new System.Drawing.Size(85, 13);
            this.tableTitleLocationLabel.TabIndex = 2;
            this.tableTitleLocationLabel.Text = "Выравнивание:";
            // 
            // tableTitleLabel
            // 
            this.tableTitleLabel.AutoSize = true;
            this.tableTitleLabel.Location = new System.Drawing.Point(53, 13);
            this.tableTitleLabel.Name = "tableTitleLabel";
            this.tableTitleLabel.Size = new System.Drawing.Size(40, 13);
            this.tableTitleLabel.TabIndex = 0;
            this.tableTitleLabel.Text = "Текст:";
            // 
            // TableDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(488, 254);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Name = "TableDialog";
            this.Text = "Таблица";
            this.tabControl.ResumeLayout(false);
            this.dimensionPage.ResumeLayout(false);
            this.dimensionPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tableHeightUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableWidthUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rowsNumberUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.columnsNumberUpDown)).EndInit();
            this.appearancePage.ResumeLayout(false);
            this.appearancePage.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.marginUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.borderUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.innerMarginUpDown)).EndInit();
            this.titlePage.ResumeLayout(false);
            this.titlePage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage dimensionPage;
        private System.Windows.Forms.ComboBox tableHeightUnitComboBox;
        private System.Windows.Forms.ComboBox tableWidthUnitComboBox;
        private System.Windows.Forms.Label tableHeightLabel;
        private System.Windows.Forms.Label tableWidthLabel;
        private System.Windows.Forms.Label rowsNumberLabel;
        private System.Windows.Forms.Label columnsNumberLabel;
        private System.Windows.Forms.TabPage appearancePage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label innerMarginLabel;
        private System.Windows.Forms.Label marginLabel;
        private System.Windows.Forms.Label frameLabel;
        private System.Windows.Forms.TabPage titlePage;
        private System.Windows.Forms.TextBox tableTitleTextBox;
        private System.Windows.Forms.ComboBox tableTitleLocationComboBox;
        private System.Windows.Forms.Label tableTitleLocationLabel;
        private System.Windows.Forms.Label tableTitleLabel;
        private System.Windows.Forms.ComboBox tableJustifyComboBox;
        private System.Windows.Forms.Button tableColorButton;
        private System.Windows.Forms.Label colorLabel;
        private System.Windows.Forms.Label tableJustifyLabel;
        private System.Windows.Forms.NumericUpDown rowsNumberUpDown;
        private System.Windows.Forms.NumericUpDown columnsNumberUpDown;
        private System.Windows.Forms.NumericUpDown tableHeightUpDown;
        private System.Windows.Forms.NumericUpDown tableWidthUpDown;
        private System.Windows.Forms.NumericUpDown marginUpDown;
        private System.Windows.Forms.NumericUpDown borderUpDown;
        private System.Windows.Forms.NumericUpDown innerMarginUpDown;
    }
}