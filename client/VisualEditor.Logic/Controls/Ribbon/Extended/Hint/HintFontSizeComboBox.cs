using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VisualEditor.Logic.Commands;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Extended.Hint
{
    internal class HintFontSizeComboBox : RibbonComboBoxEx
    {
        private static int fontSize;

        public HintFontSizeComboBox(AbstractCommand command)
            : base(command)
        {
            AllowTextEdit = false;

            var rbl = new List<RibbonButton>
                          {
                              new RibbonButton("8", Properties.Resources.Heading1),
                              new RibbonButton("10", Properties.Resources.Heading2),
                              new RibbonButton("12", Properties.Resources.Heading3),
                              new RibbonButton("14", Properties.Resources.Heading4),
                              new RibbonButton("18", Properties.Resources.Heading5),
                              new RibbonButton("24", Properties.Resources.Heading6),
                              new RibbonButton("28", Properties.Resources.Heading7)
                          };

            foreach (var rb in rbl)
            {
                rb.MouseUp += rb_MouseUp;
                DropDownItems.Add(rb);
            }

            //TextBoxText = "12";

            HintFontSizeChanged += FontSizeComboBox_HintFontSizeChanged;
        }

        private delegate void HintFontSizeChangedEventHandler(int fontSize);
        private static event HintFontSizeChangedEventHandler HintFontSizeChanged;

        public static int FontSize
        {
            get
            {
                return fontSize;
            }
            set
            {
                fontSize = value;

                if (HintFontSizeChanged != null)
                {
                    HintFontSizeChanged(fontSize);
                }
            }
        }

        void rb_MouseUp(object sender, MouseEventArgs e)
        {
            var fs = ((RibbonButton)sender).Text;
            fontSize = Convert.ToInt32(fs);

            switch (FontSize)
            {
                case 8:
                    fontSize = 1;
                    break;
                case 10:
                    fontSize = 2;
                    break;
                case 12:
                    fontSize = 3;
                    break;
                case 14:
                    fontSize = 4;
                    break;
                case 18:
                    fontSize = 5;
                    break;
                case 24:
                    fontSize = 6;
                    break;
                case 28:
                    fontSize = 7;
                    break;
            }

            TextBoxText = fs;
        }

        void FontSizeComboBox_HintFontSizeChanged(int fontSize)
        {
            TextBoxText = fontSize.ToString();
        }
    }
}