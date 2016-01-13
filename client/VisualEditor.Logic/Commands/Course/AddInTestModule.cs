using System;
using VisualEditor.Logic.Course.Items;

namespace VisualEditor.Logic.Commands.Course
{
    internal class AddInTestModule : AbstractCommand
    {
        public AddInTestModule()
        {
            name = CommandNames.AddInTestModule;
            text = CommandTexts.AddInTestModule;
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
                tm.Text = string.Concat("Входной контроль ", cr.InTestModules.Count + 1);
            }

            if (Warehouse.Warehouse.Instance.CourseTree.CurrentNode is TrainingModule)
            {
                var trm = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as TrainingModule;
                tm.Text = string.Concat("Входной контроль ", trm.InTestModules.Count + 1);
            }

            tm.TestType = Enums.TestType.InTest;
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