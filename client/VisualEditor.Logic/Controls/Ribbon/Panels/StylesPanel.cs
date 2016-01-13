using VisualEditor.Logic.Commands.Embedding;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Panels
{
    internal class StylesPanel : RibbonPanel
    {
        private const string text = "Стили";

        public StylesPanel()
        {
            InitializePanel();
        }

        private void InitializePanel()
        {
            Text = text;
            ButtonMoreVisible = false;
            FlowsTo = RibbonPanelFlowDirection.Bottom;

            RibbonHelper.AddButton(this, new Style());
        }
    }
}