namespace VisualEditor.Logic.Commands.Concept
{
    internal class ProfileOptions : AbstractCommand
    {
        public ProfileOptions()
        {
            name = CommandNames.ProfileOptions;
            text = CommandTexts.ProfileOptions;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            CommandManager.Instance.GetCommand(CommandNames.ProfileOptionsSmall).Execute(null);
        }
    }
}