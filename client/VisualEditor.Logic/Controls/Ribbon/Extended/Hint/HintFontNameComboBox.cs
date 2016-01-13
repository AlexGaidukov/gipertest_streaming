using System.Collections.Generic;
using System.Windows.Forms;
using VisualEditor.Logic.Commands;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Extended.Hint
{
    internal class HintFontNameComboBox : RibbonComboBoxEx
    {
        private static string fontName;

        public HintFontNameComboBox(AbstractCommand command)
            : base(command)
        {
            AllowTextEdit = false;
            var rbl = new List<RibbonButton>
                          {
                              new RibbonButton("Algerian", Properties.Resources.FontTT),
                              new RibbonButton("Arial", Properties.Resources.FontTT),
                              new RibbonButton("Arial Black", Properties.Resources.FontTT),
                              new RibbonButton("Broadway", Properties.Resources.FontTT),
                              new RibbonButton("Calibri", Properties.Resources.FontTT),
                              new RibbonButton("Cambria", Properties.Resources.FontTT),
                              new RibbonButton("Cambria Math", Properties.Resources.FontTT),
                              new RibbonButton("Comic Sans MS", Properties.Resources.FontTT),
                              new RibbonButton("Consolas", Properties.Resources.FontTT),
                              new RibbonButton("Corbel", Properties.Resources.FontTT),
                              new RibbonButton("Courier New", Properties.Resources.FontTT),
                              new RibbonButton("Microsoft Sans Serif", Properties.Resources.FontTT),
                              new RibbonButton("Symbol", Properties.Resources.FontTT),
                              new RibbonButton("Tahoma", Properties.Resources.FontTT),
                              new RibbonButton("Times New Roman", Properties.Resources.FontTT),
                              new RibbonButton("Verdana", Properties.Resources.FontTT),
                              new RibbonButton("Vivaldi", Properties.Resources.FontTT),
                              new RibbonButton("Webdings", Properties.Resources.FontTT)
                          };

            foreach (var rb in rbl)
            {
                rb.MouseUp += rb_MouseUp;
                DropDownItems.Add(rb);
            }

            //TextBoxText = "Times New Roman";

            HintFontNameChanged += FontNameComboBox_HintFontNameChanged;
        }

        private delegate void HintFontNameChangedEventHandler(string fontName);
        private static event HintFontNameChangedEventHandler HintFontNameChanged;

        public static string FontName
        {
            get
            {
                return fontName;
            }
            set
            {
                fontName = value;

                if (HintFontNameChanged != null)
                {
                    HintFontNameChanged(fontName);
                }
            }
        }

        void rb_MouseUp(object sender, MouseEventArgs e)
        {
            var fn = ((RibbonButton)sender).Text;
            fontName = fn;
            TextBoxText = fn;
        }

        void FontNameComboBox_HintFontNameChanged(string fontName)
        {
            TextBoxText = fontName;
        }
    }
}