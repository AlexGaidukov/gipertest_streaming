using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Course.Items.Questions;
using VisualEditor.Logic.Course.Preview;
using VisualEditor.Logic.Helpers;
using VisualEditor.Utils.Controls.HtmlEditing;

namespace VisualEditor.Logic.Dialogs
{
    internal partial class ResponseVariantDialog : DialogBase
    {
        private Question question;

        private int variantNumber;
        private int deniedNumber = -1;
        private int selectedTabIndex;
        private readonly ArrayList rvs = new ArrayList(0);

        public ResponseVariantDialog()
        {
            InitializeComponent();
            InitializeDialog();
        }

        #region InitializeDialog
        
        private void InitializeDialog()
        {
            question = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as Question;

            foreach (ResponseVariant rv in question.ResponseVariants)
            {
                rvs.Add(ResponseVariant.Clone(rv));
            }

            #region Кнопка Редактировать подсказку

            if (question.Parent is TestModule)
            {
                var tm = question.Parent as TestModule;

                hintButton.Enabled = tm.Trainer;
            }

            if (question.Parent is Group)
            {
                var tm = question.Parent.Parent as TestModule;

                hintButton.Enabled = tm.Trainer;
            }

            #endregion

            InitializeTabPages();
        }

        #endregion

        #region Добавить вариант ответа
        
        private void addVariantButton_Click(object sender, EventArgs e)
        {
            variantNumber++;
            var rv = new ResponseVariant(question);
            question.ResponseVariants.Add(rv);
            variantsTabControl.TabPages.Add(string.Concat("Вариант ", variantNumber));
            variantsTabControl.TabPages[variantsTabControl.TabPages.Count - 1].AutoScroll = true;
            FillTabPageWithResponses(variantsTabControl.TabPages[variantsTabControl.TabPages.Count - 1]);
            Check();
            variantsTabControl.SelectedTab = variantsTabControl.TabPages[variantsTabControl.TabPages.Count - 1];
            selectedTabIndex = variantsTabControl.TabPages.IndexOf(variantsTabControl.SelectedTab);
            viewVariantButton.Enabled = true;
            Warehouse.Warehouse.IsProjectModified = true;
        }

        #endregion

        #region Удалить вариант ответа
        
        private void deleteVariantButton_Click(object sender, EventArgs e)
        {
            if (variantsTabControl.SelectedTab != null)
            {
                deniedNumber = variantsTabControl.TabPages.IndexOf(variantsTabControl.SelectedTab);
                question.ResponseVariants.RemoveAt(variantsTabControl.TabPages.IndexOf(variantsTabControl.SelectedTab));
                variantsTabControl.TabPages.Remove(variantsTabControl.SelectedTab);
            }

            if (variantsTabControl.SelectedTab != null)
            {
                selectedTabIndex = variantsTabControl.TabPages.IndexOf(variantsTabControl.SelectedTab);
            }
            else
            {
                selectedTabIndex = 0;
            }

            if (variantsTabControl.TabPages.Count == 0)
            {
                viewVariantButton.Enabled = false;
            }

            Check();
        }

        #endregion

        #region Подсказка к варианту ответа
        
        private void hintButton_Click(object sender, EventArgs e)
        {
            Renderer.Instance.StopRendering();
            Renderer.Instance.StartHintDialogRendering();

            var rv = (ResponseVariant)question.ResponseVariants[selectedTabIndex];
            HintDialog.Instance.Hint = rv.Hint;

            if (HintDialog.Instance.ShowDialog(this) == DialogResult.OK)
            {
                rv.Hint = HintDialog.Instance.Hint;
            }

            Renderer.Instance.StartRendering();
            Renderer.Instance.StopHintDialogRendering();
        }

        #endregion

        #region Предварительный просмотр варианта ответа

        private Form previewDialog;

        private void viewVariantButton_Click(object sender, EventArgs e)
        {
            ((ResponseVariant)question.ResponseVariants[selectedTabIndex]).Responses = new ArrayList(0);
            if (((ResponseVariant)question.ResponseVariants[variantsTabControl.SelectedIndex]).Responses.Count == 0)
            {
                if (question is ChoiceQuestion)
                {
                    foreach (RadioButton rb in variantsTabControl.SelectedTab.Controls)
                    {
                        if (rb.Checked)
                        {
                            foreach (Response r in question.Responses)
                            {
                                if (r.Text == rb.Text && !question.ResponseVariants.Contains(r))
                                {
                                    ((ResponseVariant)question.ResponseVariants[variantsTabControl.SelectedIndex]).Responses.Add(r);
                                }
                            }
                        }
                    }
                }
                else if (question is MultichoiceQuestion)
                {
                    foreach (CheckBox ch in variantsTabControl.SelectedTab.Controls)
                    {
                        if (ch.Checked)
                        {
                            foreach (Response r in question.Responses)
                            {
                                if (r.Text == ch.Text && !question.ResponseVariants.Contains(r))
                                {
                                    ((ResponseVariant)question.ResponseVariants[variantsTabControl.SelectedIndex]).Responses.Add(r);
                                }
                            }
                        }
                    }
                }
                else if (question is OrderingQuestion)
                {
                    int i = 0;

                    foreach (Control cb in variantsTabControl.SelectedTab.Controls)
                    {
                        if (cb is ComboBox)
                        {
                            int j = 0;
                            int.TryParse(cb.Text, out j);

                            ((ResponseVariant)question.ResponseVariants[variantsTabControl.SelectedIndex]).Responses.Add(j);
                            i++;
                        }
                    }
                }
                else if (question is OpenQuestion)
                {
                    foreach (Control cb in variantsTabControl.SelectedTab.Controls)
                    {
                        ((ResponseVariant)question.ResponseVariants[variantsTabControl.SelectedIndex]).Responses.Add(cb.Text);
                    }
                }
            }

            #region Диалог предварительного просмотра
            
            previewDialog = new Form
                                {
                                    StartPosition = FormStartPosition.CenterParent,
                                    Width = 600,
                                    Height = 500,
                                    KeyPreview = true,
                                    MaximizeBox = false,
                                    MinimizeBox = false,
                                    ShowIcon = false,
                                    ShowInTaskbar = false,
                                    Text = "Предварительный просмотр варианта ответа"
                                };

            var htmlEditingTool = new HtmlEditingTool
                                      {
                                          Dock = DockStyle.Fill,
                                          Mode = Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Preview
                                      };

            Logic.Controls.HtmlEditing.HtmlEditingToolHelper.SetDefaultDocumentHtml(htmlEditingTool);
            htmlEditingTool.KeyDown += vhe_KeyDown;
            previewDialog.Controls.Add(htmlEditingTool);

            PreviewConverter.ConvertResponseVariantDocument(htmlEditingTool, question, (ResponseVariant)question.ResponseVariants[variantsTabControl.SelectedIndex]);

            previewDialog.ShowDialog(this);

            #endregion
        }

        void vhe_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Escape))
            {
                previewDialog.Close();
                previewDialog = null;
            }   
        }

        #endregion

        private void okButton_Click(object sender, EventArgs e)
        {
            ((ResponseVariant)question.ResponseVariants[selectedTabIndex]).Weight = (double)weightNumericUpDown.Value;

            #region Сохранение вариантов ответа
            
            foreach (TabPage tp in variantsTabControl.TabPages)
            {
                ((ResponseVariant)question.ResponseVariants[variantsTabControl.TabPages.IndexOf(tp)]).Responses = new ArrayList(0);
                if (question is ChoiceQuestion)
                {
                    foreach (RadioButton rb in tp.Controls)
                    {
                        if (rb.Checked)
                        {
                            foreach (Response r in question.Responses)
                            {
                                if (r.Text == rb.Text && !question.ResponseVariants.Contains(r))
                                {
                                    ((ResponseVariant)question.ResponseVariants[variantsTabControl.TabPages.IndexOf(tp)]).Responses.Add(r);
                                }
                            }
                        }
                    }
                }
                else if (question is MultichoiceQuestion)
                {
                    foreach (CheckBox ch in tp.Controls)
                    {
                        if (ch.Checked)
                        {
                            foreach (Response r in question.Responses)
                            {
                                if (r.Text == ch.Text && !question.ResponseVariants.Contains(r))
                                {
                                    ((ResponseVariant)question.ResponseVariants[variantsTabControl.TabPages.IndexOf(tp)]).Responses.Add(r);
                                }
                            }
                        }
                    }
                }
                else if (question is OrderingQuestion)
                {
                    int i = 0;

                    foreach (Control cb in tp.Controls)
                    {
                        if (cb is ComboBox)
                        {
                            int j = 0;
                            int.TryParse(cb.Text, out j);

                            ((ResponseVariant)question.ResponseVariants[variantsTabControl.TabPages.IndexOf(tp)]).Responses.Add(j);
                            i++;
                        }
                    }
                }
                else if (question is OpenQuestion)
                {
                    foreach (Control cb in tp.Controls)
                    {
                        ((ResponseVariant)question.ResponseVariants[variantsTabControl.TabPages.IndexOf(tp)]).Responses.Add(cb.Text);
                    }
                }
            }

            #endregion

            selectedTabIndex = 0;

            Warehouse.Warehouse.IsProjectModified = true;
            DialogResult = DialogResult.OK;
        }

        #region InitializeTabPages
        
        /// <summary>
        /// Отображает варианты ответа, содержащиеся в вопросе.
        /// </summary>
        private void InitializeTabPages()
        {
            if (question.ResponseVariants.Count == 0)
            {
                var rv = new ResponseVariant(question);
                question.ResponseVariants.Add(rv);
                variantNumber++;
                variantsTabControl.TabPages.Add(string.Concat("Вариант ", variantNumber));
                variantsTabControl.TabPages[variantsTabControl.TabPages.Count - 1].AutoScroll = true;
                FillTabPageWithResponses(variantsTabControl.TabPages[variantsTabControl.TabPages.Count - 1]);
                weightNumericUpDown.Value = 1;
            }
            else
            {
                foreach (ResponseVariant rv in question.ResponseVariants)
                {
                    variantNumber++;
                    variantsTabControl.TabPages.Add(string.Concat("Вариант ", variantNumber));
                    variantsTabControl.TabPages[variantsTabControl.TabPages.Count - 1].AutoScroll = true;
                    FillTabPageWithResponseVariants(variantsTabControl.TabPages[variantsTabControl.TabPages.Count - 1], rv);
                }

                weightNumericUpDown.Value = (decimal)((ResponseVariant)question.ResponseVariants[0]).Weight;
            }
        }

        #endregion

        #region FillTabPageWithResponses
        
        /// <summary>
        /// Отображает на вкладке все элементы ответа, содержащиеся в вопросе.
        /// </summary>
        /// <param name="tp">Вкладка варианта ответа.</param>
        private void FillTabPageWithResponses(TabPage tp)
        {
            int i = 1;

            if (question is ChoiceQuestion)
            {
                foreach (var response in question.Responses)
                {
                    var rb = new RadioButton();
                    rb.Text = response.Text;
                    rb.Location = new Point(20, i * 20);
                    tp.Controls.Add(rb);

                    i++;
                }
            }
            else if (question is MultichoiceQuestion)
            {
                foreach (var response in question.Responses)
                {
                    var ch = new CheckBox();
                    ch.Text = response.Text;
                    ch.Location = new Point(20, i * 20);
                    tp.Controls.Add(ch);

                    i++;
                }
            }
            else if (question is OrderingQuestion)
            {
                foreach (var response in question.Responses)
                {
                    var cb = new ComboBox();
                    for (int j = 1; j <= question.Responses.Count; j++)
                    {
                        cb.Items.Add(j);
                    }
                    cb.Width = 50;
                    cb.Location = new Point(20, i * 25);
                    cb.DropDownStyle = ComboBoxStyle.DropDownList;
                    tp.Controls.Add(cb);

                    var lb = new Label();
                    lb.Text = response.Text;
                    lb.Location = new System.Drawing.Point(80, i * 25 + 5);
                    tp.Controls.Add(lb);

                    i++;
                }
            }
            else if (question is OpenQuestion)
            {
                var tb = new TextBox();
                tb.Location = new Point(10, 10);
                tb.Width = 325;
                tp.Controls.Add(tb);
                if (((OpenQuestion)question).IsAnswerNumeric)
                {
                    tb.KeyPress += tb_KeyPress;
                }
            }
        }

        #endregion

        #region Обработка ввода с клавиатуры для открытых вопросов
        
        void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;

            if (char.IsDigit(e.KeyChar) ||
                e.KeyChar == 44 || // ","
                e.KeyChar == 45 || // "-"
                e.KeyChar == 46 || // "."
                e.KeyChar == 8) // "Backspace"
            {
                e.Handled = false;
            }
        }

        #endregion

        #region FillTabPageWithResponseVariants
        
        /// <summary>
        /// Отображает на вкладке все элементы ответа, содержащиеся в вопросе, с
        /// выделением тех, которые входят в вариант ответа.
        /// </summary> 
        private void FillTabPageWithResponseVariants(TabPage tp, ResponseVariant rv)
        {
            int i = 1;

            if (question is ChoiceQuestion)
            {
                foreach (var response in question.Responses)
                {
                    var rb = new RadioButton();
                    rb.Text = response.Text;
                    rb.Location = new Point(20, i * 20);
                    foreach (Response r in rv.Responses)
                    {
                        if (r.Text == response.Text)
                        {
                            rb.Checked = true;
                            break;
                        }
                    }
                    tp.Controls.Add(rb);

                    i++;
                }
            }
            else if (question is MultichoiceQuestion)
            {
                foreach (var response in question.Responses)
                {
                    var chb = new CheckBox();
                    chb.Text = response.Text;
                    chb.Location = new Point(20, i * 20);
                    foreach (Response r in rv.Responses)
                    {
                        if (r.Text == response.Text)
                        {
                            chb.Checked = true;
                            break;
                        }
                    }
                    tp.Controls.Add(chb);

                    i++;
                }
            }
            else if (question is OrderingQuestion)
            {
                foreach (var response in question.Responses)
                {
                    var cb = new ComboBox();
                    for (int j = 1; j <= question.Responses.Count; j++)
                    {
                        cb.Items.Add(j);
                    }
                    cb.Width = 50;
                    cb.Location = new Point(20, i * 25);
                    if ((int)rv.Responses[i - 1] != 0)
                    {
                        cb.Text = ((int)rv.Responses[i - 1]).ToString();
                    }
                    cb.DropDownStyle = ComboBoxStyle.DropDownList;
                    tp.Controls.Add(cb);

                    var lb = new Label();
                    lb.Text = response.Text;
                    lb.Location = new System.Drawing.Point(80, i * 25 + 5);
                    tp.Controls.Add(lb);

                    i++;
                }
            }
            else if (question is OpenQuestion)
            {
                var tb = new TextBox();
                tb.Location = new Point(10, 10);
                tb.Width = 325;
                tb.Text = rv.Responses[0].ToString();
                tp.Controls.Add(tb);
                if (((OpenQuestion)question).IsAnswerNumeric)
                {
                    tb.KeyPress += tb_KeyPress;
                }
            }
        }

        #endregion

        private void variantsTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedTabIndex != deniedNumber)
            {
                ((ResponseVariant)question.ResponseVariants[selectedTabIndex]).Weight = (double)weightNumericUpDown.Value;
            }
            else
            {
                deniedNumber = -1;
            }

            if (variantsTabControl.SelectedTab != null)
            {
                selectedTabIndex = variantsTabControl.TabPages.IndexOf(variantsTabControl.SelectedTab);
                weightNumericUpDown.Value = (decimal)((ResponseVariant)question.ResponseVariants[selectedTabIndex]).Weight;
                Check();
            }
            else
            {
                selectedTabIndex = 0;
                weightNumericUpDown.Value = 0;
            }
        }

        #region Check
        
        private void Check()
        {
            int counterOne = 0;

            if (selectedTabIndex != 0)
            {
                ((ResponseVariant)question.ResponseVariants[selectedTabIndex]).Weight = (double)weightNumericUpDown.Value;
            }

            for (var i = 0; i < variantsTabControl.TabPages.Count; i++)
            {
                if (((ResponseVariant)question.ResponseVariants[i]).Weight == 1.0)
                {
                    counterOne++;
                }
            }

            okButton.Enabled = counterOne != 0;
        }

        #endregion

        private void weightNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            Check();
        }

        private void ResponseVariantDialog_Load(object sender, EventArgs e)
        {
            Check();

            if (question.ResponseVariants.Count == 1)
            {
                okButton.Enabled = true;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            question.ResponseVariants = (ArrayList)rvs.Clone();
        }
    }
}
