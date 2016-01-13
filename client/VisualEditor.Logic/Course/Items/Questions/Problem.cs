namespace VisualEditor.Logic.Course.Items.Questions
{
    internal class Problem
    {
        /// <summary>
        /// Идентификатор задачи.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Название задачи.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Сложность задачи.
        /// </summary>
        public string Weight { get; set; }

        /// <summary>
        /// Описание задачи.
        /// </summary>
        public string Declaration { get; set; }

        /// <summary>
        /// Тема, к которой относится задача.
        /// </summary>
        public string Task_number { get; set; }

        /// <summary>
        /// Адрес сервиса.
        /// </summary>
        public string Url { get; set; }
    }
}