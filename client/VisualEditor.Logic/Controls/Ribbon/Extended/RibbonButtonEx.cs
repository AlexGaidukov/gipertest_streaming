using System;
using VisualEditor.Logic.Commands;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Extended
{
    internal class RibbonButtonEx : RibbonButton
    {
        private readonly AbstractCommand command;

        public RibbonButtonEx(AbstractCommand command)
        {
            this.command = command;
            Text = command.Text;

            if (command.Image != null)
            {
                Image = SmallImage = command.Image;
            }

            Update();
            command.StateChanged += (s, e) => Update();
        }

        private void Update()
        {
            if (!Enabled.Equals(command.Enabled))
            {
                Enabled = command.Enabled;

                if (Owner != null)
                {
                    _selected = false;
                }
            }

            if (!Checked.Equals(command.Checked))
            {
                Checked = command.Checked;
            }
        }

        public override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            if (command != null)
            {
                command.Execute(null);
            }
        }
    }
}