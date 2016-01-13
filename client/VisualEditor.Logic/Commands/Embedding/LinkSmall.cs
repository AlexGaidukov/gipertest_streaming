using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VisualEditor.Logic.Controls.Docking.Documents;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Dialogs;
using VisualEditor.Logic.Helpers;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.Controls.HtmlEditing;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Commands.Embedding
{
    internal class LinkSmall : AbstractCommand
    {
        private const string operationCantBePerformedMessage = "Невозможно выполнить операцию. Попробуйте повтротить снова.";

        public LinkSmall()
        {
            name = CommandNames.LinkSmall;
            text = CommandTexts.LinkSmall;
            image = Properties.Resources.LinkSmall;
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

            if (!EditorObserver.ActiveEditor.IsSelection)
            {
                return;
            }

            using (var ld = new LinkDialog())
            {
                var dtu = ld.DataTransferUnit;
                dtu.SetNodeValue("LinkText", EditorObserver.ActiveEditor.GetSelection());
                ld.InitializeData();

                if (ld.ShowDialog(EditorObserver.DialogOwner) == DialogResult.OK)
                {
                    var lt = ld.DataTransferUnit.GetNodeValue("LinkTarget");

                    if (!lt.Equals("Hyperlink"))
                    {
                        #region Ссылка на объект

                        var loi = ld.DataTransferUnit.GetNodeValue("LinkObjectId");
                        var d = new Dictionary<string, string>
                                {
                                    {"href", loi}
                                };

                        if (lt.Equals("Bookmark"))
                        {
                            d.Add("class", "linktobookmark");
                        }

                        if (lt.Equals("InternalConcept"))
                        {
                            d.Add("class", "linktointernalconcept");
                        }

                        if (lt.Equals("ExternalConcept"))
                        {
                            d.Add("class", "linktoexternalconcept");
                        }

                        if (lt.Equals("TrainingModule"))
                        {
                            d.Add("class", "linktotrainingmodule");
                        }

                        var ltxt = ld.DataTransferUnit.GetNodeValue("LinkText");

                        try
                        {
                            HtmlEditingToolHelper.SurroundWithHtml(EditorObserver.ActiveEditor, TagNames.AnchorTagName, d, ltxt);
                        }
                        catch (Exception exception)
                        {
                            ExceptionManager.Instance.LogException(exception);
                            UIHelper.ShowMessage(operationCantBePerformedMessage, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }

                        var lto = new LinkToObject
                                      {
                                          TrainingModule =
                                              (Controls.HtmlEditing.HtmlEditingToolHelper.GetParentDocument(
                                                   EditorObserver.ActiveEditor) as TrainingModuleDocument).
                                              TrainingModule,
                                          ObjectId = new Guid(loi)
                                      };
                        Warehouse.Warehouse.Instance.LinksToObjects.Add(lto);

                        #endregion
                    }
                    else
                    {
                        #region Ссылка на файл, веб-страницу

                        var url = ld.DataTransferUnit.GetNodeValue("Url");

                        var d = new Dictionary<string, string>
                                {
                                    {"href", url}
                                };

                        var ltxt = ld.DataTransferUnit.GetNodeValue("LinkText");

                        try
                        {
                            HtmlEditingToolHelper.SurroundWithHtml(EditorObserver.ActiveEditor, TagNames.AnchorTagName, d, ltxt);
                        }
                        catch (Exception exception)
                        {
                            ExceptionManager.Instance.LogException(exception);
                            UIHelper.ShowMessage(operationCantBePerformedMessage, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }

                        #endregion
                    }
                }
            }
        }
    }
}