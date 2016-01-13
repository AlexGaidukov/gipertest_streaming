namespace VisualEditor.Logic.Commands.HtmlEditing
{
    internal class Replace : AbstractCommand
    {
        public Replace()
        {
            name = CommandNames.Replace;
            text = CommandTexts.Replace;
            image = Properties.Resources.Replace;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            // POSTPONE: Реализовать логику замены текста.
            // Warehouse.Warehouse.IsProjectModified = true;
        }
    }
}