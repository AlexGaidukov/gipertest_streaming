using VisualEditor.Utils.Controls;

namespace VisualEditor.Logic.Dialogs
{
    partial class PictureDialog
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
            System.Windows.Forms.Label verticalSpaceLabel;
            this.tabControl = new System.Windows.Forms.TabControl();
            this.sourceTabPage = new System.Windows.Forms.TabPage();
            this.linkTextTextBox = new System.Windows.Forms.TextBox();
            this.linkTextLabel = new System.Windows.Forms.Label();
            this.titleTextBox = new System.Windows.Forms.TextBox();
            this.titleLabel = new System.Windows.Forms.Label();
            this.sourceButton = new System.Windows.Forms.Button();
            this.sourceTextBox = new System.Windows.Forms.TextBox();
            this.sourceLabel = new System.Windows.Forms.Label();
            this.sizeTabPage = new System.Windows.Forms.TabPage();
            this.manualSizePanel = new System.Windows.Forms.Panel();
            this.widthUpDown = new System.Windows.Forms.NumericUpDown();
            this.heightUpDown = new System.Windows.Forms.NumericUpDown();
            this.widthUnitComboBox = new System.Windows.Forms.ComboBox();
            this.heightUnitComboBox = new System.Windows.Forms.ComboBox();
            this.keepRatioCheckBox = new System.Windows.Forms.CheckBox();
            this.widthLabel = new System.Windows.Forms.Label();
            this.heightLabel = new System.Windows.Forms.Label();
            this.manualSizeRadioButton = new System.Windows.Forms.RadioButton();
            this.sourceSizeRadioButton = new System.Windows.Forms.RadioButton();
            this.appearanceTabPage = new System.Windows.Forms.TabPage();
            this.borderUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.borderlabel = new System.Windows.Forms.Label();
            this.marginGroupBox = new System.Windows.Forms.GroupBox();
            this.verticalSpaceUpDown = new System.Windows.Forms.NumericUpDown();
            this.horizontalSpaceUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.horizontalSpaceLabel = new System.Windows.Forms.Label();
            this.justifyComboBox = new System.Windows.Forms.ComboBox();
            this.justifyLabel = new System.Windows.Forms.Label();
            this.previewGroupBox = new System.Windows.Forms.GroupBox();
            this.sourceHeightLabel = new System.Windows.Forms.Label();
            this.sourceWidthLabel = new System.Windows.Forms.Label();
            this.sourceSizeLabel = new System.Windows.Forms.Label();
            this.previewPictureBox = new System.Windows.Forms.PictureBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.bevel = new Bevel();
            verticalSpaceLabel = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.sourceTabPage.SuspendLayout();
            this.sizeTabPage.SuspendLayout();
            this.manualSizePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.widthUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightUpDown)).BeginInit();
            this.appearanceTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.borderUpDown)).BeginInit();
            this.marginGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.verticalSpaceUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.horizontalSpaceUpDown)).BeginInit();
            this.previewGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // verticalSpaceLabel
            // 
            verticalSpaceLabel.AutoSize = true;
            verticalSpaceLabel.Location = new System.Drawing.Point(13, 46);
            verticalSpaceLabel.Name = "verticalSpaceLabel";
            verticalSpaceLabel.Size = new System.Drawing.Size(86, 13);
            verticalSpaceLabel.TabIndex = 3;
            verticalSpaceLabel.Text = "Сверху и снизу:";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.sourceTabPage);
            this.tabControl.Controls.Add(this.sizeTabPage);
            this.tabControl.Controls.Add(this.appearanceTabPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(586, 166);
            this.tabControl.TabIndex = 0;
            // 
            // sourceTabPage
            // 
            this.sourceTabPage.Controls.Add(this.linkTextTextBox);
            this.sourceTabPage.Controls.Add(this.linkTextLabel);
            this.sourceTabPage.Controls.Add(this.titleTextBox);
            this.sourceTabPage.Controls.Add(this.titleLabel);
            this.sourceTabPage.Controls.Add(this.sourceButton);
            this.sourceTabPage.Controls.Add(this.sourceTextBox);
            this.sourceTabPage.Controls.Add(this.sourceLabel);
            this.sourceTabPage.Location = new System.Drawing.Point(4, 22);
            this.sourceTabPage.Name = "sourceTabPage";
            this.sourceTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.sourceTabPage.Size = new System.Drawing.Size(578, 140);
            this.sourceTabPage.TabIndex = 0;
            this.sourceTabPage.Text = "Источник";
            this.sourceTabPage.UseVisualStyleBackColor = true;
            // 
            // linkTextTextBox
            // 
            this.linkTextTextBox.Location = new System.Drawing.Point(93, 67);
            this.linkTextTextBox.Name = "linkTextTextBox";
            this.linkTextTextBox.Size = new System.Drawing.Size(364, 20);
            this.linkTextTextBox.TabIndex = 6;
            // 
            // linkTextLabel
            // 
            this.linkTextLabel.AutoSize = true;
            this.linkTextLabel.Location = new System.Drawing.Point(6, 70);
            this.linkTextLabel.Name = "linkTextLabel";
            this.linkTextLabel.Size = new System.Drawing.Size(81, 13);
            this.linkTextLabel.TabIndex = 5;
            this.linkTextLabel.Text = "Текст ссылки:";
            // 
            // titleTextBox
            // 
            this.titleTextBox.Location = new System.Drawing.Point(153, 38);
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new System.Drawing.Size(304, 20);
            this.titleTextBox.TabIndex = 4;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(6, 41);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(141, 13);
            this.titleLabel.TabIndex = 3;
            this.titleLabel.Text = "Всплывающая подсказка:";
            // 
            // sourceButton
            // 
            this.sourceButton.Location = new System.Drawing.Point(463, 7);
            this.sourceButton.Name = "sourceButton";
            this.sourceButton.Size = new System.Drawing.Size(107, 23);
            this.sourceButton.TabIndex = 2;
            this.sourceButton.Text = "Выбрать файл...";
            this.sourceButton.UseVisualStyleBackColor = true;
            this.sourceButton.Click += new System.EventHandler(this.sourceButton_Click);
            // 
            // sourceTextBox
            // 
            this.sourceTextBox.Location = new System.Drawing.Point(70, 9);
            this.sourceTextBox.Name = "sourceTextBox";
            this.sourceTextBox.ReadOnly = true;
            this.sourceTextBox.Size = new System.Drawing.Size(387, 20);
            this.sourceTextBox.TabIndex = 1;
            this.sourceTextBox.TextChanged += new System.EventHandler(this.sourceTextBox_TextChanged);
            // 
            // sourceLabel
            // 
            this.sourceLabel.AutoSize = true;
            this.sourceLabel.Location = new System.Drawing.Point(6, 12);
            this.sourceLabel.Name = "sourceLabel";
            this.sourceLabel.Size = new System.Drawing.Size(58, 13);
            this.sourceLabel.TabIndex = 0;
            this.sourceLabel.Text = "Источник:";
            // 
            // sizeTabPage
            // 
            this.sizeTabPage.Controls.Add(this.manualSizePanel);
            this.sizeTabPage.Controls.Add(this.manualSizeRadioButton);
            this.sizeTabPage.Controls.Add(this.sourceSizeRadioButton);
            this.sizeTabPage.Location = new System.Drawing.Point(4, 22);
            this.sizeTabPage.Name = "sizeTabPage";
            this.sizeTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.sizeTabPage.Size = new System.Drawing.Size(578, 140);
            this.sizeTabPage.TabIndex = 1;
            this.sizeTabPage.Text = "Размер";
            this.sizeTabPage.UseVisualStyleBackColor = true;
            // 
            // manualSizePanel
            // 
            this.manualSizePanel.Controls.Add(this.widthUpDown);
            this.manualSizePanel.Controls.Add(this.heightUpDown);
            this.manualSizePanel.Controls.Add(this.widthUnitComboBox);
            this.manualSizePanel.Controls.Add(this.heightUnitComboBox);
            this.manualSizePanel.Controls.Add(this.keepRatioCheckBox);
            this.manualSizePanel.Controls.Add(this.widthLabel);
            this.manualSizePanel.Controls.Add(this.heightLabel);
            this.manualSizePanel.Enabled = false;
            this.manualSizePanel.Location = new System.Drawing.Point(19, 52);
            this.manualSizePanel.Name = "manualSizePanel";
            this.manualSizePanel.Size = new System.Drawing.Size(251, 86);
            this.manualSizePanel.TabIndex = 2;
            // 
            // widthUpDown
            // 
            this.widthUpDown.Location = new System.Drawing.Point(56, 56);
            this.widthUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.widthUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.widthUpDown.Name = "widthUpDown";
            this.widthUpDown.Size = new System.Drawing.Size(60, 20);
            this.widthUpDown.TabIndex = 5;
            this.widthUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.widthUpDown.ValueChanged += new System.EventHandler(this.widthUpDown_ValueChanged);
            // 
            // heightUpDown
            // 
            this.heightUpDown.Location = new System.Drawing.Point(56, 30);
            this.heightUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.heightUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.heightUpDown.Name = "heightUpDown";
            this.heightUpDown.Size = new System.Drawing.Size(60, 20);
            this.heightUpDown.TabIndex = 2;
            this.heightUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.heightUpDown.ValueChanged += new System.EventHandler(this.heightUpDown_ValueChanged);
            // 
            // widthUnitComboBox
            // 
            this.widthUnitComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.widthUnitComboBox.FormattingEnabled = true;
            this.widthUnitComboBox.Items.AddRange(new object[] {
            "пикселов",
            "% от размера окна"});
            this.widthUnitComboBox.Location = new System.Drawing.Point(122, 56);
            this.widthUnitComboBox.Name = "widthUnitComboBox";
            this.widthUnitComboBox.Size = new System.Drawing.Size(121, 21);
            this.widthUnitComboBox.TabIndex = 6;
            this.widthUnitComboBox.SelectedIndexChanged += new System.EventHandler(this.widthUnitComboBox_SelectedIndexChanged);
            // 
            // heightUnitComboBox
            // 
            this.heightUnitComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.heightUnitComboBox.FormattingEnabled = true;
            this.heightUnitComboBox.Items.AddRange(new object[] {
            "пикселов",
            "% от размера окна"});
            this.heightUnitComboBox.Location = new System.Drawing.Point(122, 29);
            this.heightUnitComboBox.Name = "heightUnitComboBox";
            this.heightUnitComboBox.Size = new System.Drawing.Size(121, 21);
            this.heightUnitComboBox.TabIndex = 3;
            this.heightUnitComboBox.SelectedIndexChanged += new System.EventHandler(this.heightUnitComboBox_SelectedIndexChanged);
            // 
            // keepRatioCheckBox
            // 
            this.keepRatioCheckBox.AutoSize = true;
            this.keepRatioCheckBox.Location = new System.Drawing.Point(8, 3);
            this.keepRatioCheckBox.Name = "keepRatioCheckBox";
            this.keepRatioCheckBox.Size = new System.Drawing.Size(139, 17);
            this.keepRatioCheckBox.TabIndex = 0;
            this.keepRatioCheckBox.Text = "Соблюдать пропорции";
            this.keepRatioCheckBox.UseVisualStyleBackColor = true;
            // 
            // widthLabel
            // 
            this.widthLabel.AutoSize = true;
            this.widthLabel.Location = new System.Drawing.Point(4, 58);
            this.widthLabel.Name = "widthLabel";
            this.widthLabel.Size = new System.Drawing.Size(49, 13);
            this.widthLabel.TabIndex = 4;
            this.widthLabel.Text = "Ширина:";
            // 
            // heightLabel
            // 
            this.heightLabel.AutoSize = true;
            this.heightLabel.Location = new System.Drawing.Point(5, 32);
            this.heightLabel.Name = "heightLabel";
            this.heightLabel.Size = new System.Drawing.Size(48, 13);
            this.heightLabel.TabIndex = 1;
            this.heightLabel.Text = "Высота:";
            // 
            // manualSizeRadioButton
            // 
            this.manualSizeRadioButton.AutoSize = true;
            this.manualSizeRadioButton.Location = new System.Drawing.Point(8, 29);
            this.manualSizeRadioButton.Name = "manualSizeRadioButton";
            this.manualSizeRadioButton.Size = new System.Drawing.Size(102, 17);
            this.manualSizeRadioButton.TabIndex = 1;
            this.manualSizeRadioButton.Text = "Задать размер";
            this.manualSizeRadioButton.UseVisualStyleBackColor = true;
            this.manualSizeRadioButton.CheckedChanged += new System.EventHandler(this.manualSizeRadioButton_CheckedChanged);
            // 
            // sourceSizeRadioButton
            // 
            this.sourceSizeRadioButton.AutoSize = true;
            this.sourceSizeRadioButton.Checked = true;
            this.sourceSizeRadioButton.Location = new System.Drawing.Point(8, 6);
            this.sourceSizeRadioButton.Name = "sourceSizeRadioButton";
            this.sourceSizeRadioButton.Size = new System.Drawing.Size(117, 17);
            this.sourceSizeRadioButton.TabIndex = 0;
            this.sourceSizeRadioButton.TabStop = true;
            this.sourceSizeRadioButton.Text = "Исходный размер";
            this.sourceSizeRadioButton.UseVisualStyleBackColor = true;
            this.sourceSizeRadioButton.CheckedChanged += new System.EventHandler(this.sourceSizeRadioButton_CheckedChanged);
            // 
            // appearanceTabPage
            // 
            this.appearanceTabPage.Controls.Add(this.borderUpDown);
            this.appearanceTabPage.Controls.Add(this.label3);
            this.appearanceTabPage.Controls.Add(this.borderlabel);
            this.appearanceTabPage.Controls.Add(this.marginGroupBox);
            this.appearanceTabPage.Controls.Add(this.justifyComboBox);
            this.appearanceTabPage.Controls.Add(this.justifyLabel);
            this.appearanceTabPage.Location = new System.Drawing.Point(4, 22);
            this.appearanceTabPage.Name = "appearanceTabPage";
            this.appearanceTabPage.Size = new System.Drawing.Size(578, 140);
            this.appearanceTabPage.TabIndex = 2;
            this.appearanceTabPage.Text = "Внешний вид";
            this.appearanceTabPage.UseVisualStyleBackColor = true;
            // 
            // borderUpDown
            // 
            this.borderUpDown.Location = new System.Drawing.Point(351, 50);
            this.borderUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.borderUpDown.Name = "borderUpDown";
            this.borderUpDown.Size = new System.Drawing.Size(60, 20);
            this.borderUpDown.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(417, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "пикселов";
            // 
            // borderlabel
            // 
            this.borderlabel.AutoSize = true;
            this.borderlabel.Location = new System.Drawing.Point(254, 52);
            this.borderlabel.Name = "borderlabel";
            this.borderlabel.Size = new System.Drawing.Size(91, 13);
            this.borderlabel.TabIndex = 3;
            this.borderlabel.Text = "Толщина рамки:";
            // 
            // marginGroupBox
            // 
            this.marginGroupBox.Controls.Add(this.verticalSpaceUpDown);
            this.marginGroupBox.Controls.Add(this.horizontalSpaceUpDown);
            this.marginGroupBox.Controls.Add(this.label2);
            this.marginGroupBox.Controls.Add(this.label1);
            this.marginGroupBox.Controls.Add(verticalSpaceLabel);
            this.marginGroupBox.Controls.Add(this.horizontalSpaceLabel);
            this.marginGroupBox.Location = new System.Drawing.Point(8, 6);
            this.marginGroupBox.Name = "marginGroupBox";
            this.marginGroupBox.Size = new System.Drawing.Size(237, 77);
            this.marginGroupBox.TabIndex = 0;
            this.marginGroupBox.TabStop = false;
            this.marginGroupBox.Text = "Поля";
            // 
            // verticalSpaceUpDown
            // 
            this.verticalSpaceUpDown.Location = new System.Drawing.Point(105, 44);
            this.verticalSpaceUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.verticalSpaceUpDown.Name = "verticalSpaceUpDown";
            this.verticalSpaceUpDown.Size = new System.Drawing.Size(60, 20);
            this.verticalSpaceUpDown.TabIndex = 4;
            // 
            // horizontalSpaceUpDown
            // 
            this.horizontalSpaceUpDown.Location = new System.Drawing.Point(105, 18);
            this.horizontalSpaceUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.horizontalSpaceUpDown.Name = "horizontalSpaceUpDown";
            this.horizontalSpaceUpDown.Size = new System.Drawing.Size(60, 20);
            this.horizontalSpaceUpDown.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(171, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "пикселов";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(171, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "пикселов";
            // 
            // horizontalSpaceLabel
            // 
            this.horizontalSpaceLabel.AutoSize = true;
            this.horizontalSpaceLabel.Location = new System.Drawing.Point(10, 20);
            this.horizontalSpaceLabel.Name = "horizontalSpaceLabel";
            this.horizontalSpaceLabel.Size = new System.Drawing.Size(89, 13);
            this.horizontalSpaceLabel.TabIndex = 0;
            this.horizontalSpaceLabel.Text = "Слева и справа:";
            // 
            // justifyComboBox
            // 
            this.justifyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.justifyComboBox.FormattingEnabled = true;
            this.justifyComboBox.Items.AddRange(new object[] {
            "по верхнему краю",
            "по центру",
            "по нижнему краю",
            "по левому краю",
            "по правому краю"});
            this.justifyComboBox.Location = new System.Drawing.Point(351, 23);
            this.justifyComboBox.Name = "justifyComboBox";
            this.justifyComboBox.Size = new System.Drawing.Size(169, 21);
            this.justifyComboBox.TabIndex = 2;
            // 
            // justifyLabel
            // 
            this.justifyLabel.AutoSize = true;
            this.justifyLabel.Location = new System.Drawing.Point(260, 26);
            this.justifyLabel.Name = "justifyLabel";
            this.justifyLabel.Size = new System.Drawing.Size(85, 13);
            this.justifyLabel.TabIndex = 1;
            this.justifyLabel.Text = "Выравнивание:";
            // 
            // previewGroupBox
            // 
            this.previewGroupBox.Controls.Add(this.sourceHeightLabel);
            this.previewGroupBox.Controls.Add(this.sourceWidthLabel);
            this.previewGroupBox.Controls.Add(this.sourceSizeLabel);
            this.previewGroupBox.Controls.Add(this.previewPictureBox);
            this.previewGroupBox.Location = new System.Drawing.Point(12, 172);
            this.previewGroupBox.Name = "previewGroupBox";
            this.previewGroupBox.Size = new System.Drawing.Size(214, 105);
            this.previewGroupBox.TabIndex = 1;
            this.previewGroupBox.TabStop = false;
            this.previewGroupBox.Text = "Предварительный просмотр";
            // 
            // sourceHeightLabel
            // 
            this.sourceHeightLabel.AutoSize = true;
            this.sourceHeightLabel.Location = new System.Drawing.Point(100, 46);
            this.sourceHeightLabel.Name = "sourceHeightLabel";
            this.sourceHeightLabel.Size = new System.Drawing.Size(0, 13);
            this.sourceHeightLabel.TabIndex = 1;
            this.sourceHeightLabel.Visible = false;
            // 
            // sourceWidthLabel
            // 
            this.sourceWidthLabel.AutoSize = true;
            this.sourceWidthLabel.Location = new System.Drawing.Point(100, 69);
            this.sourceWidthLabel.Name = "sourceWidthLabel";
            this.sourceWidthLabel.Size = new System.Drawing.Size(0, 13);
            this.sourceWidthLabel.TabIndex = 2;
            this.sourceWidthLabel.Visible = false;
            // 
            // sourceSizeLabel
            // 
            this.sourceSizeLabel.AutoSize = true;
            this.sourceSizeLabel.Location = new System.Drawing.Point(100, 24);
            this.sourceSizeLabel.Name = "sourceSizeLabel";
            this.sourceSizeLabel.Size = new System.Drawing.Size(99, 13);
            this.sourceSizeLabel.TabIndex = 0;
            this.sourceSizeLabel.Text = "Исходный размер";
            this.sourceSizeLabel.Visible = false;
            // 
            // previewPictureBox
            // 
            this.previewPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.previewPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.previewPictureBox.Location = new System.Drawing.Point(12, 18);
            this.previewPictureBox.Name = "previewPictureBox";
            this.previewPictureBox.Size = new System.Drawing.Size(77, 74);
            this.previewPictureBox.TabIndex = 0;
            this.previewPictureBox.TabStop = false;
            // 
            // okButton
            // 
            this.okButton.Enabled = false;
            this.okButton.Location = new System.Drawing.Point(418, 287);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(499, 287);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // bevel
            // 
            this.bevel.BevelStyle = BevelStyle.Lowered;
            this.bevel.BevelType = BevelType.BottomLine;
            this.bevel.HighlightColor = System.Drawing.SystemColors.ButtonHighlight;
            this.bevel.Location = new System.Drawing.Point(232, 267);
            this.bevel.Name = "bevel";
            this.bevel.ShadowColor = System.Drawing.SystemColors.ButtonShadow;
            this.bevel.Size = new System.Drawing.Size(346, 10);
            this.bevel.TabIndex = 2;
            this.bevel.Text = "bevel";
            // 
            // PictureDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(586, 321);
            this.Controls.Add(this.bevel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.previewGroupBox);
            this.Controls.Add(this.tabControl);
            this.Name = "PictureDialog";
            this.Text = "Рисунок";
            this.tabControl.ResumeLayout(false);
            this.sourceTabPage.ResumeLayout(false);
            this.sourceTabPage.PerformLayout();
            this.sizeTabPage.ResumeLayout(false);
            this.sizeTabPage.PerformLayout();
            this.manualSizePanel.ResumeLayout(false);
            this.manualSizePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.widthUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightUpDown)).EndInit();
            this.appearanceTabPage.ResumeLayout(false);
            this.appearanceTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.borderUpDown)).EndInit();
            this.marginGroupBox.ResumeLayout(false);
            this.marginGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.verticalSpaceUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.horizontalSpaceUpDown)).EndInit();
            this.previewGroupBox.ResumeLayout(false);
            this.previewGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage sourceTabPage;
        private System.Windows.Forms.TabPage sizeTabPage;
        private System.Windows.Forms.TabPage appearanceTabPage;
        private System.Windows.Forms.TextBox titleTextBox;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Button sourceButton;
        private System.Windows.Forms.TextBox sourceTextBox;
        private System.Windows.Forms.Label sourceLabel;
        private System.Windows.Forms.GroupBox previewGroupBox;
        private System.Windows.Forms.Label sourceHeightLabel;
        private System.Windows.Forms.Label sourceWidthLabel;
        private System.Windows.Forms.Label sourceSizeLabel;
        private System.Windows.Forms.PictureBox previewPictureBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private Bevel bevel;
        private System.Windows.Forms.TextBox linkTextTextBox;
        private System.Windows.Forms.Label linkTextLabel;
        private System.Windows.Forms.Panel manualSizePanel;
        private System.Windows.Forms.NumericUpDown widthUpDown;
        private System.Windows.Forms.NumericUpDown heightUpDown;
        private System.Windows.Forms.ComboBox widthUnitComboBox;
        private System.Windows.Forms.ComboBox heightUnitComboBox;
        private System.Windows.Forms.CheckBox keepRatioCheckBox;
        private System.Windows.Forms.Label widthLabel;
        private System.Windows.Forms.Label heightLabel;
        private System.Windows.Forms.RadioButton manualSizeRadioButton;
        private System.Windows.Forms.RadioButton sourceSizeRadioButton;
        private System.Windows.Forms.NumericUpDown borderUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label borderlabel;
        private System.Windows.Forms.GroupBox marginGroupBox;
        private System.Windows.Forms.NumericUpDown verticalSpaceUpDown;
        private System.Windows.Forms.NumericUpDown horizontalSpaceUpDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label horizontalSpaceLabel;
        private System.Windows.Forms.ComboBox justifyComboBox;
        private System.Windows.Forms.Label justifyLabel;
    }
}