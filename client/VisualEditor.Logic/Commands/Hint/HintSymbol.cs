namespace VisualEditor.Logic.Commands.Hint
{
    internal class HintSymbol : AbstractCommand
    {
        public HintSymbol()
        {
            name = CommandNames.HintSymbol;
            text = CommandTexts.HintSymbol;
            image = Properties.Resources.Symbol;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            CommandManager.Instance.GetCommand(CommandNames.HintSymbolSmall).Execute(null);
        }
    }
}