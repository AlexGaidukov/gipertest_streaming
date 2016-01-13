using System;
using VisualEditor.Logic.Commands;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Extended
{
    internal class RibbonOrbRecentButtonEx : RibbonOrbRecentItem
    {
        private readonly AbstractCommand command;

        public RibbonOrbRecentButtonEx(AbstractCommand command)
        {
            this.command = command;
            Update();
            command.StateChanged += (s, e) => Update();
        }

        public string ProjectPath { get; set; }

        private void Update()
        {
            Enabled = command.Enabled;
        }

        public override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            if (command != null)
            {
                command.Text = ProjectPath;
                command.Execute(null);
            }
        }
    }
}