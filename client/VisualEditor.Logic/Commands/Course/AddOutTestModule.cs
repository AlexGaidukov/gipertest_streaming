using System;
using VisualEditor.Logic.Course.Items;

namespace VisualEditor.Logic.Commands.Course
{
    internal class AddOutTestModule : AbstractCommand
    {
        public AddOutTestModule()
        {
            name = CommandNames.AddOutTestModule;
            text = CommandTexts.AddOutTestModule;
            image = Properties.Resources.TestModule;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            var tm = new TestModule
                         {
                             Id = Guid.NewGuid()
                         };

            if (Warehouse.Warehouse.Instance.CourseTree.CurrentNode is CourseRoot)
            {
                var cr = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as CourseRoot;
                tm.Text = string.Concat("Выходной контроль ", cr.OutTestModules.Count + 1);
            }

            if (Warehouse.Warehouse.Instance.CourseTree.CurrentNode is TrainingModule)
            {
                var trm = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as TrainingModule;
                tm.Text = string.Concat("Выходной контроль ", trm.OutTestModules.Count + 1);
            }

            tm.TestType = Enums.TestType.OutTest;
            Warehouse.Warehouse.Instance.CourseTree.CurrentNode.Nodes.Add(tm);

            if (!Warehouse.Warehouse.Instance.CourseTree.CurrentNode.IsExpanded)
            {
                Warehouse.Warehouse.Instance.CourseTree.CurrentNode.Toggle();
            }

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