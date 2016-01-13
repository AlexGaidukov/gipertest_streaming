using System.Windows.Forms;
using VisualEditor.Logic.Commands;
using VisualEditor.Logic.Controls.Ribbon;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Trees
{
    internal class ConceptTree : TreeView
    {
        private Concept currentNode;
        private RibbonContextMenu conceptContextMenu;
        private bool contextMenuDetached;

        private const string conceptAlreadyExistsMessage = "В списке компетенций уже существует компетенция с таким именем.";

        public ConceptTree()
        {
            InitializeTree();
        }

        public Concept CurrentNode
        {
            get
            {
                return currentNode;
            }
            set
            {
                currentNode = value;
                SelectedNode = currentNode;
            }
        }

        private void InitializeTree()
        {
            TreeViewNodeSorter = new ConceptSorter();
            HideSelection = false;
            ShowLines = false;
            ShowRootLines = false;
            AfterLabelEdit += ConceptsTree_AfterLabelEdit;
            AfterSelect += ConceptsTree_AfterSelect;
            KeyDown += ConceptsTree_KeyDown;
            MouseDoubleClick += ConceptsTree_MouseDoubleClick;
            MouseDown += ConceptsTree_MouseDown;

            var il = new ImageList();
            il.Images.Add(Properties.Resources.Profile);
            il.ColorDepth = ColorDepth.Depth32Bit;
            ImageList = il;

            contextMenuDetached = false;
        }

        #region InitializeContextMenu

        private void InitializeContextMenu()
        {
            conceptContextMenu = new RibbonContextMenu();
            RibbonHelper.AddButton(conceptContextMenu, CommandManager.Instance.GetCommand(CommandNames.NavigateToConcept));
            RibbonHelper.AddSeparator(conceptContextMenu);
            RibbonHelper.AddButton(conceptContextMenu, CommandManager.Instance.GetCommand(CommandNames.Profile));
            RibbonHelper.AddButton(conceptContextMenu, CommandManager.Instance.GetCommand(CommandNames.ProfileOptionsSmall));
            RibbonHelper.AddSeparator(conceptContextMenu);
            RibbonHelper.AddButton(conceptContextMenu, CommandManager.Instance.GetCommand(CommandNames.DeleteConcept));
            RibbonHelper.AddSeparator(conceptContextMenu);
            RibbonHelper.AddButton(conceptContextMenu, CommandManager.Instance.GetCommand(CommandNames.RenameItem));
        }

        #endregion

        #region AttachContextMenu

        private void AttachContextMenu()
        {
            if (CurrentNode == null)
            {
                return;
            }

            if (CurrentNode is Concept)
            {
                CurrentNode.ContextMenuStrip = conceptContextMenu;
            }
        }

        #endregion

        #region DetachContextMenu

        private void DetachContextMenu()
        {
            foreach (TreeNode n in Nodes)
            {
                n.ContextMenuStrip = null;
            }
        }

        #endregion

        private void ConceptsTree_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Left))
            {
                CommandManager.Instance.GetCommand(CommandNames.NavigateToConcept).Execute(null);
            }
        }

        private void ConceptsTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var tn = e.Node;
            if (tn != null)
            {
                CurrentNode = tn as Concept;
            }

            if (conceptContextMenu == null)
            {
                InitializeContextMenu();
            }

            if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
            {
                AttachContextMenu();
                contextMenuDetached = false;
            }
            else
            {
                if (!contextMenuDetached)
                {
                    DetachContextMenu();
                    contextMenuDetached = true;
                }
            }
        }

        private void ConceptsTree_MouseDown(object sender, MouseEventArgs e)
        {
            var tn = GetNodeAt(e.X, e.Y);
            if (tn != null)
            {
                CurrentNode = tn as Concept;
            }

            if (conceptContextMenu == null)
            {
                InitializeContextMenu();
            }

            if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
            {
                AttachContextMenu();
                contextMenuDetached = false;
            }
            else
            {
                if (!contextMenuDetached)
                {
                    DetachContextMenu();
                    contextMenuDetached = true;
                }
            }
        }

        #region Команды с клавиатуры

        private void ConceptsTree_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Delete))
            {
                if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
                {
                    CommandManager.Instance.GetCommand(CommandNames.DeleteConcept).Execute(null);
                }
            }

            if (e.KeyCode.Equals(Keys.Enter))
            {
                CommandManager.Instance.GetCommand(CommandNames.NavigateToConcept).Execute(null);
            }

            if (e.KeyCode.Equals(Keys.F2))
            {
                if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
                {
                    CommandManager.Instance.GetCommand(CommandNames.RenameItem).Execute(null);
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

            if (e.KeyCode.Equals(Keys.F7))
            {
                CommandManager.Instance.GetCommand(CommandNames.Course).Execute(null);
            }
        }

        #endregion

        // POSTPONE: Продумать.
        private void ConceptsTree_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            // Если пользователь изменил имя узла.
            //if (e.Label != null)
            //{
            //    if (e.Label != string.Empty)
            //    {
            //        foreach (TreeNode node in Nodes)
            //        {
            //            if (node.Text.Equals(e.Label))
            //            {
            //                MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                Focus();
            //                e.CancelEdit = true;

            //                if (!CurrentNode.IsEditing)
            //                {
            //                    CurrentNode.BeginEdit();
            //                }

            //                return;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        e.CancelEdit = true;
            //    }
            //}
            LabelEdit = false;
        }
    }
}