namespace VisualEditor.Logic.IO
{
    internal class TagPair
    {
        public TagPair()
        {
            OpenTagStartIndex = -1;
            OpenTagLastIndex = -1;
            CloseTagStartIndex = -1;
            CloseTagLastIndex = -1;
            OpenTag = string.Empty;
            CloseTag = string.Empty;
        }

        #region Свойства

        /// <summary>
        /// Индекс начала открывающего тега (<).
        /// </summary>
        public int OpenTagStartIndex { get; set; }

        /// <summary>
        /// Индекс окончания открывающего тега (>).
        /// </summary>
        public int OpenTagLastIndex { get; set; }

        /// <summary>
        /// Индекс начала закрывающего тега (<).
        /// </summary>
        public int CloseTagStartIndex { get; set; }

        /// <summary>
        /// Индекс окончания закрывающего тега (<).
        /// </summary>
        public int CloseTagLastIndex { get; set; }

        /// <summary>
        /// Открывающий тег.
        /// </summary>
        public string OpenTag { get; set; }

        /// <summary>
        /// Закрывающий тег.
        /// </summary>
        public string CloseTag { get; set; }

        #endregion

        /// <summary>
        /// Метод указывает, содержит ли пара тегов тег с индексом index.
        /// </summary>
        /// <param name="index">Индекс для определения, содержится ли данный тег в паре.</param>
        /// <param name="tag">Тег</param>
        /// <returns></returns>
        public bool Contains(int index, string tag)
        {
            return (index == OpenTagStartIndex && tag.Equals(OpenTag)) |
                   (index == CloseTagStartIndex && tag.Equals(CloseTag));
        }
    }
}