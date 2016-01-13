using VisualEditor.Logic.Course.Items;

namespace VisualEditor.Logic.Commands.Course
{
    internal class AddGroup : AbstractCommand
    {
        public AddGroup()
        {
            name = CommandNames.AddGroup;
            text = CommandTexts.AddGroup;
            image = Properties.Resources.Group;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            var g = new Group();

            if (Warehouse.Warehouse.Instance.CourseTree.CurrentNode is TestModule)
            {
                var tm = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as TestModule;
                g.Text = string.Concat("Группа ", tm.Groups.Count + 1);

                #region Если в контроле есть группы, то последовательность вопросов случайная

                tm.QuestionSequence = Enums.QuestionSequence.Random;

                #endregion
            }

            Warehouse.Warehouse.Instance.CourseTree.CurrentNode.Nodes.Add(g);

            if (!Warehouse.Warehouse.Instance.CourseTree.CurrentNode.IsExpanded)
            {
                Warehouse.Warehouse.Instance.CourseTree.CurrentNode.Toggle();
            }

            Warehouse.Warehouse.Instance.CourseTree.CurrentNode = g;
            Warehouse.Warehouse.IsProjectModified = true;
        }
    }
}