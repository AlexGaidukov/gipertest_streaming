namespace VisualEditor.Logic.Commands.Hint
{
    internal class HintPicture : AbstractCommand
    {
        public HintPicture()
        {
            name = CommandNames.HintPicture;
            text = CommandTexts.HintPicture;
            image = Properties.Resources.Picture;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            CommandManager.Instance.GetCommand(CommandNames.HintPictureSmall).Execute(null);
        }
    }
}