using VisualEditor.Logic.Dialogs;
using VisualEditor.Logic.Warehouse;

namespace VisualEditor.Logic.Commands.HtmlEditing
{
    internal class Find : AbstractCommand
    {
        public Find()
        {
            name = CommandNames.Find;
            text = CommandTexts.Find;
            image = Properties.Resources.Find;
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

            using (var fd = new FindDialog())
            {
                fd.ShowDialog(EditorObserver.DialogOwner);
            }
        }
    }
}