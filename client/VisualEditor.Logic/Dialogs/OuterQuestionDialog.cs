using System.Collections.Generic;
using VisualEditor.Logic.Course.Items.Questions;

namespace VisualEditor.Logic.Dialogs
{
    internal partial class OuterQuestionDialog : DialogBase
    {
        private ServiceInfo serviceInfo;
        private bool isAuthorBlocked;
        private bool isSubjectBlocked;

        public OuterQuestionDialog()
        {
            InitializeComponent();
        }

        public void InitializeData(ServiceInfo si)
        {
            serviceInfo = si;

            authorComboBox.Items.Add("");
            var authors = serviceInfo.GetAuthors(subjectComboBox.Text);
            for (var i = 0; i < authors.Count; i++)
            {
                authorComboBox.Items.Add(serviceInfo.GetAuthors(subjectComboBox.Text)[i]);
            }
            authorComboBox.SelectedIndex = 0;

            subjectComboBox.Items.Add("");
            var subjects = serviceInfo.GetSubjects(authorComboBox.Text);
            for (var i = 0; i < subjects.Count; i++)
            {
                subjectComboBox.Items.Add(serviceInfo.GetSubjects(authorComboBox.Text)[i]);
            }
            subjectComboBox.SelectedIndex = 0;

            isAuthorBlocked = false;
            isSubjectBlocked = false;
        }

        private void okButton_Click(object sender, System.EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void authorComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {/*
            if (!isAuthorBlocked && isSubjectBlocked)
            {
                return;
            }

            if (authorComboBox.Text.Equals(string.Empty))
            {
                isAuthorBlocked = false;
                authorComboBox.Items.Clear();
                authorComboBox.Items.Add("");
                var authors = serviceInfo.GetAuthors(subjectComboBox.Text);
                for (var i = 0; i < authors.Count; i++)
                {
                    authorComboBox.Items.Add(serviceInfo.GetAuthors(subjectComboBox.Text)[i]);
                }
            }
            else
            {
                isAuthorBlocked = true;
            }

            subjectComboBox.Items.Clear();
            subjectComboBox.Items.Add("");

            var subjects = serviceInfo.GetSubjects(authorComboBox.Text);
            for (var i = 0; i < subjects.Count; i++)
            {
                subjectComboBox.Items.Add(subjects[i]);
            }
            */
            questionListBox.Items.Clear();
            FillQuestionList(serviceInfo.GetQuestions(subjectComboBox.Text, authorComboBox.Text));
        }

        private void subjectComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            /*
            if (isAuthorBlocked && !isSubjectBlocked)
            {
                return;
            }
            */
            if (subjectComboBox.Text.Equals(string.Empty))
            {
                isSubjectBlocked = false;
                subjectComboBox.Items.Clear();
                subjectComboBox.Items.Add("");
                var subjects = serviceInfo.GetSubjects(authorComboBox.Text);
                for (var i = 0; i < subjects.Count; i++)
                {
                    subjectComboBox.Items.Add(serviceInfo.GetSubjects(authorComboBox.Text)[i]);
                }
            }
            else
            {
                isSubjectBlocked = true;
            }

            authorComboBox.Items.Clear();
            authorComboBox.Items.Add("");

            var authors = serviceInfo.GetAuthors(subjectComboBox.Text);
            for (var i = 0; i < authors.Count; i++)
            {
                authorComboBox.Items.Add(authors[i]);
            }

            questionListBox.Items.Clear();
            FillQuestionList(serviceInfo.GetQuestions(subjectComboBox.Text, authorComboBox.Text));
        }

        #region FillQuestionList
        
        /// <summary>
        /// Вывод списка вопросов.
        /// </summary>
        /// <param name="questionList">Список вопросов.</param>
        private void FillQuestionList(List<string> questionList)
        {
            for (var i = 0; i < questionList.Count; i++)
            {
                questionListBox.Items.Add(questionList[i]);
            }
        }

        #endregion

        #region GetSelectedItems
        
        /// <summary>
        /// Возвращает список выделенных задач.
        /// </summary>
        public List<string> GetSelectedQuestions()
        {
            var questions = new List<string>();

            for (var i = 0; i < questionListBox.CheckedItems.Count; i++)
            {
                questions.Add(questionListBox.CheckedItems[i].ToString());
            }

            return questions;
        }

        #endregion
    }
}