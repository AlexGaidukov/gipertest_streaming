using VisualEditor.Logic.Commands.Document;
using VisualEditor.Logic.Commands.HtmlEditing;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Panels
{
    internal class EditorModesPanel : RibbonPanel
    {
        private const string text = "Режимы редактора";

        public EditorModesPanel()
        {
            InitializePanel();
        }

        private void InitializePanel()
        {
            Text = text;
            ButtonMoreVisible = false;
            FlowsTo = RibbonPanelFlowDirection.Bottom;

            RibbonHelper.AddButton(this, new Design());
            RibbonHelper.AddButton(this, new Preview());
            RibbonHelper.AddButton(this, new Source());
            RibbonHelper.AddSeparator(this);

            var b = RibbonHelper.AddButton(this, new NavigateBackward());
            b.MaxSizeMode = RibbonElementSizeMode.Medium;
            b = RibbonHelper.AddButton(this, new NavigateForward());
            b.MaxSizeMode = RibbonElementSizeMode.Medium;
        }
    }
}