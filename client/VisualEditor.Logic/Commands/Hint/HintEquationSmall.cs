using System.Windows.Forms;
using VisualEditor.Logic.Dialogs;
using VisualEditor.Logic.Warehouse;

namespace VisualEditor.Logic.Commands.Hint
{
    internal class HintEquationSmall : AbstractCommand
    {
        public HintEquationSmall()
        {
            name = CommandNames.HintEquationSmall;
            text = CommandTexts.HintEquationSmall;
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