using System.Windows.Forms;
using VisualEditor.Logic.Controls.Trees;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Warehouse;

namespace VisualEditor.Logic.Course.Structuring
{
    internal class CourseTreeKeyboardHelper
    {
        public CourseTreeKeyboardHelper(CourseTree courseTree)
        {
            CourseTree = courseTree;
            CourseTree.KeyDown += CourseTree_KeyDown;
        }

        public CourseTree CourseTree { get; private set; }

        private void CourseTree_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
                {
                    if (CourseTree.CurrentNode != null)
                    {
                        var parentNode = CourseTree.CurrentNode.Parent;
                        if (parentNode != null)
                        {
                            var index = parentNode.Nodes.IndexOf(CourseTree.CurrentNode);
                            // Необходима для хранения ссылки.
                            var cn = CourseTree.CurrentNode;
                            // Запрещено перемещать следующие узлы:
                            // входы, выходы, компетенции, корень учебной программы.
                            if (!(CourseTree.CurrentNode is InConceptParent ||
                                  CourseTree.CurrentNode is OutConceptParent ||
                                  CourseTree.CurrentNode is InDummyConcept ||
                                  CourseTree.CurrentNode is OutDummyConcept ||
                                  CourseTree.CurrentNode is CourseRoot))
                            {
                                if (e.KeyCode == Keys.Up)
                                {
                                    if (CourseTree.CurrentNode.PrevNode != null)
                                    {
                                        // Запрещено менять местами узел-внешние компетенции
                                        // и узел, находящийся ниже.
                                        if (!(CourseTree.CurrentNode.PrevNode is InConceptParent ||
                                              CourseTree.CurrentNode.PrevNode is OutConceptParent))
                                        {
                                            parentNode.Nodes.Remove(CourseTree.CurrentNode);
                                            parentNode.Nodes.Insert(index - 1, cn);
                                            CourseTree.CurrentNode = parentNode.Nodes[index - 1] as CourseItem;

                                            Warehouse.Warehouse.IsProjectModified = true;
                                        }
                                    }
                                }
                                else if (e.KeyCode == Keys.Down)
                                {
                                    if (CourseTree.CurrentNode.NextNode != null)
                                    {
                                        parentNode.Nodes.Remove(CourseTree.CurrentNode);
                                        parentNode.Nodes.Insert(index + 1, cn);
                                        CourseTree.CurrentNode = parentNode.Nodes[index + 1] as CourseItem;

                                        Warehouse.Warehouse.IsProjectModified = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}