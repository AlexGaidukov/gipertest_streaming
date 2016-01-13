using System;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Course.Items.Questions;
using VisualEditor.Logic.Helpers;

namespace VisualEditor.Logic.Dialogs
{
    internal partial class QuestionInGroupDialog : DialogBase
    {
        public QuestionInGroupDialog()
        {
            InitializeComponent();
            InitializeDialog();
        }

        public void InitializeDialog()
        {
            #region Делает активным чекбокс Тип ответа число
            
            var q = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as OpenQuestion;
            if (q != null)
            {
                responseIsNumberCheckBox.Enabled = true;

                if (q.IsAnswerNumeric)
                {
                    responseIsNumberCheckBox.Checked = true;
                }
            }

            #endregion

            questionTypeTextBox.Select();
        }

        private void hintButton_Click(object sender, EventArgs e)
        {
            Renderer.Instance.StopRendering();
            Renderer.Instance.StartHintDialogRendering();

            if (HintDialog.Instance.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                var q = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as Question;
                q.Hint = HintDialog.Instance.Hint;
            }

            Renderer.Instance.StartRendering();
            Renderer.Instance.StopHintDialogRendering();
        }

        private void resposeVariantsButton_Click(object sender, EventArgs e)
        {
            using (var rvd = new ResponseVariantDialog())
            {
                if (rvd.ShowDialog(this).Equals(System.Windows.Forms.DialogResult.OK))
                {

                }
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Warehouse.Warehouse.IsProjectModified = true;
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        #region InitializeData

        public void InitializeData()
        {
            var q = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as Question;

            #region Тип вопроса
            
            if (q is ChoiceQuestion)
            {
                questionTypeTextBox.Text = "Одновариантный выбор";
            }

            if (q is MultichoiceQuestion)
            {
                questionTypeTextBox.Text = "Множественный выбор";
            }

            if (q is InteractiveQuestion)
            {
                questionTypeTextBox.Text = "Интерактивная картинка";
            }

            if (q is OrderingQuestion)
            {
                questionTypeTextBox.Text = "Ранжирование";
            }

            if (q is OpenQuestion)
            {
                questionTypeTextBox.Text = "Открытый вопрос";
            }

            if (q is CorrespondenceQuestion)
            {
                questionTypeTextBox.Text = "Парное соответствие";
            }

            if (q is OuterQuestion)
            {
                questionTypeTextBox.Text = "Внешний вопрос";
            }

            #endregion

            #region Кнопка Редактировать подсказку
            
            if (q.Parent is TestModule)
            {
                var tm = q.Parent as TestModule;

                hintButton.Enabled = tm.Trainer;
            }

            if (q.Parent is Group)
            {
                var tm = q.Parent.Parent as TestModule;

                hintButton.Enabled = tm.Trainer;
            }

            #endregion

            CheckState();
        }

        #endregion

        private void CheckState()
        {

        }

        private void responseIsNumberCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var q = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as OpenQuestion;

            if (responseIsNumberCheckBox.Checked)
            {
                q.IsAnswerNumeric = true;
            }
            else
            {
                q.IsAnswerNumeric = false;
            }
        }
    }
}