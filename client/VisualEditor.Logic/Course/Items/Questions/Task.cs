namespace VisualEditor.Logic.Course.Items.Questions
{
    internal class Task
    {
        /// <summary>
        /// Идентификатор темы.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Название темы.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Дисциплина, к которой относится тема.
        /// </summary>
        public string Externaltest_number { get; set; }

        /// <summary>
        /// Номер темы.
        /// </summary>
        public string Task_number { get; set; }

    }
}