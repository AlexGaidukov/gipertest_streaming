using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VisualEditor.Logic.Controls.Docking.Documents;
using VisualEditor.Logic.Course.Items.Questions;
using VisualEditor.Logic.IO.Questions;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using VisualEditor.Utils.Controls.HtmlEditing;

namespace VisualEditor.Logic.Course.Items
{
    internal abstract class Question : CourseItem
    {
        protected static int count;     // счётчик количества вопросов 
        protected Enums.QuestionType type; // тип вопроса
        protected Enums.InteractionType interactionType;
        protected bool isAdaptive;      // является ли вопрос адаптивным
        protected bool isTimeDependent; // есть ли у вопроса лимит по времени
        protected int timeRestriction;  // лимит по времени. не равен 0, если isTimeDependent == true
        protected string cardinality;
        protected string fileName;      // имя файла для экспорта в формате IMS QTI  
        protected string baseType;
        protected TestModule testModule; // модуль-контроль, в который входит вопрос
        protected int lowerBound;       // минимальная оценка вопроса
        protected int defaultValue;     // значение оценки по умолчанию   
        protected double mark;          // максимальная оценка вопроса
        protected bool isShuffle;       // используется ли случайный порядок для ответов    
        //protected ArrayList responseVariants; // Массив вариантов ответов.
        protected int maxChoices;       // сколько может быть выбрано ответов
        protected string description;
      //  protected string dhtml;
        public List<string> MmediaFiles = new List<string>();

        public bool success = false;
          

        /// <summary>
        /// Модуль-контроль, в который входит вопрос.
        /// </summary>
        public TestModule TestModule
        {
            get { return testModule; }
            set { testModule = value; }
        }


        /// <summary>
        /// Тип элементов ответа вопроса.
        /// </summary>
        public Enums.InteractionType InteractionType
        {
            get { return interactionType; }
        }

        /// <summary>
        /// Есть ли ограничение на время прохождения вопроса.
        /// </summary>
        public bool IsTimeDependent
        {
            get { return isTimeDependent; }
        }

        /// <summary>
        /// Описание вопроса.
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// Имя файла вопроса.
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        /// <summary>
        /// Максимально возможное количество выбранных элементов ответа вопроса.
        /// </summary>
        public int MaxChoices
        {
            get { return maxChoices; }
            set { maxChoices = value; }
        }

        /// <summary>
        /// Является ли порядок элементов ответа в вопросе случайным.
        /// </summary>
        public bool IsShuffle
        {
            get { return isShuffle; }
            set { isShuffle = value; }
        }


        /// <summary>
        /// Значение оценки вопроса по умолчанию.
        /// </summary>
        public int DefaultValue
        {
            get { return defaultValue; }
            set { defaultValue = value; }
        }

        /// <summary>
        /// Нижняя граница оценки вопроса.
        /// </summary>
        public int LowerBound
        {
            get { return lowerBound; }
            set { lowerBound = value; }
        }

        /// <summary>
        /// Базовый тип вопроса.
        /// </summary>
        public string BaseType
        {
            get { return baseType; }
            set { baseType = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Cardinality
        {
            get { return cardinality; }
            set { cardinality = value; }
        }

        /// <summary>
        /// Является ли вопрос адаптивным.
        /// </summary>
        public bool IsAdaptive
        {
            get { return isAdaptive; }
            set { isAdaptive = value; }
        }

       // public string Identifier;
        public string identifier;    // идентификатор вопроса ("VE-" + questionNumber + questionType)  
        static Question()
        {
            count = 1;
        }
        protected Question()
        {
            ImageIndex = SelectedImageIndex = 8;
            ResponseVariants = new ArrayList();
            Hint = string.Empty;
            count++;
            identifier = "VE" + count;
            mark = 1;
            success = false;
           
        }

        /// <summary>
        /// запись вопроса в файл формата IMS QTI 2.1
        /// </summary>
        /// <param name="fileName">
        /// желаемое имя файла вопроса
        /// если null, имя вопроса будет сформировано по идентификатору вопроса 
        /// </param>
        public abstract void WriteQti(string fileName);


        /// <summary>
        /// чтение информации из файла формата IMS QTI 2.1
        /// </summary>
        /// <param name="qfPath">путь к файлу</param>
        public abstract bool ReadQti(string qfPath);


        public Guid Id { get; set; }

        public List<Response> Responses
        {
            get
            {
                return Nodes.OfType<Response>().Select(treeNode => treeNode as Response).ToList();
            }
        }

        public QuestionDocument QuestionDocument { get; set; }
        public string DocumentHtml { get; set; }
        public string PreviewHtml { get; set; }

        public int TimeRestriction
        {
            get { return timeRestriction; }
            set
            {
                timeRestriction = value;
                if (timeRestriction == 0) isTimeDependent = false;
                else isTimeDependent = true;
            }
        }

        public Concept Profile { get; set; }
        public int Marks { get; set; }
        public string Hint { get; set; }
        public ArrayList ResponseVariants;
        public Question NextQuestion { get; set; }

        public QuestionXmlWriter XmlWriter { get; protected set; }
        public QuestionXmlReader XmlReader { get; protected set; }

        public static Question Clone(Question question)
        {
            Question newQuestion = null;

            #region Инициализация вопроса

            if (question is ChoiceQuestion)
            {
                newQuestion = new ChoiceQuestion();
            }
            else if (question is MultichoiceQuestion)
            {
                newQuestion = new MultichoiceQuestion();
            }
            else if (question is InteractiveQuestion)
            {
                newQuestion = new InteractiveQuestion();
            }
            else if (question is OrderingQuestion)
            {
                newQuestion = new OrderingQuestion();
            }
            else if (question is OpenQuestion)
            {
                newQuestion = new OpenQuestion();
            }
            else if (question is CorrespondenceQuestion)
            {
                newQuestion = new CorrespondenceQuestion();
            }
            else if (question is OuterQuestion)
            {
                newQuestion = new OuterQuestion();
                (newQuestion as OuterQuestion).TaskId = (question as OuterQuestion).TaskId;
                (newQuestion as OuterQuestion).TaskName = (question as OuterQuestion).TaskName;
                (newQuestion as OuterQuestion).TestName = (question as OuterQuestion).TestName;
                (newQuestion as OuterQuestion).SubjectName = (question as OuterQuestion).SubjectName;
                (newQuestion as OuterQuestion).Declaration = (question as OuterQuestion).Declaration;
                (newQuestion as OuterQuestion).Url = (question as OuterQuestion).Url;
            }

            #endregion

            newQuestion.LowerBound = question.LowerBound;
            newQuestion.Cardinality = question.Cardinality;
            newQuestion.Text = question.Text;
            newQuestion.Id = Guid.NewGuid();
            newQuestion.DocumentHtml = string.Copy(question.DocumentHtml);
            newQuestion.TimeRestriction = question.TimeRestriction;
            newQuestion.Profile = question.Profile;
            newQuestion.Marks = question.Marks;
            newQuestion.Hint = question.Hint;
            newQuestion.NextQuestion = question.NextQuestion;
            newQuestion.IsAdaptive = question.IsAdaptive;
            newQuestion.BaseType = question.BaseType;
            newQuestion.DefaultValue = question.DefaultValue;
            //newQuestion.Mark = question.Mark;
            newQuestion.IsShuffle = question.IsShuffle;
            newQuestion.MaxChoices = question.MaxChoices;
            //  newQuestion.timeRestriction = question.timeRestriction;
            newQuestion.identifier = question.identifier;    // идентификатор вопроса ("VE-" + questionNumber + questionType)  

            foreach (var questionResponse in question.Responses)
            {
                var response = Response.Clone(questionResponse);
                newQuestion.Nodes.Add(response);
            }

            foreach (ResponseVariant questionResponseVariant in question.ResponseVariants)
            {
                var responseVariant = new ResponseVariant(question);

                if (!(question is OrderingQuestion) && !(question is OpenQuestion))
                {
                    foreach (Response response in questionResponseVariant.Responses)
                    {
                        for (var i = 0; i < question.Responses.Count; i++)
                        {
                            if ((question.Responses[i]).Text.Equals(response.Text))
                            {
                                responseVariant.Responses.Add(question.Responses[i]);

                                break;
                            }
                        }
                    }
                }
                else
                {
                    for (var i = 0; i < questionResponseVariant.Responses.Count; i++)
                    {
                        responseVariant.Responses.Add(questionResponseVariant.Responses[i]);
                    }
                }

                responseVariant.Weight = questionResponseVariant.Weight;
                responseVariant.Hint = questionResponseVariant.Hint;
                responseVariant.NextQuestion = questionResponseVariant.NextQuestion; // моё 


                newQuestion.ResponseVariants.Add(responseVariant);
            }

            return newQuestion;
        }

        public override string ToString()
        {
            switch (type)
            {
                case Enums.QuestionType.GraphicalMatching:
                    return "Графическое сопоставление";
                case Enums.QuestionType.SimpleMatching:
                    return "Сопоставление";
                case Enums.QuestionType.Multichoice:
                    return "Множественный выбор";
                case Enums.QuestionType.Choice:
                    return "Одновариантный выбор";
                case Enums.QuestionType.OpenEnded:
                    return "Открытый вопрос";
                case Enums.QuestionType.Ordering:
                    return "Упорядочение элементов ответов";
                case Enums.QuestionType.TrueOrFalse:
                    return "Истина или ложь";
                default:
                    return "Неизвестный тип вопроса";
            }
        }



        #region конвертация
        /// <summary>
        /// Процедура старта конвертации из HTML в XML для элементов ответа.
        /// </summary>
        /// <param name="htmlText">HTML текст, который нужно преобразовать.</param>
        /// <param name="writer">XmlTextWriter для записи xml сразу в выходной поток.</param>
        public void ResponseHtmlToXml(string htmlText, XmlTextWriter writer)
        {
            MatchCollection tags;
            bool repeatedTagExists;

            // удаляем ненужные символы
            htmlText = htmlText.Trim();
            while (htmlText.IndexOf("\n") != -1)
            {
                htmlText = htmlText.Replace("\n", "");
            }
            while (htmlText.IndexOf("\r") != -1)
            {
                htmlText = htmlText.Replace("\r", "");
            }
            while (htmlText.IndexOf("\t") != -1)
            {
                htmlText = htmlText.Replace("\t", " ");
            }
            // отслеживание ситуации типа <p><p>...</p></p> в начале текста
            do
            {
                repeatedTagExists = false;
                tags = Regex.Matches(htmlText, @"<p[^>]*>", RegexOptions.IgnoreCase);
                if (tags.Count > 1)
                {
                    string htmlTextCopy = htmlText;
                    HtmlTextWork htw = new HtmlTextWork();
                    htw.GetTagBounds(htmlTextCopy, tags[0].ToString());
                    htw.ShiftLastIndex(ref htmlTextCopy);
                    htmlText = htmlTextCopy;
                    if (htw.OpenTagStartIndex == 0 && htw.CloseTagLastIndex == htmlText.Length - 1)
                    {
                        htw.RemoveTag(ref htmlTextCopy);
                        if (htmlTextCopy.StartsWith("<p", StringComparison.OrdinalIgnoreCase) &&
                            htmlTextCopy.EndsWith("</p>", StringComparison.OrdinalIgnoreCase))
                        {
                            repeatedTagExists = true;
                            htw.RemoveTag(ref htmlText);
                        }
                    }
                }
            } while (repeatedTagExists);
            // удаление тега, обрамляющего весь текст
            tags = Regex.Matches(htmlText, @"<p[^>]*>", RegexOptions.IgnoreCase);
            if (tags.Count == 1)
            {
                HtmlTextWork htw = new HtmlTextWork();
                htw.GetTagBounds(htmlText, tags[0].ToString());
                htw.ShiftLastIndex(ref htmlText);
                if (htw.OpenTagStartIndex == 0 && htw.CloseTagLastIndex == htmlText.Length - 1)
                {
                    htw.RemoveTag(ref htmlText);
                }
            }
            ConvertHtmlStringToXml(htmlText, writer);
        }


        /// <summary>
        /// Рекурсивная процедура для преобразования html в xml.
        /// </summary>
        /// <param name="htmlText">HTML текст, который нужно преобразовать.</param>
        /// <param name="writer">XmlTextWriter для записи xml сразу в выходной поток.</param>
        public void ConvertHtmlStringToXml(string htmlText, XmlTextWriter writer)
        {
            Match tag;
            HtmlTextWork htw = new HtmlTextWork();

            #region мое творение
            MatchCollection imgTags = Regex.Matches(htmlText, "<img[^>]*>", RegexOptions.IgnoreCase);
            foreach (Match imtag in imgTags)
            {
                // определяем границы тега
                htw.GetTagBounds(htmlText, imtag.ToString());
                htw.ShiftLastIndex(ref htmlText);
                // считываем значение аттрибута src
                string src = htw.GetValue("src");
            
                //if (Path.GetExtension(src) == ".mp4" || Path.GetExtension(src) == ".flv" || Path.GetExtension(src) == ".mp3" || Path.GetExtension(src) == ".swf")
                if (Path.GetFileNameWithoutExtension(src) == "Aud" || Path.GetFileNameWithoutExtension(src) == "Vid" || Path.GetFileNameWithoutExtension(src) == "Anim")
                {
                    string src_ = htw.GetValue("src_");
                    // удаляем тег из текста
                    htw.RemoveTag(ref htmlText);
                    // вставляем вместо удалённого тега нужный текст
                    //string sf = Path.Combine(Warehouse.Warehouse.RelativeImagesDirectory, Path.GetFileName(src));
                    htmlText = htmlText.Insert(htw.OpenTagStartIndex,
                        "<object" + " data=\"" + src_ +"\"" + ">" + "  </object>");
                }
            }

            #endregion

            var greekSymbols = Regex.Matches(htmlText, "[Α-Ωα-ω¡¢£¤¥¦§¨©ª«¬­®¯°±²³´µ¶·¸¹º»¼½¾¿™•∑∏∫∂√∞ƒ≤≥≠≡…′″∃∈∋∧∨∩∪∼≈⊂⊃⊆⊇⊕⊥°→×÷∀]", RegexOptions.IgnoreCase);
            int index;

            if (greekSymbols.Count != 0)
            {
                for (var i = greekSymbols.Count; i > 0; i--)
                {
                    var symbol = greekSymbols[i - 1].Value;
                    index = greekSymbols[i - 1].Index;
                    htmlText = htmlText.Remove(index, 1);
                    htmlText = htmlText.Insert(index, string.Concat("&#", char.ConvertToUtf32(symbol, 0), ";"));
                }
            }

            greekSymbols = Regex.Matches(htmlText, "[Ë]", RegexOptions.IgnoreCase);
            if (greekSymbols.Count != 0)
            {
                for (var i = greekSymbols.Count; i > 0; i--)
                {
                    index = greekSymbols[i - 1].Index;
                    htmlText = htmlText.Remove(index, 1);
                    htmlText = htmlText.Insert(index, "&Euml;");
                }
            }

            greekSymbols = Regex.Matches(htmlText, "[Ï]", RegexOptions.IgnoreCase);
            if (greekSymbols.Count != 0)
            {
                for (var i = greekSymbols.Count; i > 0; i--)
                {
                    index = greekSymbols[i - 1].Index;
                    htmlText = htmlText.Remove(index, 1);
                    htmlText = htmlText.Insert(index, "&Iuml;");
                }
            }

            greekSymbols = Regex.Matches(htmlText, "[Æ]", RegexOptions.IgnoreCase);
            if (greekSymbols.Count != 0)
            {
                for (var i = greekSymbols.Count; i > 0; i--)
                {
                    index = greekSymbols[i - 1].Index;
                    htmlText = htmlText.Remove(index, 1);
                    htmlText = htmlText.Insert(index, "&AElig;");
                }
            }

            greekSymbols = Regex.Matches(htmlText, "[Ä]", RegexOptions.IgnoreCase);
            if (greekSymbols.Count != 0)
            {
                for (var i = greekSymbols.Count; i > 0; i--)
                {
                    index = greekSymbols[i - 1].Index;
                    htmlText = htmlText.Remove(index, 1);
                    htmlText = htmlText.Insert(index, "&Auml;");
                }
            }

            greekSymbols = Regex.Matches(htmlText, "[Þ]", RegexOptions.IgnoreCase);
            if (greekSymbols.Count != 0)
            {
                for (var i = greekSymbols.Count; i > 0; i--)
                {
                    index = greekSymbols[i - 1].Index;
                    htmlText = htmlText.Remove(index, 1);
                    htmlText = htmlText.Insert(index, "&THORN;");
                }
            }

            do
            {
                // поиск тегов 
                tag = Regex.Match(htmlText, @"<[^>]+>");
                if (tag.ToString() != "")
                {
                    if (htmlText.IndexOf(tag.ToString()) != 0)
                    {
                        writer.WriteString(htmlText.Substring(0, htmlText.IndexOf(tag.ToString())));
                        htmlText = htmlText.Remove(0, htmlText.IndexOf(tag.ToString()));
                    }
                    htw.GetTagBounds(htmlText, tag.ToString());
                    htw.ShiftLastIndex(ref htmlText);
                    if (htw.CloseTagStartIndex != htw.OpenTagLastIndex + 1)
                    {
                        writer.WriteStartElement(htw.TagName);
                        foreach (string attr in htw.GetAttributes())
                        {
                           
                                if (htw.TagName.Equals("img", StringComparison.OrdinalIgnoreCase) && attr.Equals("src", StringComparison.OrdinalIgnoreCase))
                                {
                                    string j = Path.GetFileName(htw.GetValue(attr));
                                    writer.WriteAttributeString(attr, Path.GetFileName(htw.GetValue(attr)));
                                    if (htw.GetValue("height") == "")
                                    {
                                        SizeF sourceImageSize;
                                        SizeF imageSize;
                                        sourceImageSize = Image.FromFile(Path.Combine(Warehouse.Warehouse.AbsoluteEditorImagesDirectory, Path.GetFileName(htw.GetValue(attr)))).PhysicalDimension;
                                        imageSize = new SizeF(sourceImageSize);
                                        writer.WriteAttributeString("height", sourceImageSize.Height.ToString());
                                        writer.WriteAttributeString("width", sourceImageSize.Width.ToString());
                                    }
                                }
                                else
                                {
                                    string j = htw.GetValue(attr);
                                    writer.WriteAttributeString(attr, htw.GetValue(attr));
                                }
                           
                            
                        }
                        ConvertHtmlStringToXml(htw.GetInnerHtml(), writer);
                        htmlText = htmlText.Remove(htw.OpenTagStartIndex, htw.CloseTagLastIndex - htw.OpenTagStartIndex + 1);
                        writer.WriteFullEndElement();
                    }
                    else
                    {
                        htw.RemoveTag(ref htmlText);
                    }
                }
                else
                {
                    if (htmlText.Trim() != "")
                    {
                        writer.WriteString(htmlText);
                    }
                }
            } while (tag.ToString() != "");



            string p = "";



        }

        /// <summary>
        /// Процедура старта конвертации из HTML в XML для вопросов.
        /// </summary>
        /// <param name="htmlText">HTML текст, который нужно преобразовать.</param>
        /// <param name="writer">XmlTextWriter для записи xml сразу в выходной поток.</param>
        public void QuestionHtmlToXml(string htmlText, XmlTextWriter writer)
        {
            MatchCollection tags;
            bool repeatedTagExists = false;

            // удаляем ненужные символы
            htmlText = htmlText.Trim();
            while (htmlText.IndexOf("\n") != -1)
            {
                htmlText = htmlText.Replace("\n", "");
            }
            while (htmlText.IndexOf("\r") != -1)
            {
                htmlText = htmlText.Replace("\r", "");
            }
            while (htmlText.IndexOf("\t") != -1)
            {
                htmlText = htmlText.Replace("\t", " ");
            }
            // обрамление текста тегом абзаца, если это не сделано 
            tags = Regex.Matches(htmlText, @"<p[^>]*>", RegexOptions.IgnoreCase);
            if (tags.Count > 0)
            {
                HtmlTextWork htw = new HtmlTextWork();
                htw.GetTagBounds(htmlText, tags[0].ToString());
                htw.ShiftLastIndex(ref htmlText);
                if (htw.OpenTagStartIndex != 0)
                {
                    htmlText = "<P>" + htmlText + "<P>";
                }
            }
            // отслеживание ситуации типа <p><p>...</p></p> в начале текста
            do
            {
                repeatedTagExists = false;
                tags = Regex.Matches(htmlText, @"<p[^>]*>", RegexOptions.IgnoreCase);
                if (tags.Count > 1)
                {
                    string htmlTextCopy = htmlText;
                    HtmlTextWork htw = new HtmlTextWork();
                    htw.GetTagBounds(htmlTextCopy, tags[0].ToString());
                    htw.ShiftLastIndex(ref htmlTextCopy);
                    if (htw.OpenTagStartIndex == 0 && htw.CloseTagLastIndex == htmlText.Length - 1)
                    {
                        htw.RemoveTag(ref htmlTextCopy);
                        if (htmlTextCopy.StartsWith("<p", StringComparison.OrdinalIgnoreCase) &&
                            htmlTextCopy.EndsWith("</p>", StringComparison.OrdinalIgnoreCase))
                        {
                            repeatedTagExists = true;
                            htw.RemoveTag(ref htmlText);
                        }
                    }
                }
            } while (repeatedTagExists);
            ConvertHtmlStringToXml(htmlText, writer);
        }

        /// <summary>
        /// Процедура для преобразования xml в html.
        /// </summary>
        /// <param name="xmlText">Xml текст, который нужно преобразовать.</param>
        /// <param name="qfPath">Путь к файлу вопроса.</param>
        public string ConvertXmlStringToHtml(string xmlText, string qfPath)
        {
            xmlText = xmlText.Trim();

            var Symbols = Regex.Matches(xmlText, "&amp;#", RegexOptions.IgnoreCase);
        
            if (Symbols.Count != 0)
            {
                for (var i = Symbols.Count; i > 0; i--)
                {
                    int ind = xmlText.IndexOf("&amp;#");
                    string s = "";
                    int j = ind + 6;
                    while(xmlText[j]!=';')
                    {
                        s = s + xmlText[j];
                        j++;
                    }
                    
                    
                    xmlText = xmlText.Remove(ind, 7+s.Length);
                    
                    xmlText = xmlText.Insert(ind, char.ConvertFromUtf32(int.Parse(s)));
                }
            }


            Symbols = Regex.Matches(xmlText, "&amp;nbsp;", RegexOptions.IgnoreCase);
            
            if (Symbols.Count != 0)
            {
                for (var i = Symbols.Count; i > 0; i--)
                {
                    int ind = xmlText.IndexOf("&amp;nbsp;");
                   
                    xmlText = xmlText.Remove(ind, 10);

                    xmlText = xmlText.Insert(ind, "&nbsp;");
                }
            }



            MatchCollection imgTags = Regex.Matches(xmlText, "<img[^>]*>", RegexOptions.IgnoreCase);
            MatchCollection objTags = Regex.Matches(xmlText, "<object[^>]*>", RegexOptions.IgnoreCase);
            MatchCollection textEntryTags = Regex.Matches(xmlText, "<textEntryInteraction[^>]*>", RegexOptions.IgnoreCase);
            HtmlTextWork htw = new HtmlTextWork();

            foreach (Match tag in imgTags)
            {
                // определяем границы тега
                htw.GetTagBounds(xmlText, tag.ToString());
                htw.ShiftLastIndex(ref xmlText);
                // считываем значение аттрибута src
                string src = htw.GetValue("src");
                string height = htw.GetValue("height");
                string width = htw.GetValue("width");
                string align = htw.GetValue("align");
                string border = htw.GetValue("border");
                string hspace = htw.GetValue("hspace");
                // удаляем тег из текста
                htw.RemoveTag(ref xmlText);
                // создаём папку для картинок проекта
                if (!Directory.Exists(Warehouse.Warehouse.RelativeImagesDirectory))
                {
                    Directory.CreateDirectory(Warehouse.Warehouse.RelativeImagesDirectory);
                }

                var destPath = Path.Combine(Warehouse.Warehouse.AbsoluteEditorImagesDirectory, Path.GetFileName(src));

                // копируем туда картинку
                try
                {
                    File.Copy(Path.GetDirectoryName(qfPath) + "\\" + src, destPath, true);
                }
                catch (FileNotFoundException)
                {
                    MessageBox.Show("Изображение " + src + " не найдено.",
                                                "Visual Editor",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Error);
                }
                // вставляем вместо удалённого тега нужный текст
                string sf = Path.Combine(Warehouse.Warehouse.RelativeImagesDirectory, Path.GetFileName(src));
                xmlText = xmlText.Insert(htw.OpenTagStartIndex,
                    "<IMG" + " border=\"" + border + "\" hspace=\"" + hspace + "\" align=\"" + align + "\" sdocument=\"0\" src=\"" + sf + "\" height=\"" + height + "\" width=\"" + width + "\"" + ">");
            }

            #region самодеятельность с медиа
            foreach (Match tag in objTags)
            {
                // определяем границы тега
                htw.GetTagBounds(xmlText, tag.ToString());
                htw.ShiftLastIndex(ref xmlText);
                // считываем значение аттрибута data
                string data = htw.GetValue("data");

                // удаляем тег из текста
                htw.RemoveTag(ref xmlText);
                // создаём папку для картинок проекта

                if (Path.GetExtension(Path.GetFileName(data)) == ".mp4" || Path.GetExtension(Path.GetFileName(data)) == ".flv")
                {
                    if (!Directory.Exists(Warehouse.Warehouse.RelativeVideosDirectory))
                    {
                        Directory.CreateDirectory(Warehouse.Warehouse.RelativeVideosDirectory);
                    }
                    // копируем туда картинку
                    var destPath = Path.Combine(Warehouse.Warehouse.AbsoluteEditorVideosDirectory, Path.GetFileName(data));
                    var sourcePath = Path.Combine(Path.GetDirectoryName(qfPath), Path.GetFileName(data));

                    File.Copy(sourcePath, destPath, true);

                    string asf = Path.Combine(Warehouse.Warehouse.RelativeImagesDirectory, Path.GetFileName("\\Images\\Vid.png"));
                    string sf = Path.Combine(Warehouse.Warehouse.RelativeVideosDirectory, Path.GetFileName(data));
                    xmlText = xmlText.Insert(htw.OpenTagStartIndex,
                        string.Concat("<IMG src=\"" + asf + "\" width=\"16\" height=\"16\" sdocument=\"0\" src_=\"" + sf + "\" >"));

                }

                else if (Path.GetExtension(Path.GetFileName(data)) == ".mp3")
                {
                    if (!Directory.Exists(Warehouse.Warehouse.RelativeAudiosDirectory))
                    {
                        Directory.CreateDirectory(Warehouse.Warehouse.RelativeAudiosDirectory);
                    }
                    // копируем туда картинку
                    var destPath = Path.Combine(Warehouse.Warehouse.AbsoluteEditorAudiosDirectory, Path.GetFileName(data));
                    var sourcePath = Path.Combine(Path.GetDirectoryName(qfPath), Path.GetFileName(data));

                    File.Copy(sourcePath, destPath, true);

                    string asf = Path.Combine(Warehouse.Warehouse.RelativeImagesDirectory, Path.GetFileName("\\Images\\Aud.png"));
                    string sf = Path.Combine(Warehouse.Warehouse.RelativeAudiosDirectory, Path.GetFileName(data));
                    xmlText = xmlText.Insert(htw.OpenTagStartIndex,
                        string.Concat("<IMG src=\"" + asf + "\" width=\"16\" height=\"16\" sdocument=\"0\" src_=\"" + sf + "\" >"));
                }

                else if (Path.GetExtension(Path.GetFileName(data)) == ".swf")
                {
                    if (!Directory.Exists(Warehouse.Warehouse.RelativeFlashesDirectory))
                    {
                        Directory.CreateDirectory(Warehouse.Warehouse.RelativeFlashesDirectory);
                    }
                    // копируем туда картинку
                    var destPath = Path.Combine(Warehouse.Warehouse.AbsoluteEditorFlashesDirectory, Path.GetFileName(data));
                    var sourcePath = Path.Combine(Path.GetDirectoryName(qfPath), Path.GetFileName(data));

                    File.Copy(sourcePath, destPath, true);

                    string asf = Path.Combine(Warehouse.Warehouse.RelativeImagesDirectory, Path.GetFileName("\\Images\\Anim.png"));
                    string sf = Path.Combine(Warehouse.Warehouse.RelativeFlashesDirectory, Path.GetFileName(data));
                    xmlText = xmlText.Insert(htw.OpenTagStartIndex,
                        string.Concat("<IMG src=\"" + asf + "\" width=\"16\" height=\"16\" sdocument=\"0\" src_=\"" + sf + "\" >"));

                }
            }
            #endregion


            // если вопрос является открытым, то приводим его к формату редактора
            if (this is OpenQuestion)
            {
                foreach (Match tag in textEntryTags)
                {
                    // определяем границы тега
                    htw.GetTagBounds(xmlText, tag.ToString());
                    htw.ShiftLastIndex(ref xmlText);
                    // удаляем тег из текста
                    htw.RemoveTag(ref xmlText);
                    // вставляем вместо удалённого тега нужный текст
                    xmlText = xmlText.Insert(htw.OpenTagStartIndex, " __________");
                }
            }

            if (xmlText.Equals("&amp;nbsp;", StringComparison.OrdinalIgnoreCase))
            {
                xmlText = " ";
            }

            while (xmlText.IndexOf("\t") != -1)
            {
                xmlText = xmlText.Replace("\t", "");
            }
            while (xmlText.IndexOf("\n") != -1)
            {
                xmlText = xmlText.Replace("\n", "");
            }
            while (xmlText.IndexOf("\r") != -1)
            {
                xmlText = xmlText.Replace("\r", "");
            }

            return xmlText;
        }
        #endregion

    }
}