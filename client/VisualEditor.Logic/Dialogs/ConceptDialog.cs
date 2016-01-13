using System;
using System.Windows.Forms;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Utils.Helpers;

namespace VisualEditor.Logic.Dialogs
{
    internal partial class ConceptDialog : DialogBase
    {
        public ConceptDialog()
        {
            InitializeComponent();
            InitializeDialog();
        }

        public XmlHelper DataTransferUnit { get; set; }

        public void InitializeDialog()
        {
            DataTransferUnit = new XmlHelper();
            DataTransferUnit.AppendNode(string.Empty, "Data");
            DataTransferUnit.AppendNode("Data", "ConceptName");
            DataTransferUnit.AppendNode("Data", "ConceptType");

            HelpKeyword = "Компетенция";
            conceptNameTextBox.Select();
        }

        private void okButton_Click(object sender, System.EventArgs e)
        {
            DataTransferUnit.SetNodeValue("ConceptName", conceptNameTextBox.Text);
            if (!externalConceptCheckBox.Checked)
            {
                DataTransferUnit.SetNodeValue("ConceptType", Enums.ConceptType.Internal.ToString());
            }
            else
            {
                DataTransferUnit.SetNodeValue("ConceptType", Enums.ConceptType.External.ToString());
            }

            Warehouse.Warehouse.IsProjectModified = true;
            DialogResult = DialogResult.OK;
        }

        private void conceptNameTextBox_TextChanged(object sender, EventArgs e)
        {
            CheckState();
        }

        private void CheckState()
        {
            okButton.Enabled = !conceptNameTextBox.Text.Equals(string.Empty);

            // Проверяет на совпадение введенного имени с уже существющими в списке.
            foreach (Concept c in Warehouse.Warehouse.Instance.ConceptTree.Nodes)
            {
                if (c.Text.Equals(conceptNameTextBox.Text))
                {
                    okButton.Enabled = false;

                    break;
                }
            }
        }
    }
}