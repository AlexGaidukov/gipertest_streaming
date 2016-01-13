using VisualEditor.Logic.Commands.HtmlEditing;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Panels
{
    internal class AdditionallyPanel : RibbonPanel
    {
        private const string text = "Дополнительно";

        public AdditionallyPanel()
        {
            InitializePanel();
        }

        private void InitializePanel()
        {
            Text = text;
            ButtonMoreVisible = false;
            FlowsTo = RibbonPanelFlowDirection.Bottom;

            var g = new RibbonItemGroup();
            RibbonHelper.AddButton(g, new HorizontalRule());
            RibbonHelper.AddButton(g, new Break());
            RibbonHelper.AddGroup(this, g);
        }
    }
}