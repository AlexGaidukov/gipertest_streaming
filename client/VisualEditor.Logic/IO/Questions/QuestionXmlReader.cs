using System.IO;
using System.Windows.Forms;
using System.Xml;
using VisualEditor.Logic.Course.Items;

namespace VisualEditor.Logic.IO.Questions
{
    internal abstract class QuestionXmlReader
    {
        protected Question question;

        public abstract void ReadXml(XmlTextReader xmlReader);

        #region XmlToHtml

        public static string XmlToHtml(string documentHtml)
        {
            var html = string.Copy(documentHtml);
            var mlp = new MlParser();
            string searchString;
            string value;
            string tag;

            #region Парсинг рисунка, ссылки на рисунок, формулы

            searchString = "<image";
            while (html.Contains(searchString))
            {
                Application.DoEvents();

                mlp.GetTagBounds(html, searchString);
                value = mlp.GetValue("view");

                if (value.Equals("0"))
                {
                    #region Рисунок

                    var src = mlp.GetValue("src");
                    var width = mlp.GetValue("width");
                    var height = mlp.GetValue("height");

                    if (width == string.Empty || height == string.Empty)
                    {
                        var src_ = Path.Combine(Warehouse.Warehouse.ProjectEditorLocation, src);
                        var imageDimension = System.Drawing.Image.FromFile(src_).PhysicalDimension;
                        width = imageDimension.Width.ToString();
                        height = imageDimension.Height.ToString();
                    }

                    tag = string.Concat("<IMG src=\"", src,
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

                    tag += string.Concat("\" sdocument=0>");

                    html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                    html = html.Insert(mlp.StartIndex, tag);

                    #endregion
                }
                else if (value.Equals("1"))
                {
                    #region Ссылка на рисунок

                    var src = mlp.GetValue("src");
                    var width = mlp.GetValue("width");
                    var height = mlp.GetValue("height");

                    if (width == string.Empty || height == string.Empty)
                    {
                        var src_ = Path.Combine(Warehouse.Warehouse.ProjectEditorLocation, src);
                        var imageDimension = System.Drawing.Image.FromFile(src_).PhysicalDimension;
                        width = imageDimension.Width.ToString();
                        height = imageDimension.Height.ToString();
                    }

                    tag = string.Concat("<IMG src=\"Images\\Pic.png\"",
                                        " src_=\"", mlp.GetValue("src"),
                                        "\" alt=\"", mlp.GetValue("title"),
                                        "\" width_=\"", mlp.GetValue("width"),
                                        "\" height_=\"", mlp.GetValue("height"));

                    var attribute = mlp.GetValue("align");
                    if (!attribute.Equals(string.Empty))
                    {
                        tag += string.Concat("\" align_=\"", attribute);
                    }

                    attribute = mlp.GetValue("border");
                    if (!attribute.Equals(string.Empty))
                    {
                        tag += string.Concat("\" border_=\"", attribute);
                    }

                    attribute = mlp.GetValue("hspace");
                    if (!attribute.Equals(string.Empty))
                    {
                        tag += string.Concat("\" hspace_=\"", attribute);
                    }

                    attribute = mlp.GetValue("vspace");
                    if (!attribute.Equals(string.Empty))
                    {
                        tag += string.Concat("\" vspace_=\"", attribute);
                    }

                    tag += string.Concat("\" sdocument=1>");

                    html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                    html = html.Insert(mlp.StartIndex, tag);

                    #endregion
                }
                else if (value.Equals(string.Empty))
                {
                    #region Формула

                    tag = string.Concat("<IMG src=\"", mlp.GetValue("src"),"\" align=\"",
                                        mlp.GetValue("align"),"\" longDesc=\"",
                                        mlp.GetValue("longDesc"), "\" />");

                    html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                    html = html.Insert(mlp.StartIndex, tag);

                    #endregion
                }
            }

            #endregion

            #region Парсинг анимации, ссылки на анимацию

            searchString = "<flash";
            while (html.Contains(searchString))
            {
                Application.DoEvents();

                mlp.GetTagBounds(html, searchString);
                value = mlp.GetValue("view");

                if (value.Equals("0"))
                {
                    #region Анимация

                    tag = string.Concat("<IMG src=\"Images\\Anim.png",
                                        "\" src_=\"", mlp.GetValue("src"),
                                        "\" sdocument=0>");

                    html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                    html = html.Insert(mlp.StartIndex, tag);

                    #endregion
                }
                else if (value.Equals("1"))
                {
                    #region Ссылка на анимацию

                    tag = string.Concat("<IMG src=\"Images\\Anim.png",
                                        "\" src_=\"", mlp.GetValue("src"),
                                        "\" alt=\"", mlp.GetValue("title"),
                                        "\" sdocument=1>");

                    html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                    html = html.Insert(mlp.StartIndex, tag);

                    #endregion
                }
            }

            #endregion

            #region Парсинг аудио, ссылки на аудио

            searchString = "<audio";
            while (html.Contains(searchString))
            {
                Application.DoEvents();

                mlp.GetTagBounds(html, searchString);
                value = mlp.GetValue("view");

                if (value.Equals("0"))
                {
                    #region Аудио

                    tag = string.Concat("<IMG src=\"Images\\Aud.png",
                                        "\" src_=\"", mlp.GetValue("src"),
                                        "\" sdocument=0>");

                    html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                    html = html.Insert(mlp.StartIndex, tag);

                    #endregion
                }
                else if (value.Equals("1"))
                {
                    #region Ссылка на аудио

                    tag = string.Concat("<IMG src=\"Images\\Aud.png",
                                        "\" src_=\"", mlp.GetValue("src"),
                                        "\" alt=\"", mlp.GetValue("title"),
                                        "\" sdocument=1>");

                    html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                    html = html.Insert(mlp.StartIndex, tag);

                    #endregion
                }
            }

            #endregion

            #region Парсинг видело, ссылки на видео

            searchString = "<video";
            while (html.Contains(searchString))
            {
                Application.DoEvents();

                mlp.GetTagBounds(html, searchString);
                value = mlp.GetValue("view");

                if (value.Equals("0"))
                {
                    #region Видео

                    tag = string.Concat("<IMG src=\"Images\\Vid.png",
                                        "\" src_=\"", mlp.GetValue("src"),
                                        "\" sdocument=0>");

                    html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                    html = html.Insert(mlp.StartIndex, tag);

                    #endregion
                }
                else if (value.Equals("1"))
                {
                    #region Ссылка на видео

                    tag = string.Concat("<IMG src=\"Images\\Vid.png",
                                        "\" src_=\"", mlp.GetValue("src"),
                                        "\" alt=\"", mlp.GetValue("title"),
                                        "\" sdocument=1>");

                    html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                    html = html.Insert(mlp.StartIndex, tag);

                    #endregion
                }
            }

            #endregion

            return html;
        }

        #endregion
    }
}