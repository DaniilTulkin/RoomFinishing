using Autodesk.Revit.DB;
using Newtonsoft.Json;

namespace RoomFinishing
{
    public class WallsPairsModel
    {
        [JsonIgnore]
        public WallType OriginWall { get; set; }
        [JsonIgnore]
        public WallType FinishingWall1 { get; set; }
        [JsonIgnore]
        public WallType FinishingWall2 { get; set; }
        public string OriginWallName { get; set; }
        public string FinishingWallName1 { get; set; }
        public string FinishingWallName2 { get; set; }
    }
}
