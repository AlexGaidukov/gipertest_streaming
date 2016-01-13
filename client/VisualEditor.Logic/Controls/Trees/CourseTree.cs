using System.Windows.Forms;
using VisualEditor.Logic.Commands;
using VisualEditor.Logic.Controls.Docking;
using VisualEditor.Logic.Controls.Ribbon;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Course.Items.Questions;
using VisualEditor.Logic.Course.Structuring;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Trees
{
    internal class CourseTree : TreeView
    {
        private RibbonContextMenu courseRootContextMenu;
        private RibbonContextMenu trainingModuleContextMenu;
        private RibbonContextMenu conceptContextMenu;
        private RibbonContextMenu testModuleContextMenu;
        private RibbonContextMenu groupContextMenu;
        private RibbonContextMenu questionContextMenu;
        private RibbonContextMenu responseContextMenu;

        private bool contextMenuDetached;
        private CourseItem currentNode;

        private CourseTreeDragDropHelper courseTreeDragDropHelper;
        private CourseTreeKeyboardHelper courseTreeKeyboardHelper;

        private const string message = "В текущей области видимости уже сужествует узел учебного курса с таким именем.";

        public CourseTree()
        {
            InitializeTree();
        }

        public CourseItem CurrentNode
        {
            get { return currentNode; }
            set
            {
                currentNode = value;
                SelectedNode = currentNode;
            }
        }

        public InConceptParent InConceptsParent
        {
            get
            {
                if (Warehouse.Warehouse.IsProjectBeingDesigned)
                {
                    return (InConceptParent) Nodes[0].Nodes[0];
                }

                return null;
            }
        }

        private void InitializeTree()
        {
            HideSelection = false;

            courseTreeDragDropHelper = new CourseTreeDragDropHelper(this);
            courseTreeKeyboardHelper = new CourseTreeKeyboardHelper(this);

            AfterLabelEdit += CourseTree_AfterLabelEdit;
            AfterSelect += CourseTree_AfterSelect;
            KeyDown += CourseTree_KeyDown;
            MouseDown += CourseTree_MouseDown;
            MouseUp += CourseTree_MouseUp;

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

            contextMenuDetached = false;
        }

        private void CourseTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var tn = e.Node;
            if (tn == null)
            {
                return;
            }

            CurrentNode = tn as CourseItem;
            HandleContextMenu();
        }

        private void CourseTree_MouseDown(object sender, MouseEventArgs e)
        {
            var tn = GetNodeAt(e.X, e.Y);
            if (tn == null)
            {
                return;
            }

            CurrentNode = tn as CourseItem;
            HandleContextMenu();
        }

        private void CourseTree_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (CurrentNode is TrainingModule ||
                    CurrentNode is Question ||
                    CurrentNode is Response)
                {
                    CommandManager.Instance.GetCommand(CommandNames.ViewDocument).Execute(null);
                }
            }
        }

        #region AttachContextMenu

        private void AttachContextMenu()
        {
            if (CurrentNode == null)
            {
                return;
            }

            if (CurrentNode is CourseRoot)
            {
                if (courseRootContextMenu == null)
                {
                    courseRootContextMenu = new RibbonContextMenu();
                    RibbonHelper.AddButton(courseRootContextMenu, CommandManager.Instance.GetCommand(CommandNames.AddTrainingModule));
                    RibbonHelper.AddButton(courseRootContextMenu, CommandManager.Instance.GetCommand(CommandNames.AddInTestModule));
                    RibbonHelper.AddButton(courseRootContextMenu, CommandManager.Instance.GetCommand(CommandNames.AddOutTestModule));
                    RibbonHelper.AddButton(courseRootContextMenu, CommandManager.Instance.GetCommand(CommandNames.AddTestModuleFromOuterCourseSmall));
                    RibbonHelper.AddSeparator(courseRootContextMenu);
                    RibbonHelper.AddButton(courseRootContextMenu, CommandManager.Instance.GetCommand(CommandNames.RenameItem));
                    RibbonHelper.AddSeparator(courseRootContextMenu);
                    RibbonHelper.AddButton(courseRootContextMenu, CommandManager.Instance.GetCommand(CommandNames.LoadFromImsQti));
                }

                CurrentNode.ContextMenuStrip = courseRootContextMenu;
            }

            if (CurrentNode is TrainingModule)
            {
                if (trainingModuleContextMenu == null)
                {
                    trainingModuleContextMenu = new RibbonContextMenu();
                    RibbonHelper.AddButton(trainingModuleContextMenu, CommandManager.Instance.GetCommand(CommandNames.ViewDocument));
                    RibbonHelper.AddSeparator(trainingModuleContextMenu);
                    RibbonHelper.AddButton(trainingModuleContextMenu, CommandManager.Instance.GetCommand(CommandNames.AddTrainingModule));
                    RibbonHelper.AddButton(trainingModuleContextMenu, CommandManager.Instance.GetCommand(CommandNames.AddInTestModule));
                    RibbonHelper.AddButton(trainingModuleContextMenu, CommandManager.Instance.GetCommand(CommandNames.AddOutTestModule));
                    RibbonHelper.AddButton(trainingModuleContextMenu, CommandManager.Instance.GetCommand(CommandNames.AddTestModuleFromOuterCourseSmall));
                    RibbonHelper.AddSeparator(trainingModuleContextMenu);
                    // POSTPONE: Релизовать.
                    //RibbonHelper.AddButton(trainingModuleContextMenu, CommandManager.Instance.GetCommand(CommandNames.CutItem));
                    //RibbonHelper.AddButton(trainingModuleContextMenu, CommandManager.Instance.GetCommand(CommandNames.PasteItem));
                    RibbonHelper.AddButton(trainingModuleContextMenu, CommandManager.Instance.GetCommand(CommandNames.DeleteItem));
                    RibbonHelper.AddSeparator(trainingModuleContextMenu);
                    RibbonHelper.AddButton(trainingModuleContextMenu, CommandManager.Instance.GetCommand(CommandNames.RenameItem));

                    RibbonHelper.AddSeparator(trainingModuleContextMenu);
                    RibbonHelper.AddButton(trainingModuleContextMenu, CommandManager.Instance.GetCommand(CommandNames.LoadFromImsQti));
                   // RibbonHelper.AddButton(trainingModuleContextMenu, CommandManager.Instance.GetCommand(CommandNames.SaveToImsQti));
                }

                CurrentNode.ContextMenuStrip = trainingModuleContextMenu;
            }

            if (CurrentNode is TestModule)
            {
                if (testModuleContextMenu == null)
                {
                    testModuleContextMenu = new RibbonContextMenu();
                    RibbonHelper.AddButton(testModuleContextMenu, CommandManager.Instance.GetCommand(CommandNames.ItemOptionsSmall));
                    RibbonHelper.AddSeparator(testModuleContextMenu);
                    RibbonHelper.AddButton(testModuleContextMenu, CommandManager.Instance.GetCommand(CommandNames.AddGroup));
                    var b = RibbonHelper.AddButton(testModuleContextMenu, "Добавить вопрос");
                    RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.AddChoiceQuestionSmall));
                    RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.AddMultichoiceQuestionSmall));
                    RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.AddOrderingQuestionSmall));
                    RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.AddOpenQuestionSmall));
                    RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.AddCorrespondenceQuestionSmall));
                    RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.AddOuterQuestionSmall));
                    RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.AddQuestionFromOuterCourseSmall));
                    RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.AddInteractiveQuestionSmall));
                    RibbonHelper.AddSeparator(testModuleContextMenu);
                    // POSTPONE: Релизовать.
                    //RibbonHelper.AddButton(testModuleContextMenu, CommandManager.Instance.GetCommand(CommandNames.CutItem));
                    //RibbonHelper.AddButton(testModuleContextMenu, CommandManager.Instance.GetCommand(CommandNames.PasteItem));
                    RibbonHelper.AddButton(testModuleContextMenu, CommandManager.Instance.GetCommand(CommandNames.DeleteItem));
                    RibbonHelper.AddSeparator(testModuleContextMenu);
                    RibbonHelper.AddButton(testModuleContextMenu, CommandManager.Instance.GetCommand(CommandNames.RenameItem));
                    
                        RibbonHelper.AddSeparator(testModuleContextMenu);
                        RibbonHelper.AddButton(testModuleContextMenu, CommandManager.Instance.GetCommand(CommandNames.LoadFromImsQti));
                        RibbonHelper.AddButton(testModuleContextMenu, CommandManager.Instance.GetCommand(CommandNames.SaveToImsQti));
                   
                }

                CurrentNode.ContextMenuStrip = testModuleContextMenu;
            }

            if (CurrentNode is Group)
            {
                if (groupContextMenu == null)
                {
                    groupContextMenu = new RibbonContextMenu();
                    RibbonHelper.AddButton(groupContextMenu, CommandManager.Instance.GetCommand(CommandNames.ItemOptionsSmall));
                    RibbonHelper.AddSeparator(groupContextMenu);
                    var b = RibbonHelper.AddButton(groupContextMenu, "Добавить вопрос");
                    RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.AddChoiceQuestionSmall));
                    RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.AddMultichoiceQuestionSmall));
                    RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.AddOrderingQuestionSmall));
                    RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.AddOpenQuestionSmall));
                    RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.AddCorrespondenceQuestionSmall));
                    RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.AddOuterQuestionSmall));
                    RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.AddQuestionFromOuterCourseSmall));
                    RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.AddInteractiveQuestionSmall));
                    RibbonHelper.AddSeparator(groupContextMenu);
                    // POSTPONE: Релизовать.
                    //RibbonHelper.AddButton(groupContextMenu, CommandManager.Instance.GetCommand(CommandNames.CutItem));
                    //RibbonHelper.AddButton(groupContextMenu, CommandManager.Instance.GetCommand(CommandNames.CopyItem));
                    //RibbonHelper.AddButton(groupContextMenu, CommandManager.Instance.GetCommand(CommandNames.PasteItem));
                    RibbonHelper.AddButton(groupContextMenu, CommandManager.Instance.GetCommand(CommandNames.DeleteItem));
                    RibbonHelper.AddSeparator(groupContextMenu);
                    RibbonHelper.AddButton(groupContextMenu, CommandManager.Instance.GetCommand(CommandNames.RenameItem));
                    RibbonHelper.AddSeparator(groupContextMenu);
                    RibbonHelper.AddButton(groupContextMenu,CommandManager.Instance.GetCommand(CommandNames.LoadFromImsQti));
                }

                CurrentNode.ContextMenuStrip = groupContextMenu;
            }

            if (CurrentNode is Question)
            {
                if (questionContextMenu == null)
                {
                    questionContextMenu = new RibbonContextMenu();
                    RibbonHelper.AddButton(questionContextMenu, CommandManager.Instance.GetCommand(CommandNames.ItemOptionsSmall));
                    RibbonHelper.AddSeparator(questionContextMenu);
                    RibbonHelper.AddButton(questionContextMenu, CommandManager.Instance.GetCommand(CommandNames.AddResponse));
                    RibbonHelper.AddSeparator(questionContextMenu);
                    // POSTPONE: Релизовать.
                    //RibbonHelper.AddButton(questionContextMenu, CommandManager.Instance.GetCommand(CommandNames.CutItem));
                    //RibbonHelper.AddButton(questionContextMenu, CommandManager.Instance.GetCommand(CommandNames.CopyItem));
                    //RibbonHelper.AddButton(questionContextMenu, CommandManager.Instance.GetCommand(CommandNames.PasteItem));
                    RibbonHelper.AddButton(questionContextMenu, CommandManager.Instance.GetCommand(CommandNames.DeleteItem));
                    RibbonHelper.AddSeparator(questionContextMenu);
                    RibbonHelper.AddButton(questionContextMenu, CommandManager.Instance.GetCommand(CommandNames.RenameItem));

                    RibbonHelper.AddSeparator(questionContextMenu);
                   // RibbonHelper.AddButton(questionContextMenu, CommandManager.Instance.GetCommand(CommandNames.LoadFromImsQti));
                    RibbonHelper.AddButton(questionContextMenu, CommandManager.Instance.GetCommand(CommandNames.SaveToImsQti));
                }

                CurrentNode.ContextMenuStrip = questionContextMenu;
            }

            if (CurrentNode is Response)
            {
                if (responseContextMenu == null)
                {
                    responseContextMenu = new RibbonContextMenu();
                    RibbonHelper.AddButton(responseContextMenu, CommandManager.Instance.GetCommand(CommandNames.DeleteItem));
                    RibbonHelper.AddSeparator(responseContextMenu);
                    RibbonHelper.AddButton(responseContextMenu, CommandManager.Instance.GetCommand(CommandNames.RenameItem));
                }

                CurrentNode.ContextMenuStrip = responseContextMenu;
            }
        }

        #endregion

        #region DetachContextMenu

        private static void DetachContextMenu(TreeNodeCollection tnc)
        {
            for (var i = 0; i < tnc.Count; i++)
            {
                var tn = tnc[i];
                if (tn.Nodes.Count != 0)
                {
                    DetachContextMenu(tnc[i].Nodes);
                }

                tn.ContextMenuStrip = null;
            }
        }

        #endregion

        #region HandleContextMenu

        public void HandleContextMenu()
        {
            if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
            {
                AttachContextMenu();

                #region Делает активным/неактивным пункт меню Добавить элемент ответа

                var q = CurrentNode as OpenQuestion;
                if (q != null)
                {
                    CommandManager.Instance.GetCommand(CommandNames.AddResponse).Enabled = false;
                }
                else
                {
                    CommandManager.Instance.GetCommand(CommandNames.AddResponse).Enabled = true;
                }

                #endregion

                #region В естественном и сетевом контроле нельзя добавлять группы вопросов

                var tm = CurrentNode as TestModule;
                if (tm != null)
                {
                    if (tm.QuestionSequence.Equals(Enums.QuestionSequence.Natural) ||
                        tm.QuestionSequence.Equals(Enums.QuestionSequence.Network))
                    {
                        CommandManager.Instance.GetCommand(CommandNames.AddGroup).Enabled = false;
                    }
                    else
                    {
                        CommandManager.Instance.GetCommand(CommandNames.AddGroup).Enabled = true;
                    }
                }

                #endregion

                contextMenuDetached = false;
            }
            else
            {
                if (!contextMenuDetached)
                {
                    DetachContextMenu(Nodes);
                    contextMenuDetached = true;
                }
            }
        }

        #endregion

        #region Команды с клавиатуры

        private void CourseTree_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (CurrentNode is TrainingModule ||
                    CurrentNode is Question ||
                    CurrentNode is Response)
                {
                    CommandManager.Instance.GetCommand(CommandNames.ViewDocument).Execute(null);
                }
            }

            if (e.KeyCode == Keys.F2)
            {
                if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
                {
                    CommandManager.Instance.GetCommand(CommandNames.RenameItem).Execute(null);
                }
            }

            if (e.Alt)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
                    {
                        CommandManager.Instance.GetCommand(CommandNames.ItemOptionsSmall).Execute(null);
                    }
                }
            }

            if (e.KeyCode.Equals(Keys.F5))
            {
                CommandManager.Instance.GetCommand(CommandNames.Preview).Execute(null);
            }

            if (e.KeyCode.Equals(Keys.F6))
            {
                CommandManager.Instance.GetCommand(CommandNames.Design).Execute(null);
            }

            if (e.KeyCode.Equals(Keys.F8))
            {
                CommandManager.Instance.GetCommand(CommandNames.Concepts).Execute(null);
            }

            if (e.KeyCode.Equals(Keys.Delete))
            {
                CommandManager.Instance.GetCommand(CommandNames.DeleteItem).Execute(null);
            }
        }

        #endregion

        #region Действия после переименования узла

        private void CourseTree_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            // Если пользователь изменил имя узла.
            if (e.Label != null)
            {
                if (e.Label != string.Empty)
                {
                    if (!(CurrentNode is CourseRoot))
                    {
                        foreach (TreeNode node in CurrentNode.Parent.Nodes)
                        {
                            if (node.Text.Equals(e.Label))
                            {
                                MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Focus();
                                e.CancelEdit = true;

                                if (!CurrentNode.IsEditing)
                                {
                                    CurrentNode.BeginEdit();
                                }

                                return;
                            }
                        }
                    }

                    if (CurrentNode is CourseRoot ||
                        CurrentNode is Group ||
                        CurrentNode is InDummyConcept ||
                        CurrentNode is OutDummyConcept ||
                        CurrentNode is TestModule)
                    {
                        //eCTree.ClickedNode.Text = e.Label;
                    }

                    if (CurrentNode is TrainingModule)
                    {
                        var tm = CurrentNode as TrainingModule;

                        // Если документ был создан.
                        if (tm.TrainingModuleDocument != null)
                        {
                            if (e.Label.Length > 22)
                            {
                                (CurrentNode as TrainingModule).TrainingModuleDocument.Text = e.Label.Substring(0, 22) + "...";
                            }
                            else
                            {
                                (CurrentNode as TrainingModule).TrainingModuleDocument.Text = e.Label;
                            }

                            if (tm.TrainingModuleDocument.Equals(DockContainer.Instance.ActiveDocument))
                            {
                                MainForm.Instance.Text = string.Concat(e.Label, " - ", Application.ProductName);
                            }
                        }
                    }

                    if (CurrentNode is Question ||
                        CurrentNode is Response)
                    {

                    }
                }
                else
                {
                    e.CancelEdit = true;
                }
            }
            LabelEdit = false;
        }

        #endregion
    }
}