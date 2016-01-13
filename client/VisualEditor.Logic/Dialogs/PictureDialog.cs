using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using VisualEditor.Logic.Helpers.AppSettings;
using VisualEditor.Utils.Helpers;

namespace VisualEditor.Logic.Dialogs
{
    internal partial class PictureDialog : DialogBase
    {
        private SizeF sourceImageSize;
        private SizeF imageSize;

        public PictureDialog()
        {
            InitializeComponent();
            InitializeDialog();
        }

        public XmlHelper DataTransferUnit { get; set; }

        public void InitializeDialog()
        {
            DataTransferUnit = new XmlHelper();
            DataTransferUnit.AppendNode(string.Empty, "Data");
            DataTransferUnit.AppendNode("Data", "Source");
            DataTransferUnit.AppendNode("Data", "Title");
            DataTransferUnit.AppendNode("Data", "LinkText");
            DataTransferUnit.AppendNode("Data", "Height");
            DataTransferUnit.AppendNode("Data", "HeightUnit");
            DataTransferUnit.AppendNode("Data", "Width");
            DataTransferUnit.AppendNode("Data", "WidthUnit");
            DataTransferUnit.AppendNode("Data", "HorizontalSpace");
            DataTransferUnit.AppendNode("Data", "VerticalSpace");
            DataTransferUnit.AppendNode("Data", "Justify");
            DataTransferUnit.AppendNode("Data", "BorderPixels");

            heightUnitComboBox.Text = "пикселов";
            widthUnitComboBox.Text = "пикселов";
            justifyComboBox.Text = "по нижнему краю";
            HelpKeyword = "Рисунок";
            sourceButton.Select();
        }

        private void sourceSizeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (sourceSizeRadioButton.Checked)
            {
                if (!sourceImageSize.IsEmpty)
                {
                    manualSizePanel.Enabled = false;
                    keepRatioCheckBox.Checked = false;
                    heightUpDown.Maximum = 10000;
                    heightUnitComboBox.Text = "пикселов";
                    heightUpDown.Value = Convert.ToDecimal(sourceImageSize.Height);
                    widthUpDown.Maximum = 10000;
                    widthUnitComboBox.Text = "пикселов";
                    widthUpDown.Value = Convert.ToDecimal(sourceImageSize.Width);
                }
            }
        }

        private void manualSizeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (manualSizeRadioButton.Checked)
            {
                if (!sourceImageSize.IsEmpty)
                {
                    manualSizePanel.Enabled = true;
                }
                else
                {
                    sourceSizeRadioButton.Checked = true;
                }
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DataTransferUnit.SetNodeValue("Source", sourceTextBox.Text);
            DataTransferUnit.SetNodeValue("Title", titleTextBox.Text);
            DataTransferUnit.SetNodeValue("LinkText", linkTextTextBox.Text);
            DataTransferUnit.SetNodeValue("Height", heightUpDown.Value.ToString());
            DataTransferUnit.SetNodeValue("HeightUnit", heightUnitComboBox.Text);
            DataTransferUnit.SetNodeValue("Width", widthUpDown.Value.ToString());
            DataTransferUnit.SetNodeValue("WidthUnit", widthUnitComboBox.Text);
            DataTransferUnit.SetNodeValue("HorizontalSpace", horizontalSpaceUpDown.Value.ToString());
            DataTransferUnit.SetNodeValue("VerticalSpace", verticalSpaceUpDown.Value.ToString());
            DataTransferUnit.SetNodeValue("Justify", justifyComboBox.Text);
            DataTransferUnit.SetNodeValue("BorderPixels", borderUpDown.Value.ToString());

            Warehouse.Warehouse.IsProjectModified = true;
            DialogResult = DialogResult.OK;
        }

        private void CheckState()
        {
            okButton.Enabled = sourceTextBox.Text != string.Empty;
        }

        public void InitializeData()
        {

        }

        private void sourceButton_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                    openFileDialog.Filter = "Все типы рисунков (*.gif, *.jpg, *.jpeg, *.png)|" +
                        "*.gif;*.jpg;*.jpeg;*.png|" +
                        "GIF (*.gif)|*.gif|" +
                        "JPEG (*.jpg, *.jpeg)|*.jpg;*.jpeg|" +
                        "PNG (*.png)|*.png";

                openFileDialog.InitialDirectory = AppSettingsHelper.GetInitialDirectory();

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    sourceTextBox.Text = openFileDialog.FileName;

                    previewPictureBox.BackgroundImage = Image.FromFile(openFileDialog.FileName);
                    sourceImageSize = Image.FromFile(openFileDialog.FileName).PhysicalDimension;
                    imageSize = new SizeF(sourceImageSize);
                    sourceSizeLabel.Visible = true;
                    sourceHeightLabel.Text = string.Concat("Высота: ", sourceImageSize.Height);
                    sourceHeightLabel.Visible = true;
                    heightUpDown.Value = Convert.ToDecimal(sourceImageSize.Height);
                    sourceWidthLabel.Text = string.Concat("Ширина: ", sourceImageSize.Width);
                    sourceWidthLabel.Visible = true;
                    widthUpDown.Value = Convert.ToDecimal(sourceImageSize.Width);

                    AppSettingsManager.Instance.SetSettingByName(SettingNames.InitialDirectory, 
                        Path.GetDirectoryName(openFileDialog.FileName));
                }
            }
        }

        private void sourceTextBox_TextChanged(object sender, EventArgs e)
        {
            CheckState();
        }

        private void heightUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (keepRatioCheckBox.Checked)
            {
                var v = Convert.ToDecimal(Math.Round(((float)heightUpDown.Value / imageSize.Height) * imageSize.Width));
                if (v >= widthUpDown.Minimum && v <= widthUpDown.Maximum)
                {
                    widthUpDown.Value = v;
                }
            }
            else
            {
                if (tabControl.SelectedTab.Equals(sizeTabPage))
                {
                    imageSize = new SizeF((float)Convert.ToDouble(widthUpDown.Value), (float)Convert.ToDouble(heightUpDown.Value));
                }
            }
        }

        private void widthUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (keepRatioCheckBox.Checked)
            {
                var v = Convert.ToDecimal(Math.Round(((float)widthUpDown.Value / imageSize.Width) * imageSize.Height));
                if (v >= heightUpDown.Minimum && v <= heightUpDown.Maximum)
                {
                    heightUpDown.Value = v;
                }
            }
            else
            {
                if (tabControl.SelectedTab.Equals(sizeTabPage))
                {
                    imageSize = new SizeF((float)Convert.ToDouble(widthUpDown.Value), (float)Convert.ToDouble(heightUpDown.Value));
                }
            }
        }

        private void heightUnitComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (heightUnitComboBox.Text.Equals("пикселов"))
            {
                if (widthUnitComboBox.Text.Equals("пикселов"))
                {
                    keepRatioCheckBox.Enabled = true;
                }
                heightUpDown.Maximum = 10000;
            }
            else
            {
                keepRatioCheckBox.Checked = false;
                keepRatioCheckBox.Enabled = false;
                heightUpDown.Maximum = 100;
            }
        }

        private void widthUnitComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (widthUnitComboBox.Text.Equals("пикселов"))
            {
                if (heightUnitComboBox.Text.Equals("пикселов"))
                {
                    keepRatioCheckBox.Enabled = true;
                }
                widthUpDown.Maximum = 10000;
            }
            else
            {
                keepRatioCheckBox.Checked = false;
                keepRatioCheckBox.Enabled = false;
                widthUpDown.Maximum = 100;
            }
        }
    }
}