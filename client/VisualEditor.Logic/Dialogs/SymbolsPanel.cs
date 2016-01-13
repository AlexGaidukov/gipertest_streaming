using System.Windows.Forms;

namespace VisualEditor.Logic.Dialogs
{
    internal class SymbolPanel : Panel
    {
        private int indent;
        private int currentLeft;
        private int currentTop;
        private int symbolButtonWidth = 30;
        private int symbolButtonHeight = 30;

        public SymbolPanel()
        {
            indent = 3;
            currentLeft = indent;
            currentTop = indent;
            symbolButtonWidth = 30;
            symbolButtonHeight = 30;

            AutoScroll = true;
        }

        public void AddSymbolButton(SymbolButton symbolButton)
        {
            if (currentLeft + symbolButtonWidth > Width - indent)
            {
                currentLeft = indent;
                currentTop += symbolButtonHeight;
            }
            symbolButton.Left = currentLeft;
            symbolButton.Top = currentTop;
            symbolButton.Width = symbolButtonWidth;
            symbolButton.Height = symbolButtonHeight;
            symbolButton.FlatStyle = FlatStyle.Popup;
            Controls.Add(symbolButton);
            currentLeft += symbolButtonWidth;
        }
    }
}
