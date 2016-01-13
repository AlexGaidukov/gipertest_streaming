using VisualEditor.Logic.IO.Questions;
using System.Xml;
using System.IO;
using VisualEditor.Logic.Course.Items;
using System.Text.RegularExpressions;
using System;
using System.Windows.Forms;
using System.Collections;
using System.Globalization;
using VisualEditor.Utils.Controls.HtmlEditing;

namespace VisualEditor.Logic.Course.Items.Questions
{
    internal class ChoiceQuestion : Question
    {
        public ChoiceQuestion()
        {
            XmlWriter = new ChoiceQuestionXmlWriter(this);
            XmlReader = new ChoiceQuestionXmlReader(this);

            cardinality = "single";
            type = Enums.QuestionType.Choice;
            interactionType = Enums.InteractionType.choiceInteraction;
            identifier = identifier + type.ToString();
            fileName = "resources\\" + identifier + ".xml";
            baseType = "integer";//был identifier 
            maxChoices = 1;       // сколько может быть выбрано ответов
            // dhtml = this.DocumentHtml;
            // var MediaFiles = new li
            // MmediaFiles.
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
            // string tag;
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

            for (int i = 0; i < Responses.Count; i++)
            {
                ((Response)Responses[i]).Identifier = "a" + i.ToString();
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
            //*
            questionWriter.WriteAttributeString("toolversion", "2.0");
            //*
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
                //записываем правильные ответы
                foreach (Response resp in ((ResponseVariant)ResponseVariants[0]).Responses)
                {
                    questionWriter.WriteElementString("value", resp.Identifier);
                }
                questionWriter.WriteFullEndElement(); // correctResponse 
                questionWriter.WriteStartElement("mapping");
                questionWriter.WriteAttributeString("lowerBound", lowerBound.ToString());
                questionWriter.WriteAttributeString("upperBound", Marks.ToString());
                questionWriter.WriteAttributeString("defaultValue", defaultValue.ToString());
                foreach (Response resp in Responses)
                {
                    if (resp.MappedValue != 0)
                    {
                        questionWriter.WriteStartElement("mapEntry");
                        questionWriter.WriteAttributeString("mapKey", resp.Identifier);
                        questionWriter.WriteAttributeString("mappedValue", (mark * resp.MappedValue).ToString("0.##"));
                        questionWriter.WriteFullEndElement(); // mapEntry
                    }
                }
                questionWriter.WriteFullEndElement(); // mapping
            }
            questionWriter.WriteFullEndElement(); // responseDeclaration
            #endregion //responseDeclaration
            questionWriter.WriteStartElement("outcomeDeclaration");
            questionWriter.WriteAttributeString("identifier", "SCORE");
            questionWriter.WriteAttributeString("cardinality", "single");
            questionWriter.WriteAttributeString("baseType", "integer");
            questionWriter.WriteStartElement("defaultValue");
            questionWriter.WriteElementString("value", defaultValue.ToString());
            questionWriter.WriteFullEndElement(); // defaultValue
            questionWriter.WriteFullEndElement(); // outcomeDeclaration
            #region itemBody
            questionWriter.WriteStartElement("itemBody");
            // копируем содержимое тега BODY
            tempHtml = DocumentHtml;//.Substring( DocumentHtml.IndexOf("<BODY>") + 6,
            //DocumentHtml.IndexOf("</BODY>") - DocumentHtml.IndexOf("<BODY>") - 7);
            tempHtml = tempHtml.Trim();
            // конвертируем html текст вопроса в xml
            QuestionHtmlToXml(tempHtml, questionWriter);
            #region choiceInteraction
            questionWriter.WriteStartElement("choiceInteraction");
            questionWriter.WriteAttributeString("responseIdentifier", identifier);
            questionWriter.WriteAttributeString("shuffle", isShuffle.ToString());
            questionWriter.WriteAttributeString("maxChoices", MaxChoices.ToString());
            foreach (Response response in Responses)
            {
                // копируем все картинки, использованные в вопросе, в упаковку 
                #region решить  //решено выше
                /*
                if (response.images.Count != 0)
                {
                    foreach (string name in response.images)
                    {
                        File.Copy(name, Path.GetDirectoryName(fileName) + "/" + Path.GetFileName(name), true);
                    }
                }*/
                #endregion


                // копируем содержимое тега BODY
                tempHtml = response.DocumentHtml;//.Substring(response.DocumentHtml.IndexOf("<BODY>") + 6,
                //response.DocumentHtml.IndexOf("</BODY>") - response.DocumentHtml.IndexOf("<BODY>") - 7);
                tempHtml = tempHtml.Trim();
                questionWriter.WriteStartElement(response.Type.ToString());
                questionWriter.WriteAttributeString("identifier", response.Identifier);
                questionWriter.WriteAttributeString("fixed", response.IsFixed.ToString());
                // конвертируем html текст ответа в xml
                ResponseHtmlToXml(tempHtml, questionWriter);
                questionWriter.WriteFullEndElement(); //response.Type.ToString()
            }
            questionWriter.WriteFullEndElement(); //choiceInteraction
            #endregion //choiceInteraction
            questionWriter.WriteFullEndElement(); //itemBody
            #endregion //itemBody
            questionWriter.WriteStartElement("responseProcessing");
            if (ResponseVariants.Count == 1)
            {
                questionWriter.WriteAttributeString("template",
                    "http://www.imsglobal.org/question/qti_v2p1/rptemplates/match_correct");
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
                string response = string.Empty;
                foreach (Response r in ((ResponseVariant)ResponseVariants[0]).Responses)
                {
                    response += r.Identifier + " ";
                }
                questionWriter.WriteString(response);
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
                    string resp = string.Empty;
                    foreach (Response r in ((ResponseVariant)ResponseVariants[i]).Responses)
                    {
                        resp += r.Identifier + " ";
                    }
                    questionWriter.WriteString(resp);
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
            string HtmlText = "<HTML>\n<HEAD>\n<BASE href=\"" + Application.StartupPath + // раньше было определено где-то там
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
                            // реализовать 
                            //  r.FillImagesArray();
                            // this.Responses.Add(r);
                            Nodes.Add(r);
                            //  Responses.Add(r);
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
                                identifier = reader.GetAttribute("identifier"); //был с заглавной
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
                                    string value = reader.ReadString();
                                    foreach (Response response in Responses)
                                    {
                                        if ((response.Id.ToString() == value) || response.Identifier == value)
                                        {
                                            rv.Responses.Add(response);
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
                            if (reader.GetAttribute("lowerBound") != null)
                                LowerBound = int.Parse(reader.GetAttribute("lowerBound"));
                            if (reader.GetAttribute("upperBound") != null)
                                Marks = int.Parse(reader.GetAttribute("upperBound"));
                            if (reader.GetAttribute("defaultValue") != null)
                                DefaultValue = int.Parse(reader.GetAttribute("defaultValue"));
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
                            while (!reader.Name.Equals("choiceInteraction", StringComparison.OrdinalIgnoreCase) && reader.Read())
                            {
                                if (reader.Name.Equals("p", StringComparison.OrdinalIgnoreCase))
                                {
                                    string xml = reader.ReadInnerXml();
                                    string html = ConvertXmlStringToHtml(xml, qfPath);
                                    if (html != " ")
                                    {
                                        HtmlText += "<P>" + html + "</P>";
                                    }
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

                        if (reader.Name.Equals("choiceInteraction", StringComparison.OrdinalIgnoreCase))
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
                        //////////////////////                     \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
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
                        ////////////////////----------------------------------------------------------------

                        if (reader.Name.Equals("responseIf", StringComparison.OrdinalIgnoreCase) ||
                            reader.Name.Equals("responseElseIf", StringComparison.OrdinalIgnoreCase) ||
                            reader.Name.Equals("responseElse", StringComparison.OrdinalIgnoreCase))
                        {
                            ArrayList list = new ArrayList(0);
                            string mark = string.Empty;
                            bool endCycle = false;
                            string name = reader.Name;

                            while (!endCycle && reader.Read())
                            {
                                if (reader.Name.Equals("baseValue", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (reader.GetAttribute("baseType").Equals("identifier", StringComparison.OrdinalIgnoreCase))
                                    {
                                        string response = reader.ReadString();
                                        // разбиваем строку на слова
                                        Match match = Regex.Match(response, @"\w+");
                                        // цикл по всем словам
                                        while (match.Success)
                                        {
                                            list.Add(match.ToString());
                                            match = match.NextMatch();
                                        }
                                    }
                                    else if (reader.GetAttribute("baseType").Equals("integer", StringComparison.OrdinalIgnoreCase) ||
                                             reader.GetAttribute("baseType").Equals("float", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (name.Equals("responseIf", StringComparison.OrdinalIgnoreCase))
                                        {
                                            string m = reader.ReadString();
                                            m = m.Replace(".", NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);
                                            m = m.Replace(",", NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);
                                            Marks = int.Parse(m);// Double.Parse(m);
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

                            if (list.Count > 0)
                            {
                                ResponseVariant rv = new ResponseVariant(this);
                                foreach (Response r in Responses)
                                {
                                    foreach (string id in list)
                                    {
                                        if (r.Identifier == id && !rv.Responses.Contains(id))
                                        {
                                            rv.Responses.Add(r);
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
                //решить 
                //FillImagesArray();
            }

            // мои художества
            this.DocumentHtml = HtmlText;

            success = true;
            return result;
        }

        #endregion






    }
}