namespace VisualEditor.Logic.Commands.Hint
{
    internal class HintStyle : AbstractCommand
    {
        public HintStyle()
        {
            name = CommandNames.HintStyle;
            text = CommandTexts.HintStyle;
            image = Properties.Resources.Style;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            CommandManager.Instance.GetCommand(CommandNames.HintStyleSmall).Execute(null);
        }
    }
}