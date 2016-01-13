using System;
using System.IO;
using System.Windows.Forms;
using VisualEditor.Logic.Controls.Docking.Documents;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Course.Items.Questions;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.Controls.HtmlEditing;
using VisualEditor.Utils.ExceptionHandling;
using HtmlEditing = VisualEditor.Logic.Controls.HtmlEditing;

namespace VisualEditor.Logic.Course.Preview
{
    internal static class PreviewConverter
    {
        #region Convert

        public static void Convert(HtmlEditingTool htmlEditingTool)
        {
            DocumentBase document = HtmlEditing.HtmlEditingToolHelper.GetParentDocument(EditorObserver.ActiveEditor);
			string s = EditorObserver.ActiveEditor.BodyInnerHtml ?? string.Empty;
            if (document is TrainingModuleDocument)
            {
                var tm = ((TrainingModuleDocument)document).TrainingModule;
                tm.DocumentHtml = s;
                HtmlEditing.HtmlEditingToolHelper.SetDefaultStyle(htmlEditingTool);
                ConvertBookmarks(htmlEditingTool);
                ConvertImages(htmlEditingTool);
                ConvertFlashes(htmlEditingTool);
                ConvertAudios(htmlEditingTool);
                ConvertVideos(htmlEditingTool);
                tm.PreviewHtml = s;
            }

            if (document is QuestionDocument)
            {
                var q = ((QuestionDocument)document).Question;
                q.DocumentHtml = s;
                HtmlEditing.HtmlEditingToolHelper.SetDefaultStyle(htmlEditingTool);
                ConvertQuestionDocument(htmlEditingTool, q);
                ConvertImages(htmlEditingTool);
                ConvertFlashes(htmlEditingTool);
                ConvertAudios(htmlEditingTool);
                ConvertVideos(htmlEditingTool);
                q.PreviewHtml = s;
            }

            if (document is ResponseDocument)
            {
                var r = ((ResponseDocument)document).Response;
                r.DocumentHtml = s;
                HtmlEditing.HtmlEditingToolHelper.SetDefaultStyle(htmlEditingTool);
                ConvertImages(htmlEditingTool);
                ConvertFlashes(htmlEditingTool);
                ConvertAudios(htmlEditingTool);
                ConvertVideos(htmlEditingTool);
                r.PreviewHtml = s;
            }
        }

        #endregion

        #region ConvertBookmarks

        private static void ConvertBookmarks(HtmlEditingTool htmlEditingTool)
        {
            var bs = Warehouse.Warehouse.Instance.Bookmarks;
            var ans = htmlEditingTool.Links;

            foreach (HtmlElement he in ans)
            {
                if (he.Id != null)
                {
                    foreach (var b in bs)
                    {
                        if (he.Id.Equals(b.Id.ToString()))
                        {
                            var ahe = EditorObserver.ActiveEditor.Document.CreateElement(TagNames.AnchorTagName);
                            ahe.InnerHtml = he.InnerHtml;
                            ahe.Id = he.Id;
                            he.InsertAdjacentElement(HtmlElementInsertionOrientation.AfterEnd, ahe);
                            he.OuterHtml = string.Empty;
                            break;
                        }
                    }
                }
            }
        }

        #endregion

        #region ConvertImages

        private static void ConvertImages(HtmlEditingTool htmlEditingTool)
        {
            var ims = htmlEditingTool.Images;
            var sd = string.Empty;
            var t = string.Empty;

            foreach (HtmlElement he in ims)
            {
                sd = he.GetAttribute("sdocument");
                if (sd.Equals("1"))
                {
                    t = he.GetAttribute("src");
                    if (t.Contains("Pic.png"))
                    {
                        var ahe = EditorObserver.ActiveEditor.Document.CreateElement(TagNames.AnchorTagName);
                        ahe.InnerHtml = he.GetAttribute("alt");
                        ahe.SetAttribute("src_", he.GetAttribute("src_"));
                        ahe.SetAttribute("href", "pic");
                        he.InsertAdjacentElement(HtmlElementInsertionOrientation.AfterEnd, ahe);
                        he.OuterHtml = string.Empty;
                    }
                }
            }
        }

        #endregion

        #region ConvertFlashes

        private static void ConvertFlashes(HtmlEditingTool htmlEditingTool)
        {
            var ims = htmlEditingTool.Images;
            var sd = string.Empty;
            var t = string.Empty;

            foreach (HtmlElement he in ims)
            {
                sd = he.GetAttribute("sdocument");
                if (sd.Equals("1"))
                {
                    t = he.GetAttribute("src");
                    if (t.Contains("Anim.png"))
                    {
                        var ahe = EditorObserver.ActiveEditor.Document.CreateElement(TagNames.AnchorTagName);
                        ahe.InnerHtml = he.GetAttribute("alt");
                        ahe.SetAttribute("src_", he.GetAttribute("src_"));
                        ahe.SetAttribute("href", "anim");
                        he.InsertAdjacentElement(HtmlElementInsertionOrientation.AfterEnd, ahe);
                        he.OuterHtml = string.Empty;
                    }
                }
                else if (sd.Equals("0"))
                {
                    t = he.GetAttribute("src");
                    if (t.Contains("Anim.png"))
                    {
                        var ahe = EditorObserver.ActiveEditor.Document.CreateElement(TagNames.AnchorTagName);

                        var html = "<OBJECT width=\"800\" height=\"500\" " +
                            "classid=\"clsid:D27CDB6E-AE6D-11CF-96B8-444553540000\" " +
                            "codebase=\"http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,40,0\">" +
                            "<PARAM name=movie value=\"" + he.GetAttribute("src_").Replace("\\", "/") +
                            "\"></OBJECT>";
                        
                        ahe.InnerHtml = html;
                        try
                        {
                            he.InsertAdjacentElement(HtmlElementInsertionOrientation.AfterEnd, ahe);
                        }
                        catch (Exception exception)
                        {
                            ExceptionManager.Instance.LogException(exception);
                            return;
                        }
                        he.OuterHtml = string.Empty;
                    }
                }
            }
        }

        #endregion

        #region ConvertAudios
        
        private static void ConvertAudios(HtmlEditingTool htmlEditingTool)
        {
            var ims = htmlEditingTool.Images;
            var sd = string.Empty;
            var t = string.Empty;

            foreach (HtmlElement he in ims)
            {
                sd = he.GetAttribute("sdocument");
                if (sd.Equals("1"))
                {
                    t = he.GetAttribute("src");
                    if (t.Contains("Aud.png"))
                    {
                        var ahe = EditorObserver.ActiveEditor.Document.CreateElement(TagNames.AnchorTagName);
                        ahe.InnerHtml = he.GetAttribute("alt");
                        ahe.SetAttribute("src_", he.GetAttribute("src_"));
                        ahe.SetAttribute("href", "aud");
                        he.InsertAdjacentElement(HtmlElementInsertionOrientation.AfterEnd, ahe);
                        he.OuterHtml = string.Empty;
                    }
                }
                else if (sd.Equals("0"))
                {
                    t = he.GetAttribute("src");
                    if (t.Contains("Aud.png"))
                    {
                        var ahe = EditorObserver.ActiveEditor.Document.CreateElement(TagNames.AnchorTagName);

                        var flashPlayerAbsolutePath = Path.Combine(Application.StartupPath, Warehouse.Warehouse.FlashPlayerRelativePath);
                        var absoluteAudioSettingsPath = Path.Combine(Application.StartupPath, Warehouse.Warehouse.FlashPlayerRelativeAudioSettingsPath);
                        var absoluteAudioPath = Path.Combine(Warehouse.Warehouse.ProjectEditorLocation, he.GetAttribute("src_"));
                        var html =
                            "<P><OBJECT type=\"application/x-shockwave-flash\" width=\"500\" height=\"50\" " +
                            "data=\"" + flashPlayerAbsolutePath + "\">" +
                            "<PARAM name=\"bgcolor\" value=\"#ffffff\" />" +
                            "<PARAM name=\"allowFullScreen\" value=\"true\" />" +
                            "<PARAM name=\"allowScriptAccess\" value=\"always\" />" +
                            "<PARAM name=\"wmode\" value=\"transparent\" />" +
                            "<PARAM name=\"movie\" value=\"" + flashPlayerAbsolutePath + "\"/>" +
                            "<PARAM name=\"flashvars\" value=\"st=" + absoluteAudioSettingsPath + "&amp;file=" + absoluteAudioPath + "\"/>" +
                            "</OBJECT></P>";

                        ahe.InnerHtml = html;
                        try
                        {
                            he.InsertAdjacentElement(HtmlElementInsertionOrientation.AfterEnd, ahe);
                        }
                        catch (Exception exception)
                        {
                            ExceptionManager.Instance.LogException(exception);
                            return;
                        }
                        he.OuterHtml = string.Empty;
                    }
                }
            }
        }

        #endregion

        #region ConvertVideos
        
        private static void ConvertVideos(HtmlEditingTool htmlEditingTool)
        {
            var ims = htmlEditingTool.Images;
            var sd = string.Empty;
            var t = string.Empty;

            foreach (HtmlElement he in ims)
            {
                sd = he.GetAttribute("sdocument");
                if (sd.Equals("1"))
                {
                    t = he.GetAttribute("src");
                    if (t.Contains("Vid.png"))
                    {
                        var ahe = EditorObserver.ActiveEditor.Document.CreateElement(TagNames.AnchorTagName);
                        ahe.InnerHtml = he.GetAttribute("alt");
                        ahe.SetAttribute("src_", he.GetAttribute("src_"));
                        ahe.SetAttribute("href", "vid");
                        he.InsertAdjacentElement(HtmlElementInsertionOrientation.AfterEnd, ahe);
                        he.OuterHtml = string.Empty;
                    }
                }
                else if (sd.Equals("0"))
                {
                    t = he.GetAttribute("src");
                    if (t.Contains("Vid.png"))
                    {
                        var ahe = EditorObserver.ActiveEditor.Document.CreateElement(TagNames.AnchorTagName);

                        var flashPlayerAbsolutePath = Path.Combine(Application.StartupPath, Warehouse.Warehouse.FlashPlayerRelativePath);
                        var absoluteVideoSettingsPath = Path.Combine(Application.StartupPath, Warehouse.Warehouse.FlashPlayerRelativeVideoSettingsPath);
                        var absoluteVideoPath = Path.Combine(Warehouse.Warehouse.ProjectEditorLocation, he.GetAttribute("src_"));
                        var html =
                            "<P><OBJECT type=\"application/x-shockwave-flash\" width=\"500\" height=\"400\" " +
                            "data=\"" + flashPlayerAbsolutePath + "\">" +
                            "<PARAM name=\"bgcolor\" value=\"#ffffff\" />" +
                            "<PARAM name=\"allowFullScreen\" value=\"true\" />" +
                            "<PARAM name=\"allowScriptAccess\" value=\"always\" />" +
                            "<PARAM name=\"wmode\" value=\"transparent\" />" +
                            "<PARAM name=\"movie\" value=\"" + flashPlayerAbsolutePath + "\"/>" +
                            "<PARAM name=\"flashvars\" value=\"st=" + absoluteVideoSettingsPath + "&amp;file=" + absoluteVideoPath + "\"/>" +
                            "</OBJECT></P>";

                        ahe.InnerHtml = html;
                        try
                        {
                            he.InsertAdjacentElement(HtmlElementInsertionOrientation.AfterEnd, ahe);
                        }
                        catch (Exception exception)
                        {
                            ExceptionManager.Instance.LogException(exception);
                            return;
                        }
                        he.OuterHtml = string.Empty;
                    }
                }
            }
        }

        #endregion

        #region ConvertQuestionDocument

        public static void ConvertQuestionDocument(HtmlEditingTool htmlEditingTool, Question q)
        {
            var testModuleText = string.Empty;
            if (q.Parent is TestModule)
            {
                testModuleText = q.Parent.Text;
            }
            else
            {
                testModuleText = q.Parent.Parent.Text;
            }

            var questionHtml = q.DocumentHtml;
            if (q.QuestionDocument != null)
            {
                questionHtml = q.QuestionDocument.HtmlEditingTool.BodyInnerHtml;
            }

            var html = string.Empty;

            html = "<table width=\"90%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\">\n" +
                   "<tr>\n" +
                   "<td>" +
                   "<table border=\"0\" cellpadding=\"2\" cellspacing=\"2\" width=\"100%\">\n" +
                   "<tr align=\"center\" bgcolor=\"#CCCCFF\">\n" +
                // имя и время теста
                   "<td bgcolor=\"#eeeeee\" width=\"70%\" align=\"center\">" +
                   "<b>" + testModuleText +
                   "</b>" +
                   "</td>\n" +
                   "<td bgcolor=\"#ffeeee\"></td>\n" +
                   "</tr>\n" +
                   "<tr>\n" +
                // имя и время вопроса
                   "<td bgcolor=\"#f5f5f5\" width=\"70%\" align=\"center\">" +
                   "<i>" + q.Text +
                   "</i> " +
                   "</td>\n" +
                   "<td bgcolor=\"#ffeeee\"></td>\n" +
                   "</tr>\n" +
                   "</table>\n" +
                   "</td>\n" +
                   "</tr>\n" +
                   "<tr>\n" +
                   "<td>\n" +
                   "<table border=\"0\" cellpadding=\"2\" cellspacing=\"2\" width=\"100%\">\n" +
                   "<tr bgcolor=\"#eeeeff\">\n" +
                // вопрос
                   "<td colspan=\"3\">\n" + questionHtml +
                   "</td>\n" +
                   "</tr>\n" +
                   "</table>\n" +
                   "<table border=\"0\" cellpadding=\"2\" cellspacing=\"2\" width=\"100%\">\n";

            if (q is OpenQuestion)
            {
                html += "<tr bgcolor=\"#f5f5f5\">\n" +
                        "<td width=\"100%\" align=\"left\">\n" +
                        "<input type=\"text\"/>\n" +
                        "</td>\n" +
                        "</tr>\n";
            }
            else
            {
                char letter = 'a';
                foreach (var response in q.Responses)
                {
                    html += "<tr bgcolor=\"#f5f5f5\">\n" +
                            "<td width=\"5%\" align=\"center\">\n" +
                            letter +
                            "</td>\n" +
                            "<td width=\"5%\" align=\"center\">\n";

                    if (q is ChoiceQuestion)
                    {
                        html += "<input type=\"radio\" />\n";
                    }
                    else if (q is MultichoiceQuestion)
                    {
                        html += "<input type=\"checkbox\" />\n";
                    }

                    else if (q is OrderingQuestion)
                    {
                        html += "<input type=\"hidden\" value=\"c\" />\n";
                        html += "<select name=\"c\">\n";
                        html += "<option></option>\n";

                        for (int i = 1; i <= q.Responses.Count; i++)
                        {
                            html += "<option value=\"" + i + "\">" + i + "</option>\n";
                        }

                        html += "</select>\n";
                    }

                    var responseHtml = response.DocumentHtml;
                    if (response.ResponseDocument != null)
                    {
                        responseHtml = response.ResponseDocument.HtmlEditingTool.BodyInnerHtml;
                    }

                    html += "</td>\n" +
                            "<td>\n" + responseHtml +
                            "</td>\n" +
                            "</tr>\n";

                    letter++;
                }
            }

            html += "</table>";

            htmlEditingTool.BodyInnerHtml = html;
        }

        #endregion

        #region ConvertResponseVariantDocument

        public static void ConvertResponseVariantDocument(HtmlEditingTool htmlEditingTool, Question question, ResponseVariant rv)
        {
            string html = string.Empty;

            var testModuleText = string.Empty;
            if (question.Parent is TestModule)
            {
                testModuleText = question.Parent.Text;
            }
            else
            {
                testModuleText = question.Parent.Parent.Text;
            }

            var questionHtml = question.DocumentHtml;
            if (question.QuestionDocument != null)
            {
                questionHtml = question.QuestionDocument.HtmlEditingTool.BodyInnerHtml;
            }

            html += "<table width=\"90%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" align=\"center\">\n" +
                    "<tr>\n" +
                    "<td>" +
                    "<table border=\"0\" cellpadding=\"2\" cellspacing=\"2\" width=\"100%\">\n" +
                    "<tr align=\"center\" bgcolor=\"#CCCCFF\">\n" +
                // имя и время теста
                    "<td bgcolor=\"#eeeeee\" width=\"70%\" align=\"center\">" +
                        "<b>" + testModuleText +
                        "</b>" +
                    "</td>\n" +
                    "<td bgcolor=\"#ffeeee\"></td>\n" +
                    "</tr>\n" +
                    "<tr>\n" +
                // имя и время вопроса
                    "<td bgcolor=\"#f5f5f5\" width=\"70%\" align=\"center\">" +
                        "<i>" + question.Text +
                        "</i> " +
                    "</td>\n" +
                    "<td bgcolor=\"#ffeeee\"></td>\n" +
                    "</tr>\n" +
                    "</table>\n" +
                    "</td>\n" +
                    "</tr>\n" +
                    "<tr>\n" +
                    "<td>\n" +
                    "<table border=\"0\" cellpadding=\"2\" cellspacing=\"2\" width=\"100%\">\n" +
                    "<tr bgcolor=\"#eeeeff\">\n" +
                // сам вопрос
                    "<td colspan=\"3\">\n" + questionHtml +
                    "</td>\n" +
                    "</tr>\n" +
                    "</table>\n" +
                    "<table border=\"0\" cellpadding=\"2\" cellspacing=\"2\" width=\"100%\">\n";

            if (question is OpenQuestion)
            {
                html += "<tr bgcolor=\"#f5f5f5\">\n" +
                        "<td width=\"100%\" align=\"left\">\n" +
                        "<input type=\"text\" value=\"" + rv.Responses[0].ToString() + "\"/>\n" +
                        "</td>\n" +
                        "</tr>\n";
            }
            else
            {
                char letter = 'a';

                foreach (Response response in question.Responses)
                {
                    html += "<tr bgcolor=\"#f5f5f5\">\n" +
                            "<td width=\"5%\" align=\"center\">\n" +
                            letter +
                            "</td>\n" +
                            "<td width=\"5%\" align=\"center\">\n";

                    if (question is ChoiceQuestion)
                    {
                        if (rv.Responses.Contains(response))
                        {
                            html += "<input type=\"radio\" checked/>\n";
                        }
                        else
                        {
                            html += "<input type=\"radio\" />\n";
                        }
                    }
                    else if (question is MultichoiceQuestion)
                    {
                        if (rv.Responses.Contains(response))
                        {
                            html += "<input type=\"checkbox\" checked/>\n";
                        }
                        else
                        {
                            html += "<input type=\"checkbox\" />\n";
                        }
                    }

                    else if (question is OrderingQuestion)
                    {
                        html += "<input type=\"hidden\" value=\"c\" />\n";
                        html += "<select name=\"c\">\n";
                        html += "<option>" + (int)rv.Responses[question.Responses.IndexOf(response)] + "</option>\n";

                        for (int i = 1; i <= question.Responses.Count; i++)
                        {
                            html += "<option value=\"" + i + "\">" + i + "</option>\n";
                        }

                        html += "</select>\n";
                    }

                    var responseHtml = response.DocumentHtml;
                    if (response.ResponseDocument != null)
                    {
                        responseHtml = response.ResponseDocument.HtmlEditingTool.BodyInnerHtml;
                    }

                    html += "</td>\n" +
                            "<td>\n" + responseHtml +
                            "</td>\n" +
                            "</tr>\n";

                    letter++;
                }
            }

            html += "</table>\n" +
                    "</BODY>\n" +
                    "</HTML>";

            htmlEditingTool.BodyInnerHtml = html;

            ConvertImages(htmlEditingTool);
            ConvertFlashes(htmlEditingTool);
        }

        #endregion
    }
}