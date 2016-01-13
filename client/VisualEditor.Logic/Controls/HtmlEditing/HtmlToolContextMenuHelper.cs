using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VisualEditor.Logic.Commands;
using VisualEditor.Logic.Controls.Docking.Documents;
using VisualEditor.Logic.Controls.Ribbon;
using VisualEditor.Logic.Controls.Ribbon.Extended;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.IO;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.Controls.HtmlEditing;
using VisualEditor.Utils.Controls.Ribbon;

namespace VisualEditor.Logic.Controls.HtmlEditing
{
    internal class HtmlToolContextMenuHelper
    {
        private RibbonContextMenu bodyContextMenu;
        private RibbonContextMenu imageContextMenu;
        private RibbonContextMenu equationContextMenu;
        private RibbonContextMenu linkContextMenu;
        private RibbonContextMenu tableContextMenu;
        private RibbonContextMenu hintDialogBodyContextMenu;
        private RibbonContextMenu hintDialogEquationContextMenu;

        public HtmlToolContextMenuHelper(HtmlEditingTool htmlEditingTool)
        {
            HtmlEditingTool = htmlEditingTool;

            HtmlEditingTool.KeyDown += VisualHtmlEditor_KeyDown;
            HtmlEditingTool.KeyUp += VisualHtmlEditor_KeyUp;
            HtmlEditingTool.MouseDown += VisualHtmlEditor_MouseDown;
            HtmlEditingTool.MouseUp += VisualHtmlEditor_MouseUp;
        }

        public HtmlEditingTool HtmlEditingTool { get; private set; }

        #region AttachContextMenu

        private void AttachContextMenu(HtmlElement he)
        {
            if (he.TagName.Equals(TagNames.BodyTagName))
            {
                if (bodyContextMenu == null)
                {
                    InitializeBodyContextMenu();
                }
            }

            if (he.TagName.Equals(TagNames.AnchorTagName))
            {
                if (!he.GetAttribute("href").Equals(string.Empty))
                {
                    if (linkContextMenu == null)
                    {
                        InitializeLinkContextMenu();
                    }
                }
            }

            if (he.TagName.Equals(TagNames.ImageTagName))
            {
                if (!he.GetAttribute("longdesc").Equals(string.Empty))
                {
                    InitializeEquationContextMenu();
                }
            }
        }

        private void AttachHintDialogContextMenu(HtmlElement he)
        {
            if (he.TagName.Equals(TagNames.BodyTagName))
            {
                if (bodyContextMenu == null)
                {
                    InitializeHintDialogBodyContextMenu();
                }
            }

            if (he.TagName.Equals(TagNames.ImageTagName))
            {
                if (!he.GetAttribute("longdesc").Equals(string.Empty))
                {
                    InitializeHintDialogEquationContextMenu();
                }
            }
        }

        #endregion

        #region InitializeContextMenu

        private void InitializeBodyContextMenu()
        {
            bodyContextMenu = new RibbonContextMenu();
            RibbonHelper.AddButton(bodyContextMenu, CommandManager.Instance.GetCommand(CommandNames.Cut));
            RibbonHelper.AddButton(bodyContextMenu, CommandManager.Instance.GetCommand(CommandNames.Copy));
            RibbonHelper.AddButton(bodyContextMenu, CommandManager.Instance.GetCommand(CommandNames.Paste));
            RibbonHelper.AddSeparator(bodyContextMenu);
            RibbonHelper.AddButton(bodyContextMenu, CommandManager.Instance.GetCommand(CommandNames.Font));
            RibbonHelper.AddSeparator(bodyContextMenu);

            if (EditorObserver.RenderingStyle.Equals(Enums.RenderingStyle.TrainingModuleDocument))
            {
                RibbonHelper.AddButton(bodyContextMenu, CommandManager.Instance.GetCommand(CommandNames.BookmarkSmall));
                RibbonHelper.AddButton(bodyContextMenu, CommandManager.Instance.GetCommand(CommandNames.ConceptSmall));
                RibbonHelper.AddButton(bodyContextMenu, CommandManager.Instance.GetCommand(CommandNames.LinkSmall));
                RibbonHelper.AddSeparator(bodyContextMenu);
            }

            
            var b = RibbonHelper.AddButton(bodyContextMenu, "Вставить");
            RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.TableSmall));
            RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.PictureSmall));
            RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.AnimationSmall));
            RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.AudioSmall));
            RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.VideoSmall));
            RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.EquationSmall));
            RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.SymbolSmall));
            RibbonHelper.AddSeparator(bodyContextMenu);
            RibbonHelper.AddButton(bodyContextMenu, CommandManager.Instance.GetCommand(CommandNames.StyleSmall));

        }

        private void InitializeLinkContextMenu()
        {
            linkContextMenu = new RibbonContextMenu();
            RibbonHelper.AddButton(linkContextMenu, CommandManager.Instance.GetCommand(CommandNames.Cut));
            RibbonHelper.AddButton(linkContextMenu, CommandManager.Instance.GetCommand(CommandNames.Copy));
            RibbonHelper.AddButton(linkContextMenu, CommandManager.Instance.GetCommand(CommandNames.Paste));
            RibbonHelper.AddSeparator(linkContextMenu);
            RibbonHelper.AddButton(linkContextMenu, CommandManager.Instance.GetCommand(CommandNames.Font));
            RibbonHelper.AddSeparator(linkContextMenu);

            if (EditorObserver.RenderingStyle.Equals(Enums.RenderingStyle.TrainingModuleDocument))
            {
                RibbonHelper.AddButton(linkContextMenu, CommandManager.Instance.GetCommand(CommandNames.BookmarkSmall));
                RibbonHelper.AddButton(linkContextMenu, CommandManager.Instance.GetCommand(CommandNames.ConceptSmall));
                RibbonHelper.AddButton(linkContextMenu, CommandManager.Instance.GetCommand(CommandNames.LinkSmall));
                RibbonHelper.AddSeparator(linkContextMenu);
            }

            var b = RibbonHelper.AddButton(linkContextMenu, "Вставить");
            RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.TableSmall));
            RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.PictureSmall));
            RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.AnimationSmall));
            RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.AudioSmall));
            RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.VideoSmall));
            RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.EquationSmall));
            RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.SymbolSmall));
            RibbonHelper.AddSeparator(linkContextMenu);
            RibbonHelper.AddButton(linkContextMenu, CommandManager.Instance.GetCommand(CommandNames.StyleSmall));
            RibbonHelper.AddSeparator(linkContextMenu);
            RibbonHelper.AddButton(linkContextMenu, CommandManager.Instance.GetCommand(CommandNames.DeleteLink));

        }

        private void InitializeEquationContextMenu()
        {
            equationContextMenu = new RibbonContextMenu();
            RibbonHelper.AddButton(equationContextMenu, CommandManager.Instance.GetCommand(CommandNames.Cut));
            RibbonHelper.AddButton(equationContextMenu, CommandManager.Instance.GetCommand(CommandNames.Copy));
            RibbonHelper.AddButton(equationContextMenu, CommandManager.Instance.GetCommand(CommandNames.Paste));
            RibbonHelper.AddSeparator(equationContextMenu);
            RibbonHelper.AddButton(equationContextMenu, CommandManager.Instance.GetCommand(CommandNames.Font));
            RibbonHelper.AddSeparator(equationContextMenu);

            if (EditorObserver.RenderingStyle.Equals(Enums.RenderingStyle.TrainingModuleDocument))
            {
                RibbonHelper.AddButton(equationContextMenu, CommandManager.Instance.GetCommand(CommandNames.BookmarkSmall));
                RibbonHelper.AddButton(equationContextMenu, CommandManager.Instance.GetCommand(CommandNames.ConceptSmall));
                RibbonHelper.AddButton(equationContextMenu, CommandManager.Instance.GetCommand(CommandNames.LinkSmall));
                RibbonHelper.AddSeparator(equationContextMenu);
            }

            var b = RibbonHelper.AddButton(equationContextMenu, "Вставить");
            RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.TableSmall));
            RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.PictureSmall));
            RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.AnimationSmall));
            RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.AudioSmall));
            RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.VideoSmall));
            RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.EquationSmall));
            RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.SymbolSmall));
            RibbonHelper.AddSeparator(equationContextMenu);
            RibbonHelper.AddButton(equationContextMenu, CommandManager.Instance.GetCommand(CommandNames.StyleSmall));
            RibbonHelper.AddSeparator(equationContextMenu);
            RibbonHelper.AddButton(equationContextMenu, CommandManager.Instance.GetCommand(CommandNames.EditEquation));

        }

        private void InitializeHintDialogBodyContextMenu()
        {
            hintDialogBodyContextMenu = new RibbonContextMenu();
            RibbonHelper.AddButton(hintDialogBodyContextMenu, CommandManager.Instance.GetCommand(CommandNames.HintCut));
            RibbonHelper.AddButton(hintDialogBodyContextMenu, CommandManager.Instance.GetCommand(CommandNames.HintCopy));
            RibbonHelper.AddButton(hintDialogBodyContextMenu, CommandManager.Instance.GetCommand(CommandNames.HintPaste));
            RibbonHelper.AddSeparator(hintDialogBodyContextMenu);
            RibbonHelper.AddButton(hintDialogBodyContextMenu, CommandManager.Instance.GetCommand(CommandNames.Font));
            RibbonHelper.AddSeparator(hintDialogBodyContextMenu);

            var b = RibbonHelper.AddButton(hintDialogBodyContextMenu, "Вставить");
            RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.HintTableSmall));
            RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.HintPictureSmall));
            RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.HintAnimationSmall));
            RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.HintEquationSmall));
            RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.HintSymbolSmall));
            RibbonHelper.AddSeparator(hintDialogBodyContextMenu);
            RibbonHelper.AddButton(hintDialogBodyContextMenu, CommandManager.Instance.GetCommand(CommandNames.HintStyleSmall));
        }

        private void InitializeHintDialogEquationContextMenu()
        {
            hintDialogEquationContextMenu = new RibbonContextMenu();
            RibbonHelper.AddButton(hintDialogEquationContextMenu, CommandManager.Instance.GetCommand(CommandNames.HintCut));
            RibbonHelper.AddButton(hintDialogEquationContextMenu, CommandManager.Instance.GetCommand(CommandNames.HintCopy));
            RibbonHelper.AddButton(hintDialogEquationContextMenu, CommandManager.Instance.GetCommand(CommandNames.HintPaste));
            RibbonHelper.AddSeparator(hintDialogEquationContextMenu);
            RibbonHelper.AddButton(hintDialogEquationContextMenu, CommandManager.Instance.GetCommand(CommandNames.Font));
            RibbonHelper.AddSeparator(hintDialogEquationContextMenu);

            var b = RibbonHelper.AddButton(hintDialogEquationContextMenu, "Вставить");
            RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.TableSmall));
            RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.PictureSmall));
            RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.AnimationSmall));
            RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.EquationSmall));
            RibbonHelper.AddButton(b.DropDownItems, CommandManager.Instance.GetCommand(CommandNames.SymbolSmall));
            RibbonHelper.AddSeparator(hintDialogEquationContextMenu);
            RibbonHelper.AddButton(hintDialogEquationContextMenu, CommandManager.Instance.GetCommand(CommandNames.StyleSmall));
            RibbonHelper.AddSeparator(hintDialogEquationContextMenu);
            RibbonHelper.AddButton(hintDialogEquationContextMenu, CommandManager.Instance.GetCommand(CommandNames.EditEquation));            
        }

        #endregion

        #region Команды с клавиатуры

        private List<Concept> concepts;
        private List<Bookmark> bookmarks;

        private void VisualHtmlEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Insert))
            {
                if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
                {
                    if (EditorObserver.ActiveEditor.CanOverwrite)
                    {
                        RibbonStatusStripEx.Instance.MakeInsert();
                    }
                    else
                    {
                        RibbonStatusStripEx.Instance.MakeOverwrite();
                    }
                }
            }

            if (e.KeyCode.Equals(Keys.F1))
            {
                CommandManager.Instance.GetCommand(CommandNames.Help).Execute(null);
            }

            if (e.KeyCode.Equals(Keys.F3))
            {
                CommandManager.Instance.GetCommand(CommandNames.Find).Execute(null);
            }

            if (e.KeyCode.Equals(Keys.F5))
            {
                CommandManager.Instance.GetCommand(CommandNames.Preview).Execute(null);
            }

            if (e.KeyCode.Equals(Keys.F6))
            {
                CommandManager.Instance.GetCommand(CommandNames.Design).Execute(null);
            }

            if (e.KeyCode.Equals(Keys.F7))
            {
                CommandManager.Instance.GetCommand(CommandNames.Course).Execute(null);
            }

            if (e.KeyCode.Equals(Keys.F8))
            {
                CommandManager.Instance.GetCommand(CommandNames.Concepts).Execute(null);
            }

            if (e.KeyCode.Equals(Keys.F9))
            {
                CommandManager.Instance.GetCommand(CommandNames.Warnings).Execute(null);
            }

            if (e.KeyCode.Equals(Keys.Apps))
            {
                if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
                {
                    AttachContextMenu(HtmlEditingTool.Document.Body);

                    bodyContextMenu.Show(HtmlEditingTool, 0, 0);
                }
            }

            if (e.KeyCode.Equals(Keys.Delete))
            {
                if (HtmlEditingTool.IsSelection)
                {
                    var html = HtmlEditingTool.GetSelection();

                    // Не предусмотрено удаление контента, являющегося рисунком из css.
                    if (html.Equals(string.Empty))
                    {
                        return;
                        //html = BodyInnerHtml;
                    }

                    var mlp = new MlParser();
                    string searchString;
                    string id;

                    #region Удаление закладок

                    bookmarks = new List<Bookmark>();

                    searchString = "class=bookmark";
                    while (html.Contains(searchString))
                    {
                        mlp.GetTagBounds(html, searchString);
                        id = mlp.GetValue("id");

                        var bi = new Guid(id);

                        foreach (Bookmark b in Warehouse.Warehouse.Instance.Bookmarks)
                        {
                            if (b.Id.Equals(bi))
                            {
                                bookmarks.Add(b);

                                break;
                            }
                        }

                        mlp.ShiftLastIndex(ref html);
                        var innerHtml = mlp.GetInnerHtml();
                        html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                        html = html.Insert(mlp.StartIndex, innerHtml);
                    }

                    if (bookmarks.Count.Equals(0))
                    {
                        bookmarks = null;
                    }

                    #endregion

                    #region Удаление компетенций

                    concepts = new List<Concept>();

                    searchString = "class=concept";
                    while (html.Contains(searchString))
                    {
                        mlp.GetTagBounds(html, searchString);
                        id = mlp.GetValue("id");

                        var ci = new Guid(id);

                        foreach (Concept c in Warehouse.Warehouse.Instance.ConceptTree.Nodes)
                        {
                            if (c.Id.Equals(ci))
                            {
                                concepts.Add(c);

                                break;
                            }
                        }

                        mlp.ShiftLastIndex(ref html);
                        var innerHtml = mlp.GetInnerHtml();
                        html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                        html = html.Insert(mlp.StartIndex, innerHtml);
                    }

                    if (concepts.Count.Equals(0))
                    {
                        concepts = null;
                    }

                    #endregion

                    // Удаление ссылок из списков не предусмотрено.
                }
            }

            Warehouse.Warehouse.IsProjectModified = true;
        }

        private void VisualHtmlEditor_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Delete))
            {
                if (!HtmlEditingTool.IsSelection)
                {
                    if (HtmlEditingTool.ActiveElement.TagName == TagNames.AnchorTagName)
                    {
                        if (HtmlEditingTool.ActiveElement.OuterHtml.Contains("class=bookmark"))
                        {
                            #region Удаление закладки

                            Bookmark bm = null;

                            foreach (var b in Warehouse.Warehouse.Instance.Bookmarks)
                            {
                                if (HtmlEditingTool.ActiveElement.Id.Equals(b.Id.ToString()))
                                {
                                    bm = b;
                                    break;
                                }
                            }

                            if (bm != null)
                            {
                                Warehouse.Warehouse.Instance.Bookmarks.Remove(bm);
                            }

                            // Удаляет ссылки на закладку.
                            foreach (var lto in Warehouse.Warehouse.Instance.LinksToObjects)
                            {
                                if (lto.ObjectId.Equals(bm.Id))
                                {
                                    if (lto.TrainingModule.TrainingModuleDocument == null)
                                    {
                                        #region Документ не создан

                                        var html_ = string.Copy(lto.TrainingModule.DocumentHtml);
                                        var mlp_ = new MlParser();
                                        string searchString_;
                                        string value;
                                        int index;
                                        string innerHtml;

                                        searchString_ = lto.ObjectId.ToString();
                                        index = 0;
                                        while (html_.Contains(searchString_))
                                        {
                                            mlp_.GetTagBounds(html_, searchString_, index);
                                            mlp_.ShiftLastIndex(ref html_);

                                            value = mlp_.GetValue("class");
                                            if (value.Equals("linktobookmark"))
                                            {
                                                innerHtml = mlp_.GetInnerHtml();
                                                html_ = html_.Remove(mlp_.StartIndex, mlp_.LastIndex - mlp_.StartIndex + 1);
                                                html_ = html_.Insert(mlp_.StartIndex, innerHtml);
                                                index = mlp_.StartIndex;
                                            }
                                            else
                                            {
                                                index = mlp_.LastIndex;
                                            }

                                            if (index == mlp_.StartIndex)
                                            {
                                                break;
                                            }
                                        }

                                        lto.TrainingModule.DocumentHtml = html_;

                                        #endregion
                                    }
                                    else
                                    {
                                        #region Документ создан

                                        var html_ = string.Copy(lto.TrainingModule.TrainingModuleDocument.HtmlEditingTool.BodyInnerHtml);
                                        var mlp_ = new MlParser();
                                        string searchString_;
                                        string value;
                                        int index;
                                        string innerHtml;

                                        searchString_ = lto.ObjectId.ToString();
                                        index = 0;
                                        while (html_.Contains(searchString_))
                                        {
                                            mlp_.GetTagBounds(html_, searchString_, index);
                                            mlp_.ShiftLastIndex(ref html_);

                                            value = mlp_.GetValue("class");
                                            if (value.Equals("linktobookmark"))
                                            {
                                                innerHtml = mlp_.GetInnerHtml();
                                                html_ = html_.Remove(mlp_.StartIndex, mlp_.LastIndex - mlp_.StartIndex + 1);
                                                html_ = html_.Insert(mlp_.StartIndex, innerHtml);
                                                index = mlp_.StartIndex;
                                            }
                                            else
                                            {
                                                index = mlp_.LastIndex;
                                            }

                                            if (index == mlp_.StartIndex)
                                            {
                                                break;
                                            }
                                        }

                                        lto.TrainingModule.TrainingModuleDocument.HtmlEditingTool.BodyInnerHtml = html_;

                                        #endregion
                                    }
                                }
                            }

                            #endregion
                        }
                    }

                    var mlp = new MlParser();
                    string searchString;

                    #region Удаление закладок

                    if (bookmarks != null)
                    {
                        var html = HtmlEditingTool.BodyInnerHtml;

                        foreach (var b in bookmarks)
                        {
                            // Удаляет закладку из списка.
                            Warehouse.Warehouse.Instance.Bookmarks.Remove(b);

                            searchString = b.Id.ToString();
                            while (html.Contains(searchString))
                            {
                                mlp.GetTagBounds(html, searchString);
                                mlp.ShiftLastIndex(ref html);

                                var innerHtml = mlp.GetInnerHtml();
                                html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                                html = html.Insert(mlp.StartIndex, innerHtml);
                            }

                            // Удаляет ссылки на закладку.
                            foreach (var lto in Warehouse.Warehouse.Instance.LinksToObjects)
                            {
                                if (lto.ObjectId.Equals(b.Id))
                                {
                                    if (lto.TrainingModule.TrainingModuleDocument == null)
                                    {
                                        #region Документ не создан

                                        var html_ = string.Copy(lto.TrainingModule.DocumentHtml);
                                        var mlp_ = new MlParser();
                                        string searchString_;
                                        string innerHtml;

                                        searchString_ = lto.ObjectId.ToString();
                                        while (html_.Contains(searchString_))
                                        {
                                            mlp_.GetTagBounds(html_, searchString_);
                                            mlp_.ShiftLastIndex(ref html_);

                                            innerHtml = mlp_.GetInnerHtml();
                                            html_ = html_.Remove(mlp_.StartIndex, mlp_.LastIndex - mlp_.StartIndex + 1);
                                            html_ = html_.Insert(mlp_.StartIndex, innerHtml);
                                        }

                                        lto.TrainingModule.DocumentHtml = html_;

                                        #endregion
                                    }
                                    else
                                    {
                                        #region Документ создан

                                        var html_ = string.Copy(lto.TrainingModule.TrainingModuleDocument.HtmlEditingTool.BodyInnerHtml);
                                        var mlp_ = new MlParser();
                                        string searchString_;
                                        string innerHtml;

                                        searchString_ = lto.ObjectId.ToString();
                                        while (html_.Contains(searchString_))
                                        {
                                            mlp_.GetTagBounds(html_, searchString_);
                                            mlp_.ShiftLastIndex(ref html_);

                                            innerHtml = mlp_.GetInnerHtml();
                                            html_ = html_.Remove(mlp_.StartIndex, mlp_.LastIndex - mlp_.StartIndex + 1);
                                            html_ = html_.Insert(mlp_.StartIndex, innerHtml);
                                        }

                                        lto.TrainingModule.TrainingModuleDocument.HtmlEditingTool.BodyInnerHtml = html_;

                                        #endregion
                                    }
                                }
                            }
                        }

                        HtmlEditingTool.BodyInnerHtml = html;
                    }

                    #endregion

                    #region Удаление компетенций

                    if (concepts != null)
                    {
                        var html = HtmlEditingTool.BodyInnerHtml;

                        foreach (var c in concepts)
                        {
                            // Удаляет компетенцию из дерева компетенций. 
                            Warehouse.Warehouse.Instance.ConceptTree.Nodes.Remove(c);

                            // Удаляет компетенцию из выходов.
                            // Если уже была удалена такая компетенция.
                            if (c.OutDummyConcept.Parent != null)
                            {
                                c.OutDummyConcept.Parent.Nodes.Remove(c.OutDummyConcept);
                            }

                            // Удаляет компетенцию из входов.
                            foreach (var idc in c.InDummyConcepts)
                            {
                                idc.Parent.Nodes.Remove(idc);
                            }
                            c.InDummyConcepts.Clear();

                            searchString = c.Id.ToString();
                            while (html.Contains(searchString))
                            {
                                mlp.GetTagBounds(html, searchString);
                                mlp.ShiftLastIndex(ref html);

                                var innerHtml = mlp.GetInnerHtml();
                                html = html.Remove(mlp.StartIndex, mlp.LastIndex - mlp.StartIndex + 1);
                                html = html.Insert(mlp.StartIndex, innerHtml);
                            }

                            // Удаляет ссылки на компетенцию.
                            foreach (var lto in Warehouse.Warehouse.Instance.LinksToObjects)
                            {
                                if (lto.ObjectId.Equals(c.Id))
                                {
                                    if (lto.TrainingModule.TrainingModuleDocument == null)
                                    {
                                        #region Документ не создан

                                        var html_ = string.Copy(lto.TrainingModule.DocumentHtml);
                                        var mlp_ = new MlParser();
                                        string searchString_;
                                        string innerHtml;

                                        searchString_ = lto.ObjectId.ToString();
                                        while (html_.Contains(searchString_))
                                        {
                                            mlp_.GetTagBounds(html_, searchString_);
                                            mlp_.ShiftLastIndex(ref html_);

                                            innerHtml = mlp_.GetInnerHtml();
                                            html_ = html_.Remove(mlp_.StartIndex, mlp_.LastIndex - mlp_.StartIndex + 1);
                                            html_ = html_.Insert(mlp_.StartIndex, innerHtml);
                                        }

                                        lto.TrainingModule.DocumentHtml = html_;

                                        #endregion
                                    }
                                    else
                                    {
                                        #region Документ создан

                                        var html_ = string.Copy(lto.TrainingModule.TrainingModuleDocument.HtmlEditingTool.BodyInnerHtml);
                                        var mlp_ = new MlParser();
                                        string searchString_;
                                        string innerHtml;

                                        searchString_ = lto.ObjectId.ToString();
                                        while (html_.Contains(searchString_))
                                        {
                                            mlp_.GetTagBounds(html_, searchString_);
                                            mlp_.ShiftLastIndex(ref html_);

                                            innerHtml = mlp_.GetInnerHtml();
                                            html_ = html_.Remove(mlp_.StartIndex, mlp_.LastIndex - mlp_.StartIndex + 1);
                                            html_ = html_.Insert(mlp_.StartIndex, innerHtml);
                                        }

                                        lto.TrainingModule.TrainingModuleDocument.HtmlEditingTool.BodyInnerHtml = html_;

                                        #endregion
                                    }
                                }
                            }
                        }

                        HtmlEditingTool.BodyInnerHtml = html;
                    }

                    #endregion
                }
            }
        }

        #endregion

        private void VisualHtmlEditor_MouseDown(object sender, MouseEventArgs e)
        {
            if (HtmlEditingTool.HighlightedElement != null)
            {
                HtmlEditingTool.Highlight(HtmlEditingTool.HighlightedElement, false);
                HtmlEditingTool.HighlightedElement = null;
            }
        }

        private void VisualHtmlEditor_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                #region Контекстное меню

                if (HtmlEditingToolHelper.GetParentDocument(HtmlEditingTool) is DocumentBase)
                {
                    if (EditorObserver.HostEditorMode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
                    {
                        var he = HtmlEditingTool.GetActiveElement(e.Location);
                        AttachContextMenu(he);

                        if (he.TagName.Equals(TagNames.BodyTagName))
                        {
                            bodyContextMenu.Show(HtmlEditingTool, e.Location);
                        }

                        if (he.TagName.Equals(TagNames.AnchorTagName))
                        {
                            if (!he.GetAttribute("href").Equals(string.Empty))
                            {
                                linkContextMenu.Show(HtmlEditingTool, e.Location);
                            }
                            else
                            {
                                bodyContextMenu.Show(HtmlEditingTool, e.Location);
                            }
                        }

                        if (he.TagName.Equals(TagNames.ImageTagName))
                        {
                            if (!he.GetAttribute("longdesc").Equals(string.Empty))
                            {
                                equationContextMenu.Show(HtmlEditingTool, e.Location);
                            }
                        }
                    }
                }
                else
                {
                    var he = HtmlEditingTool.GetActiveElement(e.Location) as HtmlElement;
                    AttachHintDialogContextMenu(he);

                    if (he.TagName.Equals(TagNames.BodyTagName))
                    {
                        hintDialogBodyContextMenu.Show(HtmlEditingTool, e.Location);
                    }

                    if (he.TagName.Equals(TagNames.ImageTagName))
                    {
                        if (!he.GetAttribute("longdesc").Equals(string.Empty))
                        {
                            hintDialogEquationContextMenu.Show(HtmlEditingTool, e.Location);
                        }
                    }
                }

                #endregion
            }

            HtmlEditingTool.ActiveElement = HtmlEditingTool.GetActiveElement(e.Location) as HtmlElement;
        }
    }
}