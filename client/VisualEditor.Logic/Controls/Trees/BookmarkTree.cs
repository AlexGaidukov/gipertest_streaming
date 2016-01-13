using System;
using System.Windows.Forms;
using VisualEditor.Logic.Course.Items;

namespace VisualEditor.Logic.Controls.Trees
{
    internal class BookmarkTree : TreeView
    {
        private Bookmark currentNode;

        public BookmarkTree()
        {
            InitializeTree();
        }

        public event EventHandler CurrentNodeChanged;

        private void InitializeTree()
        {
            FullRowSelect = true;
            HideSelection = false;
            ShowLines = false;
            ShowRootLines = false;

            AfterSelect += BookmarksTree_AfterSelect;
            MouseDown += BookmarksTree_MouseDown;
        }

        public Bookmark CurrentNode
        {
            get { return currentNode; }
            set
            {
                currentNode = value;
                SelectedNode = currentNode;

                if (CurrentNodeChanged != null)
                {
                    CurrentNodeChanged(this, EventArgs.Empty);
                }
            }
        }

        private void BookmarksTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            CurrentNode = e.Node as Bookmark;
        }

        private void BookmarksTree_MouseDown(object sender, MouseEventArgs e)
        {
            CurrentNode = GetNodeAt(e.X, e.Y) as Bookmark;
        }
    }
}