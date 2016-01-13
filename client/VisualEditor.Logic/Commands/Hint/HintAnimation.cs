namespace VisualEditor.Logic.Commands.Hint
{
    internal class HintAnimation : AbstractCommand
    {
        public HintAnimation()
        {
            name = CommandNames.HintAnimation;
            text = CommandTexts.HintAnimation;
            image = Properties.Resources.Animation;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            CommandManager.Instance.GetCommand(CommandNames.HintAnimationSmall).Execute(null);
        }
    }
}