using VisualEditor.Logic.Commands.Embedding;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Panels
{
    internal class SymbolsPanel : RibbonPanel
    {
        private const string text = "Символы";

        public SymbolsPanel()
        {
            InitializePanel();
        }

        private void InitializePanel()
        {
            Text = text;
            ButtonMoreVisible = false;
            FlowsTo = RibbonPanelFlowDirection.Bottom;

            RibbonHelper.AddButton(this, new Equation());
            RibbonHelper.AddButton(this, new Symbol());
        }
    }
}