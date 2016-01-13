using System;
using System.Windows.Forms;

namespace VisualEditor.Logic.Course.Items
{
    internal class Bookmark : TreeNode
    {
        public Guid Id { get; set; }
        public Guid ModuleId { get; set; }
    }
}