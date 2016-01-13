using VisualEditor.Logic.Commands.HtmlEditing;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Panels
{
    internal class BufferPanel : RibbonPanel
    {
        private const string text = "Буфер";

        public BufferPanel()
        {
            InitializePanel();
        }

        private void InitializePanel()
        {
            Text = text;
            ButtonMoreVisible = false;
            FlowsTo = RibbonPanelFlowDirection.Bottom;

            var b = RibbonHelper.AddButton(this, new Cut());
            b.MaxSizeMode = RibbonElementSizeMode.Medium;
            b = RibbonHelper.AddButton(this, new Copy());
            b.MaxSizeMode = RibbonElementSizeMode.Medium;
            b = RibbonHelper.AddButton(this, new Paste());
            b.MaxSizeMode = RibbonElementSizeMode.Medium;
        }
    }
}