using System;
using VisualEditor.Logic.Course.Items;

namespace VisualEditor.Logic.Dialogs
{
    internal partial class TestModuleDialog : DialogBase
    {
        private int timeRestriction;

        public TestModuleDialog()
        {
            InitializeComponent();
            InitializeDialog();
        }

        public void InitializeDialog()
        {
            questionSequenceComboBox.Text = "случайная";
            mistakesNumberCheckBox.Select();
        }

        private void mistakesNumberCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (mistakesNumberCheckBox.Checked)
            {
                mistakesNumberUpDown.Enabled = true;
            }
            else
            {
                mistakesNumberUpDown.Enabled = false;
                mistakesNumberUpDown.Value = 0;
            }
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

        private void okButton_Click(object sender, EventArgs e)
        {
            var tm = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as TestModule;
            
            tm.MistakesNumber = (int)mistakesNumberUpDown.Value;
            
            tm.TimeRestriction = timeRestriction;
            
            tm.Trainer = trainerCheckBox.Checked;

            var qs = questionSequenceComboBox.Text;
            if (qs.Equals("естественная"))
            {
                tm.QuestionSequence = Enums.QuestionSequence.Natural;
            }
            else if (qs.Equals("случайная"))
            {
                tm.QuestionSequence = Enums.QuestionSequence.Random;
            }
            else if (qs.Equals("сетевая"))
            {
                tm.QuestionSequence = Enums.QuestionSequence.Network;
            }

            Warehouse.Warehouse.IsProjectModified = true;
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void secondsUpDown_DownButtonClicked(object sender, EventArgs e)
        {
            if (timeRestriction < 1)
            {
                return;
            }

            timeRestriction = (int)(minutesUpDown.Value * 60 + secondsUpDown.Value);

            timeRestriction -=1;// (int)(secondsUpDown.Value);
            RefreshTimeRestriction();
        }

        private void secondsUpDown_UpButtonClicked(object sender, EventArgs e)
        {
            timeRestriction = (int)(minutesUpDown.Value * 60 + secondsUpDown.Value);

            timeRestriction +=1;// (int)(secondsUpDown.Value);
            RefreshTimeRestriction();
        }

        private void minutesUpDown_DownButtonClicked(object sender, EventArgs e)
        {
            if (timeRestriction < 60)
            {
                return;
            }

            timeRestriction = (int)(minutesUpDown.Value * 60 + secondsUpDown.Value);

            timeRestriction -= 60;// (int)(minutesUpDown.Value * 60);
            RefreshTimeRestriction();
        }

        private void minutesUpDown_UpButtonClicked(object sender, EventArgs e)
        {
            timeRestriction = (int)(minutesUpDown.Value * 60 + secondsUpDown.Value);

            timeRestriction += 60;// (int)(minutesUpDown.Value * 60);
            RefreshTimeRestriction();
        }

        #region RefreshTimeRestriction
        
        private void RefreshTimeRestriction()
        {
            var ms = timeRestriction / 60;
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
            var tm = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as TestModule;

            mistakesNumberCheckBox.Checked = mistakesNumberUpDown.Enabled = !tm.MistakesNumber.Equals(0);
            mistakesNumberUpDown.Value = tm.MistakesNumber;

            timeRestriction = tm.TimeRestriction;
            RefreshTimeRestriction();
            timeRestrictionCheckBox.Checked = minutesUpDown.Enabled =
            secondsUpDown.Enabled = !timeRestriction.Equals(0);

            trainerCheckBox.Checked = tm.Trainer;

            #region Если в контроле есть группы, то последовательность вопросов случайная
            
            if (!tm.Groups.Count.Equals(0))
            {
                tm.QuestionSequence = Enums.QuestionSequence.Random;
                questionSequenceComboBox.Text = "случайная";
                questionSequenceComboBox.Enabled = false;

                return;
            }

            #endregion

            var qs = tm.QuestionSequence.ToString();
            if (qs.Equals("Natural"))
            {
                questionSequenceComboBox.Text = "естественная";
            }
            else if (qs.Equals("Random"))
            {
                questionSequenceComboBox.Text = "случайная";
            }
            else if (qs.Equals("Network"))
            {
                questionSequenceComboBox.Text = "сетевая";
            }
        }

        #endregion
    }
}