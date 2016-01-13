using System.Collections;
using System.Windows.Forms;

namespace VisualEditor.Logic.Controls.Trees
{
    internal class ConceptSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            var tx = x as TreeNode;
            var ty = y as TreeNode;
            var res = string.Compare(ty.Text, tx.Text);

            if (res == 1)
            {
                res = -1;
            }

            if (res == -1)
            {
                res = 1;
            }

            return res;
        }
    }
}