using VisualEditor.Logic.Commands.Course;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Panels
{
    internal class AboutAuthorsPanel : RibbonPanel
    {
        private const string text = "Об авторах";

        public AboutAuthorsPanel()
        {
            InitializePanel();
        }

        private void InitializePanel()
        {
            Text = text;
            ButtonMoreVisible = false;
            FlowsTo = RibbonPanelFlowDirection.Bottom;

            RibbonHelper.AddButton(this, new AboutAuthors());
        }
    }
}