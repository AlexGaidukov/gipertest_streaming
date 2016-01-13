using VisualEditor.Logic.Controls.Docking;

namespace VisualEditor.Logic.Commands.Document
{
    internal class CloseAll : AbstractCommand
    {
        public CloseAll()
        {
            name = CommandNames.CloseAll;
            text = CommandTexts.CloseAll;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            var dc = DockContainer.Instance;

            foreach (var d in dc.Documents)
            {
                if (!d.Equals(dc.ActiveDocument))
                {
                    d.Hide();
                }
            }

            if (dc.ActiveDocument != null)
            {
                dc.ActiveDocument.DockHandler.Hide();
            }
        }
    }
}