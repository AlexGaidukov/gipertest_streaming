using VisualEditor.Logic.Commands.Embedding;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Panels
{
    internal class LinksPanel : RibbonPanel
    {
        private const string text = "Связи";

        public LinksPanel()
        {
            InitializePanel();
        }

        private void InitializePanel()
        {
            Text = text;
            ButtonMoreVisible = false;
            FlowsTo = RibbonPanelFlowDirection.Bottom;

            RibbonHelper.AddButton(this, new Bookmark());
            RibbonHelper.AddButton(this, new Concept());
            RibbonHelper.AddButton(this, new Link());
            RibbonHelper.AddSeparator(this);
            RibbonHelper.AddButton(this, new DeleteLink());
        }
    }
}