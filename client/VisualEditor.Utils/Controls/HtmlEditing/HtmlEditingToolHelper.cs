using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using mshtml;
using VisualEditor.Utils.Helpers;


namespace VisualEditor.Utils.Controls.HtmlEditing
{
    public static class HtmlEditingToolHelper
    {
        #region Ожидание средства визуального редактирования
        
        public static void WaitForIdleness(HtmlEditingTool htmlEditingTool)
        {
            if (htmlEditingTool.IsNull())
            {
                throw new ArgumentNullException();
            }

            while (htmlEditingTool.IsBusy)
            {
                Application.DoEvents();
            }
        }

        public static void WaitForValidBody(HtmlEditingTool htmlEditingTool)
        {
            if (htmlEditingTool.IsNull() ||
                htmlEditingTool.Document.IsNull())
            {
                throw new ArgumentNullException();
            }

            while (htmlEditingTool.Document.Body.IsNull())
            {
                Application.DoEvents();
            }
        }

        #endregion

        #region Вставка html кода

        public static void InsertHtml(HtmlEditingTool htmlEditingTool, string tagName, Dictionary<string, string> attributes)
        {
            if (htmlEditingTool.IsNull() ||
                tagName.IsNull() ||
                attributes.IsNull())
            {
                throw new ArgumentNullException();
            }

            if (htmlEditingTool.Mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            // Вставка html кода разрешена только при снятом выделении.
            if (htmlEditingTool.IsSelection)
            {
                return;
            }

            var htmlDocument = htmlEditingTool.GetNativeHtmlDocument();
            IHTMLTxtRange textRange;
            try
            {
                textRange = htmlDocument.selection.createRange() as IHTMLTxtRange;
            }
            catch
            {
                throw new InvalidOperationException();
            }

            if (textRange.IsNull())
            {
                throw new NullReferenceException();
            }

            var attrs = ConcatAttributes(attributes);
            var html = string.Concat("<", tagName, " ", attrs, ">");

            try
            {
                textRange.pasteHTML(html);
                textRange.collapse(false);
                textRange.select();
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }

        public static void InsertHtml(HtmlEditingTool htmlEditingTool, object htmlElement)
        {
            if (htmlEditingTool.IsNull() ||
                htmlElement.IsNull())
            {
                throw new ArgumentNullException();
            }

            if (htmlEditingTool.Mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            // Вставка html кода разрешена только при снятом выделении.
            if (htmlEditingTool.IsSelection)
            {
                return;
            }

            var htmlDocument = htmlEditingTool.GetNativeHtmlDocument();
            IHTMLTxtRange textRange;
            try
            {
                textRange = htmlDocument.selection.createRange() as IHTMLTxtRange;
            }
            catch
            {
                throw new InvalidOperationException();
            }

            if (textRange.IsNull())
            {
                throw new NullReferenceException();
            }

            if (!(htmlElement is HtmlElement))
            {
                return;
            }

            var html = (htmlElement as HtmlElement).OuterHtml;

            try
            {
                textRange.pasteHTML(html);
                textRange.collapse(false);
                textRange.select();
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }

        public static void InsertHtml(HtmlEditingTool htmlEditingTool, string html)
        {
            if (htmlEditingTool.IsNull() ||
                html.IsNull())
            {
                throw new ArgumentNullException();
            }

            //if (htmlEditingTool.Mode == Enums.HtmlEditingToolMode.Preview)
            //{
            //    throw new InvalidOperationException();
            //}

            // Вставка html кода разрешена только при снятом выделении.
            if (htmlEditingTool.IsSelection)
            {
                return;
            }

            var htmlDocument = htmlEditingTool.GetNativeHtmlDocument();
            IHTMLTxtRange textRange;
            try
            {
                textRange = htmlDocument.selection.createRange() as IHTMLTxtRange;
            }
            catch
            {
                throw new InvalidOperationException();
            }

            if (textRange.IsNull())
            {
                throw new NullReferenceException();
            }

            try
            {
                textRange.pasteHTML(html);
                textRange.collapse(false);
                textRange.select();
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }

        #endregion

        #region Обрамление html кодом

        public static void DeleteSurroundWithHtml(HtmlEditingTool htmlEditingTool, string tagName, Dictionary<string, string> attributes)
        {
            if (htmlEditingTool.IsNull() ||
                tagName.IsNull() ||
                attributes.IsNull())
            {
                throw new ArgumentNullException();
            }

            if (htmlEditingTool.Mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            // Обрамление html кода разрешено только при наличии выделения.
            if (!htmlEditingTool.IsSelection)
            {
                return;
            }

            var htmlDocument = htmlEditingTool.GetNativeHtmlDocument();
            IHTMLTxtRange textRange;
            try
            {
                textRange = htmlDocument.selection.createRange() as IHTMLTxtRange;
            }
            catch
            {
                throw new InvalidOperationException();
            }

            if (textRange.IsNull())
            {
                throw new NullReferenceException();
            }
           
            var current = textRange.htmlText;
            var htmlText = textRange.htmlText;
            htmlText = string.Copy(htmlText);
            var htmlPaste = htmlText;
            string searchString1 = "<DIV";
            string searchString2 = "</DIV";
            while (htmlText.Contains(searchString1) && htmlText.Contains(searchString2))
            {
                int first = htmlText.IndexOf(searchString1);
                int second = htmlText.IndexOf(searchString2);
                if (first < second)
                {
                    int i = htmlPaste.IndexOf(searchString1);
                    
                    string temp="";
                    string temp2 = "";
                    while(htmlPaste[i]!='>')
                    {
                      
                        for(int j=0; j<htmlPaste.Length; j++)
                            if (j != i) 
                            { 
                                temp = string.Concat(temp, htmlPaste[j].ToString());
                                temp2 = string.Concat(temp2, htmlPaste[j].ToString());
                            }
                            else { temp2 = string.Concat(temp2, " "); }
                      
                        htmlPaste = temp;
                        htmlText = temp2;
                        temp = "";
                        temp2 = "";
                   
                    }
                    for (int j = 0; j < htmlPaste.Length; j++)
                        if (j != i) { temp = string.Concat(temp, htmlPaste[j].ToString()); }
                    htmlPaste = temp;

                    i = htmlPaste.IndexOf(searchString2);
                    temp = "";
                    for (int j = 0; j < htmlPaste.Length; j++)
                        if (j < i || j>(i+5)) { temp = string.Concat(temp, htmlPaste[j].ToString()); }
                    htmlPaste = temp;

                    i = htmlText.IndexOf(searchString2);
                    temp2 = "";
                    for (int j = 0; j < htmlText.Length; j++)
                        if (j < i || j > (i + 5)) { temp2 = string.Concat(temp2, htmlText[j].ToString()); }
                    htmlText = temp2;
                   
                }
                else 
                {
                    string temp2 = "";
                    for (int j = 0; j < htmlText.Length; j++)
                        if (j < second || j > (second + 5)) { temp2 = string.Concat(temp2, htmlText[j].ToString()); }
                    htmlText = temp2;
                }
            }
            try
            {
                textRange.pasteHTML(htmlPaste);
                textRange.collapse(false);
                textRange.select();
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }

        public static void SurroundWithHtml(HtmlEditingTool htmlEditingTool, string tagName, Dictionary<string, string> attributes)
        {
            if (htmlEditingTool.IsNull() ||
                tagName.IsNull() ||
                attributes.IsNull())
            {
                throw new ArgumentNullException();
            }

            if (htmlEditingTool.Mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            // Обрамление html кода разрешено только при наличии выделения.
            if (!htmlEditingTool.IsSelection)
            {
                return;
            }

            var htmlDocument = htmlEditingTool.GetNativeHtmlDocument();
            IHTMLTxtRange textRange;
            try
            {
                textRange = htmlDocument.selection.createRange() as IHTMLTxtRange;
            }
            catch
            {
                throw new InvalidOperationException();
            }

            if (textRange.IsNull())
            {
                throw new NullReferenceException();
            }

            var selection = htmlEditingTool.GetSelection();
            selection = string.Copy(selection);

            if (selection.IsNull())
            {
                throw new NullReferenceException();
            }

            if (selection.StartsWith(" "))
            {
                selection = selection.Remove(0, 1);
                selection = string.Concat("&nbsp;", selection);
            }

            var attrs = ConcatAttributes(attributes);
            selection = string.Concat("<", tagName, " ", attrs, ">", selection, "</", tagName, ">");

            try
            {
                textRange.pasteHTML(selection);
                textRange.collapse(false);
                textRange.select();
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }

        //моя функция для стилей
        public static void SurroundWithStyleHtml(HtmlEditingTool htmlEditingTool, string tagName, Dictionary<string, string> attributes)
        {
            if (htmlEditingTool.IsNull() ||
                tagName.IsNull() ||
                attributes.IsNull())
            {
                throw new ArgumentNullException();
            }

            if (htmlEditingTool.Mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            // Обрамление html кода разрешено только при наличии выделения.
            if (!htmlEditingTool.IsSelection)
            {
                return;
            }

            var htmlDocument = htmlEditingTool.GetNativeHtmlDocument();
            IHTMLTxtRange textRange;
            try
            {
                textRange = htmlDocument.selection.createRange() as IHTMLTxtRange;
            }
            catch
            {
                throw new InvalidOperationException();
            }

            if (textRange.IsNull())
            {
                throw new NullReferenceException();
            }

            var selection = textRange.htmlText;
           // var htmlText = textRange.htmlText;
            selection = string.Copy(selection);
           // var htmlPaste = htmlText;


         //   var selection = htmlEditingTool.GetSelection();
          //  selection = string.Copy(selection);

            if (selection.IsNull())
            {
                throw new NullReferenceException();
            }

            if (selection.StartsWith(" "))
            {
                selection = selection.Remove(0, 1);
                selection = string.Concat("&nbsp;", selection);
            }

            var attrs = ConcatAttributes(attributes);
            selection = string.Concat("<", tagName, " ", attrs, ">", selection, "</", tagName, ">");

            try
            {
                textRange.pasteHTML(selection);
                textRange.collapse(false);
                textRange.select();
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }

        public static void SurroundWithHtml(HtmlEditingTool htmlEditingTool, string tagName, Dictionary<string, string> attributes, string text)
        {
            if (htmlEditingTool.IsNull() ||
                tagName.IsNull() ||
                attributes.IsNull() ||
                text.IsNull())
            {
                throw new ArgumentNullException();
            }

            if (htmlEditingTool.Mode == Enums.HtmlEditingToolMode.Preview)
            {
                throw new InvalidOperationException();
            }

            // Обрамление html кода разрешено только при наличии выделения.
            if (!htmlEditingTool.IsSelection)
            {
                return;
            }

            var htmlDocument = htmlEditingTool.GetNativeHtmlDocument();
            IHTMLTxtRange textRange;
            try
            {
                textRange = htmlDocument.selection.createRange() as IHTMLTxtRange;
            }
            catch
            {
                throw new InvalidOperationException();
            }

            if (textRange.IsNull())
            {
                throw new NullReferenceException();
            }

            var selection = htmlEditingTool.GetSelection();
            selection = string.Copy(selection);

            if (selection.IsNull())
            {
                throw new NullReferenceException();
            }

            if (selection.StartsWith(" "))
            {
                selection = selection.Remove(0, 1);
                selection = string.Concat("&nbsp;", selection);
            }

            var attrs = ConcatAttributes(attributes);
            selection = string.Concat("<", tagName, " ", attrs, ">", text, "</", tagName, ">");

            try
            {
                textRange.pasteHTML(selection);
                textRange.collapse(false);
                textRange.select();
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }

        #endregion

        private static string ConcatAttributes(Dictionary<string, string> attributes)
        {
            if (attributes.IsNull())
            {
                throw new ArgumentNullException();
            }

            var attrs = string.Empty;
            foreach (var attribute in attributes)
            {
                attrs += string.Concat(attribute.Key, "=\"", attribute.Value, "\" ");
            }

            return attrs;
        }

        public static Color ConvertToColor(string clrs)
        {
            if (string.IsNullOrEmpty(clrs))
            {
                throw new ArgumentNullException();
            }

            int red, green, blue;
            if (clrs.StartsWith("#"))
            {
                var clrn = Convert.ToInt32(clrs.Substring(1), 16);
                red = (clrn >> 16) & 255;
                green = (clrn >> 8) & 255;
                blue = clrn & 255;
            }
            else
            {
                var clrn = Convert.ToInt32(clrs);
                red = clrn & 255;
                green = (clrn >> 8) & 255;
                blue = (clrn >> 16) & 255;
            }

            return Color.FromArgb(red, green, blue);
        }
    }
}