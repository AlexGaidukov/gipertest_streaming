namespace VisualEditor.Logic.Commands.Course
{
    internal class AddChoiceQuestion : AbstractCommand
    {
        public AddChoiceQuestion()
        {
            name = CommandNames.AddChoiceQuestion;
            text = CommandTexts.AddChoiceQuestion;
            image = Properties.Resources.Question;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            CommandManager.Instance.GetCommand(CommandNames.AddChoiceQuestionSmall).Execute(null);
        }
    }
}