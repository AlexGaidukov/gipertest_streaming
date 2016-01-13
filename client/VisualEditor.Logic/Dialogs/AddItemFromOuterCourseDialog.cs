using System;
using System.Windows.Forms;
using VisualEditor.Logic.Commands;
using VisualEditor.Logic.Commands.IO;
using VisualEditor.Logic.Controls.Trees;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Course.Preview;
using VisualEditor.Logic.Helpers;
using VisualEditor.Utils.Controls.HtmlEditing;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Dialogs
{
    internal partial class AddItemFromOuterCourseDialog : DialogBase
    {
        private const string operationCantBePerformedMessage = "Невозможно выполнить операцию. Попробуйте повтротить снова.";

        private Type _type;
        private HtmlEditingTool htmlEditingTool;

        public AddItemFromOuterCourseDialog()
        {
            InitializeComponent();
            InitializeDialog();
        }

        public static OuterCourseTree OuterCourseTree { get; private set; }

        private void InitializeDialog()
        {
            var oct = new OuterCourseTree
                          {
                              Dock = DockStyle.Fill, 
                              BorderStyle = BorderStyle.None
                          };
            splitContainer.Panel1.Controls.Add(oct);
            OuterCourseTree = oct;
            oct.AfterSelect += CourseTree_AfterSelect;
            oct.MouseDown += CourseTree_MouseDown;

            htmlEditingTool = new HtmlEditingTool
                                  {
                                      Dock = DockStyle.Fill
                                  };
            splitContainer.Panel2.Controls.Add(htmlEditingTool);
        }

        public void InitializeData(string dialogText, string buttonText, Type type)
        {
            Text = dialogText;
            addButton.Text = buttonText;
            _type = type;
        }

        private void AddItemFromOuterCourseDialog_Shown(object sender, EventArgs e)
        {
            htmlEditingTool.Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview;
            Logic.Controls.HtmlEditing.HtmlEditingToolHelper.SetDefaultDocumentHtml(htmlEditingTool);
            htmlEditingTool.BodyInnerHtml = "Загрузка учебного курса...";
            OuterCourseTree.Enabled = false;

            try
            {
                Warehouse.Warehouse.CreateOuterDirectories();
            }
            catch (Exception)
            {
                UIHelper.ShowMessage(operationCantBePerformedMessage,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                CommandManager.Instance.GetCommand(CommandNames.OuterLoadFromHtp).Execute(null);
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
                UIHelper.ShowMessage(operationCantBePerformedMessage,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            OuterCourseTree.Nodes[0].Nodes[0].Remove();
            if (_type.Equals(typeof(Question)))
            {
                var testType = Enums.TestType.OutTest;
                var cn = Warehouse.Warehouse.Instance.CourseTree.CurrentNode;

                if (cn is TestModule)
                {
                    testType = (cn as TestModule).TestType;
                }

                if (cn is Group)
                {
                    testType = (cn.Parent as TestModule).TestType;
                }

                if (testType.Equals(Enums.TestType.InTest))
                {

                }
            }

            OuterCourseTree.Enabled = true;
            htmlEditingTool.BodyInnerHtml = string.Empty;
        }

        private void AddItemFromOuterCourseDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (OuterLoadFromHtp.IsBusy)
            {
                e.Cancel = true;
                return;
            }

            try
            {
                Warehouse.Warehouse.DeleteOuterDirectories();
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
            }
        }

        #region CheckState
        
        private void CourseTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            CheckState(OuterCourseTree.SelectedNode);
        }

        private void CourseTree_MouseDown(object sender, MouseEventArgs e)
        {
            CheckState(OuterCourseTree.SelectedNode);
        }

        private void CheckState(TreeNode tn)
        {
            if (_type.Equals(typeof(TestModule)))
            {
                addButton.Enabled = tn is TestModule;
            }

            if (_type.Equals(typeof(Question)))
            {
                addButton.Enabled = tn is Question;
            }

            if (tn is Question)
            {
                var q = tn as Question;
                Logic.Controls.HtmlEditing.HtmlEditingToolHelper.SetDefaultDocumentHtml(htmlEditingTool);
                PreviewConverter.ConvertQuestionDocument(htmlEditingTool, q);
            }
            else
            {
                Logic.Controls.HtmlEditing.HtmlEditingToolHelper.SetDefaultDocumentHtml(htmlEditingTool);
                //visualHtmlEditor.BodyInnerHtml = "Предварительный просмотр вопроса";
            }
        }

        #endregion

        private void addButton_Click(object sender, EventArgs e)
        {
            var cn = Warehouse.Warehouse.Instance.CourseTree.CurrentNode;

            if (_type.Equals(typeof(TestModule)))
            {
                var tm = OuterCourseTree.SelectedNode as TestModule;
                tm = TestModule.Clone(tm);
                cn.Nodes.Add(tm);

                if (!Warehouse.Warehouse.Instance.CourseTree.CurrentNode.IsExpanded)
                {
                    Warehouse.Warehouse.Instance.CourseTree.CurrentNode.Toggle();
                }
            }

            if (_type.Equals(typeof(Question)))
            {
                var q = OuterCourseTree.SelectedNode as Question;
                q = Question.Clone(q);
                cn.Nodes.Add(q);

                if (!Warehouse.Warehouse.Instance.CourseTree.CurrentNode.IsExpanded)
                {
                    Warehouse.Warehouse.Instance.CourseTree.CurrentNode.Toggle();
                }
            }

            Warehouse.Warehouse.IsProjectModified = true;
        }
    }
}