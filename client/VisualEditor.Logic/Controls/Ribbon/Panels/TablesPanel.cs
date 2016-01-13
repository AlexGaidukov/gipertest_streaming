using VisualEditor.Logic.Commands.Embedding;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Panels
{
    internal class TablesPanel : RibbonPanel
    {
        private const string text = "Таблицы";

        public TablesPanel()
        {
            InitializePanel();
        }

        private void InitializePanel()
        {
            Text = text;
            ButtonMoreVisible = false;
            FlowsTo = RibbonPanelFlowDirection.Bottom;

            RibbonHelper.AddButton(this, new Table());
        }
    }
}