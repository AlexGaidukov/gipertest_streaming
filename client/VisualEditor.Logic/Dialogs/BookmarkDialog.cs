using System;
using System.Windows.Forms;
using VisualEditor.Logic.Controls.Docking;
using VisualEditor.Logic.Controls.Docking.Documents;
using VisualEditor.Logic.IO;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.Controls.HtmlEditing;
using VisualEditor.Utils.Helpers;

namespace VisualEditor.Logic.Dialogs
{
    internal partial class BookmarkDialog : DialogBase
    {
        private const string deleteBookmarkMessage = "Вы уверены, что хотите удалить закладку?\nВсе ссылки на удаляемую закладку будут удалены.";

        public BookmarkDialog()
        {
            InitializeComponent();
            InitializeDialog();
        }

        public XmlHelper DataTransferUnit { get; set; }

        public void InitializeDialog()
        {
            DataTransferUnit = new XmlHelper();
            DataTransferUnit.AppendNode(string.Empty, "Data");
            DataTransferUnit.AppendNode("Data", "BookmarkName");

            FillBookmarksTree();
            HelpKeyword = "Закладка";
            bookmarkNameTextBox.Select();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            DataTransferUnit.SetNodeValue("BookmarkName", bookmarkNameTextBox.Text);

            Warehouse.Warehouse.IsProjectModified = true;
            DialogResult = DialogResult.OK;
        }

        private void bookmarkNameTextBox_TextChanged(object sender, EventArgs e)
        {
            CheckState();
        }

        private void bookmarksTree_CurrentNodeChanged(object sender, EventArgs e)
        {
            CheckState();
        }

        private void wholeCourseRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            ReleaseBookmarks();
            FillBookmarksTree();
        }

        private void currentModuleRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            ReleaseBookmarks();
            FillBookmarksTree();
        }

        private void BookmarkDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            ReleaseBookmarks();
        }

        private void bookmarksTree_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Delete))
            {
                Delete();
            }

            //if (e.KeyCode.Equals(Keys.Enter))
            //{
            //    Navigate();
            //}
        }

        private void bookmarksTree_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            NavigateToBookmark();
        }

        private void navigateButton_Click(object sender, EventArgs e)
        {
            if (bookmarkTree.CurrentNode == null)
            {
                return;
            }

            NavigateToBookmark();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            Delete();
        }

        private void CheckState()
        {
            addButton.Enabled = !bookmarkNameTextBox.Text.Equals(string.Empty);
            if (bookmarkTree.CurrentNode != null)
            {
                deleteButton.Enabled = true;
                navigateButton.Enabled = true;
            }
            else
            {
                deleteButton.Enabled = false;
                navigateButton.Enabled = false;                
            }
        }

        #region ReleaseBookmarks

        private void ReleaseBookmarks()
        {
            bookmarkTree.Nodes.Clear();
        }

        #endregion

        #region FillBookmarksTree

        private void FillBookmarksTree()
        {
            var bs = Warehouse.Warehouse.Instance.Bookmarks;
            var cmi = ((TrainingModuleDocument)Logic.Controls.HtmlEditing.HtmlEditingToolHelper.GetParentDocument(EditorObserver.ActiveEditor)).TrainingModule.Id;

            if (wholeCourseRadioButton.Checked)
            {
                foreach (var b in bs)
                {
                    bookmarkTree.Nodes.Add(b);
                }
            }
            else if (currentModuleRadioButton.Checked)
            {
                foreach (var b in bs)
                {
                    if (b.ModuleId.Equals(cmi))
                    {
                        bookmarkTree.Nodes.Add(b);
                    }
                }
            }
        }

        #endregion

        #region Delete
        
        private void Delete()
        {
            if (MessageBox.Show(deleteBookmarkMessage, Application.ProductName, 
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                var b = bookmarkTree.CurrentNode;

                // Удаляет закладку из дерева закладок.
                bookmarkTree.Nodes.Remove(b);

                if (bookmarkTree.Nodes.Count.Equals(0))
                {
                    bookmarkTree.CurrentNode = null;
                }

                // Удаляет закладку из списка закладок.
                Warehouse.Warehouse.Instance.Bookmarks.Remove(b);

                // Удаляет закладку из Html-кода.
                var tm = Warehouse.Warehouse.GetTrainingModuleById(b.ModuleId);
                if (tm.TrainingModuleDocument == null)
                {
                    #region Документ не создан

                    var html = tm.DocumentHtml;
                    var mlp = new MlParser();
                    string searchString;
                    string value;
                    int index;

                    searchString = "class=bookmark";
                    index = 0;
                    while (html.Contains(searchString))
                    {
                        mlp.GetTagBounds(html, searchString, index);
                        value = mlp.GetValue("id");
                        mlp.ShiftLastIndex(ref html);

                        if (value.Equals(b.Id.ToString()))
                        {
                            html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                            index = mlp.StartIndex;
                        }
                        else
                        {
                            index = mlp.LastIndex;
                        }

                        if (index == mlp.StartIndex)
                        {
                            break;
                        }
                    }

                    tm.DocumentHtml = html;

                    #endregion
                }
                else
                {
                    #region Документ создан

                    var html = tm.TrainingModuleDocument.HtmlEditingTool.BodyInnerHtml;
                    var mlp = new MlParser();
                    string searchString;
                    string value;
                    int index;

                    searchString = "class=bookmark";
                    index = 0;
                    while (html.Contains(searchString))
                    {
                        mlp.GetTagBounds(html, searchString, index);
                        value = mlp.GetValue("id");
                        mlp.ShiftLastIndex(ref html);

                        if (value.Equals(b.Id.ToString()))
                        {
                            html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                            index = mlp.StartIndex;
                        }
                        else
                        {
                            index = mlp.LastIndex;
                        }

                        if (index == mlp.StartIndex)
                        {
                            break;
                        }
                    }

                    tm.TrainingModuleDocument.HtmlEditingTool.BodyInnerHtml = html;   

                    #endregion
                }

                // Удаляет ссылки на закладку.
                foreach (var lto in Warehouse.Warehouse.Instance.LinksToObjects)
                {
                    if (lto.ObjectId.Equals(b.Id))
                    {
                        //MessageBox.Show(lto.TrainingModule.Text);
                        if (lto.TrainingModule.TrainingModuleDocument == null)
                        {
                            #region Документ не создан

                            var html_ = string.Copy(lto.TrainingModule.DocumentHtml);
                            var mlp_ = new MlParser();
                            string searchString_;
                            string innerHtml;

                            searchString_ = lto.ObjectId.ToString();
                            while (html_.Contains(searchString_))
                            {
                                mlp_.GetTagBounds(html_, searchString_);
                                mlp_.ShiftLastIndex(ref html_);

                                innerHtml = mlp_.GetInnerHtml();
                                html_ = html_.Remove(mlp_.StartIndex, mlp_.LastIndex - mlp_.StartIndex + 1);
                                html_ = html_.Insert(mlp_.StartIndex, innerHtml);
                            }

                            lto.TrainingModule.DocumentHtml = html_;

                            #endregion
                        }
                        else
                        {
                            #region Документ создан

                            var html_ = string.Copy(lto.TrainingModule.TrainingModuleDocument.HtmlEditingTool.BodyInnerHtml);
                            var mlp_ = new MlParser();
                            string searchString_;
                            string innerHtml;

                            searchString_ = lto.ObjectId.ToString();
                            while (html_.Contains(searchString_))
                            {
                                mlp_.GetTagBounds(html_, searchString_);
                                mlp_.ShiftLastIndex(ref html_);

                                innerHtml = mlp_.GetInnerHtml();
                                html_ = html_.Remove(mlp_.StartIndex, mlp_.LastIndex - mlp_.StartIndex + 1);
                                html_ = html_.Insert(mlp_.StartIndex, innerHtml);
                            }

                            lto.TrainingModule.TrainingModuleDocument.HtmlEditingTool.BodyInnerHtml = html_;

                            #endregion
                        }
                    }
                }

                CheckState();
            }
        }

        #endregion

        #region NavigateToBookmark

        private void NavigateToBookmark()
        {
            // Снимает выделение компетенции, если оно было.
            if (EditorObserver.ActiveEditor != null)
            {
                if (EditorObserver.ActiveEditor.HighlightedElement != null)
                {
                    EditorObserver.ActiveEditor.Highlight(EditorObserver.ActiveEditor.HighlightedElement, false);
                    EditorObserver.ActiveEditor.HighlightedElement = null;
                }
            }

            // Снимает выделение.
            if (EditorObserver.ActiveEditor != null)
            {
                EditorObserver.ActiveEditor.Unselect();
            }

            var bk = bookmarkTree.CurrentNode;
            var b = ShowTrainingModuleDocument(bk.ModuleId);

            if (b)
            {
                Navigate(bk.Id);
            }
            else
            {
                CreateTrainingModuleDocument(bk.ModuleId);
                Navigate(bk.Id);
            }
        }

        private bool ShowTrainingModuleDocument(Guid id)
        {
            var dc = DockContainer.Instance;

            foreach (var d in dc.TrainingModuleDocuments)
            {
                if (d.TrainingModule.Id.Equals(id))
                {
                    d.Show();

                    // Необходимо, чтобы в DockContainer произошла смена активного документа и редактора.
                    bookmarkNameTextBox.Select();

                    return true;
                }
            }

            return false;
        }

        private static void Navigate(Guid id)
        {
            var ans = EditorObserver.ActiveEditor.GetElementsByTagName(TagNames.AnchorTagName);
            foreach (HtmlElement he in ans)
            {
                if (he.Id != null)
                {
                    if (he.Id.Equals(id.ToString()))
                    {
                        EditorObserver.ActiveEditor.ScrollToHtmlElement(he);
                        EditorObserver.ActiveEditor.HighlightedElement = he;
                        EditorObserver.ActiveEditor.Highlight(he, true);

                        break;
                    }
                }
            }
        }

        private static void CreateTrainingModuleDocument(Guid id)
        {
            var tm = Warehouse.Warehouse.GetTrainingModuleById(id);

            tm.TrainingModuleDocument = new TrainingModuleDocument
                                            {
                                                TrainingModule = tm,
                                                Text = tm.Text,
                                                HtmlEditingTool =
                                                    {Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design}
                                            };

            //tm.TrainingModuleDocument.VisualHtmlEditor.SetDefaultFont();
            Logic.Controls.HtmlEditing.HtmlEditingToolHelper.SetDefaultDocumentHtml(tm.TrainingModuleDocument.HtmlEditingTool);
            tm.TrainingModuleDocument.HtmlEditingTool.BodyInnerHtml = tm.DocumentHtml;
            // Блокирует команду Undo.
            tm.TrainingModuleDocument.HtmlEditingTool.Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview;
            tm.TrainingModuleDocument.HtmlEditingTool.Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview;
            tm.TrainingModuleDocument.Show();

            if (tm.Text.Length > 22)
            {
                tm.TrainingModuleDocument.Text = tm.Text.Substring(0, 22) + "...";
            }
            else
            {
                tm.TrainingModuleDocument.Text = tm.Text;
            }
            
            //bookmarkNameTextBox.Select();
        }

        #endregion
    }
}