using System.Collections.Generic;
using System.ComponentModel;

namespace VisualEditor.Utils.Controls.Ribbon
{
    /// <summary>
    /// Used to extract all child components from RibbonItem objects
    /// </summary>
    public interface IContainsRibbonComponents
    {
        IEnumerable<Component> GetAllChildComponents();
    }
}