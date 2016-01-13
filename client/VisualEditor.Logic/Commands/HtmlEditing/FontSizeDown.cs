namespace VisualEditor.Logic.Commands.HtmlEditing
{
    internal class FontSizeDown : AbstractCommand
    {
        public FontSizeDown()
        {
            name = CommandNames.FontSizeDown;
            text = CommandTexts.FontSizeDown;
            image = Properties.Resources.FontSizeDown;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            // POSTPONE: Реализовать логику уменьшения шрифта.
            // Warehouse.Warehouse.IsProjectModified = true;
        }
    }
}