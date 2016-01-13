namespace VisualEditor.Logic.Commands.Course
{
    internal class AddCorrespondenceQuestion : AbstractCommand
    {
        public AddCorrespondenceQuestion()
        {
            name = CommandNames.AddCorrespondenceQuestion;
            text = CommandTexts.AddCorrespondenceQuestion;
            image = Properties.Resources.Question;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            CommandManager.Instance.GetCommand(CommandNames.AddCorrespondenceQuestionSmall).Execute(null);
        }
    }
}