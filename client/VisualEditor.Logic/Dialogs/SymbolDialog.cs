using System;

namespace VisualEditor.Logic.Dialogs
{
    internal partial class SymbolDialog : DialogBase
    {
        public SymbolDialog()
        {
            InitializeComponent();
            InitializeDialog();
        }

        #region InitializeDialog
        
        private void InitializeDialog()
        {
            SymbolButton sb;
            for (var i = 913; i < 930; i++)
            {
                sb = new SymbolButton(i);
                symbolsPanel.AddSymbolButton(sb);
            }
            for (var i = 931; i < 937; i++)
            {
                sb = new SymbolButton(i);
                symbolsPanel.AddSymbolButton(sb);
            }
            for (var i = 945; i < 969; i++)
            {
                sb = new SymbolButton(i);
                symbolsPanel.AddSymbolButton(sb);
            }
            for (var i = 161; i < 192; i++)
            {
                sb = new SymbolButton(i);
                symbolsPanel.AddSymbolButton(sb);
            }
            sb = new SymbolButton(8482);
            symbolsPanel.AddSymbolButton(sb);
            sb = new SymbolButton(8226);
            symbolsPanel.AddSymbolButton(sb);
            sb = new SymbolButton(8721);
            symbolsPanel.AddSymbolButton(sb);
            sb = new SymbolButton(8719);
            symbolsPanel.AddSymbolButton(sb);
            sb = new SymbolButton(8747);
            symbolsPanel.AddSymbolButton(sb);
            sb = new SymbolButton(8706);
            symbolsPanel.AddSymbolButton(sb);
            sb = new SymbolButton(8730);
            symbolsPanel.AddSymbolButton(sb);
            sb = new SymbolButton(8734);
            symbolsPanel.AddSymbolButton(sb);
            sb = new SymbolButton(402);
            symbolsPanel.AddSymbolButton(sb);
            sb = new SymbolButton(8804);
            symbolsPanel.AddSymbolButton(sb);
            sb = new SymbolButton(8805);
            symbolsPanel.AddSymbolButton(sb);
            sb = new SymbolButton(8800);
            symbolsPanel.AddSymbolButton(sb);
            sb = new SymbolButton(8801, Properties.Resources.Symbol1);
            symbolsPanel.AddSymbolButton(sb);

            sb = new SymbolButton(8230);
            symbolsPanel.AddSymbolButton(sb);
            sb = new SymbolButton(8242);
            symbolsPanel.AddSymbolButton(sb);
            sb = new SymbolButton(8243);
            symbolsPanel.AddSymbolButton(sb);

            sb = new SymbolButton(8707, Properties.Resources.Symbol2);
            symbolsPanel.AddSymbolButton(sb);
            sb = new SymbolButton(8712, Properties.Resources.Symbol3);
            symbolsPanel.AddSymbolButton(sb);
            sb = new SymbolButton(8715, Properties.Resources.Symbol4);
            symbolsPanel.AddSymbolButton(sb);

            sb = new SymbolButton(8743, Properties.Resources.Symbol5);
            symbolsPanel.AddSymbolButton(sb);
            sb = new SymbolButton(8744, Properties.Resources.Symbol6);
            symbolsPanel.AddSymbolButton(sb);
            sb = new SymbolButton(8745, Properties.Resources.Symbol7);
            symbolsPanel.AddSymbolButton(sb);
            sb = new SymbolButton(8746, Properties.Resources.Symbol8);
            symbolsPanel.AddSymbolButton(sb);

            sb = new SymbolButton(8764, Properties.Resources.Symbol9);
            symbolsPanel.AddSymbolButton(sb);
            sb = new SymbolButton(8776);
            symbolsPanel.AddSymbolButton(sb);

            sb = new SymbolButton(8834, Properties.Resources.Symbol10);
            symbolsPanel.AddSymbolButton(sb);
            sb = new SymbolButton(8835, Properties.Resources.Symbol11);
            symbolsPanel.AddSymbolButton(sb);

            sb = new SymbolButton(8838, Properties.Resources.Symbol12);
            symbolsPanel.AddSymbolButton(sb);

            sb = new SymbolButton(8839, Properties.Resources.Symbol13);
            symbolsPanel.AddSymbolButton(sb);

            sb = new SymbolButton(8853, Properties.Resources.Symbol14);
            symbolsPanel.AddSymbolButton(sb);
            sb = new SymbolButton(8869, Properties.Resources.Symbol15);
            symbolsPanel.AddSymbolButton(sb);

            sb = new SymbolButton(176);
            symbolsPanel.AddSymbolButton(sb);
            sb = new SymbolButton(8594, Properties.Resources.Symbol16);
            symbolsPanel.AddSymbolButton(sb);
            sb = new SymbolButton(215);
            symbolsPanel.AddSymbolButton(sb);
            sb = new SymbolButton(247);
            symbolsPanel.AddSymbolButton(sb);

            sb = new SymbolButton(8704, Properties.Resources.Symbol17);
            symbolsPanel.AddSymbolButton(sb);

            sb = new SymbolButton(0, Properties.Resources.Symbol18);
            sb.Symbol = "<SPAN style=\"FONT-FAMILY: Symbol; mso-bidi-font-size: 12.0pt; mso-ascii-font-family: 'Times New Roman'; mso-fareast-font-family: 'Times New Roman'; mso-hansi-font-family: 'Times New Roman'; mso-bidi-font-family: 'Times New Roman'; mso-ansi-language: EN-US; mso-fareast-language: RU; mso-bidi-language: AR-SA; mso-char-type: symbol; mso-symbol-font-family: Symbol\">Ë</SPAN>";
            symbolsPanel.AddSymbolButton(sb);

            sb = new SymbolButton(0, Properties.Resources.Symbol19);
            sb.Symbol = "<SPAN style=\"FONT-FAMILY: Symbol; mso-bidi-font-size: 12.0pt; mso-ascii-font-family: 'Times New Roman'; mso-fareast-font-family: 'Times New Roman'; mso-hansi-font-family: 'Times New Roman'; mso-bidi-font-family: 'Times New Roman'; mso-ansi-language: EN-US; mso-fareast-language: RU; mso-bidi-language: AR-SA; mso-char-type: symbol; mso-symbol-font-family: Symbol\">Ï</SPAN>";
            symbolsPanel.AddSymbolButton(sb);

            sb = new SymbolButton(0, Properties.Resources.Symbol20);
            sb.Symbol = "<SPAN style=\"FONT-FAMILY: Symbol; mso-ascii-font-family: 'Times New Roman'; mso-hansi-font-family: 'Times New Roman'; mso-char-type: symbol; mso-symbol-font-family: Symbol\">Æ</SPAN>";
            symbolsPanel.AddSymbolButton(sb);

            sb = new SymbolButton(0, Properties.Resources.Symbol21);
            sb.Symbol = "<SPAN style=\"FONT-FAMILY: Symbol; mso-ascii-font-family: 'Times New Roman'; mso-hansi-font-family: 'Times New Roman'; mso-ansi-language: EN-US; mso-char-type: symbol; mso-symbol-font-family: Symbol\">Ä</SPAN>";
            symbolsPanel.AddSymbolButton(sb);

            sb = new SymbolButton(0, Properties.Resources.Symbol22);
            sb.Symbol = "<SPAN style=\"FONT-FAMILY: Symbol; mso-ascii-font-family: 'Times New Roman'; mso-hansi-font-family: 'Times New Roman'; mso-ansi-language: EN-US; mso-char-type: symbol; mso-symbol-font-family: Symbol\">Þ</SPAN>";
            symbolsPanel.AddSymbolButton(sb);

            HelpKeyword = "Символ";
        }

        #endregion

        private void closeButton_Click(object sender, EventArgs e)
        {

        }
    }
}