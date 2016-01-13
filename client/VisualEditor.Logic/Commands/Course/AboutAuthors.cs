namespace VisualEditor.Logic.Commands.Course
{
    internal class AboutAuthors : AbstractCommand
    {
        public AboutAuthors()
        {
            name = CommandNames.AboutAuthors;
            text = CommandTexts.AboutAuthors;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }
        }
    }
}