using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using VisualEditor.Logic.Course.Items;

namespace VisualEditor.Logic.IO.Questions
{
    internal abstract class QuestionXmlWriter
    {
        protected Question question;

        public abstract void WriteXml(XmlTextWriter xmlWriter);

        #region HtmlToXml

        public static string HtmlToXml(string documentHtml)
        {
            var html = string.Copy(documentHtml);
            var mlp = new MlParser();
            string searchString;
            string value;
            string tag;

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
                        src = TrainingModuleXmlWriter.ExtractRelativeSrc(src);
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
                        src = TrainingModuleXmlWriter.ExtractRelativeSrc(src);

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

            return html;
        }

        #endregion
    }
}