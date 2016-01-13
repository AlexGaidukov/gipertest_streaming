namespace VisualEditor.Logic.Commands.Hint
{
    internal class HintTable : AbstractCommand
    {
        public HintTable()
        {
            name = CommandNames.HintTable;
            text = CommandTexts.HintTable;
            image = Properties.Resources.Table;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            CommandManager.Instance.GetCommand(CommandNames.HintTableSmall).Execute(null);
        }
    }
}