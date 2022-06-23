using Autodesk.Revit.DB;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;

namespace RoomFinishing
{
    public partial class MainWindowModelService
    {
        internal void AddWallsPair(MainWindowViewModel vm)
        {
            if (vm.SelectedOriginWall == null) return;

            if (vm.WallsPairs == null) vm.WallsPairs = new ObservableCollection<WallsPairsModel>();

            WallsPairsModel wallsPair = new WallsPairsModel
            {
                OriginWall = vm.SelectedOriginWall,
                FinishingWall1 = vm.SelectedFinishingWall1,
                FinishingWall2 = vm.SelectedFinishingWall2,

                OriginWallName = vm.SelectedOriginWall.Name,
                FinishingWallName1 = vm.SelectedFinishingWall1 == null ? null : vm.SelectedFinishingWall1.Name,
                FinishingWallName2 = vm.SelectedFinishingWall2 == null? null : vm.SelectedFinishingWall2.Name
            };
            vm.WallsPairs.Add(wallsPair);
        }

        internal void SaveWallsPairs(MainWindowViewModel vm)
        {
            if (vm.WallsPairs == null) return;

            Json.Write(vm.WallsPairs, doc.Title);
        }

        internal void LoadWallsPairs(MainWindowViewModel vm)
        {
            DialogResult dialogResult = MessageBox.Show($"Загрузка набора может переоределить настроенный в данный момент список параметров. Загрузить набор?",
                                                        "Загрузка набора",
                                                        MessageBoxButtons.OKCancel);
            if (dialogResult == DialogResult.Cancel) return;

            ObservableCollection<WallsPairsModel> wallsPairs = Json.Read<ObservableCollection<WallsPairsModel>>(doc.Title);
            if (wallsPairs == null) return;

            foreach (WallsPairsModel wallsPair in wallsPairs)
            {
                wallsPair.OriginWall = new FilteredElementCollector(doc)
                                           .OfClass(typeof(WallType))
                                           .Cast<WallType>()
                                           .Where(x => x.Name == wallsPair.OriginWallName)
                                           .FirstOrDefault();

                wallsPair.FinishingWall1 = new FilteredElementCollector(doc)
                                             .OfClass(typeof(WallType))
                                             .Cast<WallType>()
                                             .Where(x => x.Name == wallsPair.FinishingWallName1)
                                             .FirstOrDefault();

                wallsPair.FinishingWall2 = new FilteredElementCollector(doc)
                                             .OfClass(typeof(WallType))
                                             .Cast<WallType>()
                                             .Where(x => x.Name == wallsPair.FinishingWallName2)
                                             .FirstOrDefault();
            }

            foreach (WallsPairsModel wallsPair in wallsPairs.ToList())
                if (wallsPair.OriginWall == null) wallsPairs.Remove(wallsPair);

            vm.WallsPairs = wallsPairs;
        }
    }
}
