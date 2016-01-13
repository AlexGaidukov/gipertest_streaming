using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using VisualEditor.Logic.Course.Items;

namespace VisualEditor.Logic.IO
{
    internal class TrainingModuleXmlReader
    {
        private TrainingModule trainingModule;

        static TrainingModuleXmlReader()
        {
            BookmarksIds = new Hashtable();
        }

        public TrainingModuleXmlReader(TrainingModule trainingModule)
        {
            this.trainingModule = trainingModule;
        }

        public static Hashtable BookmarksIds { get; private set; }

        #region XmlToHtml

        public string XmlToHtml(string documentHtml)
        {
            var html = string.Copy(documentHtml);
            var mlp = new MlParser();
            string searchString;
            string value;
            string tag;

            #region Парсинг закладки

            searchString = "<anchor";
            while (html.Contains(searchString))
            {
                Application.DoEvents();

                mlp.GetTagBounds(html, searchString);
                value = mlp.GetValue("id");
                value = value.ToLower();
                mlp.ShiftLastIndex(ref html);

                // Заменяет id закладки типа #a1 на Guid.
                if (!value.Length.Equals(36))
                {
                    value = ((Guid)BookmarksIds[value]).ToString();
                }

                tag = string.Concat("<A id=", value, " class=bookmark></A>");

                html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                html = html.Insert(mlp.StartIndex, tag);

                var b = new Bookmark
                {
                    Id = new Guid(value),
                    ModuleId = trainingModule.Id,
                    Text = mlp.GetValue("name_anchor")
                };
                Warehouse.Warehouse.Instance.Bookmarks.Add(b);
            }

            #endregion

            #region Парсинг компетенции

            searchString = "<glossary_definition";
            while (html.Contains(searchString))
            {
                Application.DoEvents();

                mlp.GetTagBounds(html, searchString);
                if (html[mlp.StartIndex] == '<' && html[mlp.StartIndex + 1] != '/')
                {
                    value = mlp.GetValue("concept_id");
                    value = value.Substring(6, 36);
                    value = value.ToLower();
                    mlp.ShiftLastIndex(ref html);

                    tag = string.Concat("<A id=", value, " class=concept>", mlp.GetInnerHtml(), "</A>");

                    html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                    html = html.Insert(mlp.StartIndex, tag);
                }
            }

            #endregion

            #region Парсинг ссылки на закладку

            searchString = "type=\"reference\"";
            while (html.Contains(searchString))
            {
                Application.DoEvents();

                mlp.GetTagBounds(html, searchString);
                value = mlp.GetValue("linkitem_id");
                value = value.ToLower();
                // Заменяет id закладки типа #a1 на Guid.
                if (!value.Length.Equals(36))
                {
                    value = ((Guid)BookmarksIds[value]).ToString();
                }

                mlp.ShiftLastIndex(ref html);

                tag = string.Concat("<A href=", value, " class=linktobookmark>", mlp.GetInnerHtml(), "</A>");

                html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                html = html.Insert(mlp.StartIndex, tag);

                var lto = new LinkToObject();
                lto.TrainingModule = trainingModule;
                lto.ObjectId = new Guid(value);
                Warehouse.Warehouse.Instance.LinksToObjects.Add(lto);
            }

            #endregion

            #region Парсинг ссылки на внутреннюю компетенцию

            searchString = "type=\"concept\"";
            while (html.Contains(searchString))
            {
                Application.DoEvents();

                mlp.GetTagBounds(html, searchString);
                value = mlp.GetValue("linkitem_id");
                value = value.Substring(6, 36);
                value = value.ToLower();
                mlp.ShiftLastIndex(ref html);

                tag = string.Concat("<A href=", value, " class=linktointernalconcept>", mlp.GetInnerHtml(), "</A>");

                html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                html = html.Insert(mlp.StartIndex, tag);

                LinkToObject lto = new LinkToObject();
                lto.TrainingModule = trainingModule;
                lto.ObjectId = new Guid(value);
                Warehouse.Warehouse.Instance.LinksToObjects.Add(lto);
            }

            #endregion

            #region Парсинг ссылки на модуль

            searchString = "type=\"module\"";
            while (html.Contains(searchString))
            {
                Application.DoEvents();

                mlp.GetTagBounds(html, searchString);
                value = mlp.GetValue("linkitem_id");
                value = value.Substring(8, 36);
                value = value.ToLower();
                mlp.ShiftLastIndex(ref html);

                tag = string.Concat("<A href=", value, " class=linktotrainingmodule>", mlp.GetInnerHtml(), "</A>");

                html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                html = html.Insert(mlp.StartIndex, tag);

                LinkToObject lto = new LinkToObject();
                lto.TrainingModule = trainingModule;
                lto.ObjectId = new Guid(value);
                Warehouse.Warehouse.Instance.LinksToObjects.Add(lto);
            }

            #endregion

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
                                            mlp.GetValue("align"), "\" longDesc=\"",
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

        #region ReadBookmarksIds

        public static void ReadBookmarksIds(string documentHtml)
        {
            var html = string.Copy(documentHtml);
            var mlp = new MlParser();
            string searchString;
            string value;

            searchString = "<anchor";
            while (html.Contains(searchString))
            {
                Application.DoEvents();

                mlp.GetTagBounds(html, searchString);
                value = mlp.GetValue("id");
                value = value.ToLower();

                // Заменяет id закладки типа #a1 на Guid.
                if (!value.Length.Equals(36))
                {
                    if (!BookmarksIds.ContainsKey(value))
                    {
                        BookmarksIds.Add(value, Guid.NewGuid());
                    }
                }

                mlp.ShiftLastIndex(ref html);
                html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
            }
        }

        #endregion
    }
}