using System.Windows.Forms;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Dialogs;

namespace VisualEditor.Logic.Controls.Trees
{
    internal class WarningTree : TreeView
    {
        public WarningTree()
        {
            InitializeTree();
        }

        private void InitializeTree()
        {
            ShowLines = false;
            ShowRootLines = false;
            MouseDoubleClick += WarningsTree_MouseDoubleClick;

            var il = new ImageList();
            il.Images.Add(Properties.Resources.Warning);
            il.ColorDepth = ColorDepth.Depth32Bit;
            ImageList = il;
        }

        private void WarningsTree_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var currentWarning = GetNodeAt(e.Location) as WarningNode;
            if (currentWarning == null)
            {
                return;
            }

            if (currentWarning.WarningType == Enums.WarningType.EmptyTestModule)
            {
                if (currentWarning.WarningTestModule != null)
                {
                    var tm = currentWarning.WarningTestModule;
                    tm.Expand();
                    Warehouse.Warehouse.Instance.CourseTree.CurrentNode = tm;
                    Warehouse.Warehouse.Instance.CourseTree.HandleContextMenu();
                }
            }

            if (currentWarning.WarningType == Enums.WarningType.MissedProfile ||
                currentWarning.WarningType == Enums.WarningType.ZeroMarks ||
                currentWarning.WarningType == Enums.WarningType.ZeroChosenQuestionsCount)
            {
                if (currentWarning.WarningGroup != null)
                {
                    var g = currentWarning.WarningGroup;
                    g.Expand();
                    Warehouse.Warehouse.Instance.CourseTree.CurrentNode = g;
                    Warehouse.Warehouse.Instance.CourseTree.HandleContextMenu();

                    using (var gd = new GroupDialog())
                    {
                        gd.InitializeData();

                        if (gd.ShowDialog(MainForm.Instance).Equals(DialogResult.OK))
                        {

                        }
                    }
                }

                if (currentWarning.WarningQuestion != null)
                {
                    var q = currentWarning.WarningQuestion;
                    q.Expand();
                    Warehouse.Warehouse.Instance.CourseTree.CurrentNode = q;
                    Warehouse.Warehouse.Instance.CourseTree.HandleContextMenu();

                    if (!(q.Parent as TestModule).QuestionSequence.Equals(Enums.QuestionSequence.Network))
                    {
                        using (var qd = new QuestionDialog())
                        {
                            qd.InitializeData();

                            if (qd.ShowDialog(MainForm.Instance).Equals(DialogResult.OK))
                            {

                            }
                        }
                    }
                    else
                    {
                        using (var qd = new NetQuestionDialog())
                        {
                            qd.InitializeData(q);

                            if (qd.ShowDialog(MainForm.Instance).Equals(DialogResult.OK))
                            {

                            }
                        }
                    }
                }
            }

            if (currentWarning.WarningType == Enums.WarningType.EmptyGroup)
            {
                if (currentWarning.WarningGroup != null)
                {
                    var g = currentWarning.WarningGroup;
                    g.Expand();
                    Warehouse.Warehouse.Instance.CourseTree.CurrentNode = g;
                    Warehouse.Warehouse.Instance.CourseTree.HandleContextMenu();
                }
            }

            if (currentWarning.WarningType == Enums.WarningType.NoResponses)
            {
                if (currentWarning.WarningQuestion != null)
                {
                    var q = currentWarning.WarningQuestion;
                    q.Expand();
                    Warehouse.Warehouse.Instance.CourseTree.CurrentNode = q;
                    Warehouse.Warehouse.Instance.CourseTree.HandleContextMenu();
                }
            }

            if (currentWarning.WarningType == Enums.WarningType.NoResponseVariants)
            {
                if (currentWarning.WarningQuestion != null)
                {
                    var q = currentWarning.WarningQuestion;
                    q.Expand();
                    Warehouse.Warehouse.Instance.CourseTree.CurrentNode = q;
                    Warehouse.Warehouse.Instance.CourseTree.HandleContextMenu();
                }

                using (var rvd = new ResponseVariantDialog())
                {
                    if (rvd.ShowDialog(MainForm.Instance).Equals(DialogResult.OK))
                    {

                    }
                }
            }
        }
    }
}