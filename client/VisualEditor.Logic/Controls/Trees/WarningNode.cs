using System.Windows.Forms;
using VisualEditor.Logic.Course.Items;

namespace VisualEditor.Logic.Controls.Trees
{
    internal class WarningNode : TreeNode
    {
        private Enums.WarningType warningType;

        public WarningNode(Enums.WarningType warningType)
        {
            this.warningType = warningType;
            ImageIndex = SelectedImageIndex = 0;
        }

        public Enums.WarningType WarningType
        {
            get { return warningType; }
        }
        public Question WarningQuestion { get; set; }
        public Group WarningGroup { get; set; }
        public TestModule WarningTestModule { get; set; }
    }
}