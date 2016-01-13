using VisualEditor.Logic.Commands;
using VisualEditor.Logic.Commands.Hint;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Panels.Hint
{
    internal class HintSymbolsPanel : RibbonPanel
    {
        private const string text = "Символы";

        public HintSymbolsPanel()
        {
            InitializePanel();
        }

        private void InitializePanel()
        {
            Text = text;
            ButtonMoreVisible = false;
            FlowsTo = RibbonPanelFlowDirection.Bottom;

            RibbonHelper.AddButton(this, new HintEquation());
            RibbonHelper.AddButton(this, new HintSymbol());
            CommandManager.Instance.GetCommand(CommandNames.HintEquation).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.HintEquationSmall).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.HintSymbol).Enabled = true;
            CommandManager.Instance.GetCommand(CommandNames.HintSymbolSmall).Enabled = true;
        }
    }
}