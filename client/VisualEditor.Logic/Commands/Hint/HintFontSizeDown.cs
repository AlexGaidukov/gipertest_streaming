namespace VisualEditor.Logic.Commands.Hint
{
    internal class HintFontSizeDown : AbstractCommand
    {
        public HintFontSizeDown()
        {
            name = CommandNames.HintFontSizeDown;
            text = CommandTexts.HintFontSizeDown;
            image = Properties.Resources.FontSizeDown;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            // POSTPONE: Реализовать логику уменьшения шрифта.
        }
    }
}