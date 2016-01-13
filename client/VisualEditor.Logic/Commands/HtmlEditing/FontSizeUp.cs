namespace VisualEditor.Logic.Commands.HtmlEditing
{
    internal class FontSizeUp : AbstractCommand
    {
        public FontSizeUp()
        {
            name = CommandNames.FontSizeUp;
            text = CommandTexts.FontSizeUp;
            image = Properties.Resources.FontSizeUp;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            // POSTPONE: Реализовать логику увеличения шрифта.
            // Warehouse.Warehouse.IsProjectModified = true;
        }
    }
}