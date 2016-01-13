namespace VisualEditor.Logic.Commands.Course
{
    internal class AddOpenQuestion : AbstractCommand
    {
        public AddOpenQuestion()
        {
            name = CommandNames.AddOpenQuestion;
            text = CommandTexts.AddOpenQuestion;
            image = Properties.Resources.Question;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            CommandManager.Instance.GetCommand(CommandNames.AddOpenQuestionSmall).Execute(null);
        }
    }
}