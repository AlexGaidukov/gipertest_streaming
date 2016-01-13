using VisualEditor.Logic.Commands.HtmlEditing;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Panels
{
    internal class EditingPanel : RibbonPanel
    {
        private const string text = "Редактирование";

        public EditingPanel()
        {
            InitializePanel();
        }

        private void InitializePanel()
        {
            Text = text;
            ButtonMoreVisible = false;
            FlowsTo = RibbonPanelFlowDirection.Bottom;

            var b = RibbonHelper.AddButton(this, new Find());
            b.MaxSizeMode = RibbonElementSizeMode.Medium;
            b = RibbonHelper.AddButton(this, new Replace());
            b.MaxSizeMode = RibbonElementSizeMode.Medium;
        }
    }
}