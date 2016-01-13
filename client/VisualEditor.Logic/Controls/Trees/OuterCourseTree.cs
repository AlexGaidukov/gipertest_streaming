using System.Windows.Forms;

namespace VisualEditor.Logic.Controls.Trees
{
    internal class OuterCourseTree : TreeView
    {
        public OuterCourseTree()
        {
            InitializeTree();
        }

        #region InitializeTree

        private void InitializeTree()
        {
            HideSelection = false;

            var il = new ImageList();
            il.Images.Add(Properties.Resources.CourseRoot);
            il.Images.Add(Properties.Resources.TrainingModule);
            il.Images.Add(Properties.Resources.InConceptParent);
            il.Images.Add(Properties.Resources.OutConceptParent);
            il.Images.Add(Properties.Resources.InDummyConcept);
            il.Images.Add(Properties.Resources.OutDummyConcept);
            il.Images.Add(Properties.Resources.TestModule);
            il.Images.Add(Properties.Resources.Group);
            il.Images.Add(Properties.Resources.Question);
            il.Images.Add(Properties.Resources.Response);
            il.ColorDepth = ColorDepth.Depth32Bit;
            ImageList = il;
        }

        #endregion
    }
}