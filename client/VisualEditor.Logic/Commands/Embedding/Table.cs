namespace VisualEditor.Logic.Commands.Embedding
{
    internal class Table : AbstractCommand
    {
        public Table()
        {
            name = CommandNames.Table;
            text = CommandTexts.Table;
            image = Properties.Resources.Table;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            CommandManager.Instance.GetCommand(CommandNames.TableSmall).Execute(null);
        }
    }
}