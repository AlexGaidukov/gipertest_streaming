using VisualEditor.Logic.Commands.FloatingWindows;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Panels
{
    internal class FloatingWindowsPanel : RibbonPanel
    {
        private const string text = "Плавающие окна";

        public FloatingWindowsPanel()
        {
            InitializePanel();
        }

        private void InitializePanel()
        {
            Text = text;
            ButtonMoreVisible = false;
            FlowsTo = RibbonPanelFlowDirection.Bottom;

            var b = RibbonHelper.AddButton(this, new Commands.FloatingWindows.Course());
            b.MaxSizeMode = RibbonElementSizeMode.Medium;
            b = RibbonHelper.AddButton(this, new Concepts());
            b.MaxSizeMode = RibbonElementSizeMode.Medium;
            b = RibbonHelper.AddButton(this, new Warnings());
            b.MaxSizeMode = RibbonElementSizeMode.Medium;
        }
    }
}