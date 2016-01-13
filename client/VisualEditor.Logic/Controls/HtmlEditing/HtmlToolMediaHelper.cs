using System;
using System.IO;
using System.Windows.Forms;
using VisualEditor.Logic.Commands;
using VisualEditor.Logic.Controls.Docking.Documents;
using VisualEditor.Logic.Helpers;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.Controls.HtmlEditing;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Controls.HtmlEditing
{
    internal class HtmlToolMediaHelper
    {
        public HtmlToolMediaHelper(HtmlEditingTool htmlEditingTool)
        {
            HtmlEditingTool = htmlEditingTool;

            HtmlEditingTool.MouseUp += VisualHtmlEditor_MouseUp;
            HtmlEditingTool.MouseDoubleClick += VisualHtmlEditor_MouseDoubleClick;
        }

        public HtmlEditingTool HtmlEditingTool { get; private set; }

        private void VisualHtmlEditor_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Left))
            {
                var he = HtmlEditingTool.GetActiveElement(e.Location) as HtmlElement;

                if (he.TagName.Equals(TagNames.ImageTagName))
                {
                    if (!he.GetAttribute("longdesc").Equals(string.Empty))
                    {
                        CommandManager.Instance.GetCommand(CommandNames.EditEquation).Execute(null);
                    }
                }
            }
        }


        private void VisualHtmlEditor_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Left))
            {
                if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview)
                {
                    var he = HtmlEditingTool.GetActiveElement(e.Location);
                    if (he.TagName.Equals(TagNames.AnchorTagName))
                    {
                        var href = he.GetAttribute("href");
                        if (!string.IsNullOrEmpty(href))
                        {
              string width = he.GetAttribute("width_");
              string height = he.GetAttribute("height_");
              if (string.IsNullOrEmpty(width))
                width = "800";
              if (string.IsNullOrEmpty(height))
                height = "600";
              if (!href.Equals("pic") && !href.Equals("anim") &&
                                !href.Equals("aud") && !href.Equals("vid")/* && !href.Contains("//")*/)
                            {
                                CommandManager.Instance.GetCommand(CommandNames.NavigateToLinkObject).Execute(he);
                            }

                            if (href.Equals("pic"))
                            {
                                var pd = new PictureDocument
                                             {
                                                 HtmlEditingTool =
                                                     { Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design }
                                             };
                                HtmlEditingToolHelper.SetDefaultDocumentHtml(pd.HtmlEditingTool);

                                var i = pd.HtmlEditingTool.Document.CreateElement(TagNames.ImageTagName);
                                i.SetAttribute("src", he.GetAttribute("src_"));
                                try
                                {
                                    Utils.Controls.HtmlEditing.HtmlEditingToolHelper.InsertHtml(pd.HtmlEditingTool, i);
                                }
                                catch (Exception exception)
                                {
                                    ExceptionManager.Instance.LogException(exception);
                                    return;
                                }

                                pd.Show();
                                //pd.HtmlEditingTool.Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview;
                            }

                            if (href.Equals("anim"))
                            {
                            	/*
                                var ad = new AnimationDocument
                                             {
                                                 HtmlEditingTool =
                                                     { Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview }
                                             };
                                HtmlEditingToolHelper.SetDefaultDocumentHtml(ad.HtmlEditingTool);

                                var html = "<OBJECT width=\"800\" height=\"500\" " +
                                    "classid=\"clsid:D27CDB6E-AE6D-11CF-96B8-444553540000\" " +
                                    "codebase=\"http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,40,0\">" +
                                    "<PARAM name=movie value=\"" + he.GetAttribute("src_") +
                                    "\"></OBJECT>";

                                


                                try
                                {
                                    Utils.Controls.HtmlEditing.HtmlEditingToolHelper.InsertHtml(ad.HtmlEditingTool, html);
                                }
                                catch (Exception exception)
                                {
                                    ExceptionManager.Instance.LogException(exception);
                                    return;
                                }                             

                                ad.Show();
                                //ad.HtmlEditingTool.Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview;
                                */
                                /*AnimationDocument ad = new AnimationDocument
                                            {
                                                HtmlEditingTool =
                                                    { Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview }
                                            };
                              HtmlEditingToolHelper.SetDefaultDocumentHtml(ad.HtmlEditingTool);*/
                              PreviewDocument ad = new PreviewDocument(Properties.Resources.AnimationSmall);
                              string fv = he.GetAttribute("FlashVars_");
                              string mv = "file:///" + Path.Combine(Warehouse.Warehouse.ProjectEditorLocation, he.GetAttribute("src_")).Replace('\\', '/').Replace(" ", "%20");
                              if (!string.IsNullOrEmpty(fv))
                                fv = string.Concat("<param name=\"FlashVars\" value=\"", fv.Replace('\\', '/').Replace(" ", "%20"), "\"/>");
                                string html = "<object width=\"" + width + "\" height=\"" + height + "\" type=\"application/x-shockwave-flash\" data=\"" + mv + "\">" +
                                    "<param name=\"movie\" value=\"" + mv + "\"/><param name=\"wmode\" value=\"gpu\" /><param name=\"allowScriptAccess\" value=\"sameDomain\" />" + fv + "</object>";

                                try
                                {
                                    //Utils.Controls.HtmlEditing.HtmlEditingToolHelper.InsertHtml(ad.HtmlEditingTool, html);
                                  ad.Content = "<!DOCTYPE html><html><body>" + html + "</body></html>";
                                }
                                catch (Exception exception)
                                {
                                    ExceptionManager.Instance.LogException(exception);
                                    return;
                                }

                                ad.Show();
                                //ad.HtmlEditingTool.Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview;
                            }

                            if (href.Equals("aud"))
                            {
                                var ad = new AudioDocument
                                             {
                                                 HtmlEditingTool = { Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview }
                                             };
                                HtmlEditingToolHelper.SetDefaultDocumentHtml(ad.HtmlEditingTool);

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

                                try
                                {
                                    Utils.Controls.HtmlEditing.HtmlEditingToolHelper.InsertHtml(ad.HtmlEditingTool, html);
                                }
                                catch (Exception exception)
                                {
                                    ExceptionManager.Instance.LogException(exception);
                                    return;
                                }

                                ad.Show();
                            }

                            if (href.Equals("vid"))
                            {
                                var vd = new VideoDocument
                                             {
                                                 HtmlEditingTool = { Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview }
                                             };
                                HtmlEditingToolHelper.SetDefaultDocumentHtml(vd.HtmlEditingTool);

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

                                try
                                {
                                    Utils.Controls.HtmlEditing.HtmlEditingToolHelper.InsertHtml(vd.HtmlEditingTool, html);
                                }
                                catch (Exception exception)
                                {
                                    ExceptionManager.Instance.LogException(exception);
                                    return;
                                }

                                vd.Show();
                            }
                        }
                    }
                }

            //    //FontNameComboBox.FontName = GetFontName();
            //    //FontSizeComboBox.FontSize = GetFontSize();
            }
        }
    }
}