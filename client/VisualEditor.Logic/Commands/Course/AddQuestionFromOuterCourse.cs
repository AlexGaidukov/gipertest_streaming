namespace VisualEditor.Logic.Commands.Course
{
    internal class AddQuestionFromOuterCourse : AbstractCommand
    {
        public AddQuestionFromOuterCourse()
        {
            name = CommandNames.AddQuestionFromOuterCourse;
            text = CommandTexts.AddQuestionFromOuterCourse;
            image = Properties.Resources.Question;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            CommandManager.Instance.GetCommand(CommandNames.AddQuestionFromOuterCourseSmall).Execute(null);
        }
    }
}