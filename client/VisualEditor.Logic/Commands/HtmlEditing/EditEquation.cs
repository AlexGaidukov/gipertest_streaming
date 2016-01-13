using System.Windows.Forms;
using VisualEditor.Logic.Dialogs;
using VisualEditor.Logic.Warehouse;

namespace VisualEditor.Logic.Commands.HtmlEditing
{
    internal class EditEquation : AbstractCommand
    {
        public EditEquation()
        {
            name = CommandNames.EditEquation;
            text = CommandTexts.EditEquation;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            //if (EditorObserver.ActiveEditor.IsSelection)
            //{
            //    return;
            //}

            using (var ed = new EquationDialog())
            {
                ed.InitializeData();

                if (ed.ShowDialog(EditorObserver.DialogOwner) == DialogResult.OK)
                {

                }
            }
        }
    }
}