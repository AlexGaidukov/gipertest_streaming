using System;
using VisualEditor.Logic.Commands;
using VisualEditor.Logic.Commands.Course;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.Ribbon.Panels
{
    internal class StructurePanel : RibbonPanel
    {
        private const string text = "Структура";
        private RibbonButton addItemButton;

        public StructurePanel()
        {
            InitializeStructurePanel();
        }

        private void InitializeStructurePanel()
        {
            Text = text;
            ButtonMoreVisible = false;
            FlowsTo = RibbonPanelFlowDirection.Bottom;

            addItemButton = RibbonHelper.AddButton(this, new AddItem());
            addItemButton.Style = RibbonButtonStyle.DropDown;
            addItemButton.DropDownShowing += addItemButton_DropDownShowing;
            RibbonHelper.AddSeparator(this);

            // POSTPONE: Реализовать.
            //var b = RibbonHelper.AddButton(this, new CopyItem());
            //b.MaxSizeMode = RibbonElementSizeMode.Medium;
            //b = RibbonHelper.AddButton(this, new PasteItem());
            //b.MaxSizeMode = RibbonElementSizeMode.Medium;
            var b = RibbonHelper.AddButton(this, new DeleteItem());
            b.MaxSizeMode = RibbonElementSizeMode.Medium;
            RibbonHelper.AddSeparator(this);
            RibbonHelper.AddButton(this, new ItemOptions());
        }

        #region Изменение пунктов меню "Добавить" в зависимости от выбранного узла

        private void addItemButton_DropDownShowing(object sender, EventArgs e)
        {
            var cn = Warehouse.Warehouse.Instance.CourseTree.CurrentNode;

            if (cn == null)
            {
                addItemButton.DropDownItems.Clear();
                return;
            }

            if (cn is InConceptParent ||
                cn is OutConceptParent ||
                cn is InDummyConcept ||
                cn is OutDummyConcept ||
                cn is Response)
            {
                addItemButton.DropDownItems.Clear();
                return;
            }

            if (cn is CourseRoot)
            {
                addItemButton.DropDownItems.Clear();
                RibbonHelper.AddButton(addItemButton, CommandManager.Instance.GetCommand(CommandNames.AddTrainingModule));
                RibbonHelper.AddButton(addItemButton, CommandManager.Instance.GetCommand(CommandNames.AddInTestModule));
                RibbonHelper.AddButton(addItemButton, CommandManager.Instance.GetCommand(CommandNames.AddOutTestModule));
                RibbonHelper.AddButton(addItemButton, CommandManager.Instance.GetCommand(CommandNames.AddTestModuleFromOuterCourse));
               // RibbonHelper.AddSeparator(addItemButton);
               // RibbonHelper.AddButton(addItemButton, CommandManager.Instance.GetCommand(CommandNames.LoadFromImsQti));
            }

            if (cn is TrainingModule)
            {
                addItemButton.DropDownItems.Clear();
                RibbonHelper.AddButton(addItemButton, CommandManager.Instance.GetCommand(CommandNames.AddTrainingModule));
                RibbonHelper.AddButton(addItemButton, CommandManager.Instance.GetCommand(CommandNames.AddInTestModule));
                RibbonHelper.AddButton(addItemButton, CommandManager.Instance.GetCommand(CommandNames.AddOutTestModule));
                RibbonHelper.AddButton(addItemButton, CommandManager.Instance.GetCommand(CommandNames.AddTestModuleFromOuterCourse));
               // RibbonHelper.AddButton(addItemButton, CommandManager.Instance.GetCommand(CommandNames.LoadFromImsQti));
            }

            if (cn is TestModule)
            {
                addItemButton.DropDownItems.Clear();
                RibbonHelper.AddButton(addItemButton, CommandManager.Instance.GetCommand(CommandNames.AddGroup));
                RibbonHelper.AddButton(addItemButton, CommandManager.Instance.GetCommand(CommandNames.AddChoiceQuestion));
                RibbonHelper.AddButton(addItemButton, CommandManager.Instance.GetCommand(CommandNames.AddMultichoiceQuestion));
                RibbonHelper.AddButton(addItemButton, CommandManager.Instance.GetCommand(CommandNames.AddOrderingQuestion));
                RibbonHelper.AddButton(addItemButton, CommandManager.Instance.GetCommand(CommandNames.AddOpenQuestion));
                RibbonHelper.AddButton(addItemButton, CommandManager.Instance.GetCommand(CommandNames.AddCorrespondenceQuestion));
                RibbonHelper.AddButton(addItemButton, CommandManager.Instance.GetCommand(CommandNames.AddOuterQuestion));
                RibbonHelper.AddButton(addItemButton, CommandManager.Instance.GetCommand(CommandNames.AddQuestionFromOuterCourse));
                RibbonHelper.AddButton(addItemButton, CommandManager.Instance.GetCommand(CommandNames.AddInteractiveQuestion));
               // RibbonHelper.AddButton(addItemButton, CommandManager.Instance.GetCommand(CommandNames.LoadFromImsQti));
            }

            if (cn is Group)
            {
                addItemButton.DropDownItems.Clear();
                RibbonHelper.AddButton(addItemButton, CommandManager.Instance.GetCommand(CommandNames.AddChoiceQuestion));
                RibbonHelper.AddButton(addItemButton, CommandManager.Instance.GetCommand(CommandNames.AddMultichoiceQuestion));
                RibbonHelper.AddButton(addItemButton, CommandManager.Instance.GetCommand(CommandNames.AddOrderingQuestion));
                RibbonHelper.AddButton(addItemButton, CommandManager.Instance.GetCommand(CommandNames.AddOpenQuestion));
                RibbonHelper.AddButton(addItemButton, CommandManager.Instance.GetCommand(CommandNames.AddCorrespondenceQuestion));
                RibbonHelper.AddButton(addItemButton, CommandManager.Instance.GetCommand(CommandNames.AddOuterQuestion));
                RibbonHelper.AddButton(addItemButton, CommandManager.Instance.GetCommand(CommandNames.AddQuestionFromOuterCourse));
                RibbonHelper.AddButton(addItemButton, CommandManager.Instance.GetCommand(CommandNames.AddInteractiveQuestion));
              //  RibbonHelper.AddButton(addItemButton, CommandManager.Instance.GetCommand(CommandNames.LoadFromImsQti));
            }

            if (cn is Question)
            {
                addItemButton.DropDownItems.Clear();
                RibbonHelper.AddButton(addItemButton, CommandManager.Instance.GetCommand(CommandNames.AddResponse));
            }
        }

        #endregion

    }
}