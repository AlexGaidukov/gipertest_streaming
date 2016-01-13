namespace VisualEditor.Logic.Commands.Embedding
{
    internal class Audio : AbstractCommand
    {
        public Audio()
        {
            name = CommandNames.Audio;
            text = CommandTexts.Audio;
            image = Logic.Properties.Resources.Audio;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            CommandManager.Instance.GetCommand(CommandNames.AudioSmall).Execute(null);
        }
    }
}