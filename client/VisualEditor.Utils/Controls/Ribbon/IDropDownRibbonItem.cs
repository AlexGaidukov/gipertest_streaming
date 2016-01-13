using System.Drawing;

namespace VisualEditor.Utils.Controls.Ribbon
{
    public interface IDropDownRibbonItem
    {
        RibbonItemCollection DropDownItems{get;}

        Rectangle DropDownButtonBounds { get;}

        bool DropDownButtonVisible { get;}

        bool DropDownButtonSelected { get;}

        bool DropDownButtonPressed { get;}
    }
}