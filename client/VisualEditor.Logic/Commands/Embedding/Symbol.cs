namespace VisualEditor.Logic.Commands.Embedding
{
    internal class Symbol : AbstractCommand
    {
        public Symbol()
        {
            name = CommandNames.Symbol;
            text = CommandTexts.Symbol;
            image = Properties.Resources.Symbol;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            CommandManager.Instance.GetCommand(CommandNames.SymbolSmall).Execute(null);
        }
    }
}