using VisualEditor.Logic.IO.Questions;
using System.Xml;
using System.IO;
using System;
using System.Windows.Forms;
using System.Globalization;

namespace VisualEditor.Logic.Course.Items.Questions
{
    internal class OrderingQuestion : Question
    {
        public OrderingQuestion()
        {
            XmlWriter = new OrderingQuestionXmlWriter(this);
            XmlReader = new OrderingQuestionXmlReader(this);
            type = Enums.QuestionType.Ordering;
            interactionType = Enums.InteractionType.orderInteraction;
            identifier = identifier + type.ToString();
            cardinality = "ordered";
            baseType = "identifier";
            maxChoices = 0;
            fileName = "resources\\" + identifier + ".xml";
        }

        #region пишем в imsqti
        /// <summary>
        /// Запись вопроса в файл формата IMS QTI 2.1.
        /// </summary>
        /// <param name="fileName">
        /// Желаемое имя файла вопроса. Если null, имя вопроса будет сформировано по идентификатору вопроса.
        /// </param>
        public override void WriteQti(string fileName)
        {
            //TODO* toolversion писать в соответствии с версией

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


            #endregion

            #region медиа из ответов
            foreach (Response resp in Responses)
            {
                dhtml = resp.DocumentHtml;
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
            }
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
            if (ResponseVariants.Count <= 1)
            {
                questionWriter.WriteStartElement("correctResponse");
                //записываем правильные ответы                
                if (ResponseVariants.Count > 0)
                {
                    for (int i = 1; i <= Responses.Count; i++)
                    {
                        foreach (int respNumber in ((ResponseVariant)ResponseVariants[0]).Responses)
                        {
                            if (respNumber == i)
                            {
                                questionWriter.WriteElementString("value",
                                    ((Response)Responses[((ResponseVariant)ResponseVariants[0]).Responses.IndexOf(respNumber)]).Identifier);
                            }
                        }
                    }
                    foreach (Response r in Responses)
                    {
                        r.MappedValue = 1.0 / ((ResponseVariant)ResponseVariants[0]).Responses.Count;
                    }
                }
                questionWriter.WriteFullEndElement(); // correctResponse                
            }
            questionWriter.WriteFullEndElement(); // responseDeclaration
            #endregion //responseDeclaration
            questionWriter.WriteStartElement("outcomeDeclaration");
            questionWriter.WriteAttributeString("identifier", "SCORE");
            questionWriter.WriteAttributeString("cardinality", "single");
            questionWriter.WriteAttributeString("baseType", "integer");
            questionWriter.WriteFullEndElement(); // outcomeDeclaration
            #region itemBody
            questionWriter.WriteStartElement("itemBody");
            // копируем содержимое тега BODY
            tempHtml = DocumentHtml;// HtmlText.Substring(HtmlText.IndexOf("<BODY>") + 6,
            // HtmlText.IndexOf("</BODY>") - HtmlText.IndexOf("<BODY>") - 7);
            tempHtml = tempHtml.Trim();
            // конвертируем html текст вопроса в xml
            QuestionHtmlToXml(tempHtml, questionWriter);
            #region choiceInteraction
            questionWriter.WriteStartElement("orderInteraction");
            questionWriter.WriteAttributeString("responseIdentifier", identifier);
            questionWriter.WriteAttributeString("shuffle", isShuffle.ToString());
            foreach (Response response in Responses)
            {
                // копируем все картинки, использованные в вопросе, в упаковку 
                /* реализовано выше
                if (response.images.Count != 0)
                {
                    foreach (string name in response.images)
                    {
                        File.Copy(name, Path.GetDirectoryName(fileName) + "/" + Path.GetFileName(name), true);
                    }
                }*/
                // копируем содержимое тега BODY
                tempHtml = response.DocumentHtml;// response.HtmlText.Substring(response.HtmlText.IndexOf("<BODY>") + 6,
                // response.HtmlText.IndexOf("</BODY>") - response.HtmlText.IndexOf("<BODY>") - 7);
                tempHtml = tempHtml.Trim();
                questionWriter.WriteStartElement(response.Type.ToString());
                questionWriter.WriteAttributeString("identifier", response.Identifier);
                questionWriter.WriteAttributeString("fixed", response.IsFixed.ToString());
                // конвертируем html текст ответа в xml
                ResponseHtmlToXml(tempHtml, questionWriter);
                questionWriter.WriteFullEndElement(); //response.Type.ToString()
            }
            questionWriter.WriteFullEndElement(); //orderInteraction
            #endregion //choiceInteraction
            questionWriter.WriteFullEndElement(); //itemBody
            #endregion //itemBody
            questionWriter.WriteStartElement("responseProcessing");
            if (ResponseVariants.Count == 1)
            {
                questionWriter.WriteAttributeString("template",
                    "http://www.imsglobal.org/question/qti_v2p1/rptemplates/match_correct/");
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
                questionWriter.WriteStartElement("ordered");
                // записываем элементы ответов в порядке следования
                for (int i = 1; i <= Responses.Count; i++)
                {
                    questionWriter.WriteStartElement("baseValue");
                    questionWriter.WriteAttributeString("baseType", "identifier");
                    foreach (int r in ((ResponseVariant)ResponseVariants[0]).Responses)
                    {
                        if (r == i)
                        {
                            questionWriter.WriteString(((Response)Responses[((ResponseVariant)ResponseVariants[0]).Responses.IndexOf(r)]).Identifier);
                            break;
                        }
                    }
                    questionWriter.WriteFullEndElement(); // baseValue
                }
                questionWriter.WriteFullEndElement(); // ordered                
                questionWriter.WriteFullEndElement(); // match
                questionWriter.WriteStartElement("setOutcomeValue");
                questionWriter.WriteAttributeString("identifier", "SCORE");
                questionWriter.WriteStartElement("baseValue");
                questionWriter.WriteAttributeString("baseType", "integer");
                questionWriter.WriteString((((ResponseVariant)ResponseVariants[0]).Weight * Marks).ToString());
                questionWriter.WriteFullEndElement(); // baseValue
                questionWriter.WriteFullEndElement(); // setOutcomeValue
                questionWriter.WriteFullEndElement(); // responseIf 
                //responseElseIf
                for (int i = 1; i < ResponseVariants.Count; i++)
                {
                    questionWriter.WriteStartElement("responseElseIf");
                    questionWriter.WriteStartElement("match");
                    questionWriter.WriteStartElement("variable");
                    questionWriter.WriteAttributeString("identifier", identifier);
                    questionWriter.WriteFullEndElement(); // variable
                    questionWriter.WriteStartElement("ordered");
                    for (int j = 1; j <= Responses.Count; j++)
                    {
                        questionWriter.WriteStartElement("baseValue");
                        questionWriter.WriteAttributeString("baseType", "identifier");
                        foreach (int r in ((ResponseVariant)ResponseVariants[i]).Responses)
                        {
                            if (r == j)
                            {
                                questionWriter.WriteString(((Response)Responses[((ResponseVariant)ResponseVariants[i]).Responses.IndexOf(r)]).Identifier);
                                break;
                            }
                        }
                        questionWriter.WriteFullEndElement(); // baseValue
                    }
                    questionWriter.WriteFullEndElement(); // ordered
                    questionWriter.WriteFullEndElement(); // match
                    questionWriter.WriteStartElement("setOutcomeValue");
                    questionWriter.WriteAttributeString("identifier", "SCORE");
                    questionWriter.WriteStartElement("baseValue");
                    questionWriter.WriteAttributeString("baseType", "integer");
                    questionWriter.WriteString((((ResponseVariant)ResponseVariants[i]).Weight * Marks).ToString());
                    questionWriter.WriteFullEndElement(); // baseValue
                    questionWriter.WriteFullEndElement(); // setOutcomeValue
                    questionWriter.WriteFullEndElement(); // responseElseIf
                } // for (int i = 1; i < responseVariants.Count; i++)                              
                questionWriter.WriteStartElement("responseElse"); //responseElse
                questionWriter.WriteStartElement("setOutcomeValue");
                questionWriter.WriteAttributeString("identifier", "SCORE");
                questionWriter.WriteStartElement("baseValue");
                questionWriter.WriteAttributeString("baseType", "integer");
                questionWriter.WriteString("0");
                questionWriter.WriteFullEndElement(); // baseValue
                questionWriter.WriteFullEndElement(); // setOutcomeValue
                questionWriter.WriteFullEndElement(); // responseElse               
                questionWriter.WriteFullEndElement(); // responseCondition
            } // else if (responseVariants.Count > 1)
            questionWriter.WriteFullEndElement(); //responseProcessing 
            questionWriter.WriteFullEndElement(); // assessmentItem
            #endregion // question
            questionWriter.Close();
        }
        #endregion

        #region читаем из imsqti
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
                //"вынимаем" элементы ответа               
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name.Equals("simpleChoice"))
                        {
                            Response r = new Response();
                            r.DocumentHtml = "<HTML>\n<HEAD>\n<BASE href=\"" + Application.StartupPath +
                                         "\\\">\n</HEAD>\n<BODY>\n<P>";
                            if (reader.GetAttribute("identifier") != null)
                            {
                                try
                                {
                                    r.Id = Guid.NewGuid();//new Guid(reader.GetAttribute("identifier"));
                                    r.Text = reader.GetAttribute("identifier");
                                    r.Identifier = reader.GetAttribute("identifier");
                                }
                                catch
                                {
                                    r.Identifier = reader.GetAttribute("identifier");
                                    r.Id = Guid.NewGuid();
                                    r.Text = reader.GetAttribute("identifier");
                                }
                            }
                            if (reader.GetAttribute("fixed") != null)
                            {
                                r.IsFixed = bool.Parse(reader.GetAttribute("fixed"));
                            }
                            string s = reader.ReadInnerXml();
                            r.DocumentHtml += ConvertXmlStringToHtml(s, qfPath);
                            r.DocumentHtml += "</P>\n</BODY>\n</HTML>";
                            // r.FillImagesArray();
                            // Responses.Add(r);
                            Nodes.Add(r);
                        }
                    }
                }
                reader.Close();

                //затем "вынимаем" всё остальное
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
                            int counter = 1;
                            bool endCycle = false;
                            ResponseVariant rv = new ResponseVariant(this);
                            rv.Weight = 1.0;

                            for (int i = 0; i < Responses.Count; i++)
                            {
                                rv.Responses.Add(0);
                            }

                            while (!endCycle && reader.Read())
                            {
                                if (reader.Name.Equals("value", StringComparison.OrdinalIgnoreCase))
                                {
                                    string value = reader.ReadString();
                                    foreach (Response response in Responses)
                                    {
                                        if ((response.Id.ToString() == value) || response.Identifier == value)
                                        {
                                            rv.Responses[Responses.IndexOf(response)] = counter;
                                            counter++;
                                            break;
                                        }
                                    }
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

                        if (reader.Name.Equals("mapping", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                if (reader.GetAttribute("lowerBound") != null)
                                    LowerBound = int.Parse(reader.GetAttribute("lowerBound"));
                                if (reader.GetAttribute("upperBound") != null)
                                    Marks = int.Parse(reader.GetAttribute("upperBound"));
                                if (reader.GetAttribute("defaultValue") != null)
                                    DefaultValue = int.Parse(reader.GetAttribute("defaultValue"));
                            }
                            catch
                            {
                            }
                        }

                        if (reader.Name.Equals("mapEntry", StringComparison.OrdinalIgnoreCase))
                        {
                            foreach (Response response in Responses)
                            {
                                if (response.Id.ToString() == reader.GetAttribute("mapKey"))
                                {
                                    if (reader.GetAttribute("mappedValue") != null)
                                    {
                                        string s = reader.GetAttribute("mappedValue");
                                        // разделитель, используемый в системе
                                        string decimalSeparator = NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator;
                                        // подменяем точки и запятые на разделитель, используемый в системе, чтобы преобразовать строку в число без ошибок
                                        s = s.Replace(".", decimalSeparator);
                                        s = s.Replace(",", decimalSeparator);
                                        double d;
                                        Double.TryParse(s, out d);
                                        response.MappedValue = d;
                                    }
                                }
                            }
                        }

                        if (reader.Name.Equals("itemBody", StringComparison.OrdinalIgnoreCase))
                        {
                            // считываем содержимое тега itemBody
                            while (!reader.Name.Equals("orderInteraction", StringComparison.OrdinalIgnoreCase) && reader.Read())
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
                        }

                        if (reader.Name.Equals("orderInteraction", StringComparison.OrdinalIgnoreCase))
                        {
                            if (reader.GetAttribute("responseIdentifier") != null)
                            {
                                identifier = reader.GetAttribute("responseIdentifier");
                            }
                            if (reader.GetAttribute("shuffle") != null)
                            {
                                IsShuffle = bool.Parse(reader.GetAttribute("shuffle"));
                            }
                            if (reader.GetAttribute("maxChoices") != null)
                            {
                                MaxChoices = int.Parse(reader.GetAttribute("maxChoices"));
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
                            string mark = string.Empty;
                            bool endCycle = false;
                            string name = reader.Name;

                            while (!endCycle && reader.Read())
                            {
                                if (reader.Name.Equals("ordered", StringComparison.OrdinalIgnoreCase))
                                {
                                    ResponseVariant rv = new ResponseVariant(this);
                                    int counter = 1;
                                    for (int i = 0; i < Responses.Count; i++)
                                    {
                                        rv.Responses.Add(0);
                                    }
                                    while (!endCycle && reader.Read())
                                    {
                                        if (reader.Name.Equals("baseValue", StringComparison.OrdinalIgnoreCase))
                                        {
                                            string value = reader.ReadString();
                                            foreach (Response response in Responses)
                                            {
                                                if ((response.Id.ToString() == value) || response.Identifier == value)
                                                {
                                                    rv.Responses[Responses.IndexOf(response)] = counter;
                                                    counter++;
                                                    break;
                                                }
                                            }
                                        }
                                        else if (reader.NodeType == XmlNodeType.EndElement)
                                        {
                                            if (reader.Name.Equals("ordered", StringComparison.OrdinalIgnoreCase))
                                            {
                                                endCycle = true;
                                            }
                                        }
                                    }
                                    endCycle = false;
                                    ResponseVariants.Add(rv);
                                }

                                if (reader.Name.Equals("baseValue", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (reader.GetAttribute("baseType").Equals("integer", StringComparison.OrdinalIgnoreCase) ||
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

                            if (mark != string.Empty)
                            {
                                if (!name.Equals("responseElse", StringComparison.OrdinalIgnoreCase))
                                {
                                    mark = mark.Replace(".", NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);
                                    mark = mark.Replace(",", NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);
                                    if (Marks != 0)
                                        ((ResponseVariant)ResponseVariants[ResponseVariants.Count - 1]).Weight = Double.Parse(mark) / Marks;
                                    else ((ResponseVariant)ResponseVariants[ResponseVariants.Count - 1]).Weight = 0;
                                }
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
                // FillImagesArray();
            }

            // мои художества
            this.DocumentHtml = HtmlText;

            this.success = true;
            return result;
        }
        #endregion
    }
}