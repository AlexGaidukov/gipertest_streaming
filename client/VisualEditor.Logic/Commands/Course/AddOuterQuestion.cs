namespace VisualEditor.Logic.Commands.Course
{
    internal class AddOuterQuestion : AbstractCommand
    {
        public AddOuterQuestion()
        {
            name = CommandNames.AddOuterQuestion;
            text = CommandTexts.AddOuterQuestion;
            image = Properties.Resources.Question;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            CommandManager.Instance.GetCommand(CommandNames.AddOuterQuestionSmall).Execute(null);
        }
    }
}