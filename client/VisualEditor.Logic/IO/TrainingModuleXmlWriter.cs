using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using VisualEditor.Logic.Course.Items;

namespace VisualEditor.Logic.IO
{
    internal class TrainingModuleXmlWriter
    {
        private readonly TrainingModule trainingModule;

        public TrainingModuleXmlWriter(TrainingModule trainingModule)
        {
            this.trainingModule = trainingModule;
        }

        public void WriteXml(XmlTextWriter xmlWriter)
        {
            foreach (var idc in trainingModule.InConceptParent.InDummyConcepts)
            {
                xmlWriter.WriteStartElement("input");
                xmlWriter.WriteAttributeString("concept_id", "#elem{" + idc.Concept.Id.ToString().ToUpper() + "}");
                xmlWriter.WriteEndElement();
            }

            foreach (var odc in trainingModule.OutConceptParent.OutDummyConcepts)
            {
                xmlWriter.WriteStartElement("output");
                xmlWriter.WriteAttributeString("concept_id", "#elem{" + odc.Concept.Id.ToString().ToUpper() + "}");
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteStartElement("html_text");
            xmlWriter.WriteAttributeString("id", "{" + Guid.NewGuid().ToString().ToUpper() + "}");
            xmlWriter.WriteCData(HtmlToXml(trainingModule));
            xmlWriter.WriteFullEndElement();
        }

        #region HtmlToXml

        private static string HtmlToXml(TrainingModule trainingModule)
        {
            var html = string.Copy(trainingModule.DocumentHtml);
            var mlp = new MlParser();
            string searchString;
            string value;
            string tag;

            #region Парсинг закладки

            searchString = "class=bookmark";
            while (html.Contains(searchString))
            {
                mlp.GetTagBounds(html, searchString);
                value = mlp.GetValue("id");

                tag = string.Concat("<anchor id=\"", value, "\" name_anchor=\"",
                                    Warehouse.Warehouse.GetBookmarkNameById(new Guid(value)), "\"></anchor>");

                mlp.ShiftLastIndex(ref html);
                html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                html = html.Insert(mlp.StartIndex, tag);
            }

            #endregion

            #region Парсинг компетенции

            searchString = "class=concept";
            while (html.Contains(searchString))
            {
                mlp.GetTagBounds(html, searchString);
                mlp.ShiftLastIndex(ref html);

                tag = string.Concat("<glossary_definition concept_id=\"#elem{", mlp.GetValue("id").ToUpper(),
                                    "}\" index=\"1\">", mlp.GetInnerHtml(),
                                    "</glossary_definition>");

                html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                html = html.Insert(mlp.StartIndex, tag);
            }

            #endregion

            #region Парсинг ссылки на закладку

            searchString = "class=linktobookmark";
            while (html.Contains(searchString))
            {
                mlp.GetTagBounds(html, searchString);
                mlp.ShiftLastIndex(ref html);

                tag = string.Concat("<ref type=\"reference\" linkitem_id=\"", ExtractRelativeHref(mlp.GetValue("href")),
                                    "\">", mlp.GetInnerHtml(), "</ref>");

                html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                html = html.Insert(mlp.StartIndex, tag);
            }

            #endregion

            #region Парсинг ссылки на внутреннюю компетенцию

            searchString = "class=linktointernalconcept";
            while (html.Contains(searchString))
            {
                mlp.GetTagBounds(html, searchString);
                mlp.ShiftLastIndex(ref html);

                tag = string.Concat("<ref type=\"concept\" linkitem_id=\"#elem{", ExtractRelativeHref(mlp.GetValue("href")).ToUpper(),
                                    "}\">", mlp.GetInnerHtml(), "</ref>");

                html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                html = html.Insert(mlp.StartIndex, tag);
            }

            #endregion

            #region Парсинг ссылки на внешнюю компетенцию

            // Парсинга не происходит.
            // При нажатии на ссылку выдает сообщение "Ссылка на внешнюю компетенцию не настроена".

            #endregion

            #region Парсинг ссылки на модуль

            searchString = "class=linktotrainingmodule";
            while (html.Contains(searchString))
            {
                mlp.GetTagBounds(html, searchString);
                mlp.ShiftLastIndex(ref html);

                tag = string.Concat("<ref type=\"module\" linkitem_id=\"#module{", ExtractRelativeHref(mlp.GetValue("href")).ToUpper(),
                                    "}\">", mlp.GetInnerHtml(), "</ref>");

                html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                html = html.Insert(mlp.StartIndex, tag);
            }

            #endregion

            #region Парсинг рисунка, анимации, аудио, видео, ссылки на них, формулы

            searchString = "<IMG";
            while (html.Contains(searchString))
            {
                mlp.GetTagBounds(html, searchString);
                value = mlp.GetValue("src_");
                if (!value.Equals(string.Empty))
                {
                    value = value.Substring(0, value.IndexOf("\\"));
                }
                // value может быть равно Flashes, Images или string.Empty.
                // value равно string.Empty, если обрабатывается рисунок или формула.

                if (value.Equals(Warehouse.Warehouse.RelativeFlashesDirectory))
                {
                    value = mlp.GetValue("sdocument");

                    if (value.Equals("0"))
                    {
                        #region Анимация

                        tag = string.Concat("<flash src=\"" + mlp.GetValue("src_"),
                                            "\" width=\"", "835",
                                            "\" height=\"", "615",
                                            "\" view=\"0\" />");

                        html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                        html = html.Insert(mlp.StartIndex, tag);

                        #endregion
                    }
                    else if (value.Equals("1"))
                    {
                        #region Ссылка на анимацию

                        tag = string.Concat("<flash src=\"" + mlp.GetValue("src_"),
                                            "\" width=\"", "835",
                                            "\" height=\"", "615",
                                            "\" title=\"", mlp.GetValue("alt"),
                                            "\" view=\"1\" />");

                        html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                        html = html.Insert(mlp.StartIndex, tag);

                        #endregion
                    }
                }
                else if (value.Equals(Warehouse.Warehouse.RelativeImagesDirectory) ||
                    value == string.Empty)
                {
                    value = mlp.GetValue("sdocument");

                    if (value.Equals("0"))
                    {
                        #region Рисунок

                        var src = mlp.GetValue("src");
                        src = ExtractRelativeSrc(src);
                        var width = mlp.GetValue("width");
                        var height = mlp.GetValue("height");

                        if (width == string.Empty || height == string.Empty)
                        {
                            var src_ = Path.Combine(Warehouse.Warehouse.ProjectEditorLocation, src);
                            var imageDimension = System.Drawing.Image.FromFile(src_).PhysicalDimension;
                            width = imageDimension.Width.ToString();
                            height = imageDimension.Height.ToString();
                        }

                        tag = string.Concat("<image src=\"", src,
                                            "\" width=\"", width,
                                            "\" height=\"", height);

                        var attribute = mlp.GetValue("style");
                        if (!attribute.Equals(string.Empty))
                        {
                            tag += string.Concat("\" style=\"", attribute);
                        }

                        attribute = mlp.GetValue("title");
                        if (!attribute.Equals(string.Empty))
                        {
                            tag += string.Concat("\" title=\"", attribute);
                        }

                        attribute = mlp.GetValue("align");
                        if (!attribute.Equals(string.Empty))
                        {
                            tag += string.Concat("\" align=\"", attribute);
                        }

                        attribute = mlp.GetValue("border");
                        if (!attribute.Equals(string.Empty))
                        {
                            tag += string.Concat("\" border=\"", attribute);
                        }

                        attribute = mlp.GetValue("hspace");
                        if (!attribute.Equals(string.Empty))
                        {
                            tag += string.Concat("\" hspace=\"", attribute);
                        }

                        attribute = mlp.GetValue("vspace");
                        if (!attribute.Equals(string.Empty))
                        {
                            tag += string.Concat("\" vspace=\"", attribute);
                        }

                        tag += "\" view=\"0\" />";

                        html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                        html = html.Insert(mlp.StartIndex, tag);

                        #endregion
                    }
                    else if (value.Equals("1"))
                    {
                        #region Ссылка на рисунок

                        var src = mlp.GetValue("src_");
                        var width = mlp.GetValue("width_");
                        var height = mlp.GetValue("height_");

                        if (width == string.Empty || height == string.Empty)
                        {
                            var src_ = Path.Combine(Warehouse.Warehouse.ProjectEditorLocation, src);
                            var imageDimension = System.Drawing.Image.FromFile(src_).PhysicalDimension;
                            width = imageDimension.Width.ToString();
                            height = imageDimension.Height.ToString();
                        }

                        tag = string.Concat("<image title=\"", mlp.GetValue("alt"), "\" src=\"", src, "\" width=\"",
                                            width, "\" height=\"", height);

                        var attribute = mlp.GetValue("align_");
                        if (!attribute.Equals(string.Empty))
                        {
                            tag += string.Concat("\" align=\"", attribute);
                        }

                        attribute = mlp.GetValue("border_");
                        if (!attribute.Equals(string.Empty))
                        {
                            tag += string.Concat("\" border=\"", attribute);
                        }

                        attribute = mlp.GetValue("hspace_");
                        if (!attribute.Equals(string.Empty))
                        {
                            tag += string.Concat("\" hspace=\"", attribute);
                        }

                        attribute = mlp.GetValue("vspace_");
                        if (!attribute.Equals(string.Empty))
                        {
                            tag += string.Concat("\" vspace=\"", attribute);
                        }

                        tag += "\" view=\"1\" />";

                        html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                        html = html.Insert(mlp.StartIndex, tag);

                        #endregion
                    }
                    else if (value.Equals(string.Empty))
                    {
                        #region Формула

                        var src = mlp.GetValue("src");
                        src = ExtractRelativeSrc(src);

                        tag = string.Concat("<image src=\"", src,"\" align=\"",
                                            mlp.GetValue("align"), "\" longDesc=\"",
                                            mlp.GetValue("longDesc"), "\" />");

                        html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                        html = html.Insert(mlp.StartIndex, tag);

                        #endregion
                    }
                }
                else if (value.Equals(Warehouse.Warehouse.RelativeAudiosDirectory))
                {
                    value = mlp.GetValue("sdocument");

                    if (value.Equals("0"))
                    {
                        #region Аудио

                        tag = string.Concat("<audio src=\"" + mlp.GetValue("src_"),
                                            "\" view=\"0\" />");

                        html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                        html = html.Insert(mlp.StartIndex, tag);

                        #endregion
                    }
                    else if (value.Equals("1"))
                    {
                        #region Ссылка на аудио

                        tag = string.Concat("<audio src=\"" + mlp.GetValue("src_"),
                                            "\" title=\"", mlp.GetValue("alt"),
                                            "\" view=\"1\" />");

                        html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                        html = html.Insert(mlp.StartIndex, tag);

                        #endregion
                    }
                }
                else if (value.Equals(Warehouse.Warehouse.RelativeVideosDirectory))
                {
                    value = mlp.GetValue("sdocument");

                    if (value.Equals("0"))
                    {
                        #region Видео

                        tag = string.Concat("<video src=\"" + mlp.GetValue("src_"),
                                            "\" view=\"0\" />");

                        html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                        html = html.Insert(mlp.StartIndex, tag);

                        #endregion
                    }
                    else if (value.Equals("1"))
                    {
                        #region Ссылка на видео

                        tag = string.Concat("<video src=\"" + mlp.GetValue("src_"),
                                            "\" title=\"", mlp.GetValue("alt"),
                                            "\" view=\"1\" />");

                        html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                        html = html.Insert(mlp.StartIndex, tag);

                        #endregion
                    }
                }
            }

            #endregion

            #region Парсинг греческих символов

            var greekSymbols = Regex.Matches(html, "[Α-Ωα-ω¡¢£¤¥¦§¨©ª«¬­®¯°±²³´µ¶·¸¹º»¼½¾¿™•∑∏∫∂√∞ƒ≤≥≠≡…′″∃∈∋∧∨∩∪∼≈⊂⊃⊆⊇⊕⊥°→×÷∀]", RegexOptions.IgnoreCase);
            int index;

            if (greekSymbols.Count != 0)
            {
                for (var i = greekSymbols.Count; i > 0; i--)
                {
                    var symbol = greekSymbols[i - 1].Value;
                    index = greekSymbols[i - 1].Index;
                    html = html.Remove(index, 1);
                    html = html.Insert(index, string.Concat("&#", char.ConvertToUtf32(symbol, 0), ";"));
                }
            }

            greekSymbols = Regex.Matches(html, "[Ë]", RegexOptions.IgnoreCase);
            if (greekSymbols.Count != 0)
            {
                for (var i = greekSymbols.Count; i > 0; i--)
                {
                    index = greekSymbols[i - 1].Index;
                    html = html.Remove(index, 1);
                    html = html.Insert(index, "&Euml;");
                }
            }

            greekSymbols = Regex.Matches(html, "[Ï]", RegexOptions.IgnoreCase);
            if (greekSymbols.Count != 0)
            {
                for (var i = greekSymbols.Count; i > 0; i--)
                {
                    index = greekSymbols[i - 1].Index;
                    html = html.Remove(index, 1);
                    html = html.Insert(index, "&Iuml;");
                }
            }

            greekSymbols = Regex.Matches(html, "[Æ]", RegexOptions.IgnoreCase);
            if (greekSymbols.Count != 0)
            {
                for (var i = greekSymbols.Count; i > 0; i--)
                {
                    index = greekSymbols[i - 1].Index;
                    html = html.Remove(index, 1);
                    html = html.Insert(index, "&AElig;");
                }
            }

            greekSymbols = Regex.Matches(html, "[Ä]", RegexOptions.IgnoreCase);
            if (greekSymbols.Count != 0)
            {
                for (var i = greekSymbols.Count; i > 0; i--)
                {
                    index = greekSymbols[i - 1].Index;
                    html = html.Remove(index, 1);
                    html = html.Insert(index, "&Auml;");
                }
            }

            greekSymbols = Regex.Matches(html, "[Þ]", RegexOptions.IgnoreCase);
            if (greekSymbols.Count != 0)
            {
                for (var i = greekSymbols.Count; i > 0; i--)
                {
                    index = greekSymbols[i - 1].Index;
                    html = html.Remove(index, 1);
                    html = html.Insert(index, "&THORN;");
                }
            }

            #endregion

            // POSTPONE: Протестировать.
            //html = Regex.Replace(html, "<[ \n\t]*/[ \n\t]*sup[ \n\t]*>", "</SUP>", RegexOptions.IgnoreCase);

            return html;
        }

        #endregion

        #region ExtractRelativeSrc

        public static string ExtractRelativeSrc(string rawSrc)
        {
            if (string.IsNullOrEmpty(rawSrc))
            {
                throw new ArgumentNullException();
            }

            if (rawSrc.StartsWith("file:///"))
            {
                var index = rawSrc.IndexOf(Warehouse.Warehouse.RelativeImagesDirectory);
                index += Warehouse.Warehouse.RelativeImagesDirectory.Length + 1;
                var src = rawSrc.Substring(index, rawSrc.Length - index);
                src = string.Concat(Warehouse.Warehouse.RelativeImagesDirectory, "\\", src);

                return src;
            }

            return rawSrc;
        }

        #endregion

        #region ExtractRelativeHref

        public static string ExtractRelativeHref(string rawHref)
        {
            if (string.IsNullOrEmpty(rawHref))
            {
                throw new ArgumentNullException();
            }

            if (rawHref.StartsWith("file:///"))
            {
                var index = rawHref.LastIndexOf("/") + 1;
                var href = rawHref.Substring(index, rawHref.Length - index);

                return href;
            }

            return rawHref;
        }

        #endregion
    }
}