using System;
using System.Windows.Forms;
using VisualEditor.Logic.Commands;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Course.Items.Questions;
using VisualEditor.Logic.Course.Preview;
using VisualEditor.Logic.Helpers;
using VisualEditor.Utils.Controls.HtmlEditing;

namespace VisualEditor.Logic.Dialogs
{
    internal partial class NetQuestionDialog : DialogBase
    {
        private int timeRestriction;

        public NetQuestionDialog()
        {
            InitializeComponent();
            InitializeDialog();
        }

        #region InitializeDialog

        public void InitializeDialog()
        {
            var q_ = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as Question;
            FillProfiles(q_);
            FillNextQuestions(q_);

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

        #endregion

        #region FillProfiles

        private void FillProfiles(Question q)
        {
            // Вопрос в контроле.
            if (q.Parent is TestModule)
            {
                var tm = q.Parent as TestModule;

                // Контроль в учебном курсе.
                if (tm.Parent is CourseRoot)
                {
                    // Входной контроль.
                    if (tm.TestType.Equals(Enums.TestType.InTest))
                    {
                        foreach (Concept c in Warehouse.Warehouse.Instance.ConceptTree.Nodes)
                        {
                            if (c.IsProfile && c.InDummyConcepts.Count > 0)
                            {
                                profileComboBox.Items.Add(c.Text);
                            }
                        }
                    }
                    // Выходной контроль.
                    else
                    {
                        foreach (Concept c in Warehouse.Warehouse.Instance.ConceptTree.Nodes)
                        {
                            if (c.IsProfile && c.OutDummyConcept != null)
                            {
                                profileComboBox.Items.Add(c.Text);
                            }
                        }
                    }
                }

                // Контроль в учебном модуле.
                if (tm.Parent is TrainingModule)
                {
                    var trm = tm.Parent as TrainingModule;

                    // Входной контроль.
                    if (tm.TestType.Equals(Enums.TestType.InTest))
                    {
                        foreach (var idc in trm.InConceptParent.InDummyConcepts)
                        {
                            if (idc.Concept.IsProfile)
                            {
                                profileComboBox.Items.Add(idc.Concept.Text);
                            }
                        }
                    }
                    // Выходной контроль.
                    else
                    {
                        foreach (var odc in trm.OutConceptParent.OutDummyConcepts)
                        {
                            if (odc.Concept.IsProfile)
                            {
                                profileComboBox.Items.Add(odc.Concept.Text);
                            }
                        }
                    }
                }
            }

            // Вопрос в группе.
            if (q.Parent is Group)
            {
                var tm = q.Parent.Parent as TestModule;

                // Контроль в учебном курсе.
                if (tm.Parent is CourseRoot)
                {
                    // Входной контроль.
                    if (tm.TestType.Equals(Enums.TestType.InTest))
                    {
                        foreach (Concept c in Warehouse.Warehouse.Instance.ConceptTree.Nodes)
                        {
                            if (c.IsProfile && c.InDummyConcepts.Count > 0)
                            {
                                profileComboBox.Items.Add(c.Text);
                            }
                        }
                    }
                    // Выходной контроль.
                    else
                    {
                        foreach (Concept c in Warehouse.Warehouse.Instance.ConceptTree.Nodes)
                        {
                            if (c.IsProfile && c.OutDummyConcept != null)
                            {
                                profileComboBox.Items.Add(c.Text);
                            }
                        }
                    }
                }

                // Контроль в учебном модуле.
                if (tm.Parent is TrainingModule)
                {
                    var trm = tm.Parent as TrainingModule;

                    // Входной контроль.
                    if (tm.TestType.Equals(Enums.TestType.InTest))
                    {
                        foreach (var idc in trm.InConceptParent.InDummyConcepts)
                        {
                            if (idc.Concept.IsProfile)
                            {
                                profileComboBox.Items.Add(idc.Concept.Text);
                            }
                        }
                    }
                    // Выходной контроль.
                    else
                    {
                        foreach (var odc in trm.OutConceptParent.OutDummyConcepts)
                        {
                            if (odc.Concept.IsProfile)
                            {
                                profileComboBox.Items.Add(odc.Concept.Text);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region FillNextQuestions

        private void FillNextQuestions(Question q)
        {
            nextQuestionComboBox.Items.Clear();
            nextQuestionComboBox.Items.Add("");

            var tm = q.Parent as TestModule;
            foreach (var question in tm.Questions)
            {
                if (!question.Id.Equals(q.Id))
                {
                    nextQuestionComboBox.Items.Add(question.Text);
                }
            }

            nextQuestionComboBox.Text = q.NextQuestion == null ? "" : q.NextQuestion.Text;
            int rr = 1;
        }

        #endregion

        #region GetNextQuestion

        private Question GetNextQuestion()
        {
            var tm = Warehouse.Warehouse.Instance.CourseTree.CurrentNode.Parent as TestModule;

            foreach (var q in tm.Questions)
            {
                if (nextQuestionComboBox.Text.Equals(q.Text))
                {
                    return q;
                }
            }

            return null;
        }

        #endregion

        private void hintButton_Click(object sender, EventArgs e)
        {
            Renderer.Instance.StopRendering();
            Renderer.Instance.StartHintDialogRendering();

            if (HintDialog.Instance.ShowDialog(this) == DialogResult.OK)
            {
                var q = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as Question;
                q.Hint = HintDialog.Instance.Hint;
            }

            Renderer.Instance.StartRendering();
            Renderer.Instance.StopHintDialogRendering();
        }

        private void resposeVariantsButton_Click(object sender, EventArgs e)
        {
            using (var rvd = new NetResponseVariantDialog())
            {
                if (rvd.ShowDialog(this).Equals(DialogResult.OK))
                {

                }
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            SaveQuestionInfo();
            Warehouse.Warehouse.IsProjectModified = true;
            DialogResult = DialogResult.OK;
            int tr = 0;
        }

        private void minutesUpDown_DownButtonClicked(object sender, EventArgs e)
        {
            if (timeRestriction < 60)
            {
                return;
            }

            timeRestriction = (int)(minutesUpDown.Value * 60 + secondsUpDown.Value);

            timeRestriction -= 60;
            RefreshTimeRestriction();
        }

        private void minutesUpDown_UpButtonClicked(object sender, EventArgs e)
        {
            timeRestriction = (int)(minutesUpDown.Value * 60 + secondsUpDown.Value);

            timeRestriction += 60;
            RefreshTimeRestriction();
        }

        private void secondsUpDown_DownButtonClicked(object sender, EventArgs e)
        {
            if (timeRestriction < 1)
            {
                return;
            }

            timeRestriction = (int)(minutesUpDown.Value * 60 + secondsUpDown.Value);

            timeRestriction--;
            RefreshTimeRestriction();
        }

        private void secondsUpDown_UpButtonClicked(object sender, EventArgs e)
        {
            timeRestriction = (int)(minutesUpDown.Value * 60 + secondsUpDown.Value);

            timeRestriction++;
            RefreshTimeRestriction();
        }

        private void timeRestrictionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (timeRestrictionCheckBox.Checked)
            {
                minutesUpDown.Enabled = true;
                secondsUpDown.Enabled = true;
            }
            else
            {
                minutesUpDown.Enabled = false;
                secondsUpDown.Enabled = false;
                timeRestriction = 0;
                RefreshTimeRestriction();
            }
        }

        #region RefreshTimeRestriction

        private void RefreshTimeRestriction()
        {
            var ms = timeRestriction / 59;
            var ss = timeRestriction - ms * 60;
            if (ss < 0)
            {
                ss = 59 + ss + 1;
                ms -= 1;
            }

            minutesUpDown.Value = Convert.ToDecimal(ms);
            secondsUpDown.Value = Convert.ToDecimal(ss);
        }

        #endregion

        #region InitializeData

        public void InitializeData(Question q)
        {
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

            timeRestriction = q.TimeRestriction;
            RefreshTimeRestriction();
            timeRestrictionCheckBox.Checked = minutesUpDown.Enabled =
            secondsUpDown.Enabled = !timeRestriction.Equals(0);

            if (q.Profile != null)
            {
                profileComboBox.Text = q.Profile.Text;
            }

            marksUpDown.Value = Convert.ToDecimal(q.Marks);

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

        #region CheckState

        private void CheckState()
        {
            okButton.Enabled = !profileComboBox.Text.Equals(string.Empty);
        }

        #endregion

        private void profileComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckState();
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

        private void nextQuestionButton_Click(object sender, EventArgs e)
        {
            var q = GetNextQuestion();

            if (q != null)
            {
                SaveQuestionInfo();
                var q_ = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as Question;
                q_.NextQuestion = q;
                Warehouse.Warehouse.Instance.CourseTree.CurrentNode = q;
                CommandManager.Instance.GetCommand(CommandNames.ViewDocument).Execute(null);
                InitializeData(q);
                InitializeDialog();
            }
            else
            {
                var q_ = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as Question;
                q_.NextQuestion = null;
            }
        }

        #region SaveQuestionInfo

        private void SaveQuestionInfo()
        {
            var q = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as Question;

            q.TimeRestriction = timeRestriction;

            foreach (Concept c in Warehouse.Warehouse.Instance.ConceptTree.Nodes)
            {
                if (c.Text.Equals(profileComboBox.Text))
                {
                    q.Profile = c;

                    break;
                }
            }

            q.Marks = (int)marksUpDown.Value;
        }

        #endregion

        #region Предварительный просмотр вопроса

        private Form previewDialog;

        private void viewNextQuestionButton_Click(object sender, EventArgs e)
        {
            var q = GetNextQuestion();

            if (q != null)
            {
                previewDialog = new Form
                {
                    StartPosition = FormStartPosition.CenterParent,
                    Width = 600,
                    Height = 500,
                    KeyPreview = true,
                    MaximizeBox = false,
                    MinimizeBox = false,
                    ShowIcon = false,
                    ShowInTaskbar = false,
                    Text = "Предварительный просмотр вопроса"
                };

                var htmlEditingTool = new HtmlEditingTool
                {
                    Dock = DockStyle.Fill,
                    Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview
                };

                Logic.Controls.HtmlEditing.HtmlEditingToolHelper.SetDefaultDocumentHtml(htmlEditingTool);
                htmlEditingTool.KeyDown += vhe_KeyDown;
                previewDialog.Controls.Add(htmlEditingTool);

                PreviewConverter.ConvertQuestionDocument(htmlEditingTool, q);

                previewDialog.ShowDialog(this);
            }
        }

        void vhe_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Escape))
            {
                previewDialog.Close();
                previewDialog = null;
            }
        }

        #endregion

        private void nextQuestionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            viewNextQuestionButton.Enabled = !nextQuestionComboBox.Text.Equals("");
        }
    }
}