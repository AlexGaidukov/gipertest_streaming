using VisualEditor.Utils.Controls.Ribbon;
using VisualEditor.Logic.Commands.IO;

namespace VisualEditor.Logic.Controls.Ribbon.Panels
{
    internal class ImsQtiPanel : RibbonPanel
    {
        private const string text = "IMS QTI";

        public ImsQtiPanel()
        {
            InitializePanel();
        }

        private void InitializePanel()
        {
            Text = text;
            ButtonMoreVisible = false;
            FlowsTo = RibbonPanelFlowDirection.Bottom;

            RibbonHelper.AddButton(this, new LoadFromImsQti());
            RibbonHelper.AddButton(this, new SaveToImsQti());
        }
    }
}