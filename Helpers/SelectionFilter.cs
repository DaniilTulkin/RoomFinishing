using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace RoomFinishing
{
    public class SelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            if (elem?.Category?.Id?.IntegerValue == (int)BuiltInCategory.OST_Rooms)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
}
