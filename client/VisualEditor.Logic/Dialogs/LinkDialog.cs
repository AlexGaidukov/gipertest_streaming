using System;
using System.Windows.Forms;
using VisualEditor.Logic.Controls.Docking;
using VisualEditor.Logic.Controls.Docking.Documents;
using VisualEditor.Utils.Helpers;

namespace VisualEditor.Logic.Dialogs
{
    internal partial class LinkDialog : DialogBase
    {
        private Enums.LinkTarget linkTarget;

        public LinkDialog()
        {
            InitializeComponent();
            InitializeDialog();
        }

        public XmlHelper DataTransferUnit { get; set; }

        public void InitializeDialog()
        {
            DataTransferUnit = new XmlHelper();
            DataTransferUnit.AppendNode(string.Empty, "Data");
            DataTransferUnit.AppendNode("Data", "LinkText");
            DataTransferUnit.AppendNode("Data", "LinkObjectId");
            DataTransferUnit.AppendNode("Data", "LinkTarget");
            DataTransferUnit.AppendNode("Data", "Url");

            linkTarget = Enums.LinkTarget.Bookmark;
            HelpKeyword = "Ссылка";
            linkTextTextBox.Select();
            FillList();
            linkTypeComboBox.SelectedIndexChanged += linkTypeComboBox_SelectedIndexChanged;
            urlTextBox.Text = "http://";
            linkTypeComboBox.Text = "http:";
        }

        private void bookmarkButton_Click(object sender, EventArgs e)
        {
            crosslinkPanel.Visible = true;
            hyperlinkPanel.Visible = false;
            linkTarget = Enums.LinkTarget.Bookmark;

            ClearList();
            CheckState();
            FillList();
        }

        private void internalConceptButton_Click(object sender, EventArgs e)
        {
            crosslinkPanel.Visible = true;
            hyperlinkPanel.Visible = false;
            linkTarget = Enums.LinkTarget.InternalConcept;

            ClearList();
            CheckState();
            FillList();
        }

        private void externalConceptButton_Click(object sender, EventArgs e)
        {
            crosslinkPanel.Visible = true;
            hyperlinkPanel.Visible = false;
            linkTarget = Enums.LinkTarget.ExternalConcept;

            ClearList();
            CheckState();
            FillList();
        }

        private void moduleButton_Click(object sender, EventArgs e)
        {
            crosslinkPanel.Visible = true;
            hyperlinkPanel.Visible = false;
            linkTarget = Enums.LinkTarget.TrainingModule;

            ClearList();
            CheckState();
            FillList();
        }

        private void hyperlinkButton_Click(object sender, EventArgs e)
        {
            hyperlinkPanel.Visible = true;
            crosslinkPanel.Visible = false;
            linkTarget = Enums.LinkTarget.Hyperlink;

            ClearList();
            CheckState();
        }

        void linkTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var slashIndex = urlTextBox.Text.IndexOf("//");
                if (!slashIndex.Equals(-1))
                {
                    var oldText = urlTextBox.Text.Substring(slashIndex + 2, urlTextBox.Text.Length - slashIndex - 2);
                    urlTextBox.Text = string.Concat(linkTypeComboBox.Text, "//", oldText);
                }
                else
                {
                    urlTextBox.Text = string.Concat(linkTypeComboBox.Text, "//");
                }
            }
            catch 
            {
                urlTextBox.Text = string.Concat(linkTypeComboBox.Text, "//");
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DataTransferUnit.SetNodeValue("LinkText", linkTextTextBox.Text);
            DataTransferUnit.SetNodeValue("LinkObjectId", Warehouse.Warehouse.GetLinkObjectIdByText(linkTarget, (string)linkObjectListBox.SelectedItem));
            DataTransferUnit.SetNodeValue("LinkTarget", linkTarget.ToString());
            DataTransferUnit.SetNodeValue("Url", urlTextBox.Text);

            Warehouse.Warehouse.IsProjectModified = true;
            DialogResult = DialogResult.OK;
        }

        private void linkObjectListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckState();
        }

        private void CheckState()
        {
            if (!linkTarget.Equals(Enums.LinkTarget.Hyperlink))
            {
                okButton.Enabled = linkObjectListBox.SelectedIndex != -1;
            }
            else
            {
                okButton.Enabled = !urlTextBox.Text.EndsWith("//");
            }
        }

        private void ClearList()
        {
            linkObjectListBox.Items.Clear();
        }

        #region FillList
        
        private void FillList()
        {
            if (linkTarget.Equals(Enums.LinkTarget.Bookmark))
            {
                var bs = Warehouse.Warehouse.Instance.Bookmarks;
                foreach (var b in bs)
                {
                    linkObjectListBox.Items.Add(b.Text);
                }
            }

            if (linkTarget.Equals(Enums.LinkTarget.InternalConcept))
            {
                var ics = Warehouse.Warehouse.Instance.InternalConcepts;
                foreach (var c in ics)
                {
                    linkObjectListBox.Items.Add(c.Text);
                }
            }

            if (linkTarget.Equals(Enums.LinkTarget.ExternalConcept))
            {
                var ecs = Warehouse.Warehouse.Instance.ExternalConcepts;
                foreach (var c in ecs)
                {
                    linkObjectListBox.Items.Add(c.Text);
                }
            }

            if (linkTarget.Equals(Enums.LinkTarget.TrainingModule))
            {
                var tms = Warehouse.Warehouse.Instance.TrainingModules;
                var atm = DockContainer.Instance.ActiveDocument as TrainingModuleDocument;

                foreach (var tm in tms)
                {
                    if (!tm.Id.Equals(atm.TrainingModule.Id))
                    {
                        linkObjectListBox.Items.Add(tm.Text);
                    }
                }
            }
        }

        #endregion

        public void InitializeData()
        {
            linkTextTextBox.Text = DataTransferUnit.GetNodeValue("LinkText");
        }

        private void linkObjectListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (linkObjectListBox.SelectedIndex != -1)
            {
                okButton_Click(sender, e);
            }
        }

        private void urlTextBox_TextChanged(object sender, EventArgs e)
        {
            CheckState();
        }
    }
}