namespace VisualEditor.Logic.Commands.Course
{
    internal class AddMultichoiceQuestion : AbstractCommand
    {
        public AddMultichoiceQuestion()
        {
            name = CommandNames.AddMultichoiceQuestion;
            text = CommandTexts.AddMultichoiceQuestion;
            image = Properties.Resources.Question;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            CommandManager.Instance.GetCommand(CommandNames.AddMultichoiceQuestionSmall).Execute(null);
        }
    }
}