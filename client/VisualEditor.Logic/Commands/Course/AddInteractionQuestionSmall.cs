using System;
using VisualEditor.Logic.Controls.Docking.Documents;
using VisualEditor.Logic.Controls.HtmlEditing;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Course.Items.Questions;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Logic.Helpers.AppSettings;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using VisualEditor.Utils.ExceptionHandling;
using VisualEditor.Logic.Helpers;
using VisualEditor.Utils.Controls.HtmlEditing;
using VisualEditor.Utils.Controls.Docking;

namespace VisualEditor.Logic.Commands.Course
{
    class AddInteractiveQuestionSmall: AbstractCommand
    {
        private const string fileIsAlreadyUsedMessage = "Файл с данным именем уже используется в проекте.\nЗаменить?";
        private const string operationCantBePerformedMessage = "Невозможно вставить рисунок в редактор.";

        private SizeF sourceImageSize;
        private SizeF imageSize;
        private string source;

        public AddInteractiveQuestionSmall()
        {
            name = CommandNames.AddInteractiveQuestionSmall;
            text = CommandTexts.AddInteractiveQuestionSmall;
            image = Properties.Resources.Question;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Все типы рисунков (*.gif, *.jpg, *.jpeg, *.png)|" +
                        "*.gif;*.jpg;*.jpeg;*.png|" +
                        "GIF (*.gif)|*.gif|" +
                        "JPEG (*.jpg, *.jpeg)|*.jpg;*.jpeg|" +
                        "PNG (*.png)|*.png";
                openFileDialog.InitialDirectory = AppSettingsHelper.GetInitialDirectory();

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //  sourceTextBox.Text = openFileDialog.FileName;
                    sourceImageSize = Image.FromFile(openFileDialog.FileName).PhysicalDimension;
                    imageSize = new SizeF(sourceImageSize);

                    source = openFileDialog.FileName;
                    var imageName = Guid.NewGuid().ToString();
                    var destPath = Path.Combine(Warehouse.Warehouse.AbsoluteEditorImagesDirectory, imageName);
                    destPath += Path.GetExtension(source);

                    if (!File.Exists(destPath))
                    {
                        try
                        {
                            File.Copy(source, destPath);
                        }
                        catch (Exception exception)
                        {
                            ExceptionManager.Instance.LogException(exception);
                            UIHelper.ShowMessage(operationCantBePerformedMessage, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        var dr = MessageBox.Show(fileIsAlreadyUsedMessage,
                            System.Windows.Forms.Application.ProductName,
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (dr.Equals(DialogResult.OK))
                        {
                            try
                            {
                                File.Copy(source, destPath, true);
                            }
                            catch (Exception exception)
                            {
                                ExceptionManager.Instance.LogException(exception);
                                UIHelper.ShowMessage(operationCantBePerformedMessage, MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                                return;
                            }

                            // POSTPONE: Реализовать обновление изображения в редакторе.
                            //VisualHtmlEditor.ActiveEditor.Refresh();
                            //VisualHtmlEditor.ActiveEditor.Update();
                        }
                    }

                    var imageNameWithoutExtension = Path.GetFileNameWithoutExtension(source);
                    source = destPath;

                    AppSettingsManager.Instance.SetSettingByName(SettingNames.InitialDirectory,
                        Path.GetDirectoryName(openFileDialog.FileName));
                }
                else { return; }  
            }

            var q = new InteractiveQuestion
                        {
                            Id = Guid.NewGuid()
                        };

            if (Warehouse.Warehouse.Instance.CourseTree.CurrentNode is TestModule)
            {
                var tm = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as TestModule;
                q.Text = string.Concat("Вопрос ", tm.Questions.Count + 1);
            }

            if (Warehouse.Warehouse.Instance.CourseTree.CurrentNode is Group)
            {
                var g = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as Group;
                q.Text = string.Concat("Вопрос ", g.Questions.Count + 1);

                q.TimeRestriction = g.TimeRestriction;
                q.Profile = g.Profile;
                q.Marks = g.Marks;
            }

            
            Warehouse.Warehouse.Instance.CourseTree.CurrentNode.Nodes.Add(q);

            if (!Warehouse.Warehouse.Instance.CourseTree.CurrentNode.IsExpanded)
            {
                Warehouse.Warehouse.Instance.CourseTree.CurrentNode.Toggle();
            }
            /* */

            // Создает и отображает редактор.
            q.QuestionDocument = new QuestionDocument
                                     {
                                         Question = q,
                                         Text = q.Text,
                                         HtmlEditingTool =
                                             {Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design}
                                     };

            VisualEditor.Logic.Controls.HtmlEditing.HtmlEditingToolHelper.SetDefaultDocumentHtml(q.QuestionDocument.HtmlEditingTool);
            PreviewObserver.AddDocument(q.QuestionDocument);
            q.QuestionDocument.Show();
            

            var flashPlayerAbsolutePath = Path.Combine(System.Windows.Forms.Application.StartupPath, Warehouse.Warehouse.InteractiveFlashRelativePath);
         
                #region Рисунок
           
            source = source.Replace('\\', '/');
            //" + sourceImageSize.Width + "\" height=\"" + sourceImageSize.Height + "
            string i = "<!DOCTYPE HTML><html><head><script type=\"text/javascript\">//<![CDATA[\r\nfunction fl_set(){f.setImage(\"" + source + "\");f.setModeString(\"d\");}\r\nfunction fl_get(m){return (m <= 0) ? f.getSelectString() : ((m == 1) ? f.getModeString() : f.getRegionString());}\r\n//]]></script></head><body onload=\"fl_set()\"><object id=\"f\" classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" width=\"800\" height=\"700\">" +
                          "<param name=\"movie\" value=\"" + flashPlayerAbsolutePath + "\"/></object></body></html>";
            var ahe = EditorObserver.ActiveEditor.Document.CreateElement(TagNames.AnchorTagName);
            ahe.InnerHtml = i;
                try
                {
                   // p.Show();
                   // VisualEditor.Utils.Controls.HtmlEditing.HtmlEditingToolHelper.InsertHtml(EditorObserver.ActiveEditor, i);
                    EditorObserver.ActiveEditor.DocumentText = i;
                }
                catch (Exception exception)
                {
                    ExceptionManager.Instance.LogException(exception);
                    UIHelper.ShowMessage(operationCantBePerformedMessage, MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                #endregion
           // }

            Warehouse.Warehouse.Instance.CourseTree.CurrentNode = q;
            Warehouse.Warehouse.IsProjectModified = true;
        }
    }
}
