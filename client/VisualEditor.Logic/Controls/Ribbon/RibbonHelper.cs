using System;
using System.Windows.Forms;
using VisualEditor.Logic.Commands;
using VisualEditor.Logic.Controls.Ribbon.Extended;
using VisualEditor.Logic.Controls.Ribbon.Extended.Hint;
using VisualEditor.Utils.Controls.Ribbon;
using VisualEditor.Utils.Helpers;

namespace VisualEditor.Logic.Controls.Ribbon
{
    internal static class RibbonHelper
    {
        public static void AddTab(Utils.Controls.Ribbon.Ribbon ribbon, RibbonTab ribbonTab)
        {
            if (ribbon.IsNull() ||
                ribbonTab.IsNull())
            {
                throw new ArgumentNullException();
            }

            ribbon.Tabs.Add(ribbonTab);
        }

        public static void AddPanel(RibbonTab ribbonTab, RibbonPanel ribbonPanel)
        {
            if (ribbonTab.IsNull() ||
                ribbonPanel.IsNull())
            {
                throw new ArgumentNullException();
            }

            ribbonTab.Panels.Add(ribbonPanel);
        }

        public static void AddButton(RibbonQuickAccessToolbar toolbar, AbstractCommand command)
        {
            if (toolbar.IsNull() ||
                command.IsNull())
            {
                throw new ArgumentNullException();
            }

            toolbar.Items.Add(new RibbonButtonEx(command));

            CommandManager.Instance.Register(command);
        }

        public static void AddOrbMenuItem(Utils.Controls.Ribbon.Ribbon ribbon, AbstractCommand command)
        {
            if (ribbon.IsNull() ||
                command.IsNull())
            {
                throw new ArgumentNullException();
            }

            var orbMenuItem = new RibbonOrbMenuItemEx(command);
            ribbon.OrbDropDown.MenuItems.Add(orbMenuItem);

            CommandManager.Instance.Register(command);
        }

        public static void AddSeparator(Utils.Controls.Ribbon.Ribbon ribbon)
        {
            ribbon.OrbDropDown.MenuItems.Add(new RibbonSeparator());
        }

        public static void AddOrbOptionButton(Utils.Controls.Ribbon.Ribbon ribbon, AbstractCommand command)
        {
            var oob = new RibbonOrbOptionButtonEx(command);
            ribbon.OrbDropDown.OptionItems.Add(oob);

            CommandManager.Instance.Register(command);
        }

        public static ToolStripMenuItem AddButton(RibbonContextMenu contextMenu, string text)
        {
            var menuItem = new ToolStripMenuItem(text);
            contextMenu.Items.Add(menuItem);

            return menuItem;
        }

        public static void AddButton(RibbonContextMenu contextMenu, AbstractCommand command)
        {
            contextMenu.Items.Add(new RibbonContextMenuItemEx(command));
        }

        public static void AddSeparator(RibbonContextMenu contextMenu)
        {
            contextMenu.Items.Add(new ToolStripSeparator());
        }

        public static void AddButton(ToolStripItemCollection itemCollection, AbstractCommand command)
        {
            itemCollection.Add(new RibbonContextMenuItemEx(command));
        }

        public static RibbonButtonEx AddButton(RibbonPanel ribbonPanel, AbstractCommand command)
        {
            var button = new RibbonButtonEx(command);
            ribbonPanel.Items.Add(button);

            CommandManager.Instance.Register(command);

            return button;
        }

        public static RibbonComboBoxEx AddComboBox(RibbonItemGroup rig, AbstractCommand command, Type t)
        {
            RibbonComboBoxEx cb = null;

            if (t == typeof(FontNameComboBox))
            {
                cb = new FontNameComboBox(command);
            }

            if (t == typeof(FontSizeComboBox))
            {
                cb = new FontSizeComboBox(command);
            }
            
            if (t == typeof(HintFontNameComboBox))
            {
                cb = new HintFontNameComboBox(command);
            }

            if (t == typeof(HintFontSizeComboBox))
            {
                cb = new HintFontSizeComboBox(command);
            }

            rig.Items.Add(cb);

            CommandManager.Instance.Register(command);

            return cb;
        }

        public static void AddGroup(RibbonPanel panel, RibbonItemGroup rig)
        {
            panel.Items.Add(rig);
        }

        public static RibbonButtonEx AddButton(RibbonItemGroup rig, AbstractCommand command)
        {
            var button = new RibbonButtonEx(command);
            rig.Items.Add(button);

            CommandManager.Instance.Register(command);

            return button;
        }

        public static void AddSeparator(RibbonPanel ribbonPanel)
        {
            ribbonPanel.Items.Add(new RibbonSeparator());
        }

        public static void AddButton(RibbonButton button, AbstractCommand command)
        {
            button.DropDownItems.Add(new RibbonButtonEx(command));
        }

        public static void AddOrbRecentButton(Utils.Controls.Ribbon.Ribbon ribbon, string text)
        {
            var orb = new RibbonOrbRecentButtonEx(CommandManager.Instance.GetCommand(CommandNames.RecentProject));

            // Подсчитывает количество "\\" в строке.
            int tempIndex = 0;
            int count = 0;
            tempIndex = text.IndexOf("\\", tempIndex);
            do
            {
                if (tempIndex != -1)
                {
                    count++;
                }
                tempIndex = text.IndexOf("\\", tempIndex + 1);
            } while (tempIndex != -1);

            if (count > 2)
            {
                int index = text.IndexOf("\\");
                var str1 = text.Substring(0, index + 1);
                index = text.LastIndexOf("\\");
                index = text.LastIndexOf("\\", index - 1);
                var str2 = text.Substring(index, text.Length - index);
                orb.Text = string.Concat(str1, " ... ", str2);
                orb.ProjectPath = text;
                ribbon.OrbDropDown.RecentItems.Add(orb);
            }
            else
            {
                orb.Text = text;
                orb.ProjectPath = text;
                ribbon.OrbDropDown.RecentItems.Add(orb);
            }
        }

        public static void ClearOrbRecentButtons(Utils.Controls.Ribbon.Ribbon ribbon)
        {
            ribbon.OrbDropDown.RecentItems.Clear();
        }
    }
}