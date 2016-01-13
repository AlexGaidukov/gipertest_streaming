using VisualEditor.Logic.Commands.HtmlEditing;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Panels
{
    internal class ParagraphPanel : RibbonPanel
    {
        private const string text = "Абзац";

        public ParagraphPanel()
        {
            InitializePanel();
        }

        private void InitializePanel()
        {
            Text = text;
            ButtonMoreVisible = false;
            FlowsTo = RibbonPanelFlowDirection.Right;

            var g = new RibbonItemGroup();
            RibbonHelper.AddButton(g, new JustifyLeft());
            RibbonHelper.AddButton(g, new JustifyCenter());
            RibbonHelper.AddButton(g, new JustifyRight());
            RibbonHelper.AddButton(g, new JustifyFull());
            RibbonHelper.AddGroup(this, g);

            g = new RibbonItemGroup();
            RibbonHelper.AddButton(g, new Outdent());
            RibbonHelper.AddButton(g, new Indent());
            RibbonHelper.AddGroup(this, g);

            g = new RibbonItemGroup();
            RibbonHelper.AddButton(g, new UnorderedList());
            RibbonHelper.AddButton(g, new OrderedList());
            RibbonHelper.AddGroup(this, g);
        }
    }
}