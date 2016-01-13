using VisualEditor.Logic.IO.Questions;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System;
using System.Globalization;

namespace VisualEditor.Logic.Course.Items.Questions
{
    internal class OpenQuestion : Question
    {
        private int expectedLength;  // длина текстбокса для ввода ответа

        public OpenQuestion()
        {
            XmlWriter = new OpenQuestionXmlWriter(this);
            XmlReader = new OpenQuestionXmlReader(this);
            type = Enums.QuestionType.OpenEnded;
            interactionType = Enums.InteractionType.textEntryInteraction;
            identifier = identifier + type.ToString();
            cardinality = "single";
            baseType = "string";
            fileName = "resources\\" + identifier + ".xml";
            expectedLength = 30;
        }

        public bool IsAnswerNumeric { get; set; }

        #region запись в imsqti
        /// <summary>
        /// Запись вопроса в файл формата IMS QTI 2.1.
        /// </summary>
        /// <param name="fileName">
        /// Желаемое имя файла вопроса. Если null, имя вопроса будет сформировано по идентификатору вопроса.
        /// </param>
        public override void WriteQti(string fileName)
        {
            XmlTextWriter questionWriter;
            string tempHtml = string.Empty; // промежуточный текст для записи текста вопроса в формат IMS QTI

            if (fileName == null)
            {
                questionWriter = new XmlTextWriter(identifier + ".xml", null);
            }
            else
            {
                if (Path.GetDirectoryName(fileName) != null && Path.GetDirectoryName(fileName) != "")
                {
                    if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                    }
                }
                questionWriter = new XmlTextWriter(fileName, null);
            }

            #region медиа из вопросов

            string dhtml = this.DocumentHtml;
            var mlp = new VisualEditor.Logic.IO.MlParser();
            string searchString;
            string value;
            string tag;
            searchString = "<IMG";

            while (dhtml.Contains(searchString))
            {
                mlp.GetTagBounds(dhtml, searchString);
                value = mlp.GetValue("src");

                if (value.StartsWith("file:///"))
                {
                    var index = value.IndexOf(Warehouse.Warehouse.RelativeImagesDirectory);
                    index += Warehouse.Warehouse.RelativeImagesDirectory.Length + 1;
                    var lsrc = value.Substring(index, value.Length - index);
                    lsrc = string.Concat(Warehouse.Warehouse.RelativeImagesDirectory, "\\", lsrc);

                    value = lsrc;
                }

                string name = value;//.Substring(value.IndexOf("\\")+1);
                if (name.Contains("Aud.png") || name.Contains("Vid.png")) name = mlp.GetValue("src_");
                //-------------------------
                MmediaFiles.Add(Path.GetFileName(name));
                //-------------------------
                string d = Warehouse.Warehouse.ProjectEditorLocation;
                if (!value.Equals(string.Empty))
                {
                    value = value.Substring(0, value.IndexOf("\\"));
                }
                if (value.Equals(Warehouse.Warehouse.RelativeImagesDirectory) ||
                    value == string.Empty)
                {
                    string source = d + "\\" + name;
                    string target;
                    if (Path.GetDirectoryName(fileName) != "")
                        target = Path.GetDirectoryName(fileName) + "\\" + Path.GetFileName(name);
                    else target = Path.GetFileName(name);
                    File.Copy(source, target, true);
                }
                dhtml = dhtml.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
            }

            /* заменила на свой код)
            if (images.Count != 0)
            {
                foreach (string name in images)
                {
                    File.Copy(name, Path.GetDirectoryName(fileName) + "\\" + Path.GetFileName(name), true);
                }
            }*/
            #endregion
            /*
            if (images.Count != 0)
            {
                foreach (string name in images)
                {
                    File.Copy(name, Path.GetDirectoryName(fileName) + "\\" + Path.GetFileName(name), true);
                }
            }*/
            for (int i = 1; i <= Responses.Count; i++)
            {
                ((Response)Responses[i - 1]).Identifier = "a" + i.ToString();
            }
            // задание форматирования файла XML
            questionWriter.Formatting = Formatting.Indented;
            // создание файла вопроса
            #region question
            questionWriter.WriteStartDocument();
            questionWriter.WriteStartElement("assessmentItem");
            #region assessmentItem attr
            questionWriter.WriteAttributeString("xmlns",
                    @"http://www.imsglobal.org/xsd/imsqti_item_v2p1");
            questionWriter.WriteAttributeString("xmlns:xsi",
                    @"http://www.w3.org/2001/XMLSchema-instance");
            questionWriter.WriteAttributeString("xsi:schemaLocation",
                    @"http://www.imsglobal.org/xsd/imsqti_v2p1 http://www.imsglobal.org/xsd/imsqti_v2p1.xsd");
            questionWriter.WriteAttributeString("toolname", "Visual Editor");
            questionWriter.WriteAttributeString("toolversion", "2.0");
            questionWriter.WriteAttributeString("identifier", identifier);
            questionWriter.WriteAttributeString("title", Text);
            questionWriter.WriteAttributeString("adaptive", isAdaptive.ToString());
            questionWriter.WriteAttributeString("timeDependent", isTimeDependent.ToString());
            #endregion //assessmentItem attr
            #region responseDeclaration
            questionWriter.WriteStartElement("responseDeclaration");
            questionWriter.WriteAttributeString("identifier", identifier);
            questionWriter.WriteAttributeString("cardinality", cardinality);
            questionWriter.WriteAttributeString("baseType", baseType);
            // если вариант ответа 1, пишем его в начале файла, если нет, то варианты ответа будут в конце
            if (ResponseVariants.Count == 1)
            {
                questionWriter.WriteStartElement("correctResponse");
                //записываем правильный ответ                
                questionWriter.WriteElementString("value", ((ResponseVariant)ResponseVariants[0]).Responses[0].ToString());
                questionWriter.WriteFullEndElement(); // correctResponse 
            }
            questionWriter.WriteFullEndElement(); // responseDeclaration
            #endregion //responseDeclaration
            questionWriter.WriteStartElement("outcomeDeclaration");
            questionWriter.WriteAttributeString("identifier", "SCORE");
            questionWriter.WriteAttributeString("cardinality", "single");
            questionWriter.WriteAttributeString("baseType", "float");
            questionWriter.WriteFullEndElement(); // outcomeDeclaration
            #region itemBody
            questionWriter.WriteStartElement("itemBody");
            // копируем содержимое тега BODY
            tempHtml = DocumentHtml;//HtmlText.Substring(HtmlText.IndexOf("<BODY>") + 6,
            // HtmlText.IndexOf("</BODY>") - HtmlText.IndexOf("<BODY>") - 7);
            tempHtml = tempHtml.Trim();
            // конвертируем html текст вопроса в xml
            QuestionHtmlToXml(tempHtml, questionWriter);
            #region textEntryInteraction
            questionWriter.WriteStartElement("textEntryInteraction");
            questionWriter.WriteAttributeString("responseIdentifier", identifier);
            questionWriter.WriteAttributeString("expectedLength", expectedLength.ToString());
            questionWriter.WriteFullEndElement(); //textEntryInteraction
            #endregion //textEntryInteraction
            questionWriter.WriteFullEndElement(); //itemBody
            #endregion //itemBody
            questionWriter.WriteStartElement("responseProcessing");
            if (ResponseVariants.Count == 1)
            {
                questionWriter.WriteAttributeString("template",
                    "http://www.imsglobal.org/question/qti_v2p1/rptemplates/map_response/");
            }
            // если больше одного варианта ответа
            else if (ResponseVariants.Count > 1)
            {
                questionWriter.WriteStartElement("responseCondition");
                questionWriter.WriteStartElement("responseIf");
                questionWriter.WriteStartElement("match");
                questionWriter.WriteStartElement("variable");
                questionWriter.WriteAttributeString("identifier", identifier);
                questionWriter.WriteFullEndElement(); // variable
                questionWriter.WriteStartElement("baseValue");
                questionWriter.WriteAttributeString("baseType", "identifier");
                questionWriter.WriteString(((ResponseVariant)ResponseVariants[0]).Responses[0].ToString());
                questionWriter.WriteFullEndElement(); // baseValue
                questionWriter.WriteFullEndElement(); // match
                questionWriter.WriteStartElement("setOutcomeValue");
                questionWriter.WriteAttributeString("identifier", "SCORE");
                questionWriter.WriteStartElement("baseValue");
                questionWriter.WriteAttributeString("baseType", "float");
                questionWriter.WriteString((((ResponseVariant)ResponseVariants[0]).Weight * Marks).ToString());
                questionWriter.WriteFullEndElement(); // baseValue
                questionWriter.WriteFullEndElement(); // setOutcomeValue
                questionWriter.WriteFullEndElement(); // responseIf 
                for (int i = 1; i < ResponseVariants.Count; i++)
                {
                    questionWriter.WriteStartElement("responseElseIf");
                    questionWriter.WriteStartElement("match");
                    questionWriter.WriteStartElement("variable");
                    questionWriter.WriteAttributeString("identifier", identifier);
                    questionWriter.WriteFullEndElement(); // variable
                    questionWriter.WriteStartElement("baseValue");
                    questionWriter.WriteAttributeString("baseType", "identifier");
                    questionWriter.WriteString(((ResponseVariant)ResponseVariants[i]).Responses[0].ToString());
                    questionWriter.WriteFullEndElement(); // baseValue
                    questionWriter.WriteFullEndElement(); // match
                    questionWriter.WriteStartElement("setOutcomeValue");
                    questionWriter.WriteAttributeString("identifier", "SCORE");
                    questionWriter.WriteStartElement("baseValue");
                    questionWriter.WriteAttributeString("baseType", "float");
                    questionWriter.WriteString((((ResponseVariant)ResponseVariants[i]).Weight * Marks).ToString());
                    questionWriter.WriteFullEndElement(); // baseValue
                    questionWriter.WriteFullEndElement(); // setOutcomeValue
                    questionWriter.WriteFullEndElement(); // responseElseIf
                }
                questionWriter.WriteFullEndElement(); // responseCondition
            }
            questionWriter.WriteFullEndElement(); //responseProcessing  
            questionWriter.WriteFullEndElement(); // assessmentItem
            #endregion // question
            questionWriter.Close();
        }
        #endregion

        #region чтение из imsqti
        /// <summary>
        /// Чтение информации из файла формата IMS QTI 2.1.
        /// </summary>
        /// <param name="qfPath">Путь к файлу.</param>
        public override bool ReadQti(string qfPath)
        {
            XmlTextReader reader = new XmlTextReader(qfPath);
            bool result = true;
            Id = Guid.NewGuid();
            string HtmlText = "<HTML>\n<HEAD>\n<BASE href=\"" + Application.StartupPath +
                "\\\">\n</HEAD>\n<BODY>\n<P>";

            try
            {
                reader = new XmlTextReader(qfPath);
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name.Equals("assessmentItem", StringComparison.OrdinalIgnoreCase))
                        {
                            if (reader.GetAttribute("timeDependent") != null)
                            {
                                bool.TryParse(reader.GetAttribute("timeDependent"), out isTimeDependent);
                            }
                            if (reader.GetAttribute("identifier") != null)
                            {
                                identifier = reader.GetAttribute("identifier");
                            }
                            if (reader.GetAttribute("adaptive") != null)
                            {
                                bool.TryParse(reader.GetAttribute("adaptive"), out isAdaptive);
                            }
                            if (reader.GetAttribute("title") != null)
                            {
                                Text = reader.GetAttribute("title");
                            }
                            else Text = reader.GetAttribute("identifier");
                        }

                        if (reader.Name.Equals("correctResponse", StringComparison.OrdinalIgnoreCase))
                        {
                            bool endCycle = false;
                            ResponseVariant rv = new ResponseVariant(this);
                            rv.Weight = 1.0;

                            while (!endCycle && reader.Read())
                            {
                                if (reader.Name.Equals("value", StringComparison.OrdinalIgnoreCase))
                                {
                                    rv.Responses.Add(reader.ReadString());
                                }
                                else if (reader.NodeType == XmlNodeType.EndElement)
                                {
                                    if (reader.Name.Equals("correctResponse", StringComparison.OrdinalIgnoreCase))
                                    {
                                        endCycle = true;
                                    }
                                }
                            }
                            ResponseVariants.Add(rv);
                        }

                        if (reader.Name.Equals("itemBody", StringComparison.OrdinalIgnoreCase))
                        {
                            bool endCycle = false; // флаг окончания цикла
                            // считываем содержимое тега itemBody
                            while (!endCycle && reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Element)
                                {
                                    if (reader.Name.Equals("p", StringComparison.OrdinalIgnoreCase))
                                    {
                                        string s = reader.ReadInnerXml();
                                        HtmlText += "<P>" + ConvertXmlStringToHtml(s, qfPath) + "</P>";
                                    }

                                    if (reader.Name.Equals("img", StringComparison.OrdinalIgnoreCase))
                                    {
                                        try
                                        {
                                            // создаём папку для картинок проекта
                                            if (!Directory.Exists(Warehouse.Warehouse.RelativeImagesDirectory))
                                            {
                                                Directory.CreateDirectory(Warehouse.Warehouse.RelativeImagesDirectory);
                                            }
                                            // копируем туда картинку
                                            var destPath = Path.Combine(Warehouse.Warehouse.AbsoluteEditorImagesDirectory, Path.GetFileName(reader.GetAttribute("src")));

                                            // var destPath = Path.Combine(Warehouse.Warehouse.AbsoluteEditorImagesDirectory, Path.GetFileName(reader.GetAttribute("src")));
                                            File.Copy(Path.GetDirectoryName(qfPath) + "\\" + reader.GetAttribute("src"), destPath, true);
                                            // var inner = TagNames.ImageTagName;
                                            string height = reader.GetAttribute("height");
                                            string width = reader.GetAttribute("width");
                                            string align = reader.GetAttribute("align");
                                            string border = reader.GetAttribute("border");
                                            string hspace = reader.GetAttribute("hspace");
                                            string sf = Path.Combine(Warehouse.Warehouse.RelativeImagesDirectory, Path.GetFileName(reader.GetAttribute("src")));
                                            HtmlText += "<IMG" + " border=\"" + border + "\" hspace=\"" + hspace + "\" align=\"" + align + "\" sdocument=\"0\" src=\"" + sf + "\" height=\"" + height + "\" width=\"" + width + "\"" + ">";
                                        }
                                        catch (FileNotFoundException)
                                        {
                                            MessageBox.Show("Изображение " + reader.GetAttribute("src") + " не найдено.",
                                                            "Visual Editor",
                                                            MessageBoxButtons.OK,
                                                            MessageBoxIcon.Error);
                                        }
                                    }

                                    #region самодеятельность с медиа
                                    if (reader.Name.Equals("object", StringComparison.OrdinalIgnoreCase))
                                    {
                                        try
                                        {
                                            if (Path.GetExtension(Path.GetFileName(reader.GetAttribute("data"))) == ".mp4" || Path.GetExtension(Path.GetFileName(reader.GetAttribute("data"))) == ".flv")
                                            {
                                                if (!Directory.Exists(Warehouse.Warehouse.RelativeVideosDirectory))
                                                {
                                                    Directory.CreateDirectory(Warehouse.Warehouse.RelativeVideosDirectory);
                                                }
                                                // копируем туда картинку
                                                string data = reader.GetAttribute("data");
                                                var destPath = Path.Combine(Warehouse.Warehouse.AbsoluteEditorVideosDirectory, Path.GetFileName(data));
                                                var sourcePath = Path.Combine(Path.GetDirectoryName(qfPath), Path.GetFileName(data));

                                                File.Copy(sourcePath, destPath, true);

                                                string asf = Path.Combine(Warehouse.Warehouse.RelativeImagesDirectory, Path.GetFileName("\\Images\\Vid.png"));
                                                string sf = Path.Combine(Warehouse.Warehouse.RelativeVideosDirectory, Path.GetFileName(data));
                                                HtmlText += string.Concat("<IMG src=\"" + asf + "\" width=\"16\" height=\"16\" sdocument=\"0\" src_=\"" + sf + "\" >");
                                            }

                                            else if (Path.GetExtension(Path.GetFileName(reader.GetAttribute("data"))) == ".mp3")
                                            {
                                                if (!Directory.Exists(Warehouse.Warehouse.RelativeAudiosDirectory))
                                                {
                                                    Directory.CreateDirectory(Warehouse.Warehouse.RelativeAudiosDirectory);
                                                }
                                                // копируем туда картинку
                                                var destPath = Path.Combine(Warehouse.Warehouse.AbsoluteEditorAudiosDirectory, Path.GetFileName(reader.GetAttribute("data")));
                                                var sourcePath = Path.Combine(Path.GetDirectoryName(qfPath), Path.GetFileName(reader.GetAttribute("data")));

                                                File.Copy(sourcePath, destPath, true);

                                                string asf = Path.Combine(Warehouse.Warehouse.RelativeImagesDirectory, Path.GetFileName("\\Images\\Aud.png"));
                                                string sf = Path.Combine(Warehouse.Warehouse.RelativeAudiosDirectory, Path.GetFileName(reader.GetAttribute("data")));

                                                HtmlText += string.Concat("<IMG src=\"" + asf + "\" width=\"16\" height=\"16\" sdocument=\"0\" src_=\"" + sf + "\" >");
                                            }

                                            else if (Path.GetExtension(Path.GetFileName(reader.GetAttribute("data"))) == ".swf")
                                            {
                                                if (!Directory.Exists(Warehouse.Warehouse.RelativeFlashesDirectory))
                                                {
                                                    Directory.CreateDirectory(Warehouse.Warehouse.RelativeFlashesDirectory);
                                                }
                                                // копируем туда картинку
                                                string data = reader.GetAttribute("data");
                                                var destPath = Path.Combine(Warehouse.Warehouse.AbsoluteEditorFlashesDirectory, Path.GetFileName(data));
                                                var sourcePath = Path.Combine(Path.GetDirectoryName(qfPath), Path.GetFileName(data));

                                                File.Copy(sourcePath, destPath, true);

                                                string asf = Path.Combine(Warehouse.Warehouse.RelativeImagesDirectory, Path.GetFileName("\\Images\\Anim.png"));
                                                string sf = Path.Combine(Warehouse.Warehouse.RelativeFlashesDirectory, Path.GetFileName(data));
                                                HtmlText += string.Concat("<IMG src=\"" + asf + "\" width=\"16\" height=\"16\" sdocument=\"0\" src_=\"" + sf + "\" >");

                                            }
                                        }
                                        catch (FileNotFoundException)
                                        {
                                            MessageBox.Show("Изображение " + reader.GetAttribute("src") + " не найдено.",
                                                            "Visual Editor",
                                                            MessageBoxButtons.OK,
                                                            MessageBoxIcon.Error);
                                        }
                                    }
                                    #endregion
                                }
                                else if (reader.NodeType == XmlNodeType.EndElement)
                                {
                                    if (reader.Name.Equals("itemBody", StringComparison.OrdinalIgnoreCase))
                                    {
                                        endCycle = true;
                                    }
                                }
                            }
                        }

                        if (reader.Name.Equals("prompt", StringComparison.OrdinalIgnoreCase))
                        {
                            string s = reader.ReadInnerXml();
                            HtmlText += "<P>" + ConvertXmlStringToHtml(s, qfPath) + "</P>";
                        }
                        /*
                        if (reader.Name.Equals("object", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                if (Path.GetExtension(Path.GetFileName(reader.GetAttribute("data"))) == ".mp4" || Path.GetExtension(Path.GetFileName(reader.GetAttribute("data"))) == ".flv")
                                {
                                    if (!Directory.Exists(Warehouse.Warehouse.RelativeVideosDirectory))
                                    {
                                        Directory.CreateDirectory(Warehouse.Warehouse.RelativeVideosDirectory);
                                    }
                                    // копируем туда картинку
                                    // копируем туда картинку
                                    string data = reader.GetAttribute("data");
                                    var destPath = Path.Combine(Warehouse.Warehouse.AbsoluteEditorVideosDirectory, Path.GetFileName(data));
                                    var sourcePath = Path.Combine(Path.GetDirectoryName(qfPath), Path.GetFileName(data));

                                    File.Copy(sourcePath, destPath, true);

                                    string asf = Path.Combine(Warehouse.Warehouse.RelativeImagesDirectory, Path.GetFileName("\\Images\\Vid.png"));
                                    string sf = Path.Combine(Warehouse.Warehouse.RelativeVideosDirectory, Path.GetFileName(data));
                                    HtmlText += string.Concat("<IMG src=\"" + asf + "\" width=\"16\" height=\"16\" sdocument=\"0\" src_=\"" + sf + "\" >");
                                }

                                else if (Path.GetExtension(Path.GetFileName(reader.GetAttribute("data"))) == ".mp3")
                                {
                                    if (!Directory.Exists(Warehouse.Warehouse.RelativeAudiosDirectory))
                                    {
                                        Directory.CreateDirectory(Warehouse.Warehouse.RelativeAudiosDirectory);
                                    }
                                    // копируем туда картинку
                                    var destPath = Path.Combine(Warehouse.Warehouse.AbsoluteEditorAudiosDirectory, Path.GetFileName(reader.GetAttribute("data")));
                                    var sourcePath = Path.Combine(Path.GetDirectoryName(qfPath), Path.GetFileName(reader.GetAttribute("data")));

                                    File.Copy(sourcePath, destPath, true);

                                    string asf = Path.Combine(Warehouse.Warehouse.RelativeImagesDirectory, Path.GetFileName("\\Images\\Aud.png"));
                                    string sf = Path.Combine(Warehouse.Warehouse.RelativeAudiosDirectory, Path.GetFileName(reader.GetAttribute("data")));

                                    HtmlText += string.Concat("<IMG src=\"" + asf + "\" width=\"16\" height=\"16\" sdocument=\"0\" src_=\"" + sf + "\" >");
                                }

                                else if (Path.GetExtension(Path.GetFileName(reader.GetAttribute("data"))) == ".swf")
                                {
                                    if (!Directory.Exists(Warehouse.Warehouse.RelativeFlashesDirectory))
                                    {
                                        Directory.CreateDirectory(Warehouse.Warehouse.RelativeFlashesDirectory);
                                    }
                                    // копируем туда картинку
                                    string data = reader.GetAttribute("data");
                                    var destPath = Path.Combine(Warehouse.Warehouse.AbsoluteEditorFlashesDirectory, Path.GetFileName(data));
                                    var sourcePath = Path.Combine(Path.GetDirectoryName(qfPath), Path.GetFileName(data));

                                    File.Copy(sourcePath, destPath, true);

                                    string asf = Path.Combine(Warehouse.Warehouse.RelativeImagesDirectory, Path.GetFileName("\\Images\\Anim.png"));
                                    string sf = Path.Combine(Warehouse.Warehouse.RelativeFlashesDirectory, Path.GetFileName(data));
                                    HtmlText += string.Concat("<IMG src=\"" + asf + "\" width=\"16\" height=\"16\" sdocument=\"0\" src_=\"" + sf + "\" >");

                                }
                            }
                            catch (FileNotFoundException)
                            {
                                MessageBox.Show("Изображение " + reader.GetAttribute("src") + " не найдено.",
                                                "Visual Editor",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Error);
                            }
                        }*/

                        if (reader.Name.Equals("responseIf", StringComparison.OrdinalIgnoreCase) ||
                            reader.Name.Equals("responseElseIf", StringComparison.OrdinalIgnoreCase) ||
                            reader.Name.Equals("responseElse", StringComparison.OrdinalIgnoreCase))
                        {
                            string responseString = string.Empty;
                            string mark = string.Empty;
                            bool endCycle = false;
                            string name = reader.Name;

                            while (!endCycle && reader.Read())
                            {
                                if (reader.Name.Equals("baseValue", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (reader.GetAttribute("baseType").Equals("identifier", StringComparison.OrdinalIgnoreCase))
                                    {
                                        responseString = reader.ReadString();
                                    }
                                    else if (reader.GetAttribute("baseType").Equals("integer", StringComparison.OrdinalIgnoreCase) ||
                                             reader.GetAttribute("baseType").Equals("float", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (name.Equals("responseIf", StringComparison.OrdinalIgnoreCase))
                                        {
                                            string m = reader.ReadString();
                                            m = m.Replace(".", NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);
                                            m = m.Replace(",", NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);
                                            Marks = int.Parse(m);
                                            mark = m;
                                        }
                                        else
                                        {
                                            mark = reader.ReadString();
                                        }
                                    }
                                }
                                else if (reader.NodeType == XmlNodeType.EndElement)
                                {
                                    if (reader.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                                    {
                                        endCycle = true;
                                    }
                                }
                            }

                            if (!responseString.Equals(string.Empty))
                            {
                                ResponseVariant rv = new ResponseVariant(this);
                                rv.Responses.Add(responseString);
                                if (mark != string.Empty)
                                {
                                    if (!name.Equals("responseElse", StringComparison.OrdinalIgnoreCase))
                                    {
                                        mark = mark.Replace(".", NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);
                                        mark = mark.Replace(",", NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);
                                        if (Marks != 0)
                                            rv.Weight = Double.Parse(mark) / Marks;
                                        else rv.Weight = 0;
                                    }
                                }
                                ResponseVariants.Add(rv);
                            }
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("При чтении файла вопроса произошла ошибка. (" + qfPath + ")",
                                "Visual Editor",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                result = false;
            }
            finally
            {
                reader.Close();
                HtmlText += "</P>\n</BODY>\n</HTML>";
                //   FillImagesArray();
            }

            // мои художества
            this.DocumentHtml = HtmlText;
            success = true;
            return result;
        }
        #endregion
    }
}