using System;
using VisualEditor.Logic.Commands;
using VisualEditor.Logic.Commands.HtmlEditing;
using VisualEditor.Logic.Controls.Ribbon.Extended;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Panels
{
    internal class FontPanel : RibbonPanel
    {
        private const string text = "Шрифт";

        public FontPanel()
        {
            InitializePanel();
        }

        private void InitializePanel()
        {
            Text = text;
            ButtonMoreVisible = true;
            FlowsTo = RibbonPanelFlowDirection.Right;

            var g = new RibbonItemGroup
                        {
                            DrawBackground = false
                        };
            var t = typeof(FontNameComboBox);
            var cb = RibbonHelper.AddComboBox(g, new FontName(), t);
            cb.TextBoxWidth = 130;
            t = typeof(FontSizeComboBox);
            cb = RibbonHelper.AddComboBox(g, new FontSize(), t);
            cb.TextBoxWidth = 50;
            RibbonHelper.AddGroup(this, g);

            g = new RibbonItemGroup();
            RibbonHelper.AddButton(g, new RemoveFormat());
            RibbonHelper.AddGroup(this, g);

            g = new RibbonItemGroup();
            RibbonHelper.AddButton(g, new FontSizeUp());
            RibbonHelper.AddButton(g, new FontSizeDown());
            RibbonHelper.AddGroup(this, g);

            g = new RibbonItemGroup();
            RibbonHelper.AddButton(g, new BackColor());
            RibbonHelper.AddButton(g, new ForeColor());
            RibbonHelper.AddGroup(this, g);

            g = new RibbonItemGroup();
            RibbonHelper.AddButton(g, new Bold());
            RibbonHelper.AddButton(g, new Italic());
            RibbonHelper.AddButton(g, new Underline());
            RibbonHelper.AddButton(g, new StrikeThrough());
            RibbonHelper.AddButton(g, new Inferior());
            RibbonHelper.AddButton(g, new Ascender());
            RibbonHelper.AddGroup(this, g);

            ButtonMoreEnabled = false;
            ButtonMoreClick += FontPanel_ButtonMoreClick;
        }

        private void FontPanel_ButtonMoreClick(object sender, EventArgs e)
        {
            CommandManager.Instance.GetCommand(CommandNames.Font).Execute(null);
        }
    }
}