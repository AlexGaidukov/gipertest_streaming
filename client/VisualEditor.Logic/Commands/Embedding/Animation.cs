namespace VisualEditor.Logic.Commands.Embedding
{
    internal class Animation : AbstractCommand
    {
        public Animation()
        {
            name = CommandNames.Animation;
            text = CommandTexts.Animation;
            image = Properties.Resources.Animation;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            CommandManager.Instance.GetCommand(CommandNames.AnimationSmall).Execute(null);
        }
    }
}