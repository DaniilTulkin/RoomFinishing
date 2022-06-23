using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;

namespace RoomFinishing
{
    public partial class MainWindowModelService
    {
        private UIApplication app;
        private UIDocument uidoc;
        private Document doc;
        private RevitEvent revitEvent;
        private BuiltInParameter RoomNumber = BuiltInParameter.ROOM_NUMBER;
        private BuiltInParameter RoomName = BuiltInParameter.ROOM_NAME;
        private Guid ADSK_RoomIndex = new Guid("58dc458a-4c0d-447c-9f8f-e230a45bda87"); //ADSK_Индекс кваритры
        private Guid ADSK_Position = new Guid("ae8ff999-1f22-4ed7-ad33-61503d85f0f4"); //ADSK_Позиция
        private Guid ADSK_Note = new Guid("a85b7661-26b0-412f-979c-66af80b4b2c3"); // ADSK_Примечание
        private Guid ADSK_Material = new Guid("8b5e61a2-b091-491c-8092-0b01a55d4f44"); // ADSK_Материал
        private Guid ADSK_Dimension_Height = new Guid("da753fe3-ecfa-465b-9a2c-02f55d0c2ff1"); // ADSK_Размер_Высота
        private Guid ADSK_Dimension_Thickness = new Guid("293f055d-6939-4611-87b7-9a50d0c1f50e"); // ADSK_Размер_Толщина
        private Guid ID_RoomNumber = new Guid("73586776-2480-47a0-b4da-83c8aee948c9"); // ID_Номер помещения
        private Guid ID_Name = new Guid("c39e8a85-7c2d-4259-b972-157c317a9f27"); // ID_Имя
        private Guid ID_PlinthLength = new Guid("ff6bc852-b3cd-4053-a1fa-57d0258ff04c"); // ID_Длина плинтуса
        private Guid ID_PlinthFinishing = new Guid("7fcb7790-6dc9-46d9-a6fd-1ebaf519fa77"); // ID_Отделка плинтуса
        private Guid ID_FloorFinishing = new Guid("971da00f-e9c7-4cba-9e99-49f09599d6d3"); // ID_Отделка пола
        private Guid ID_CeilingFinishing = new Guid("985b5cdc-caef-44eb-ac7c-f2415dc8288d"); // ID_Отделка потолка
        private Guid ID_FloorArea = new Guid("59358d2a-3326-43fd-8b57-a511eb30d034"); // ID_Площадь пола
        private Guid ID_CeilingArea = new Guid("0976904c-9e4d-41e3-bfe8-c6c58cbb2049"); // ID_Площадь потолка
        private Guid ID_WallFinishingBrick1 = new Guid("da836606-f165-41bc-a852-6d675cbdb640"); // ID_Отделка стен кладка1
        private Guid ID_WallFinishingBrick2 = new Guid("b99094c2-610d-419f-8096-fa05a86a2a94"); // ID_Отделка стен кладка2
        private Guid ID_WallFinishingBrick3 = new Guid("aac01ded-9949-44cf-b133-e7837355f655"); // ID_Отделка стен кладка3
        private Guid ID_WallFinishingBrick4 = new Guid("3dbd8f5a-53a2-4889-bb84-07320ce49136"); // ID_Отделка стен кладка4
        private Guid ID_WallFinishingConcrete1 = new Guid("ac3f4576-4ef4-4fbe-a34d-f33986a05934"); // ID_Отделка стен жб1
        private Guid ID_WallFinishingConcrete2 = new Guid("31a4f5dd-d406-4a93-960d-dba3719bfdb9"); // ID_Отделка стен жб2
        private Guid ID_WallFinishingConcrete3 = new Guid("26967869-47c8-4ded-b7b9-49cd8f50c58d"); // ID_Отделка стен жб3
        private Guid ID_WallFinishingConcrete4 = new Guid("3b0ac5c9-226a-43a3-aac3-8374b84a62b0"); // ID_Отделка стен жб4
        private Guid ID_WallAreaBrick1 = new Guid("eacb9acd-9021-48bb-b317-de6891a8d9d4"); // ID_Площадь стен кладка1
        private Guid ID_WallAreaBrick2 = new Guid("f8649106-12b4-40b9-afc3-065cdf48d1c0"); // ID_Площадь стен кладка2
        private Guid ID_WallAreaBrick3 = new Guid("ff8f102d-0194-41a1-8f52-75c12b9c7416"); // ID_Площадь стен кладка3
        private Guid ID_WallAreaBrick4 = new Guid("b692b138-7dec-4f34-a7f3-b6953a31c5b3"); // ID_Площадь стен кладка4
        private Guid ID_WallAreaConcrete1 = new Guid("c6d8f97e-7573-4710-8d02-e9948fff9675"); // ID_Площадь стен жб1
        private Guid ID_WallAreaConcrete2 = new Guid("a8851e6d-7da4-49e7-b020-6c35ae907c1a"); // ID_Площадь стен жб2
        private Guid ID_WallAreaConcrete3 = new Guid("97810208-5962-4a0d-bbba-c9601d7e0278"); // ID_Площадь стен жб3
        private Guid ID_WallAreaConcrete4 = new Guid("e7ebc029-dd77-4e6e-b5ae-042e14ffa9ab"); // ID_Площадь стен жб4

        public MainWindowModelService(UIApplication app)
        {
            this.app = app;
            uidoc = app.ActiveUIDocument;
            doc = uidoc.Document;
            revitEvent = new RevitEvent();
        }

        internal void PopulateWalls(MainWindowViewModel vm)
        {
            vm.Walls = new ObservableCollection<WallType>(new FilteredElementCollector(doc)
                                                              .OfClass(typeof(WallType))
                                                              .Cast<WallType>()
                                                              .OrderBy(x => x.Name));
        }

        internal void PopulateFloors(MainWindowViewModel vm)
        {
            vm.Floors = new ObservableCollection<FloorType>(new FilteredElementCollector(doc)
                                                                .OfClass(typeof(FloorType))
                                                                .Cast<FloorType>()
                                                                .OrderBy(x => x.Name));
        }

        internal void PopulatePlinths(MainWindowViewModel vm)
        {
            vm.Plinths = new ObservableCollection<FamilySymbol>(new FilteredElementCollector(doc)
                                                                    .OfClass(typeof(FamilySymbol))
                                                                    .Cast<FamilySymbol>()
                                                                    .Where(x => x.Name.Contains("Плинтус"))
                                                                    .OrderBy(x => x.Name));
        }

        internal void PopulateWorksets(MainWindowViewModel vm)
        {
            vm.Worksets = new ObservableCollection<Workset>(new FilteredWorksetCollector(doc)
                                                                .OfKind(WorksetKind.UserWorkset)
                                                                .ToWorksets()
                                                                .OrderBy(x => x.Name));
        }

        internal void CreateFinishing(MainWindowViewModel vm)
        {
            if (vm.Rooms == null) return;

            revitEvent.Run(app =>
            {
                using(TransactionGroup tGr = new TransactionGroup(doc, "Генерация отделки"))
                {
                    tGr.Start();

                    foreach (RoomModel roomModel in vm.Rooms)
                    {
                        Room room = roomModel.Room;

                        if (vm.IsFloorGenerating) GenerateFloor(vm, room);
                        if (vm.IsCeilingGenerating) GenerateCeiling(vm, room);
                        if (vm.IsWallGenerating) GenerateWall(vm, room);
                        if (vm.IsPlinthGenerating) GeneratePlinth(vm, room);
                    }

                    MessageBox.Show($"Отделка сгенерирована.",
                                    "Генерация отделки",
                                    MessageBoxButtons.OK);

                    tGr.Assimilate();
                }
            });
        }

        private void GenerateFloor(MainWindowViewModel vm, Room room)
        {
            Floor floor = null;
            using (Transaction t = new Transaction(doc, "Генерация пола"))
            {
                t.Start();

                if (vm.SelectedFloorType == null) return;

                Level level = doc.GetElement(room.LevelId) as Level;
                IList<BoundarySegment> boundarySegments = room.GetBoundarySegments(new SpatialElementBoundaryOptions())[0];

                CurveArray curveArray = new CurveArray();
                foreach (BoundarySegment boundary in boundarySegments)
                {
                    Curve boundaryCurve = boundary.GetCurve();
                    curveArray.Append(boundaryCurve);
                }

                floor = doc.Create.NewFloor(curveArray, vm.SelectedFloorType, level, false);
                if (vm.IsAddToWorkset && vm.SelectedWorkset != null) floor.get_Parameter(BuiltInParameter.ELEM_PARTITION_PARAM).Set(vm.SelectedWorkset.Id.IntegerValue);
                floor.get_Parameter(BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM).Set(CustomUnitUtils.ConvertToInternalUnits(vm.FloorBaseOffset, UnitType.MILLIMETERS));

                floor.get_Parameter(ID_RoomNumber)?.Set(room.get_Parameter(ID_RoomNumber)?.AsString());
                floor.get_Parameter(ID_Name)?.Set(room.get_Parameter(ID_Name)?.AsString());
                floor.get_Parameter(ADSK_RoomIndex)?.Set(room.get_Parameter(ADSK_RoomIndex)?.AsString());
                floor.get_Parameter(ADSK_Position)?.Set(room.Id.IntegerValue.ToString());

                t.Commit();
            }

            using (Transaction t = new Transaction(doc, "Генерация отверстий пола"))
            {
                t.Start();

                IList<IList<BoundarySegment>> boundarySegments = room.GetBoundarySegments(new SpatialElementBoundaryOptions());

                if (boundarySegments.Count() > 1)
                {
                    for (int i = 1; i < boundarySegments.Count(); i++)
                    {
                        IList<BoundarySegment> subBoundaries = boundarySegments[i];

                        CurveArray subCurveArray = new CurveArray();
                        foreach (BoundarySegment boundary in subBoundaries)
                        {
                            Curve boundaryCurve = boundary.GetCurve();
                            subCurveArray.Append(boundaryCurve);
                        }

                        doc.Create.NewOpening(floor, subCurveArray, true);
                    }
                }

                t.Commit();
            }
        }

        private void GenerateCeiling(MainWindowViewModel vm, Room room)
        {
            Floor ceiling = null;
            using (Transaction t = new Transaction(doc, "Генерация потолка"))
            {
                t.Start();

                if (vm.SelectedCeilingType == null) return;

                Level level = doc.GetElement(room.LevelId) as Level;
                IList<BoundarySegment> boundarySegments = room.GetBoundarySegments(new SpatialElementBoundaryOptions())[0];

                CurveArray curveArray = new CurveArray();
                foreach (BoundarySegment boundarySegment in boundarySegments)
                {
                    Curve boundaryCurve = boundarySegment.GetCurve();
                    curveArray.Append(boundaryCurve);
                }

                ceiling = doc.Create.NewFloor(curveArray, vm.SelectedCeilingType, level, false);
                if (vm.IsAddToWorkset && vm.SelectedWorkset != null) ceiling.get_Parameter(BuiltInParameter.ELEM_PARTITION_PARAM).Set(vm.SelectedWorkset.Id.IntegerValue);
                ceiling.get_Parameter(BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM).Set(CustomUnitUtils.ConvertToInternalUnits(vm.CeilingBaseOffset, UnitType.MILLIMETERS));
                ceiling.get_Parameter(ID_RoomNumber)?.Set(room.get_Parameter(ID_RoomNumber)?.AsString());
                ceiling.get_Parameter(ID_Name)?.Set(room.get_Parameter(ID_Name)?.AsString());
                ceiling.get_Parameter(ADSK_RoomIndex)?.Set(room.get_Parameter(ADSK_RoomIndex)?.AsString());
                ceiling.get_Parameter(ADSK_Position)?.Set(room.Id.IntegerValue.ToString());

                t.Commit();
            }

            using (Transaction t = new Transaction(doc, "Генерация отверстий потолка"))
            {
                t.Start();

                IList<IList<BoundarySegment>> boundarySegments = room.GetBoundarySegments(new SpatialElementBoundaryOptions());

                if (boundarySegments.Count() > 1)
                {
                    for (int i = 1; i < boundarySegments.Count(); i++)
                    {
                        IList<BoundarySegment> subBoundaries = boundarySegments[i];

                        CurveArray subCurveArray = new CurveArray();
                        foreach (BoundarySegment boundary in subBoundaries)
                        {
                            Curve boundaryCurve = boundary.GetCurve();
                            subCurveArray.Append(boundaryCurve);
                        }

                        doc.Create.NewOpening(ceiling, subCurveArray, true);
                    }
                }

                t.Commit();
            }
        }

        private void GenerateWall(MainWindowViewModel vm, Room room)
        {
            if (vm.SelectedWallType == null) return;

            Dictionary<Element, List<Wall>> PairsToJoin = new Dictionary<Element, List<Wall>>();

            IList<BoundarySegment> boundarySegments = GetBoundaries(room);
            foreach (BoundarySegment boundarySegment in boundarySegments)
            {
                Element originElement = doc.GetElement(boundarySegment.ElementId);
                if (originElement is ModelLine || 
                   (originElement is Wall && (originElement as Wall).WallType.Kind == WallKind.Curtain)) continue;

                Curve boundaryCurve = boundarySegment.GetCurve();

                WallType finishingWall1 = null;
                WallType finishingWall2 = null;
                ProperFinishingWall(originElement, vm, ref finishingWall1, ref finishingWall2);               

                using (Transaction t = new Transaction(doc, "Генерация стены"))
                {
                    t.Start();

                    Wall wall = null;
                    if (finishingWall1 != null)
                    {
                        Curve curveOffset = CurveOffset(boundaryCurve, finishingWall1.Width / 2);
                        Curve curve = CurveReduce(curveOffset, room);

                        wall = Wall.Create(doc, curve, finishingWall1.Id, room.LevelId, 10, 0, false, false);
                        wall.get_Parameter(BuiltInParameter.WALL_HEIGHT_TYPE).Set(room.UpperLimit.Id);
                        wall.get_Parameter(BuiltInParameter.WALL_TOP_OFFSET).Set(CustomUnitUtils.ConvertToInternalUnits(vm.WallTopOffset, UnitType.MILLIMETERS));

                        WriteParametersToWall(originElement, room, wall, vm);
                    }                                      

                    Wall secondWall = null;
                    if (vm.IsSecondWallGenerating && vm.SecondWallHeight != 0)
                    {
                        wall?.get_Parameter(BuiltInParameter.WALL_BASE_OFFSET).Set(
                              CustomUnitUtils.ConvertToInternalUnits(vm.WallBaseOffset, UnitType.MILLIMETERS) +
                              CustomUnitUtils.ConvertToInternalUnits(vm.SecondWallHeight, UnitType.MILLIMETERS));

                        if (finishingWall2 != null)
                        {
                            Curve secondCurveOffset = CurveOffset(boundaryCurve, finishingWall2.Width / 2);
                            Curve secondCurve = CurveReduce(secondCurveOffset, room);

                            secondWall = Wall.Create(doc, secondCurve, finishingWall2.Id, room.LevelId,
                                         CustomUnitUtils.ConvertToInternalUnits(vm.SecondWallHeight, UnitType.MILLIMETERS),
                                         CustomUnitUtils.ConvertToInternalUnits(vm.WallBaseOffset, UnitType.MILLIMETERS), false, false);

                            WriteParametersToWall(originElement, room, secondWall, vm);
                        }                        
                    }
                    else
                    {
                        wall?.get_Parameter(BuiltInParameter.WALL_BASE_OFFSET).Set(CustomUnitUtils.ConvertToInternalUnits(vm.WallBaseOffset, UnitType.MILLIMETERS));
                    }

                    if (originElement != null) PairsToJoin[originElement] = new List<Wall> { wall, secondWall };

                    t.Commit();
                }
            }

            using (Transaction t = new Transaction(doc, "Соединение стены"))
            {
                t.Start();

                FailureHandlingOptions failOpt = t.GetFailureHandlingOptions();
                failOpt.SetFailuresPreprocessor(new ElementWarningSwallower());
                t.SetFailureHandlingOptions(failOpt);

                foreach (KeyValuePair<Element, List<Wall>> pair in PairsToJoin)
                {
                    if (pair.Key is Wall || pair.Key is FamilyInstance)
                    {
                        if (pair.Value[0] != null) JoinGeometryUtils.JoinGeometry(doc, pair.Key, pair.Value[0]);
                        if (pair.Value[1] != null) JoinGeometryUtils.JoinGeometry(doc, pair.Key, pair.Value[1]);
                    }
                }

                List<List<Wall>> values = PairsToJoin.Values.ToList();
                foreach (List<Wall> listWall in values)
                {
                    foreach (Wall wall in listWall)
                    {
                        if (wall == null) continue;
                        WallUtils.DisallowWallJoinAtEnd(wall, 0);
                        WallUtils.DisallowWallJoinAtEnd(wall, 1);
                    }
                }

                t.Commit();
            }
        }

        private void WriteParametersToWall(Element originElement, 
                                           Room room, 
                                           Wall wall, 
                                           MainWindowViewModel vm)
        {
            if (vm.IsAddToWorkset && vm.SelectedWorkset != null) wall.get_Parameter(BuiltInParameter.ELEM_PARTITION_PARAM).Set(vm.SelectedWorkset.Id.IntegerValue);
            wall.get_Parameter(BuiltInParameter.WALL_ATTR_ROOM_BOUNDING).Set(vm.IsRoomBounding ? 1 : 0);
            wall.get_Parameter(ID_RoomNumber)?.Set(room.get_Parameter(ID_RoomNumber)?.AsString());
            wall.get_Parameter(ID_Name)?.Set(room.get_Parameter(ID_Name)?.AsString());
            wall.get_Parameter(ADSK_RoomIndex)?.Set(room.get_Parameter(ADSK_RoomIndex)?.AsString());
            wall.get_Parameter(ADSK_Position)?.Set(room.Id.IntegerValue.ToString());

            if (originElement is Wall)
            {
                wall.get_Parameter(ADSK_Note)?.Set(
                    doc.GetElement((originElement as Wall).WallType.GetCompoundStructure()
                    .GetLayers().FirstOrDefault(x => x.LayerId == 0).MaterialId)
                    .get_Parameter(BuiltInParameter.ALL_MODEL_DESCRIPTION).AsString());
            }
            else if (originElement is FamilyInstance)
            {
                wall.get_Parameter(ADSK_Note)?.Set(doc.GetElement(doc.GetElement(originElement.GetTypeId())
                    .get_Parameter(BuiltInParameter.STRUCTURAL_MATERIAL_PARAM)?.AsElementId())
                    .get_Parameter(BuiltInParameter.ALL_MODEL_DESCRIPTION).AsString());
            }
        }

        private void GeneratePlinth(MainWindowViewModel vm, Room room)
        {
            using (Transaction t = new Transaction(doc, "Генерация плинтуса"))
            {
                t.Start();

                if (vm.SelectedPlinthType == null) return;

                FamilySymbol plinthType = vm.SelectedPlinthType;
                plinthType.Activate();

                Level level = doc.GetElement(room.LevelId) as Level;
                IList<BoundarySegment> boundarySegments = GetBoundaries(room);

                foreach (BoundarySegment boundarySegment in boundarySegments)
                {
                    if (doc.GetElement(boundarySegment.ElementId) is ModelLine) continue;

                    Curve boundaryCurve = boundarySegment.GetCurve();
                    Curve curve = null;
                    if (vm.IsWallGenerating && vm.SelectedWallType != null)
                        curve = CurveOffset(boundaryCurve, vm.SelectedWallType.Width) as Line;
                    else curve = boundaryCurve as Line;

                    FamilyInstance plinth =  doc.Create.NewFamilyInstance(curve, plinthType, level, StructuralType.NonStructural);
                    if (vm.IsAddToWorkset && vm.SelectedWorkset != null) plinth.get_Parameter(BuiltInParameter.ELEM_PARTITION_PARAM).Set(vm.SelectedWorkset.Id.IntegerValue);
                    plinth.get_Parameter(BuiltInParameter.INSTANCE_FREE_HOST_OFFSET_PARAM).Set(CustomUnitUtils.ConvertToInternalUnits(vm.PlinthBaseOffset, UnitType.MILLIMETERS));

                    plinth.get_Parameter(ID_RoomNumber)?.Set(room.get_Parameter(ID_RoomNumber)?.AsString());
                    plinth.get_Parameter(ID_Name)?.Set(room.get_Parameter(ID_Name)?.AsString());
                    plinth.get_Parameter(ADSK_RoomIndex)?.Set(room.get_Parameter(ADSK_RoomIndex)?.AsString());
                    plinth.get_Parameter(ADSK_Position)?.Set(room.Id.IntegerValue.ToString());
                }

                t.Commit();
            }
        }

        private void ProperFinishingWall(Element originElement,
                                         MainWindowViewModel vm,
                                         ref WallType finishingWall1,
                                         ref WallType finishingWall2)
        {
            WallType originWall = null;
            foreach (WallsPairsModel wallsPair in vm.WallsPairs ?? new ObservableCollection<WallsPairsModel>())
            {
                if (originElement?.Name == wallsPair.OriginWallName)
                {
                    originWall = wallsPair.OriginWall;
                    finishingWall1 = wallsPair.FinishingWall1;
                    finishingWall2 = wallsPair.FinishingWall2;

                    break;
                }
            }
            if (originWall == null)
            {
                finishingWall1 = vm.SelectedWallType;
                finishingWall2 = vm.SelectedSecondWallType;
            }
        }

        private Curve CurveOffset(Curve curve, double distance, bool outside = false)
        {
            XYZ vectorZ = XYZ.BasisZ;
            if (outside)
            {
                vectorZ = new XYZ(0, 0, -1);
            }

            XYZ point_1 = curve.GetEndPoint(0);
            XYZ point_2 = curve.GetEndPoint(1);
            XYZ vector = point_1 - point_2;
            XYZ vectorOffset = vector.CrossProduct(vectorZ).Normalize();
            XYZ distanceOffset = vectorOffset.Multiply(distance);

            XYZ startPoint = point_1.Add(distanceOffset);
            XYZ endPoint = point_2.Add(distanceOffset);

            return Line.CreateBound(startPoint, endPoint);
        }

        private Curve CurveReduce(Curve curveOffset, Room room)
        {
            XYZ point_1 = curveOffset.GetEndPoint(0);
            XYZ point_2 = curveOffset.GetEndPoint(1);
            XYZ vector = point_1 - point_2;
            XYZ distanceOffset = vector.Normalize().Multiply(CustomUnitUtils.ConvertToInternalUnits(0.5, UnitType.MILLIMETERS));

            XYZ startPoint = IsPointInRoom(point_1, room, distanceOffset);
            XYZ endPoint = IsPointInRoom(point_2, room, distanceOffset);

            try
            {
                return Line.CreateBound(startPoint, endPoint);
            }
            catch
            {
                return curveOffset;
            }
        }

        private XYZ IsPointInRoom(XYZ point, Room room, XYZ distanceOffset)
        {
            if (room.IsPointInRoom(point.Add(distanceOffset)) && room.IsPointInRoom(point.Add(-distanceOffset)))
            {
                return point;
            }
            else if (room.IsPointInRoom(point.Add(-distanceOffset)))
            {
                return point.Add(-distanceOffset);
            }
            else if (room.IsPointInRoom(point.Add(distanceOffset)))
            {
                return point.Add(distanceOffset);
            }
            else
            {
                return point;
            }
        }

        private IList<BoundarySegment> GetBoundaries(Room room)
        {
            IList<BoundarySegment> boundarySegments = new List<BoundarySegment>();
            foreach (IList<BoundarySegment> boundarys in room.GetBoundarySegments(new SpatialElementBoundaryOptions()))
            {
                foreach (BoundarySegment boundary in boundarys)
                {
                    boundarySegments.Add(boundary);
                }
            }

            return boundarySegments;
        }
    }
}
