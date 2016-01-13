using System.Windows.Forms;
using VisualEditor.Logic.Commands;

namespace VisualEditor.Logic.Controls.Ribbon.Extended
{
    internal class RibbonContextMenuItemEx : ToolStripMenuItem
    {
        private readonly AbstractCommand command;

        public RibbonContextMenuItemEx(AbstractCommand command)
        {
            this.command = command;
            Text = command.Text;

            if (command.Image != null)
            {
                Image = command.Image;
            }

            Update();
            command.StateChanged += (s, e) => Update();
        }

        private void Update()
        {
            Enabled = command.Enabled;
            Checked = command.Checked;
        }

        protected override void OnClick(System.EventArgs e)
        {
            base.OnClick(e);

            if (command != null)
            {
                command.Execute(null);
            }
        }
    }
}