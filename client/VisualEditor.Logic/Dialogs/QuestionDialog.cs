using System;
using System.Collections.Generic;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Course.Items.Questions;
using VisualEditor.Logic.Helpers;

namespace VisualEditor.Logic.Dialogs
{
    internal partial class QuestionDialog : DialogBase
    {
        private int timeRestriction;

        public QuestionDialog()
        {
            InitializeComponent();
            InitializeDialog();
        }

        public void InitializeDialog()
        {
            FillProfiles();

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

        #region FillProfiles

        private void FillProfiles()
        {
            var q = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as Question;

            #region Вопрос в контроле
            
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
                    var conceptNames = new List<string>();

                    // Входной контроль.
                    if (tm.TestType.Equals(Enums.TestType.InTest))
                    {
                        WalkTrainingModulesForInConcepts(trm, conceptNames);

                        foreach (var idc in trm.InConceptParent.InDummyConcepts)
                        {
                            if (idc.Concept.IsProfile)
                            {
                                profileComboBox.Items.Add(idc.Concept.Text);
                            }
                        }

                        profileComboBox.Items.AddRange(conceptNames.ToArray());
                    }
                    // Выходной контроль.
                    else
                    {
                        // Collects concept names recursively in all TMs of current TM and in it itself.
                        WalkTrainingModulesForOutConcepts(trm, conceptNames);

                        foreach (var odc in trm.OutConceptParent.OutDummyConcepts)
                        {
                            if (odc.Concept.IsProfile)
                            {
                                profileComboBox.Items.Add(odc.Concept.Text);
                            }
                        }

                        profileComboBox.Items.AddRange(conceptNames.ToArray());
                    }
                }
            }

            #endregion

            #region Вопрос в группе
            
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

            #endregion
        }

        #endregion

        #region WalkTrainingModulesForOutConcepts

        private static void WalkTrainingModulesForOutConcepts(TrainingModule trainingModule, List<string> conceptNames)
        {
            for (var i = 0; i < trainingModule.TrainingModules.Count; i++)
            {
                var tm = trainingModule.TrainingModules[i];
                if (tm.TrainingModules.Count != 0)
                {
                    WalkTrainingModulesForOutConcepts(tm, conceptNames);
                }

                foreach (var odc in trainingModule.TrainingModules[i].OutConceptParent.OutDummyConcepts)
                {
                    if (odc.Concept.IsProfile)
                    {
                        conceptNames.Add(odc.Concept.Text);
                    }
                }
            }
        }

        #endregion

        #region WalkTrainingModulesForInConcepts
        
        private static void WalkTrainingModulesForInConcepts(TrainingModule trainingModule, List<string> conceptNames)
        {
            for (var i = 0; i < trainingModule.TrainingModules.Count; i++)
            {
                var tm = trainingModule.TrainingModules[i];
                if (tm.TrainingModules.Count != 0)
                {
                    WalkTrainingModulesForInConcepts(tm, conceptNames);
                }

                foreach (var idc in trainingModule.TrainingModules[i].InConceptParent.InDummyConcepts)
                {
                    if (idc.Concept.IsProfile)
                    {
                        conceptNames.Add(idc.Concept.Text);
                    }
                }
            }
        }

        #endregion

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

            Warehouse.Warehouse.IsProjectModified = true;
            DialogResult = System.Windows.Forms.DialogResult.OK;
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

        private void CheckState()
        {
            okButton.Enabled = !profileComboBox.Text.Equals(string.Empty);
        }

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
    }
}