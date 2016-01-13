using VisualEditor.Logic.Dialogs;
using VisualEditor.Logic.Warehouse;

namespace VisualEditor.Logic.Commands.Embedding
{
    internal class SymbolSmall : AbstractCommand
    {
        public SymbolSmall()
        {
            name = CommandNames.SymbolSmall;
            text = CommandTexts.SymbolSmall;
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