namespace VisualEditor.Logic.Commands.Project
{
    internal class SaveProject : AbstractCommand
    {
        public SaveProject()
        {
            name = CommandNames.SaveProject;
            text = CommandTexts.SaveProject;
            image = Properties.Resources.SaveProject;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            CommandManager.Instance.GetCommand(CommandNames.SaveProjectSmall).Execute(null);
        }
    }
}