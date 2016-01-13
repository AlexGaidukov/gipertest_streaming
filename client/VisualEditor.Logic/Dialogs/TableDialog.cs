using System;
using System.Drawing;
using System.Windows.Forms;
using VisualEditor.Utils.Helpers;

namespace VisualEditor.Logic.Dialogs
{
    internal partial class TableDialog : DialogBase
    {
        public TableDialog()
        {
            InitializeComponent();
            InitializeDialog();
        }

        public XmlHelper DataTransferUnit { get; set; }

        public void InitializeDialog()
        {
            DataTransferUnit = new XmlHelper();
            DataTransferUnit.AppendNode(string.Empty, "Data");
            DataTransferUnit.AppendNode("Data", "ColumnsNumber");
            DataTransferUnit.AppendNode("Data", "RowsNumber");
            DataTransferUnit.AppendNode("Data", "TableWidth");
            DataTransferUnit.AppendNode("Data", "TableWidthUnit");
            DataTransferUnit.AppendNode("Data", "TableHeight");
            DataTransferUnit.AppendNode("Data", "TableHeightUnit");
            DataTransferUnit.AppendNode("Data", "BorderPixels");
            DataTransferUnit.AppendNode("Data", "MarginPixels");
            DataTransferUnit.AppendNode("Data", "InnerMarginPixels");
            DataTransferUnit.AppendNode("Data", "TableJustify");
            DataTransferUnit.AppendNode("Data", "TableColor");
            DataTransferUnit.AppendNode("Data", "TableTitle");
            DataTransferUnit.AppendNode("Data", "TableTitleLocation");

            tableWidthUnitComboBox.Text = "% от размера окна";
            tableHeightUnitComboBox.Text = "пикселов";
            tableJustifyComboBox.Text = "влево";
            tableColorButton.BackColor = Color.FromArgb(255, 255, 255);
            tableTitleLocationComboBox.Text = "по центру";
            HelpKeyword = "Таблица";
            columnsNumberUpDown.Select();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DataTransferUnit.SetNodeValue("ColumnsNumber", columnsNumberUpDown.Value.ToString());
            DataTransferUnit.SetNodeValue("RowsNumber", rowsNumberUpDown.Value.ToString());
            DataTransferUnit.SetNodeValue("TableWidth", tableWidthUpDown.Value.ToString());
            DataTransferUnit.SetNodeValue("TableWidthUnit", tableWidthUnitComboBox.Text);
            DataTransferUnit.SetNodeValue("TableHeight", tableHeightUpDown.Value.ToString());
            DataTransferUnit.SetNodeValue("TableHeightUnit", tableHeightUnitComboBox.Text);
            DataTransferUnit.SetNodeValue("BorderPixels", borderUpDown.Value.ToString());
            DataTransferUnit.SetNodeValue("MarginPixels", marginUpDown.Value.ToString());
            DataTransferUnit.SetNodeValue("InnerMarginPixels", innerMarginUpDown.Value.ToString());
            DataTransferUnit.SetNodeValue("TableJustify", tableJustifyComboBox.Text);
            var color = tableColorButton.BackColor;
            DataTransferUnit.SetNodeValue("TableColor", string.Concat(color.R.ToString(), " ",
                color.G.ToString(), " ", color.B.ToString()));
            DataTransferUnit.SetNodeValue("TableTitle", tableTitleTextBox.Text);
            DataTransferUnit.SetNodeValue("TableTitleLocation", tableTitleLocationComboBox.Text);

            Warehouse.Warehouse.IsProjectModified = true;
            DialogResult = DialogResult.OK;
        }

        private void CheckState()
        {

        }

        private void tableColorButton_Click(object sender, EventArgs e)
        {
            using (var cd = new ColorDialog())
            {
                if (cd.ShowDialog(this) == DialogResult.OK)
                {
                    tableColorButton.BackColor = cd.Color;
                }
            }
        }

        private void tableWidthUnitComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            tableWidthUpDown.Maximum = tableWidthUnitComboBox.Text.Equals("пикселов") ? 1000 : 100;
        }

        private void tableHeightUnitComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            tableHeightUpDown.Maximum = tableHeightUnitComboBox.Text.Equals("пикселов") ? 1000 : 100;
        }
    }
}