using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace RoomFinishing
{
    public class ElementWarningSwallower : IFailuresPreprocessor
    {
        public FailureProcessingResult PreprocessFailures(
          FailuresAccessor a)
        {
            IList<FailureMessageAccessor> failures = a.GetFailureMessages();

            foreach (FailureMessageAccessor f in failures)
            {
                FailureDefinitionId id = f.GetFailureDefinitionId();

                if (BuiltInFailures.JoinElementsFailures.JoiningDisjointWarn == id ||
                    BuiltInFailures.OverlapFailures.WallsOverlap == id ||
                    BuiltInFailures.OverlapFailures.WallRoomSeparationOverlap == id ||
                    BuiltInFailures.OverlapFailures.FloorsOverlap == id ||
                    BuiltInFailures.InfillFailures.InsertJoinedWall == id ||
                    BuiltInFailures.InaccurateFailures.InaccurateWall == id ||
                    BuiltInFailures.InaccurateFailures.InaccurateCurveBasedFamily == id)
                {
                    a.DeleteWarning(f);
                }
                else if (BuiltInFailures.JoinElementsFailures.CannotJoinElementsError == id ||
                         BuiltInFailures.JoinElementsFailures.CannotJoinElementsMultiPlaneError == id ||
                         BuiltInFailures.JoinElementsFailures.CannotJoinElementsStructuralError == id ||
                         BuiltInFailures.ColumnInsideWallFailures.ColumnInsideWall == id)
                {
                    f.SetCurrentResolutionType(FailureResolutionType.DetachElements);
                    a.ResolveFailure(f);
                    return FailureProcessingResult.ProceedWithCommit;
                }
            }

            return FailureProcessingResult.Continue;
        }
    }
}
