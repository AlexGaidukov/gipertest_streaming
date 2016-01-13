using VisualEditor.Logic.Commands.Embedding;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Panels
{
    internal class MultimediaPanel : RibbonPanel
    {
        private const string text = "Мультимедиа";

        public MultimediaPanel()
        {
            InitializePanel();
        }

        private void InitializePanel()
        {
            Text = text;
            ButtonMoreVisible = false;
            FlowsTo = RibbonPanelFlowDirection.Bottom;
            
            RibbonHelper.AddButton(this, new Picture());
            RibbonHelper.AddButton(this, new Animation());
            RibbonHelper.AddButton(this, new Audio());
            RibbonHelper.AddButton(this, new Video());
        }
    }
}