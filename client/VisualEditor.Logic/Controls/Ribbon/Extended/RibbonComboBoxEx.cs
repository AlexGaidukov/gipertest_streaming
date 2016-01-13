using System;
using VisualEditor.Logic.Commands;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Extended
{
    internal class RibbonComboBoxEx : RibbonComboBox
    {
        private readonly AbstractCommand command;

        public RibbonComboBoxEx(AbstractCommand command)
        {
            this.command = command;
            Update();
            command.StateChanged += (s, e) => Update();
            TextBoxTextChanged += RibbonComboBoxEx_TextBoxTextChanged;
        }

        private void Update()
        {
            Enabled = command.Enabled;
        }

        private void RibbonComboBoxEx_TextBoxTextChanged(object sender, EventArgs e)
        {
            if (command != null)
            {
                command.Execute(null);
            }
        }
    }
}