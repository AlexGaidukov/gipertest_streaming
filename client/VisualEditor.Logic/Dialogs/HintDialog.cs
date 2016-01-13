using System;
using System.Windows.Forms;
using VisualEditor.Logic.Controls.Docking;
using VisualEditor.Logic.Controls.Docking.Documents;
using VisualEditor.Logic.Controls.Ribbon;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.Controls.HtmlEditing;

namespace VisualEditor.Logic.Dialogs
{
    internal partial class HintDialog : DialogBase
    {
        private static HintDialog instance;
        private HintRibbon hintRibbon;
        private HtmlEditingTool htmlEditingTool;
        private string hint;

        private HintDialog()
        {
            InitializeComponent();
            InitializeDialog();
        }

        #region Properties
        
        public static HintDialog Instance
        {
            get { return instance ?? (instance = new HintDialog()); }
        }

        public string Hint
        {
            get { return htmlEditingTool.BodyInnerHtml; }
            set { hint = value; }
        }

        #endregion

        #region InitializeDialog

        private void InitializeDialog()
        {
            FormBorderStyle = FormBorderStyle.Sizable;
        }

        #endregion

        #region Открытие диалога
        
        private void HintDialog_Load(object sender, EventArgs e)
        {
            InitializeRibbon();
            InitializeEditor();

            EditorObserver.ActiveEditor = htmlEditingTool;
        }

        #endregion

        #region Закрытие диалога
        
        private void HintDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            DocumentBase ad = null;

            if (DockContainer.Instance.ActiveDocument is DocumentBase)
            {
                ad = DockContainer.Instance.ActiveDocument as DocumentBase;
            }

            if (ad == null)
            {
                EditorObserver.ActiveEditor = null;

                return;
            }

            EditorObserver.ActiveEditor = ad.HtmlEditingTool;
        }

        #endregion

        private void okButton_Click(object sender, EventArgs e)
        {
            Warehouse.Warehouse.IsProjectModified = true;
            DialogResult = DialogResult.OK;
        }

        #region InitializeRibbon
        
        private void InitializeRibbon()
        {
            hintRibbon = new HintRibbon();
            splitContainer.Panel1.Controls.Clear();
            splitContainer.Panel1.Controls.Add(hintRibbon);
            splitContainer.SplitterDistance = hintRibbon.Height;
        }

        #endregion

        #region InitializeEditor
        
        private void InitializeEditor()
        {
            htmlEditingTool = new HtmlEditingTool
                                  {
                                      Dock = DockStyle.Fill,
                                      Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design
                                  };

            Logic.Controls.HtmlEditing.HtmlEditingToolHelper.SetDefaultDocumentHtml(htmlEditingTool);
            splitContainer.Panel2.Controls.Clear();
            splitContainer.Panel2.Controls.Add(htmlEditingTool);

            if (Owner is QuestionDialog ||
                Owner is QuestionInGroupDialog ||
                Owner is NetQuestionDialog)
            {
                var q = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as Question;
                htmlEditingTool.BodyInnerHtml = q.Hint;
            }
            else if (Owner is ResponseVariantDialog ||
                Owner is NetResponseVariantDialog)
            {
                htmlEditingTool.BodyInnerHtml = hint;
            }

            htmlEditingTool.Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview;
            htmlEditingTool.Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design;
        }

        #endregion
    }
}