using System;
using VisualEditor.Logic.Controls.Docking.Documents;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Warehouse;
using HtmlEditingToolHelper=VisualEditor.Logic.Controls.HtmlEditing.HtmlEditingToolHelper;

namespace VisualEditor.Logic.Commands.Course
{
    internal class AddTrainingModule : AbstractCommand
    {
        public AddTrainingModule()
        {
            name = CommandNames.AddTrainingModule;
            text = CommandTexts.AddTrainingModule;
            image = Properties.Resources.TrainingModule;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            // Создает и добавляет учебный модуль в дерево учебного курса.
            var tm = new TrainingModule();
            TrainingModule.Count++;
            tm.Id = Guid.NewGuid();
            tm.Text = string.Concat("Учебный модуль ", TrainingModule.Count);
            Warehouse.Warehouse.Instance.CourseTree.CurrentNode.Nodes.Add(tm);

            if (!Warehouse.Warehouse.Instance.CourseTree.CurrentNode.IsExpanded)
            {
                Warehouse.Warehouse.Instance.CourseTree.CurrentNode.Toggle();
            }

            // Добавляет учебный модуль в список.
            Warehouse.Warehouse.Instance.TrainingModules.Add(tm);

            // Создает и отображает редактор.
            tm.TrainingModuleDocument = new TrainingModuleDocument
                                            {
                                                TrainingModule = tm,
                                                Text = tm.Text,
                                                HtmlEditingTool =
                                                    {Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design}
                                            };

            //tm.TrainingModuleDocument.VisualHtmlEditor.SetDefaultFont();
            HtmlEditingToolHelper.SetDefaultDocumentHtml(tm.TrainingModuleDocument.HtmlEditingTool);
            PreviewObserver.AddDocument(tm.TrainingModuleDocument);
            tm.TrainingModuleDocument.Show();

            // Предлагает переименовать учебный модуль.
            Warehouse.Warehouse.Instance.CourseTree.LabelEdit = true;
            if (!tm.IsEditing)
            {
                tm.BeginEdit();
            }

            Warehouse.Warehouse.Instance.CourseTree.CurrentNode = tm;
            Warehouse.Warehouse.IsProjectModified = true;
        }
    }
}