using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace VisualEditor.Logic.IO
{
    internal class MlParser
    {
        private readonly List<TagPair> tagPairs;

        /// <summary>
        /// Html код, в котором осуществляется поиск.
        /// </summary>
        private string html;

        public MlParser()
        {
            tagPairs = new List<TagPair>();
        }

        #region Свойства

        /// <summary>
        /// Границы открывающего тега.
        /// </summary>
        public int OpenTagStartIndex { get; private set; }
        public int OpenTagLastIndex { get; private set; }

        /// <summary>
        /// Границы закрывающего тега.
        /// </summary>
        public int CloseTagStartIndex { get; private set; }
        public int CloseTagLastIndex { get; private set; }

        /// <summary>
        /// Имя тега.
        /// </summary>
        public string TagName { get; private set; }

        #region Индексы из TagStructure

        public int StartIndex
        {
            get { return OpenTagStartIndex; }
        }

        public int LastIndex
        {
            get
            {
                if (CloseTagLastIndex == 0)
                {
                    return OpenTagLastIndex;
                }

                return CloseTagLastIndex;
            }
        }

        #endregion

        #endregion

        #region IndexOf

        /// <summary>
        /// Поиск первого вхождения символа value слева от указанной позиции.
        /// </summary>
        /// <param name="source">Строка для поиска.</param>
        /// <param name="value">Искомый символ.</param>
        /// <param name="startIndex">Индекс, с которого начинается поиск.</param>
        /// <returns>Индекс искомого символа.</returns>
        private static int IndexOf(string source, char value, int startIndex)
        {
            int index;
            for (index = startIndex; index > 0; index--)
            {
                if (source[index].Equals(value))
                {
                    break;
                }
            }

            return index;
        }

        #endregion

        #region GetTagBounds

        /// <summary>
        /// Определение границ тега в html-коде.
        /// </summary>
        /// <param name="htmlText">Представляет собой html-код, в котором осуществляется поиск.</param>
        /// <param name="searchString">Строка для определения тега.</param>
        public void GetTagBounds(string htmlText, string searchString, int startIndex)
        {
            OpenTagStartIndex =
            OpenTagLastIndex =
            CloseTagStartIndex =
            CloseTagLastIndex = 0;
            this.html = htmlText;
            OpenTagStartIndex = htmlText.IndexOf(searchString, startIndex, StringComparison.OrdinalIgnoreCase);
            if (OpenTagStartIndex != -1)
            {
                OpenTagStartIndex = IndexOf(htmlText, '<', OpenTagStartIndex);
                OpenTagLastIndex = htmlText.IndexOf(">", OpenTagStartIndex);

                // Ищет имя тега.
                if ((htmlText.IndexOf(">", OpenTagStartIndex) < htmlText.IndexOf(" ", OpenTagStartIndex) && htmlText.IndexOf(">", OpenTagStartIndex) != -1) ||
                    (htmlText.IndexOf(">", OpenTagStartIndex) != -1 && htmlText.IndexOf(" ", OpenTagStartIndex) == -1))
                {
                    TagName = htmlText.Substring(OpenTagStartIndex + 1, htmlText.IndexOf(">", OpenTagStartIndex) - OpenTagStartIndex - 1);
                }
                else if ((htmlText.IndexOf(">", OpenTagStartIndex) > htmlText.IndexOf(" ", OpenTagStartIndex) && htmlText.IndexOf(" ", OpenTagStartIndex) != -1) ||
                        (htmlText.IndexOf(">", OpenTagStartIndex) == -1 && htmlText.IndexOf(" ", OpenTagStartIndex) != -1))
                {
                    TagName = htmlText.Substring(OpenTagStartIndex + 1, htmlText.IndexOf(" ", OpenTagStartIndex) - OpenTagStartIndex - 1);
                }
            }
            else
            {
                OpenTagStartIndex = 0;
                OpenTagLastIndex = 0;
                TagName = string.Empty;
            }
        }

        public void GetTagBounds(string htmlText, string searchString)
        {
            GetTagBounds(htmlText, searchString, 0);
        }

        #endregion

        #region GetValue

        /// <summary>
        /// Определение значения атрибута.
        /// Возможны следующие способы записи атрибута и значения:
        /// 1. attr=value
        /// 2. attr="value"
        /// </summary>
        /// <param name="attrName">Имя атрибута.</param>
        /// <returns>Значение атрибута.</returns>
        public string GetValue(string attrName)
        {
            int index;
            string value = string.Empty;

            // Если определены границы тега.
            if (OpenTagLastIndex != 0)
            {
                index = html.IndexOf(attrName, OpenTagStartIndex);
                // Если имя атрибута попало в границы тега.
                if (index > OpenTagStartIndex && index < OpenTagLastIndex)
                {
                    // Позиция, следующая за знаком "=".
                    index = html.IndexOf("=", index) + 1;
                    // если после "=" идут пробелы
                    while (html[index].Equals(' '))
                    {
                        index++;
                    }

                    int valueStartIndex = index;
                    char valueStartChar = html[valueStartIndex];

                    for (int i = index + 1; i <= OpenTagLastIndex; i++)
                    {
                        // Если значение атрибута заключено в кавычки.
                        if (html[i] == '"' && valueStartChar == '"')
                        {
                            value = html.Substring(valueStartIndex + 1, i - valueStartIndex - 1);
                            break;
                        }

                        // Если значение атрибута не заключено в кавычки.
                        if (html[i] == ' ' && valueStartChar != '"' ||
                            html[i] == '/' && valueStartChar != '"' ||
                            html[i] == '>' && valueStartChar != '"')
                        {
                            value = html.Substring(valueStartIndex, i - valueStartIndex);
                            break;
                        }
                    }
                }
            }

            return value;
        }

        #endregion

        #region GetInnerHtml

        /// <summary>
        /// Получение внутреннего HTML-текста из двойного тега.
        /// </summary>
        /// <returns>Внутренний HTML-текст.</returns>
        public string GetInnerHtml()
        {
            string value = string.Empty;
            // Если определены границы тега.
            if (CloseTagLastIndex == OpenTagLastIndex && CloseTagStartIndex == OpenTagStartIndex)
            {
                return string.Empty;
            }
            if (CloseTagLastIndex != 0)
            {
                value = html.Substring(OpenTagLastIndex + 1, Math.Abs(CloseTagStartIndex - OpenTagLastIndex) - 1);
            }
            return value;
        }

        #endregion

        #region TagPairsContains

        private bool TagPairsContains(int index, string tag)
        {
            var contains = false;
            foreach (var t in tagPairs)
            {
                if (t.Contains(index, tag))
                {
                    contains = true;
                    break;
                }
            }

            return contains;
        }

        #endregion

        #region ShiftLastIndex

        /// <summary>
        /// Определение индекса последнего символа ">" в двойном теге. Применяется после метода GetTagBounds().
        /// </summary>
        /// <param name="html">
        /// 
        /// </param>        
        public void ShiftLastIndex(ref string _html)
        {
            MatchCollection openTags;  // массив открывающих тегов
            MatchCollection closeTags; // массив закрывающих тегов  
            tagPairs.Clear();

            _html = string.Empty;

            if (TagName != string.Empty && !TagName.Equals("img", StringComparison.OrdinalIgnoreCase) &&
                !TagName.Equals("br", StringComparison.OrdinalIgnoreCase))
            {
                openTags = Regex.Matches(html, "<" + TagName + @"[^>]*>", RegexOptions.IgnoreCase);
                closeTags = Regex.Matches(html, "</" + TagName + ">", RegexOptions.IgnoreCase);

                // Заполняет массив пар тегов.
                if (openTags.Count > 0)
                {
                    // Начинает с первого закрывающего тега.
                    for (var i = 0; i < closeTags.Count; i++)
                    {
                        var tp = new TagPair
                        {
                            CloseTag = closeTags[i].ToString(),
                            CloseTagStartIndex = closeTags[i].Index,
                            CloseTagLastIndex = (closeTags[i].Index + closeTags[i].Length - 1)
                        };

                        // Идём навстречу с последнего открывающего.
                        for (var j = openTags.Count - 1; j >= 0; j--)
                        {
                            // Когда находит первый открывающий тег, индекс которого меньше данного закрывающего.
                            if (closeTags[i].Index > openTags[j].Index)
                            {
                                // Если открывающий тег не добавлен в другую пару.
                                if (!TagPairsContains(openTags[j].Index, openTags[j].ToString()))
                                {
                                    tp.OpenTag = openTags[j].ToString();
                                    tp.OpenTagStartIndex = openTags[j].Index;
                                    tp.OpenTagLastIndex = openTags[j].Index + openTags[j].Length - 1;
                                    tagPairs.Add(tp);
                                    break;
                                }
                            }
                        }
                        if ((tagPairs.Count == openTags.Count) | (tagPairs.Count == openTags.Count))
                        {
                            break;
                        }
                    }
                }

                foreach (var tp in tagPairs)
                {
                    if (tp.OpenTagStartIndex == OpenTagStartIndex)
                    {
                        CloseTagStartIndex = tp.CloseTagStartIndex;
                        CloseTagLastIndex = tp.CloseTagLastIndex;
                    }
                }
                _html = html;
            }
            else
            {
                CloseTagStartIndex = OpenTagStartIndex;
                CloseTagLastIndex = OpenTagLastIndex;
                _html = html;
            }
        }

        #endregion

        #region GetLengthOfCloseTag

        /// <summary>
        /// Определение длины закрывающего тега. Применяется после метода GetTagBounds()
        /// </summary>
        /// <returns>Длину закрывающего тега.</returns>
        public int GetLengthOfCloseTag()
        {
            // Длина имени тега + < + / + >.
            return TagName.Length + 3;
        }

        #endregion

        #region RemoveTag

        /// <summary>
        /// Удаление тега. Применяется после методов GetTagBounds() и ShiftLastIndex()
        /// </summary>
        /// <param name="htmlText">текст, из которого надо удалить тег</param>
        public void RemoveTag(ref string htmlText)
        {
            if (CloseTagLastIndex != 0 && CloseTagLastIndex != OpenTagLastIndex)
            {
                htmlText = htmlText.Remove(CloseTagStartIndex, CloseTagLastIndex - CloseTagStartIndex + 1);
            }
            if (OpenTagLastIndex != 0)
            {
                htmlText = htmlText.Remove(OpenTagStartIndex, OpenTagLastIndex - OpenTagStartIndex + 1);
            }
        }

        #endregion

        #region GetAttributes

        /// <summary>
        /// Поиск атрибутов тега.
        /// </summary>
        public ArrayList GetAttributes()
        {
            var result = new ArrayList(0);
            var tempIndex = 0; // индекс для поиска аттрибутов
            var tag = html.Substring(OpenTagStartIndex, OpenTagLastIndex - OpenTagStartIndex + 1); // строка, в которую помещается тег

            while (tag.IndexOf("=", tempIndex) != -1)
            {
                int i = tag.IndexOf("=", tempIndex) - 1;
                string s = string.Empty;
                while (tag[i] == ' ')
                {
                    i--;
                }
                while (tag[i] != ' ')
                {
                    s = tag[i] + s;
                    i--;
                }
                result.Add(s);
                tempIndex = tag.IndexOf("=", tempIndex) + 1;
            }

            return result;
        }

        #endregion
    }
}