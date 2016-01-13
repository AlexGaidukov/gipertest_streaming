namespace VisualEditor.Logic.Commands.Course
{
    internal class AddOrderingQuestion : AbstractCommand
    {
        public AddOrderingQuestion()
        {
            name = CommandNames.AddOrderingQuestion;
            text = CommandTexts.AddOrderingQuestion;
            image = Properties.Resources.Question;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            CommandManager.Instance.GetCommand(CommandNames.AddOrderingQuestionSmall).Execute(null);
        }
    }
}