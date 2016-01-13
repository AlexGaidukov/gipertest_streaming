using System;
using System.Drawing;
using System.Windows.Forms;
using VisualEditor.Logic.Controls.Trees;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Warehouse;

namespace VisualEditor.Logic.Course.Structuring
{
    internal class CourseTreeDragDropHelper
    {
        private TreeNode dragNode;
        private TreeNode tempDropNode;
        private readonly Timer timer;
        private readonly ImageList dragImageList;

        private bool isCycle;
        private const string message = "Данная операция запрещена, так как приводит к зацикливанию компетенций.";

        public CourseTreeDragDropHelper(CourseTree courseTree)
        {
            CourseTree = courseTree;
            CourseTree.AllowDrop = true;

            dragImageList = new ImageList();
            timer = new Timer
            {
                Interval = 200
            };
            timer.Tick += timer_Tick;

            CourseTree.ItemDrag += CourseTree_ItemDrag;
            CourseTree.DragDrop += CourseTree_DragDrop;
            CourseTree.DragEnter += CourseTree_DragEnter;
            CourseTree.DragLeave += CourseTree_DragLeave;
            CourseTree.DragOver += CourseTree_DragOver;
            CourseTree.GiveFeedback += CourseTree_GiveFeedback;
        }

        public CourseTree CourseTree { get; private set; }

        #region Начало перетаскивания

        private void CourseTree_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
            {
                dragNode = (TreeNode)e.Item;
                CourseTree.SelectedNode = dragNode;

                // Запрещено перемещать следующие узлы: 
                // корень учебной программы, входы, выходы, ответы, компетенции 
                // во входах, кроме внешних компетенций.
                if (!(dragNode is CourseRoot ||
                      dragNode is InConceptParent ||
                      dragNode is OutConceptParent ||
                      dragNode is Response ||
                      dragNode is InConceptParent &&
                      dragNode.Parent == CourseTree.InConceptsParent ||
                      dragNode is InDummyConcept &&
                      dragNode.Parent != CourseTree.InConceptsParent))
                {
                    //if (((CourseItem)dragNode).NodeType == Enums.ItemType.Question)
                    //{
                    //    question = (dragNode as Question);
                    //    priorParent = (dragNode.Parent as ECNode);
                    //}

                    dragImageList.Images.Clear();
                    if (dragNode.Bounds.Size.Width + CourseTree.Indent > 256)
                    {
                        dragImageList.ImageSize = new Size(256, dragNode.Bounds.Height);
                    }
                    else
                    {
                        dragImageList.ImageSize = new Size(dragNode.Bounds.Size.Width + CourseTree.Indent,
                                                           dragNode.Bounds.Height);
                    }
                    var bmp = new Bitmap(dragNode.Bounds.Width + CourseTree.Indent, dragNode.Bounds.Height);
                    var g = Graphics.FromImage(bmp);
                    g.DrawString(dragNode.Text, CourseTree.Font, new SolidBrush(CourseTree.ForeColor), CourseTree.Indent,
                                 1.0f);
                    dragImageList.Images.Add(bmp);
                    var p = CourseTree.PointToClient(Control.MousePosition);
                    var dx = p.X + CourseTree.Indent - dragNode.Bounds.Left;
                    var dy = p.Y - dragNode.Bounds.Top;
                    if (DragHelper.ImageList_BeginDrag(dragImageList.Handle, 0, dx, dy))
                    {
                        CourseTree.DoDragDrop(bmp, DragDropEffects.Move);
                        DragHelper.ImageList_EndDrag();
                    }
                }
            }
        }

        #endregion

        #region Перетаскивание

        private void CourseTree_DragOver(object sender, DragEventArgs e)
        {
            if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
            {
                var fromP = CourseTree.PointToClient(new Point(e.X, e.Y));
                DragHelper.ImageList_DragMove(fromP.X - CourseTree.Left, fromP.Y - CourseTree.Top);
                // Узел, над которым движется указатель мыши.
                var dropNode = CourseTree.GetNodeAt(CourseTree.PointToClient(new Point(e.X, e.Y)));
                if (dropNode == null)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }

                #region Условия перетаскивания узлов

                if (dragNode is TrainingModule)
                {
                    // Теоретические модули можно перемещать только в
                    // теоретические модули и в корень учебной программы.
                    if (!(dropNode is TrainingModule ||
                          dropNode is CourseRoot))
                    {
                        e.Effect = DragDropEffects.None;
                        return;
                    }
                }
                else if (dragNode is Group)
                {
                    // Группы можно перемещать только в тестовые модули.
                    if (!(dropNode is TestModule))
                    {
                        e.Effect = DragDropEffects.None;
                        return;
                    }
                }
                else if (dragNode is TestModule)
                {
                    // Тестовые модули можно перемещать только в теоретические модули
                    // и в корень учебной программы.
                    if (!(dropNode is TrainingModule ||
                          dropNode is CourseRoot))
                    {
                        e.Effect = DragDropEffects.None;
                        return;
                    }
                }
                else if (dragNode is Question)
                {
                    // Вопросы можно перемещать только в тестовые модули
                    // и в группы.
                    if (!(dropNode is TestModule ||
                          dropNode is Group))
                    {
                        e.Effect = DragDropEffects.None;
                        return;
                    }
                }
                else if (dragNode is InDummyConcept)
                {
                    // Внешнюю компетенцию можно копировать только во входы учебных модулей.
                    if (!(dropNode is InConceptParent) ||
                        dropNode is InConceptParent &&
                        dropNode == CourseTree.InConceptsParent)
                    {
                        e.Effect = DragDropEffects.None;
                        return;
                    }
                }
                else if (dragNode is OutDummyConcept)
                {
                    // Компетенции можно копировать из выходов только во входы 
                    // учебных модулей, кроме родительского узла внешних компетенций.
                    if (!(dropNode is InConceptParent) ||
                        dropNode == CourseTree.InConceptsParent)
                    {
                        e.Effect = DragDropEffects.None;
                        return;
                    }
                }

                #endregion

                // Не раскрывать конечный узел, если он совпадает с начальным.
                if (dragNode != dropNode)
                {
                    if (!dropNode.IsExpanded)
                    {
                        //dropNode.Expand();
                    }
                }

                e.Effect = DragDropEffects.Move;
                if (tempDropNode != dropNode)
                {
                    DragHelper.ImageList_DragShowNolock(false);
                    CourseTree.SelectedNode = dropNode;
                    // Обновление дерева.
                    CourseTree.Refresh();
                    DragHelper.ImageList_DragShowNolock(true);
                    tempDropNode = dropNode;
                }
                var tmpNode = dropNode;
                while (tmpNode.Parent != null)
                {
                    if (tmpNode.Parent == dragNode)
                    {
                        e.Effect = DragDropEffects.None;
                    }
                    tmpNode = tmpNode.Parent;
                }
            }
        }

        #endregion

        #region Отпускание

        private void CourseTree_DragDrop(object sender, DragEventArgs e)
        {
            if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
            {
                DragHelper.ImageList_DragLeave(CourseTree.Handle);
                // Узел, над которым находится указатель мыши.
                var dropNode = CourseTree.GetNodeAt(CourseTree.PointToClient(new Point(e.X, e.Y)));
                if (dragNode != dropNode)
                {
                    if (dragNode.Parent != dropNode)
                    {
                        // Если копируется входная компетенция из родительского узла внешних компетенций.
                        if (dragNode is InDummyConcept)
                        {
                            // Предотвращение копирования уже имеющеейся во входах компетенции.
                            var ico = IsConceptOccurrence(dropNode as InConceptParent, (dragNode as InDummyConcept).Concept.Id);

                            if (!ico)
                            {
                                var inDummyConcept1 = dragNode as InDummyConcept;
                                var inDummyConcept2 = new InDummyConcept
                                {
                                    Text = inDummyConcept1.Text,
                                    Concept = inDummyConcept1.Concept
                                };

                                dropNode.Nodes.Add(inDummyConcept2);

                                Warehouse.Warehouse.IsProjectModified = true;
                            }
                        }

                        // Если копируется выходная компетенция.
                        if (dragNode is OutDummyConcept)
                        {
                            // Предотвращение копирования уже имеющеейся во входах компетенции.
                            var ico = IsConceptOccurrence(dropNode as InConceptParent, (dragNode as OutDummyConcept).Concept.Id);

                            if (!ico)
                            {
                                var icc = false;

                                // Если не перетаскиваем внешнюю компетенцию учебного курса.
                                if (dragNode.Parent.Parent is TrainingModule)
                                {
                                    isCycle = false;
                                    icc = IsConceptCycle((TrainingModule)dragNode.Parent.Parent, (TrainingModule)dropNode.Parent);
                                }

                                // Если нет зацикливания компетенций.
                                if (!icc)
                                {
                                    var outDummyConcept = dragNode as OutDummyConcept;
                                    var inDummyConcept = new InDummyConcept
                                    {
                                        Text = outDummyConcept.Text,
                                        Concept = outDummyConcept.Concept
                                    };
                                    inDummyConcept.Concept.InDummyConcepts.Add(inDummyConcept);

                                    dropNode.Nodes.Add(inDummyConcept);

                                    Warehouse.Warehouse.IsProjectModified = true;
                                }
                                else
                                {
                                    MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }

                        // Если перемещается контроль.
                        if (dragNode is TestModule)
                        {
                            // Если перемещаем из корня учебной программы в учебный модуль или
                            // из учебного модуля в учебный модуль в сторону уменьшения ответственности.
                            if (dragNode.Parent is CourseRoot &&
                                dropNode is TrainingModule ||
                                dragNode.Parent is TrainingModule &&
                                dropNode is TrainingModule &&
                                dropNode.Parent.Equals(dragNode.Parent))
                            {
                                MessageBox.Show("Данная операция запрещена.", Application.ProductName,
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                                return;
                            }
                        }

                        // Если перемещается узел, отличный от компетенции.
                        if (!(dragNode is OutDummyConcept) &&
                            !(dragNode is InDummyConcept))
                        {
                            #region Перемещается вопрос

                            if (dragNode is Question)
                            {
                                // Перемещение вопроса из контроля в группу.
                                if (dragNode.Parent is TestModule &&
                                    dropNode is Group)
                                {
                                    var g = dropNode as Group;
                                    var q = dragNode as Question;

                                    q.TimeRestriction = g.TimeRestriction;
                                    q.Profile = g.Profile;
                                    q.Marks = g.Marks;
                                }

                                // Перемещение вопроса из группы в контроль.
                                if (dragNode.Parent is Group &&
                                    dropNode is TestModule)
                                {
                                    var q = dragNode as Question;

                                    q.TimeRestriction = 0;
                                    q.Profile = null;
                                    q.Marks = 0;

                                    // Если перемещается последний вопрос из группы, параметры группы обнуляются.
                                    var g = dragNode.Parent as Group;
                                    if (g.Questions.Count.Equals(1))
                                    {
                                        g.TimeRestriction = 0;
                                        g.Profile = null;
                                        g.Marks = 0;
                                    }

                                    if (!g.ChosenQuestionsCount.Equals(0))
                                    {
                                        if (g.ChosenQuestionsCount > g.Questions.Count - 1)
                                        {
                                            g.ChosenQuestionsCount = g.Questions.Count - 1;
                                        }
                                    }
                                }

                                // Перемещение вопроса из группы в группу.
                                if (dragNode.Parent is Group &&
                                    dropNode is Group)
                                {
                                    var g = dropNode as Group;
                                    var q = dragNode as Question;

                                    q.TimeRestriction = g.TimeRestriction;
                                    q.Profile = g.Profile;
                                    q.Marks = g.Marks;

                                    // Если перемещается последний вопрос из группы, параметры группы обнуляются.
                                    g = dragNode.Parent as Group;
                                    if (g.Questions.Count.Equals(1))
                                    {
                                        g.TimeRestriction = 0;
                                        g.Profile = null;
                                        g.Marks = 0;
                                    }

                                    if (!g.ChosenQuestionsCount.Equals(0))
                                    {
                                        if (g.ChosenQuestionsCount > g.Questions.Count - 1)
                                        {
                                            g.ChosenQuestionsCount = g.Questions.Count - 1;
                                        }
                                    }
                                }
                            }

                            #endregion

                            if (dragNode.Parent == null)
                            {
                                CourseTree.Nodes.Remove(dragNode);
                            }
                            else
                            {
                                dragNode.Parent.Nodes.Remove(dragNode);
                            }

                            dropNode.Nodes.Add(dragNode);
                            Warehouse.Warehouse.IsProjectModified = true;
                            //dropNode.ExpandAll();
                            dragNode = null;
                        }
                    }
                    timer.Enabled = false;
                }
            }
        }

        #endregion

        #region Определение образования цикла компетенций

        private bool IsConceptCycle(TrainingModule initialTrainingModule, TrainingModule targetTrainingModule)
        {
            if (initialTrainingModule == targetTrainingModule)
            {
                isCycle = true;

                return true;
            }

            if (targetTrainingModule != null)
            {
                if (targetTrainingModule.OutConceptParent.OutDummyConcepts.Count != 0)
                {
                    foreach (var odc in targetTrainingModule.OutConceptParent.OutDummyConcepts)
                    {
                        try
                        {
                            foreach (var idc in odc.Concept.InDummyConcepts)
                            {
                                IsConceptCycle(initialTrainingModule, idc.Parent.Parent as TrainingModule);
                            }
                        }
                        catch { }
                    }
                }
            }

            return isCycle;
        }

        #endregion

        #region Определение вхождения компетенций во входы

        private static bool IsConceptOccurrence(InConceptParent icp, Guid id)
        {
            foreach (var c in icp.InDummyConcepts)
            {
                if (c.Concept.Id == id)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        private void CourseTree_DragEnter(object sender, DragEventArgs e)
        {
            if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
            {
                DragHelper.ImageList_DragEnter(CourseTree.Handle, e.X - CourseTree.Left, e.Y - CourseTree.Top);
                timer.Enabled = true;
            }
        }

        private void CourseTree_DragLeave(object sender, EventArgs e)
        {
            if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
            {
                DragHelper.ImageList_DragLeave(CourseTree.Handle);
                timer.Enabled = false;
            }
        }

        private void CourseTree_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
            {
                if (e.Effect == DragDropEffects.Move)
                {
                    e.UseDefaultCursors = false;
                    CourseTree.Cursor = Cursors.Default;
                }
                else e.UseDefaultCursors = true;
            }
        }

        #region Скроллирование дерева

        private void timer_Tick(object sender, EventArgs e)
        {
            var pt = CourseTree.PointToClient(Control.MousePosition);
            var node = CourseTree.GetNodeAt(pt);
            if (node == null)
            {
                return;
            }
            if (pt.Y < 10)
            {
                if (node.PrevVisibleNode != null)
                {
                    node = node.PrevVisibleNode;
                    DragHelper.ImageList_DragShowNolock(false);
                    node.EnsureVisible();
                    CourseTree.Refresh();
                    DragHelper.ImageList_DragShowNolock(true);
                }
            }
            else if (pt.Y > CourseTree.Size.Height - 10)
            {
                if (node.NextVisibleNode != null)
                {
                    node = node.NextVisibleNode;
                    DragHelper.ImageList_DragShowNolock(false);
                    node.EnsureVisible();
                    CourseTree.Refresh();
                    DragHelper.ImageList_DragShowNolock(true);
                }
            }
        }

        #endregion 
    }
}