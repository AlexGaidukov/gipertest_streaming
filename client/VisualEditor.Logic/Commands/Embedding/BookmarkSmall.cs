using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VisualEditor.Logic.Controls.Docking.Documents;
using VisualEditor.Logic.Dialogs;
using VisualEditor.Logic.Helpers;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.Controls.HtmlEditing;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Commands.Embedding
{
    internal class BookmarkSmall : AbstractCommand
    {
        private const string operationCantBePerformedMessage = "Невозможно выполнить операцию. Попробуйте повтротить снова.";

        public BookmarkSmall()
        {
            name = CommandNames.BookmarkSmall;
            text = CommandTexts.BookmarkSmall;
            image = Properties.Resources.BookmarkSmall;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            if (EditorObserver.ActiveEditor == null)
            {
                return;
            }

            if (EditorObserver.ActiveEditor.IsSelection)
            {
                return;
            }

            using (var bd = new BookmarkDialog())
            {
                if (bd.ShowDialog(EditorObserver.DialogOwner) == DialogResult.OK)
                {
                    // Добавляет закладку в список закладок.
                    var b = new Logic.Course.Items.Bookmark
                                {
                                    Id = Guid.NewGuid(),
                                    ModuleId = ((TrainingModuleDocument)Controls.HtmlEditing.HtmlEditingToolHelper.GetParentDocument(EditorObserver.ActiveEditor)).TrainingModule.Id,
                                    Text = bd.DataTransferUnit.GetNodeValue("BookmarkName")
                                };
                    Warehouse.Warehouse.Instance.Bookmarks.Add(b);

                    // Добавляет закладку в Html-код.
                    var d = new Dictionary<string, string>
                                {
                                    {"id", b.Id.ToString()},
                                    {"class", "bookmark"}
                                };

                    try
                    {
                        HtmlEditingToolHelper.InsertHtml(EditorObserver.ActiveEditor, TagNames.AnchorTagName, d);
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
    }
}