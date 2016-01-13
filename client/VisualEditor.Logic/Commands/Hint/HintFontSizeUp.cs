namespace VisualEditor.Logic.Commands.Hint
{
    internal class HintFontSizeUp : AbstractCommand
    {
        public HintFontSizeUp()
        {
            name = CommandNames.HintFontSizeUp;
            text = CommandTexts.HintFontSizeUp;
            image = Properties.Resources.FontSizeUp;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            // POSTPONE: Реализовать логику увеличения шрифта.
        }
    }
}