using System.Windows.Forms;
using VisualEditor.Logic.Dialogs;
using VisualEditor.Logic.Warehouse;

namespace VisualEditor.Logic.Commands.Embedding
{
    internal class EquationSmall : AbstractCommand
    {
        public EquationSmall()
        {
            name = CommandNames.EquationSmall;
            text = CommandTexts.EquationSmall;
            image = Properties.Resources.EquationSmall;
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

            using (var ed = new EquationDialog())
            {
                if (ed.ShowDialog(EditorObserver.DialogOwner) == DialogResult.OK)
                {

                }
            }
        }
    }
}