using System;
using VisualEditor.Logic.Commands;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Extended
{
    internal class RibbonOrbMenuItemEx : RibbonOrbMenuItem
    {
        private readonly AbstractCommand command;

        public RibbonOrbMenuItemEx(AbstractCommand command)
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
            Enabled = command.Enabled;
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