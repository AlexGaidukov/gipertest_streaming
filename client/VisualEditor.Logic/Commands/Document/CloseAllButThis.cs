using VisualEditor.Logic.Controls.Docking;

namespace VisualEditor.Logic.Commands.Document
{
    internal class CloseAllButThis : AbstractCommand
    {
        public CloseAllButThis()
        {
            name = CommandNames.CloseAllButThis;
            text = CommandTexts.CloseAllButThis;
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
        }
    }
}