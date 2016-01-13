using System;
using System.Drawing;
using System.Windows.Forms;
using VisualEditor.Logic.Helpers;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.Controls.HtmlEditing;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Dialogs
{
    internal class SymbolButton : Button
    {
        private const string operationCantBePerformedMessage = "Невозможно вставить рисунок в редактор.";

        private SymbolButton()
        {
            Click += SymbolButton_Click;
        }

        public SymbolButton(int utf32)
            : this()
        {
            Symbol = char.ConvertFromUtf32(utf32);
            Text = Symbol;
        }

        public SymbolButton(int utf32, Image image)
            : this()
        {
            Symbol = char.ConvertFromUtf32(utf32);
            Image = image;
            ImageAlign = ContentAlignment.MiddleCenter;
        }

        public string Symbol { get; set; }

        private void SymbolButton_Click(object sender, EventArgs e)
        {
            try
            {
                HtmlEditingToolHelper.InsertHtml(EditorObserver.ActiveEditor, Symbol);
                Warehouse.Warehouse.IsProjectModified = true;
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
                UIHelper.ShowMessage(operationCantBePerformedMessage, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
        }
    }
}