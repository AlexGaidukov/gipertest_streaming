using System;
using VisualEditor.Logic.Controls.Docking.Documents;
using VisualEditor.Logic.Controls.HtmlEditing;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Course.Items.Questions;
using VisualEditor.Logic.Warehouse;

namespace VisualEditor.Logic.Commands.Course
{
    internal class AddChoiceQuestionSmall : AbstractCommand
    {
        public AddChoiceQuestionSmall()
        {
            name = CommandNames.AddChoiceQuestionSmall;
            text = CommandTexts.AddChoiceQuestionSmall;
            image = Properties.Resources.Question;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            var q = new ChoiceQuestion
                        {
                            Id = Guid.NewGuid()
                        };

            if (Warehouse.Warehouse.Instance.CourseTree.CurrentNode is TestModule)
            {
                var tm = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as TestModule;
                q.Text = string.Concat("Вопрос ", tm.Questions.Count + 1);
            }

            if (Warehouse.Warehouse.Instance.CourseTree.CurrentNode is Group)
            {
                var g = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as Group;
                q.Text = string.Concat("Вопрос ", g.Questions.Count + 1);

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
                                         Text = q.Text,
                                         HtmlEditingTool =
                                             {Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design}
                                     };

            //q.QuestionDocument.VisualHtmlEditor.SetDefaultFont();
            HtmlEditingToolHelper.SetDefaultDocumentHtml(q.QuestionDocument.HtmlEditingTool);
            PreviewObserver.AddDocument(q.QuestionDocument);
            q.QuestionDocument.Show();

            Warehouse.Warehouse.Instance.CourseTree.CurrentNode = q;
            Warehouse.Warehouse.IsProjectModified = true;
        }
    }
}