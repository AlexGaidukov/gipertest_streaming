using System.Windows.Forms;
using VisualEditor.Logic.Controls;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Dialogs;

namespace VisualEditor.Logic.Commands.Course
{
    internal class ItemOptionsSmall : AbstractCommand
    {
        public ItemOptionsSmall()
        {
            name = CommandNames.ItemOptionsSmall;
            text = CommandTexts.ItemOptionsSmall;
            image = Properties.Resources.ItemOptionsSmall;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            #region Свойства контроля
            
            if (Warehouse.Warehouse.Instance.CourseTree.CurrentNode is TestModule)
            {
                using (var tmd = new TestModuleDialog())
                {
                    tmd.InitializeData();

                    if (tmd.ShowDialog(MainForm.Instance).Equals(DialogResult.OK))
                    {

                    }
                }
            }

            #endregion

            #region Свойства группы
            
            if (Warehouse.Warehouse.Instance.CourseTree.CurrentNode is Group)
            {
                using (var gd = new GroupDialog())
                {
                    gd.InitializeData();

                    if (gd.ShowDialog(MainForm.Instance).Equals(DialogResult.OK))
                    {

                    }
                }
            }

            #endregion

            #region Свойства вопроса
            
            if (Warehouse.Warehouse.Instance.CourseTree.CurrentNode is Question)
            {
                var q = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as Question;

                if (!(q.Parent is Group))
                {
                    #region Вопрос в контроле

                    if (!(q.Parent as TestModule).QuestionSequence.Equals(Enums.QuestionSequence.Network))
                    {
                        using (var qd = new QuestionDialog())
                        {
                            qd.InitializeData();

                            if (qd.ShowDialog(MainForm.Instance).Equals(DialogResult.OK))
                            {

                            }
                        }
                    }
                    else
                    {
                        using (var qd = new NetQuestionDialog())
                        {
                            qd.InitializeData(q);

                            if (qd.ShowDialog(MainForm.Instance).Equals(DialogResult.OK))
                            {

                            }
                        }
                    }

                    #endregion
                }
                else
                {
                    #region Вопрос в группе

                    using (var qigd = new QuestionInGroupDialog())
                    {
                        qigd.InitializeData();

                        if (qigd.ShowDialog(MainForm.Instance).Equals(DialogResult.OK))
                        {

                        }
                    }

                    #endregion
                }
            }

            #endregion
        }
    }
}