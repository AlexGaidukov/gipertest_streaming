using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;
using VisualEditor.Logic.Helpers;
using VisualEditor.Logic.IO;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.Controls.HtmlEditing;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Dialogs
{
    internal partial class EquationDialog : DialogBase
    {
        private const string operationCantBePerformedMessage = "Невозможно вставить рисунок в редактор.";
        private static string equationPath = string.Empty;
        private bool isNewEquation;
        private static string[] CyrillicMap = { "A", "B", "V", "G", "D", "E", "Zh", "Z", "I", "\\u I", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "F", "Kh", "Ts", "Ch", "Sh", "Shch", "\\Cdprime", "Y", "\\Cprime", "\\`E", "Yu", "Ya", "a", "b", "v", "g", "d", "e", "zh", "z", "i", "\\u\\i", "k", "l", "m", "n", "o", "p", "r", "s", "t", "u", "f", "kh", "ts", "ch", "sh", "shch", "\\cdprime", "y", "\\cprime", "\\`e", "yu", "ya", "\\\"E", "\\\"e" };

        public EquationDialog()
        {
            InitializeComponent();
            HelpKeyword = "Формула";
            equationTextBox.Select();
            equationPath = Path.Combine(Warehouse.Warehouse.ProjectEditorLocation, Guid.NewGuid().ToString());
            equationTextBox_TextChanged(null, null);
            isNewEquation = true;
        }

        private void helpLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("http://www.latexeditor.org/links_tex.html");
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
            }
        }

        private void onlineEditorLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("http://thornahawk.unitedti.org/equationeditor/equationeditor.php");
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
            }
        }

        private void examplesButton_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("TexExamples.doc");
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
            }
        }

        [SuppressUnmanagedCodeSecurity()]
        internal class NativeMimeTexMethods
        {
            [DllImport("VisualEditor.Utils.MimeTeX.dll")]
            internal static extern int CreateGifFromEq(string expr, string fileName);
        }

        private void WriteEquation(string equation)
        {
            if (equationPictureBox.Image != null)
            {
                equationPictureBox.Image.Dispose();
            }

            if (equation.Length > 0)
            {
              string s2 = string.Empty;
              //bool g = false;
              foreach (char c in equation)
              {
                bool b1 = (c >= 'А') && (c <= 'я');
                bool b2 = (c == 'Ё');
                bool b3 = (c == 'ё');
                if (b1 || b2 || b3)
                {
                  /*if (!g)
                  {
                    g = !g;
                    s2 += "{\\cyr ";
                  }*/
                  int i = c - 'А';
                  if (b2)
                    i = CyrillicMap.Length - 2;
                  else if (b3)
                    i = CyrillicMap.Length - 1;
                  s2 += "{\\cyr " + CyrillicMap[i] + "}";
                }
                else
                {
                  /*if (g)
                  {
                    g = !g;
                    s2 += "}";
                  }*/
                  s2 += c;
                }
              }
              /*if (g)
              	  s2 += "}";*/
              equation = s2;
                try
                {
                    NativeMimeTexMethods.CreateGifFromEq(equation, equationPath);
                    equationPictureBox.Image = Image.FromFile(equationPath);
                }
                catch { }
            }
            else
            {
                WriteEquation("\\text Empty Equation");
            }
        }

        private void CheckState()
        {
            insertButton.Enabled = equationTextBox.Text != string.Empty;
        }

        private void equationTextBox_TextChanged(object sender, EventArgs e)
        {
            CheckState();

            WriteEquation(equationTextBox.Text);
        }

        private void insertButton_Click(object sender, EventArgs e)
        {
            if (isNewEquation)
            {
                var path = Path.Combine(Warehouse.Warehouse.AbsoluteEditorImagesDirectory, Guid.NewGuid().ToString());
                path = path + ".gif";
                File.Copy(equationPath, path);

                var i = EditorObserver.ActiveEditor.Document.CreateElement(TagNames.ImageTagName);
                var s = Path.Combine(Warehouse.Warehouse.RelativeImagesDirectory, Path.GetFileName(path));
                i.SetAttribute("src", s);
                i.SetAttribute("longdesc", equationTextBox.Text);
                string align = "center";
                i.SetAttribute("align",align);

                try
                {
                    HtmlEditingToolHelper.InsertHtml(EditorObserver.ActiveEditor, i);
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
                var he = EditorObserver.ActiveEditor.ActiveElement;
                var value = he.GetAttribute("src");
                value = TrainingModuleXmlWriter.ExtractRelativeSrc(value);
                var path = Path.Combine(Warehouse.Warehouse.ProjectEditorLocation, value);
                File.Delete(path);
                path = Path.Combine(Warehouse.Warehouse.AbsoluteEditorImagesDirectory, Guid.NewGuid().ToString());
                path = path + ".gif";
                File.Copy(equationPath, path);

                var s = Path.Combine(Warehouse.Warehouse.RelativeImagesDirectory, Path.GetFileName(path));
                var i = EditorObserver.ActiveEditor.Document.CreateElement(TagNames.ImageTagName);
                i.SetAttribute("src", s);
                i.SetAttribute("longdesc", equationTextBox.Text);
                string align = "center";
                i.SetAttribute("align", align);
                //var imageSize = Image.FromFile(path).PhysicalDimension;
                //i.SetAttribute("width", imageSize.Width.ToString());
                //i.SetAttribute("height", imageSize.Height.ToString());
                he.InsertAdjacentElement(HtmlElementInsertionOrientation.AfterEnd, i);
                he.OuterHtml = string.Empty;
            }

            Warehouse.Warehouse.IsProjectModified = true;
            DialogResult = DialogResult.OK;
        }

        public void InitializeData()
        {
            var he = EditorObserver.ActiveEditor.ActiveElement;

            if (he.TagName.Equals(TagNames.ImageTagName))
            {
                var value = he.GetAttribute("longdesc");
                if (!value.Equals(string.Empty))
                {
                    equationTextBox.Text = value;
                }
            }

            isNewEquation = false;
        }
    }
}