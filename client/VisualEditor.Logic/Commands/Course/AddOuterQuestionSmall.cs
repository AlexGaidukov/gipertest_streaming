using System;
using System.Windows.Forms;
using VisualEditor.Logic.Controls.Docking.Documents;
using VisualEditor.Logic.Controls.HtmlEditing;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Course.Items.Questions;
using VisualEditor.Logic.Dialogs;
using VisualEditor.Logic.Helpers;
using VisualEditor.Logic.Warehouse;

namespace VisualEditor.Logic.Commands.Course
{
    internal class AddOuterQuestionSmall : AbstractCommand
    {
        private const string operationCantBePerformedMessage = "Невозможно выполнить операцию. Попробуйте повтротить снова.";

        public AddOuterQuestionSmall()
        {
            name = CommandNames.AddOuterQuestionSmall;
            text = CommandTexts.AddOuterQuestionSmall;
            image = Properties.Resources.Question;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            ServiceInfo serviceInfo;

            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Открыть";
                openFileDialog.Filter = "XML (*.xml)|*.xml";

                if (!openFileDialog.ShowDialog().Equals(DialogResult.OK))
                {
                    return;
                }

                try
                {
                    serviceInfo = new ServiceInfo(openFileDialog.FileName);
                }
                catch (Exception exception)
                {
                    UIHelper.ShowMessage(operationCantBePerformedMessage,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            using (var oqd = new OuterQuestionDialog())
            {
                oqd.InitializeData(serviceInfo);

                if (!oqd.ShowDialog(EditorObserver.DialogOwner).Equals(DialogResult.OK))
                {
                    return;
                }

                // Выбранные в диалоге вопросы.
                var outerQuestions = oqd.GetSelectedQuestions();

                for (var i = 0; i < outerQuestions.Count; i++)
                {
                    var q = PrepareOuterQuestion();

                    for (var j = 0; j < serviceInfo.Problems.Count; j++)
                    {
                        if (serviceInfo.Problems[j].Name.Equals(outerQuestions[i]))
                        {
                            q.Url = serviceInfo.Problems[j].Url;
                            q.TaskId = serviceInfo.Problems[j].Id;
                            q.TaskName = serviceInfo.Problems[j].Name;
                            q.Declaration = serviceInfo.Problems[j].Declaration;
                            q.Text = outerQuestions[i];
                            q.QuestionDocument.Text = q.Text;
                            for (var k = 0; k < serviceInfo.task_number; k++)
                            {
                                if (serviceInfo.Problems[j].Task_number == serviceInfo.Tasks[k].Task_number)
                                {
                                    q.TestId = serviceInfo.Tasks[k].Id;
                                    q.TestName = serviceInfo.Tasks[k].Name;
                                    for (var l = 0; l < serviceInfo.externaltest_number; l++)
                                    {
                                        if (serviceInfo.Tasks[k].Externaltest_number == serviceInfo.Externaltests[l].Externaltest_number)
                                        {
                                            q.SubjectName = serviceInfo.Externaltests[l].Name;
                                        }
                                    }
                                }
                            }
                            
                               
                        }
                    }

                    EditorObserver.ActiveEditor.BodyInnerHtml = q.Declaration;
                }

                var cn = Warehouse.Warehouse.Instance.CourseTree.CurrentNode;
                Warehouse.Warehouse.Instance.CourseTree.CurrentNode = cn.Nodes[cn.Nodes.Count - 1] as CourseItem;
                Warehouse.Warehouse.IsProjectModified = true;
            }
        }

        #region PrepareOuterQuestion

        private static OuterQuestion PrepareOuterQuestion()
        {
            var q = new OuterQuestion
                        {
                            Id = Guid.NewGuid()
                        };

            if (Warehouse.Warehouse.Instance.CourseTree.CurrentNode is Group)
            {
                var g = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as Group;
                q.TimeRestriction = g.TimeRestriction;
                q.Profile = g.Profile;
                q.Marks = g.Marks;
            }

            Warehouse.Warehouse.Instance.CourseTree.CurrentNode.Nodes.Add(q);

            if (!Warehouse.Warehouse.Instance.CourseTree.CurrentNode.IsExpanded)
            {
                Warehouse.Warehouse.Instance.CourseTree.CurrentNode.Toggle();
            }

            // Создает и отображает редактор.
            q.QuestionDocument = new QuestionDocument
                                     {
                                         Question = q,
                                         HtmlEditingTool =
                                             {Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design}
                                     };

            //q.QuestionDocument.VisualHtmlEditor.SetDefaultFont();
            HtmlEditingToolHelper.SetDefaultDocumentHtml(q.QuestionDocument.HtmlEditingTool);
            PreviewObserver.AddDocument(q.QuestionDocument);
            q.QuestionDocument.Show();

            return q;
        }

        #endregion
    }
}