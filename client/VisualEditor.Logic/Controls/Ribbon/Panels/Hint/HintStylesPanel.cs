using VisualEditor.Logic.Commands;
using VisualEditor.Logic.Commands.Hint;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Panels.Hint
{
    internal class HintStylesPanel : RibbonPanel
    {
        private const string text = "Стили";

        public HintStylesPanel()
        {
            InitializePanel();
        }

        private void InitializePanel()
        {
            Text = text;
            ButtonMoreVisible = false;
            FlowsTo = RibbonPanelFlowDirection.Bottom;

            RibbonHelper.AddButton(this, new HintStyle());
            CommandManager.Instance.GetCommand(CommandNames.HintStyle).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.HintStyleSmall).Enabled = true;
        }
    }
}