using VisualEditor.Logic.Dialogs;
using VisualEditor.Logic.Warehouse;

namespace VisualEditor.Logic.Commands.Hint
{
    internal class HintSymbolSmall : AbstractCommand
    {
        public HintSymbolSmall()
        {
            name = CommandNames.HintSymbolSmall;
            text = CommandTexts.HintSymbolSmall;
            image = Properties.Resources.SymbolSmall;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            if (EditorObserver.ActiveEditor == null)
            {
                return;
            }

            if (EditorObserver.ActiveEditor.IsSelection)
            {
                return;
            }

            using (var sd = new SymbolDialog())
            {
                sd.ShowDialog(EditorObserver.DialogOwner);
            }
        }
    }
}