using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using VisualEditor.Logic.IO;

namespace VisualEditor.Logic
{

    /// <summary>
    /// Работа с Html-текстом.
    /// </summary>
    class HtmlTextWork
    {
        private ArrayList tagPairs = new ArrayList(0);
        /// <summary>
        /// Представляет собой html-код, в котором осуществляется поиск.
        /// </summary>
        private string htmlText;

        /// <summary>
        /// Границы открывающего тега.
        /// </summary>
        private int openTagStartIndex;
        private int openTagLastIndex;

        /// <summary>
        /// Границы закрывающего тега.
        /// </summary>
        private int closeTagStartIndex;
        private int closeTagLastIndex;

        /// <summary>
        /// Имя тега.
        /// </summary>
        private string tagName;

        #region Свойства
        /// <summary>
        /// Границы открывающего тега.
        /// </summary>
        public int OpenTagStartIndex
        {
            get { return openTagStartIndex; }
        }

        public int OpenTagLastIndex
        {
            get { return openTagLastIndex; }
        }

        /// <summary>
        /// Границы закрывающего тега.
        /// </summary>
        public int CloseTagStartIndex
        {
            get { return closeTagStartIndex; }
        }

        public int CloseTagLastIndex
        {
            get { return closeTagLastIndex; }
        }

        #region Индексы от TagStructure
        public int StartIndex
        {
            get
            {
                return openTagStartIndex;
            }
        }

        public int LastIndex
        {
            get
            {
                if (closeTagLastIndex == 0)
                {
                    return openTagLastIndex;
                }
                else
                {
                    return closeTagLastIndex;
                }
            }
        }
        #endregion

        /// <summary>
        /// Имя тега.
        /// </summary>
        public string TagName
        {
            get { return tagName; }
        }
        #endregion

        /// <summary>
        /// Поиск первого вхождения символа value слева от указанной позиции.
        /// </summary>
        /// <param name="source">Строка для поиска.</param>
        /// <param name="value">Искомый символ.</param>
        /// <param name="startIndex">Индекс, с которого начинается поиск.</param>
        /// <returns>Индекс искомого символа.</returns>
        private int IndexOf(string source, char value, int startIndex)
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

        /// <summary>
        /// Определение границ тега в html-коде.
        /// </summary>
        /// <param name="htmlText">Представляет собой html-код, в котором осуществляется поиск.</param>
        /// <param name="searchString">Строка для определения тега.</param>
        public void GetTagBounds(string htmlText, string searchString, int startIndex)
        {
            openTagStartIndex =
            openTagLastIndex =
            closeTagStartIndex =
            closeTagLastIndex = 0;
            this.htmlText = htmlText;
            openTagStartIndex = htmlText.IndexOf(searchString, startIndex, StringComparison.OrdinalIgnoreCase);
            if (openTagStartIndex != -1)
            {
                openTagStartIndex = IndexOf(htmlText, '<', openTagStartIndex);
                openTagLastIndex = htmlText.IndexOf(">", openTagStartIndex);
                // ищем имя тега
                if ((htmlText.IndexOf(">", openTagStartIndex) < htmlText.IndexOf(" ", openTagStartIndex) && htmlText.IndexOf(">", openTagStartIndex) != -1) ||
                    (htmlText.IndexOf(">", openTagStartIndex) != -1 && htmlText.IndexOf(" ", openTagStartIndex) == -1))
                {
                    tagName = htmlText.Substring(openTagStartIndex + 1, htmlText.IndexOf(">", openTagStartIndex) - openTagStartIndex - 1);
                }
                else if ((htmlText.IndexOf(">", openTagStartIndex) > htmlText.IndexOf(" ", openTagStartIndex) && htmlText.IndexOf(" ", openTagStartIndex) != -1) ||
                        (htmlText.IndexOf(">", openTagStartIndex) == -1 && htmlText.IndexOf(" ", openTagStartIndex) != -1))
                {
                    tagName = htmlText.Substring(openTagStartIndex + 1, htmlText.IndexOf(" ", openTagStartIndex) - openTagStartIndex - 1);
                }
            }
            else
            {
                openTagStartIndex = 0;
                openTagLastIndex = 0;
                tagName = string.Empty;
            }
        }

        public void GetTagBounds(string htmlText, string searchString)
        {
            GetTagBounds(htmlText, searchString, 0);
        }

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
            if (openTagLastIndex != 0)
            {
                index = htmlText.IndexOf(attrName, openTagStartIndex);
                // Если имя атрибута попало в границы тега.
                if (index > openTagStartIndex && index < openTagLastIndex)
                {
                    // Позиция, следующая за знаком "=".
                    index = htmlText.IndexOf("=", index) + 1;
                    // если после "=" идут пробелы
                    while (htmlText[index].Equals(' '))
                    {
                        index++;
                    }

                    int valueStartIndex = index;
                    char valueStartChar = htmlText[valueStartIndex];

                    for (int i = index + 1; i <= openTagLastIndex; i++)
                    {
                        // Значение атрибута не может содержать пробелов.
                        //if (attrName != "alt" && attrName != "title"
                        //    && attrName != "longDesc" && attrName != "src_" && attrName != "src")
                        //{
                        // Если значение атрибута заключено в кавычки.
                        if (htmlText[i] == '"' && valueStartChar == '"')
                        {
                            value = htmlText.Substring(valueStartIndex + 1, i - valueStartIndex - 1);
                            break;
                        }
                        // Если значение атрибута не заключено в кавычки.
                        else if (htmlText[i] == ' ' && valueStartChar != '"' ||
                            htmlText[i] == '/' && valueStartChar != '"' ||
                            htmlText[i] == '>' && valueStartChar != '"')
                        {
                            value = htmlText.Substring(valueStartIndex, i - valueStartIndex);
                            break;
                        }
                        //// Если кавычка стоит только перед значением атрибута.
                        //else if (htmlText[i] == ' ' && valueStartChar == '"' ||
                        //    htmlText[i] == '/' && valueStartChar == '"' ||
                        //    htmlText[i] == '>' && valueStartChar == '"')
                        //{
                        //    value = htmlText.Substring(valueStartIndex + 1, i - valueStartIndex - 1);
                        //    break;
                        //}
                        //// Если кавычка стоит только после значения атрибута.
                        //else if (htmlText[i] == '"' && valueStartChar != '"')
                        //{
                        //    value = htmlText.Substring(valueStartIndex, i - valueStartIndex);
                        //    break;
                        //}
                        //}
                        // Значение атрибута может содержать пробелы.
                        // В этом случае оно обязательно заключено в кавычки.
                        //else
                        //{
                        //    if (htmlText[i] == '"' && valueStartChar == '"')
                        //    {
                        //        value = htmlText.Substring(valueStartIndex + 1, i - valueStartIndex - 1);
                        //        break;
                        //    }
                        //}
                    }

                    //if (htmlText[index].Equals('"'))
                    //{
                    //    index += 1;
                    //    value = htmlText.Substring(index, htmlText.IndexOf("\"", index) - index);
                    //}
                    //else
                    //{
                    //    // В значении атрибута не должно быть пробелов.
                    //    value = htmlText.Substring(index, htmlText.IndexOfAny(new char[] { ' ', '>' }, index) - index);
                    //}
                }
            }
            return value;
        }

        /// <summary>
        /// Получение внутреннего HTML-текста из двойного тега.
        /// </summary>
        /// <returns>Внутренний HTML-текст.</returns>
        public string GetInnerHtml()
        {
            string value = string.Empty;
            // Если определены границы тега.
            if (closeTagLastIndex == openTagLastIndex && closeTagStartIndex == openTagStartIndex)
            {
                return string.Empty;
            }
            if (closeTagLastIndex != 0)
            {
                value = htmlText.Substring(openTagLastIndex + 1, Math.Abs(closeTagStartIndex - openTagLastIndex) - 1);
            }
            return value;
        }

        private bool TagPairsContains(int index, string tag)
        {
            bool contains = false;
            foreach (TagPair t in tagPairs)
            {
                if (t.Contains(index, tag))
                {
                    contains = true;
                    break;
                }
            }
            return contains;

        }

        /// <summary>
        /// Определение индекса последнего символа ">" в двойном теге. Применяется после метода GetTagBounds()
        /// </summary>
        /// <param name="html">
        /// 
        /// </param>        
        public void ShiftLastIndex(ref string html)
        {
            MatchCollection openTags;  // массив открывающих тегов
            MatchCollection closeTags; // массив закрывающих тегов  
            tagPairs.Clear();

            html = string.Empty;

            if (tagName != string.Empty && !tagName.Equals("img", StringComparison.OrdinalIgnoreCase) &&
                !tagName.Equals("br", StringComparison.OrdinalIgnoreCase))
            {
                openTags = Regex.Matches(htmlText, "<" + tagName + @"[^>]*>", RegexOptions.IgnoreCase);
                //openTags = Regex.Matches(htmlText, "<" + tagName + @"(.*?)+>", RegexOptions.IgnoreCase);
                closeTags = Regex.Matches(htmlText, "</" + tagName + ">", RegexOptions.IgnoreCase);
                // заполняем массив пар тегов
                if (openTags.Count > 0)
                {
                    // начинаем с первого закрывающего тега
                    for (int i = 0; i < closeTags.Count; i++)
                    {
                        TagPair tp = new TagPair();
                        tp.CloseTag = closeTags[i].ToString();
                        tp.CloseTagStartIndex = closeTags[i].Index;
                        tp.CloseTagLastIndex = closeTags[i].Index + closeTags[i].Length - 1;
                        // идём навстречу с последнего открывающего
                        for (int j = openTags.Count - 1; j >= 0; j--)
                        {
                            // когда находим первый открывающий тег, индекс которого меньше данного закрывающего
                            if (closeTags[i].Index > openTags[j].Index)
                            {
                                // если открывающий тег не добавлен в другую пару
                                if (!TagPairsContains(openTags[j].Index, openTags[j].ToString()))
                                {
                                    tp.OpenTag = openTags[j].ToString();
                                    tp.OpenTagStartIndex = openTags[j].Index;
                                    tp.OpenTagLastIndex = openTags[j].Index + openTags[j].Length - 1;
                                    tagPairs.Add(tp);
                                    break;
                                } // if (!TagPairsContains(...
                            } // if (closeTags[i].Index ...
                        } // for (int j = ...    
                        if ((tagPairs.Count == openTags.Count) | (tagPairs.Count == openTags.Count))
                        {
                            break;
                        } // if ((tagPairs.Count == ...
                    } // for (int i = 0; ...
                } // if (openTags.Count > 0)

                foreach (TagPair tp in tagPairs)
                {
                    if (tp.OpenTagStartIndex == openTagStartIndex)
                    {
                        closeTagStartIndex = tp.CloseTagStartIndex;
                        closeTagLastIndex = tp.CloseTagLastIndex;
                    } // if (tp.OpenTagStartIndex...
                } // foreach (TagPair tp in tagPairs)
                html = htmlText;
            } //  if (tagName != string.Empty...
            else
            {
                closeTagStartIndex = openTagStartIndex;
                closeTagLastIndex = openTagLastIndex;
                html = htmlText;
            } // else
        } // ShiftLastIndex

        /// <summary>
        /// Определение длины закрывающего тега. Применяется после метода GetTagBounds()
        /// </summary>
        /// <returns>Длину закрывающего тега.</returns>
        public int GetLengthOfCloseTag()
        {
            return tagName.Length + 3; // длина имени тега + < + / + >
        }

        /// <summary>
        /// Удаление тега. Применяется после методов GetTagBounds() и ShiftLastIndex()
        /// </summary>
        /// <param name="htmlText">текст, из которого надо удалить тег</param>
        public void RemoveTag(ref string htmlText)
        {
            if (closeTagLastIndex != 0 && closeTagLastIndex != openTagLastIndex)
            {
                htmlText = htmlText.Remove(closeTagStartIndex, closeTagLastIndex - closeTagStartIndex + 1);
            }
            if (openTagLastIndex != 0)
            {
                htmlText = htmlText.Remove(openTagStartIndex, openTagLastIndex - openTagStartIndex + 1);
            }
        }

        /// <summary>
        /// Поиск атрибутов тега.
        /// </summary>
        public ArrayList GetAttributes()
        {
            ArrayList result = new ArrayList(0);
            int tempIndex = 0; // индекс для поиска аттрибутов
            string tag = htmlText.Substring(openTagStartIndex, openTagLastIndex - openTagStartIndex + 1); // строка, в которую помещается тег

            while (tag.IndexOf("=", tempIndex) != -1)
            {
                int i = tag.IndexOf("=", tempIndex) - 1;
                int mi = tag.IndexOf("=", tempIndex) + 1;
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
                    while (tag[mi] == ' ')
                    {
                        mi++;
                    }
                    if (tag[mi] == '\"')
                    result.Add(s);
                    tempIndex = tag.IndexOf("=", tempIndex) + 1;
                
            }

            return result;
        }
    }
}

