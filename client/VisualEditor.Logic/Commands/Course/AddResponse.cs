using System;
using VisualEditor.Logic.Controls.Docking.Documents;
using VisualEditor.Logic.Controls.HtmlEditing;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Warehouse;

namespace VisualEditor.Logic.Commands.Course
{
    internal class AddResponse : AbstractCommand
    {
        public AddResponse()
        {
            name = CommandNames.AddResponse;
            text = CommandTexts.AddResponse;
            image = Properties.Resources.Response;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            var r = new Response
                        {
                            Id = Guid.NewGuid()
                        };

            if (Warehouse.Warehouse.Instance.CourseTree.CurrentNode is Question)
            {
                var q = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as Question;
                r.Text = string.Concat("Ответ ", q.Responses.Count + 1);
            }

            Warehouse.Warehouse.Instance.CourseTree.CurrentNode.Nodes.Add(r);

            if (!Warehouse.Warehouse.Instance.CourseTree.CurrentNode.IsExpanded)
            {
                Warehouse.Warehouse.Instance.CourseTree.CurrentNode.Toggle();
            }

            // Создает и отображает редактор.
            r.ResponseDocument = new ResponseDocument
                                     {
                                         Response = r,
                                         Text = r.Text,
                                         HtmlEditingTool =
                                             {Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design}
                                     };

            //r.ResponseDocument.VisualHtmlEditor.SetDefaultFont();
            HtmlEditingToolHelper.SetDefaultDocumentHtml(r.ResponseDocument.HtmlEditingTool);
            PreviewObserver.AddDocument(r.ResponseDocument);
            r.ResponseDocument.Show();

            Warehouse.Warehouse.Instance.CourseTree.CurrentNode = r;
            Warehouse.Warehouse.IsProjectModified = true;
        }
    }
}