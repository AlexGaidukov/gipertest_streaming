using VisualEditor.Logic.IO.Questions;

namespace VisualEditor.Logic.Course.Items.Questions
{
    internal class OuterQuestion : Question
    {
        public override void WriteQti(string fileName)
        { }

        public override bool ReadQti(string qfPath)
        { return true; }

        public OuterQuestion()
        {
            XmlWriter = new OuterQuestionXmlWriter(this);
            XmlReader = new OuterQuestionXmlReader(this);
        }

        /// <summary>
        /// Идентификатор задачи.
        /// </summary>
        public string TaskId { get; set; }

        /// <summary>
        /// Название задачи.
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// Название темы.
        /// </summary>
        public string TestName { get; set; }

        /// <summary>
        /// Наименование дисциплины.
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// Описание задачи.
        /// </summary>
        public string Declaration { get; set; }

        /// <summary>
        /// Адрес сервиса (внешнего вопроса).
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Идентификатор теста.
        /// </summary>
        public string TestId { get; set; }
    }
}