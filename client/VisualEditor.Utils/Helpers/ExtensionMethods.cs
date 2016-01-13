using System;
using System.Collections;

namespace VisualEditor.Utils.Helpers
{
    public static class ExtensionMethods
    {
        public static bool IsNull(this Object @object)
        {
            if (@object == null)
            {
                return true;
            }

            return false;
        }

        public static bool IsEmpty(this ICollection collection)
        {
            if (collection.Count == 0)
            {
                return true;
            }

            return false;
        }
    }
}