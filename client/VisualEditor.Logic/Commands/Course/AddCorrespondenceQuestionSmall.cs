using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Course.Items.Questions;
using VisualEditor.Logic.Warehouse;

namespace VisualEditor.Logic.Commands.Course
{
    internal class AddCorrespondenceQuestionSmall : AbstractCommand
    {
        public AddCorrespondenceQuestionSmall()
        {
            name = CommandNames.AddCorrespondenceQuestionSmall;
            text = CommandTexts.AddCorrespondenceQuestionSmall;
            image = Properties.Resources.Question;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            var q = new CorrespondenceQuestion();

            if (Warehouse.Warehouse.Instance.CourseTree.CurrentNode is TestModule)
            {
                var tm = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as TestModule;
                q.Text = string.Concat("Вопрос ", tm.Questions.Count + 1);
            }

            if (Warehouse.Warehouse.Instance.CourseTree.CurrentNode is Group)
            {
                var g = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as Group;
                q.Text = string.Concat("Вопрос ", g.Questions.Count + 1);
            }

            Warehouse.Warehouse.Instance.CourseTree.CurrentNode.Nodes.Add(q);

            if (!Warehouse.Warehouse.Instance.CourseTree.CurrentNode.IsExpanded)
            {
                Warehouse.Warehouse.Instance.CourseTree.CurrentNode.Toggle();
            }

            Warehouse.Warehouse.Instance.CourseTree.CurrentNode = q;
            PreviewObserver.AddDocument(q.QuestionDocument);
            Warehouse.Warehouse.IsProjectModified = true;
        }
    }
}