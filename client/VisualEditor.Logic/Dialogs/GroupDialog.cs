using System;
using System.Collections.Generic;
using VisualEditor.Logic.Course.Items;

namespace VisualEditor.Logic.Dialogs
{
    internal partial class GroupDialog : DialogBase
    {
        private int timeRestriction;
        private int chosenQuestionsCount;

        public GroupDialog()
        {
            InitializeComponent();
            InitializeDialog();
        }
        
        public void InitializeDialog()
        {
            FillProfiles();
            questionsCountTextBox.Select();
        }

        #region FillProfiles

        private void FillProfiles()
        {
            var g = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as Group;
            var tm = g.Parent as TestModule;

            // Контроль в учебном курсе.
            if (g.Parent.Parent is CourseRoot)
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
            if (g.Parent.Parent is TrainingModule)
            {
                var trm = g.Parent.Parent as TrainingModule;
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

        private void okButton_Click(object sender, EventArgs e)
        {
            var g = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as Group;
            g.TimeRestriction = timeRestriction;

            foreach (Concept c in Warehouse.Warehouse.Instance.ConceptTree.Nodes)
            {
                if (c.Text.Equals(profileComboBox.Text))
                {
                    g.Profile = c;

                    break;
                }
            }

            g.Marks = (int)marksUpDown.Value;
            g.ChosenQuestionsCount = (int)questionsCountUpDown.Value;

            foreach (var q in g.Questions)
            {
                q.TimeRestriction = g.TimeRestriction;
                q.Profile = g.Profile;
                q.Marks = g.Marks;
            }

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

        private void questionsCountUpDown_DownButtonClicked(object sender, EventArgs e)
        {
            if (chosenQuestionsCount < 1)
            {
                return;
            }

            chosenQuestionsCount--;
            RefreshChosenQuestionsCount();
            CheckState();
        }

        private void questionsCountUpDown_UpButtonClicked(object sender, EventArgs e)
        {
            var g = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as Group;

            if (chosenQuestionsCount == g.Questions.Count)
            {
                return;
            }

            chosenQuestionsCount++;
            RefreshChosenQuestionsCount();
            CheckState();
        }

        private void percentUpDown_DownButtonClicked(object sender, EventArgs e)
        {
            if (percentUpDown.Value < 1)
            {
                return;
            }

            percentUpDown.Value--;
            questionsCountUpDown.Value = ConvertPercentToNumber((int)percentUpDown.Value);
            chosenQuestionsCount = (int)questionsCountUpDown.Value;
            CheckState();
        }

        private void percentUpDown_UpButtonClicked(object sender, EventArgs e)
        {
            if (percentUpDown.Value.Equals(100))
            {
                return;
            }

            percentUpDown.Value++;
            questionsCountUpDown.Value = ConvertPercentToNumber((int)percentUpDown.Value);
            chosenQuestionsCount = (int)questionsCountUpDown.Value;
            CheckState();
        }

        #region ConvertNumberToPercent
        
        private int ConvertNumberToPercent(int number)
        {
            var g = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as Group;
            var questionsCount = g.Questions.Count;

            if (questionsCount != 0)
            {
                return (int)(number * 100) / questionsCount;
            }

            return 0;
        }

        #endregion

        #region ConvertPercentToNumber
        
        private decimal ConvertPercentToNumber(int percent)
        {
            var g = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as Group;
            var questionsCount = g.Questions.Count;

            return Convert.ToDecimal((percent * questionsCount) / 100);
        }

        #endregion

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

        #region RefreshChosenQuestionsCount

        private void RefreshChosenQuestionsCount()
        {
            questionsCountUpDown.Value = Convert.ToDecimal(chosenQuestionsCount);
            percentUpDown.Value = Convert.ToDecimal(ConvertNumberToPercent(chosenQuestionsCount));
        }

        #endregion

        private void questionsCountRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (questionsCountRadioButton.Checked)
            {
                percentUpDown.Enabled = false;
                questionsCountUpDown.Enabled = true;
            }
        }

        private void percentRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (percentRadioButton.Checked)
            {
                questionsCountUpDown.Enabled = false;
                percentUpDown.Enabled = true;
            }
        }

        #region InitializeData

        public void InitializeData()
        {
            var g = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as Group;

            questionsCountTextBox.Text = g.Questions.Count.ToString();

            if (!g.Questions.Count.Equals(0))
            {
                var q = g.Questions[0] as Question;

                timeRestriction = q.TimeRestriction;
                RefreshTimeRestriction();
                timeRestrictionCheckBox.Checked = minutesUpDown.Enabled =
                secondsUpDown.Enabled = !timeRestriction.Equals(0);

                if (q.Profile != null)
                {
                    profileComboBox.Text = q.Profile.Text;
                }

                marksUpDown.Value = Convert.ToDecimal(q.Marks);

                questionsCountUpDown.Maximum = g.Questions.Count;
                chosenQuestionsCount = g.ChosenQuestionsCount;
                RefreshChosenQuestionsCount();
            }
            else
            {
                timeRestriction = g.TimeRestriction;
                RefreshTimeRestriction();
                timeRestrictionCheckBox.Checked = minutesUpDown.Enabled =
                secondsUpDown.Enabled = !timeRestriction.Equals(0);

                if (g.Profile != null)
                {
                    profileComboBox.Text = g.Profile.Text;
                }

                marksUpDown.Value = Convert.ToDecimal(g.Marks);

                questionsCountUpDown.Maximum = g.Questions.Count;
                chosenQuestionsCount = g.ChosenQuestionsCount;
                RefreshChosenQuestionsCount();
            }

            CheckState();
        }

        #endregion

        private void CheckState()
        {
            if (!profileComboBox.Text.Equals(string.Empty) &&
                chosenQuestionsCount != 0)
            {
                okButton.Enabled = true;
            }
            else
            {
                okButton.Enabled = false;
            }
        }

        private void profileComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckState();
        }
    }
}