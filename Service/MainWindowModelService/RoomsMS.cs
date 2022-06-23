using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace RoomFinishing
{
    public partial class MainWindowModelService
    {
        internal void AddRooms(MainWindowViewModel vm)
        {
            IList<Reference> references = null;
            try
            {
                references = uidoc.Selection.PickObjects(ObjectType.Element, new SelectionFilter(), "Выберите помещения");
            }
            catch 
            {
                return;
            }

            if (vm.Rooms == null) vm.Rooms = new ObservableCollection<RoomModel>();
            foreach (Reference reference in references ?? new Reference[0])
            {
                Room room = doc.GetElement(reference.ElementId) as Room;
                RoomModel roomModel = new RoomModel
                {
                    Room = room,
                    RoomNumber = room.get_Parameter(RoomNumber)?.AsString(),
                    RoomName = room.get_Parameter(RoomName)?.AsString()
                };

                if (!vm.Rooms.Select(x => x.Room.Id).Contains(room.Id)) vm.Rooms.Add(roomModel);
            }

            if (!vm.Rooms.Any()) vm.Rooms = null;
        }

        internal void WriteVolumes(MainWindowViewModel vm)
        {
            if (vm.Rooms == null) return;

            revitEvent.Run(app =>
            {
                using (Transaction t = new Transaction(doc, "Запись объёмов"))
                {
                    t.Start();

                    foreach (RoomModel roomModel in vm.Rooms)
                    {  
                        Room room = roomModel.Room;
                        string roomId = room.Id.IntegerValue.ToString();

                        try
                        {
                            room.get_Parameter(ID_PlinthLength)?.ClearValue();
                            room.get_Parameter(ID_PlinthFinishing)?.ClearValue();
                            room.get_Parameter(ID_FloorFinishing)?.ClearValue();
                            room.get_Parameter(ID_CeilingFinishing)?.ClearValue();
                            room.get_Parameter(ID_FloorArea)?.ClearValue();
                            room.get_Parameter(ID_CeilingArea)?.ClearValue();
                            room.get_Parameter(ID_WallFinishingBrick1)?.ClearValue();
                            room.get_Parameter(ID_WallFinishingBrick2)?.ClearValue();
                            room.get_Parameter(ID_WallFinishingBrick3)?.ClearValue();
                            room.get_Parameter(ID_WallFinishingBrick4)?.ClearValue();
                            room.get_Parameter(ID_WallFinishingConcrete1)?.ClearValue();
                            room.get_Parameter(ID_WallFinishingConcrete2)?.ClearValue();
                            room.get_Parameter(ID_WallFinishingConcrete3)?.ClearValue();
                            room.get_Parameter(ID_WallFinishingConcrete4)?.ClearValue();
                            room.get_Parameter(ID_WallAreaBrick1)?.ClearValue();
                            room.get_Parameter(ID_WallAreaBrick2)?.ClearValue();
                            room.get_Parameter(ID_WallAreaBrick3)?.ClearValue();
                            room.get_Parameter(ID_WallAreaBrick4)?.ClearValue();
                            room.get_Parameter(ID_WallAreaConcrete1)?.ClearValue();
                            room.get_Parameter(ID_WallAreaConcrete2)?.ClearValue();
                            room.get_Parameter(ID_WallAreaConcrete3)?.ClearValue();
                            room.get_Parameter(ID_WallAreaConcrete4)?.ClearValue();
                        }
                        catch { }

                        #region ceiling

                        var ceilings = new FilteredElementCollector(doc)
                                        .OfClass(typeof(Floor))
                                        .WhereElementIsNotElementType()
                                        .Cast<Floor>()
                                        .Where(x => x.get_Parameter(ADSK_Position)?.AsString() == roomId)
                                        .Where(x => x.Name.Contains("Потолок"));

                        if (ceilings.Any())
                        {
                            double ceilingArea = 0;
                            foreach (Floor ceiling in ceilings)
                            {
                                ceilingArea += ceiling.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED).AsDouble();

                            }

                            room.get_Parameter(ID_CeilingFinishing)?.Set(GetLayersInfo(ceilings.FirstOrDefault()));
                            room.get_Parameter(ID_CeilingArea)?.Set(ceilingArea);
                        }

                        #endregion

                        #region floor

                        var floors = new FilteredElementCollector(doc)
                                          .OfClass(typeof(Floor))
                                          .WhereElementIsNotElementType()
                                          .Cast<Floor>()
                                          .Where(x => x.get_Parameter(ADSK_Position)?.AsString() == roomId)
                                          .Where(x => x.Name.Contains("Пол"));

                        if (floors.Any())
                        {
                            double floorArea = 0;
                            foreach(Floor floor in floors)
                            {
                                floorArea += floor.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED).AsDouble();
                            }

                            room.get_Parameter(ID_FloorArea)?.Set(floorArea);
                            room.get_Parameter(ID_FloorFinishing)?.Set(GetLayersInfo(floors.FirstOrDefault()));
                        }

                        #endregion

                        #region plinth

                        double plinthLength = 0;
                        string plinthMaterialName = string.Empty;
                        var plinths = new FilteredElementCollector(doc)
                                          .OfClass(typeof(FamilyInstance))
                                          .WhereElementIsNotElementType()
                                          .Cast<FamilyInstance>()
                                          .Where(x => x.Name.Contains("Плинтус"))
                                          .Where(x => x.get_Parameter(ADSK_Position)?.AsString() == roomId);

                        if (plinths.Any())
                        {
                            plinthMaterialName = doc.GetElement(doc.GetElement(plinths.FirstOrDefault().GetTypeId())
                                .get_Parameter(ADSK_Material).AsElementId()).get_Parameter(BuiltInParameter.ALL_MODEL_DESCRIPTION).AsString();
                            foreach (FamilyInstance plinth in plinths)
                            {
                                Element plinthType = doc.GetElement(plinth.GetTypeId());
                                double height = plinthType.get_Parameter(ADSK_Dimension_Height) != null ?
                                                plinthType.get_Parameter(ADSK_Dimension_Height).AsDouble() : 1;
                                double thickness = plinthType.get_Parameter(ADSK_Dimension_Thickness) != null ?
                                                   plinthType.get_Parameter(ADSK_Dimension_Thickness).AsDouble() : 1;
                                plinthLength += plinth.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble() /
                                                height / thickness;
                            }

                            room.get_Parameter(ID_PlinthLength)?.Set(plinthLength);
                            room.get_Parameter(ID_PlinthFinishing)?.Set(plinthMaterialName);
                        }

                        #endregion

                        #region wall

                        var wallGroupsBrick = new FilteredElementCollector(doc)
                                             .OfClass(typeof(Wall))
                                             .WhereElementIsNotElementType()
                                             .Cast<Wall>()
                                             .Where(x => x.get_Parameter(ADSK_Position)?.AsString() == roomId)
                                             .Where(x => x.get_Parameter(ADSK_Note)?.AsString() != "Железобетон")
                                             .OrderBy(x => x.Name)
                                             .GroupBy(x => x.Name)
                                             .ToList();

                        var wallGroupsConcrete = new FilteredElementCollector(doc)
                                             .OfClass(typeof(Wall))
                                             .WhereElementIsNotElementType()
                                             .Cast<Wall>()
                                             .Where(x => x.get_Parameter(ADSK_Position)?.AsString() == roomId)
                                             .Where(x => x.get_Parameter(ADSK_Note)?.AsString() == "Железобетон")
                                             .OrderBy(x => x.Name)
                                             .GroupBy(x => x.Name)
                                             .ToList();

                        WriteWallInformation(room, wallGroupsBrick, "Brick");
                        WriteWallInformation(room, wallGroupsConcrete, "Concrete");

                        #endregion
                    }

                    MessageBox.Show($"Объёмы записаны в помещения.",
                                    "Запись объёмов",
                                    MessageBoxButtons.OK);

                    t.Commit();
                }
            });
        }

        private void WriteWallInformation(Room room, List<IGrouping<string, Wall>> wallGroups, string origin)
        {
            if (wallGroups.Any())
            {
                for (int i = 0; i < wallGroups.Count(); i++)
                {
                    if (i > 3) break;

                    double wallArea = 0;
                    foreach (Wall wall in wallGroups[i]) wallArea += wall.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED).AsDouble();

                    FieldInfo ID_WallAreaInfo = GetType().GetField($"ID_WallArea{origin}{i + 1}", BindingFlags.Instance | BindingFlags.NonPublic);
                    var ID_WallAreaGuid = ID_WallAreaInfo.GetValue(this);
                    FieldInfo ID_WallFinishingInfo = GetType().GetField($"ID_WallFinishing{origin}{i + 1}", BindingFlags.Instance | BindingFlags.NonPublic);
                    var ID_WallFinishingGuid = ID_WallFinishingInfo.GetValue(this);

                    room.get_Parameter((Guid)ID_WallAreaGuid)?.Set(wallArea);
                    room.get_Parameter((Guid)ID_WallFinishingGuid)?.Set(GetLayersInfo(wallGroups[i].ElementAt(0)));
                }
            }
        }

        private string GetLayersInfo(Element element)
        {
            HostObjAttributes hostObj = (HostObjAttributes)(doc.GetElement(element.GetTypeId()));

            int c = 1;
            string constitution = string.Empty;
            foreach (CompoundStructureLayer layer in hostObj.GetCompoundStructure().GetLayers())
            {
                string materialName = doc.GetElement(layer.MaterialId)?.get_Parameter(BuiltInParameter.ALL_MODEL_DESCRIPTION)?.AsString();
                constitution += $"{c}. {materialName};{Environment.NewLine}";
                c++;
            }

            return constitution;
        }
    }
}
