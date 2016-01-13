namespace VisualEditor.Logic.Commands.Course
{
    class AddInteractiveQuestion: AbstractCommand
    {
        public AddInteractiveQuestion()
        {
            name = CommandNames.AddInteractiveQuestion;
            text = CommandTexts.AddInteractiveQuestion;
            image = Properties.Resources.Question;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            CommandManager.Instance.GetCommand(CommandNames.AddInteractiveQuestionSmall).Execute(null);
        }
    }
}
