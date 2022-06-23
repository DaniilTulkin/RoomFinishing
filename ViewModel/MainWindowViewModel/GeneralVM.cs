using Autodesk.Revit.DB;
using System.Collections.ObjectModel;
using System.Linq;

namespace RoomFinishing
{
    public partial class MainWindowViewModel : ModelBase
    {
        #region wall

        private bool isWallGenerating;
        public bool IsWallGenerating
        {
            get
            {
                return isWallGenerating;
            }
            set
            {
                isWallGenerating = value;
                OnPropertyChanged("IsWallGenerating");
            }
        }

        private bool isRoomBounding;
        public bool IsRoomBounding
        {
            get
            {
                return isRoomBounding;
            }
            set
            {
                isRoomBounding = value;
                OnPropertyChanged("IsRoomBounding");
            }
        }

        private ObservableCollection<WallType> walls;
        public ObservableCollection<WallType> Walls
        {
            get
            {
                return walls;
            }
            set
            {
                walls = value;
                OnPropertyChanged("Walls");
            }
        }

        private ObservableCollection<WallType> walls1;
        public ObservableCollection<WallType> Walls1
        {
            get
            {
                return walls1;
            }
            set
            {
                walls1 = value;
                OnPropertyChanged("Walls1");
            }
        }

        private string searchTextWallType;
        public string SearchTextWallType
        {
            get
            {
                return searchTextWallType;
            }
            set
            {
                searchTextWallType = value;
                OnPropertyChanged("SearchTextWallType");
                Walls1 = new ObservableCollection<WallType>(Walls.ToList().Where(x => x.Name.ToLower().Contains(SearchTextWallType.ToLower())));
            }
        }

        private WallType selectedWallType;
        public WallType SelectedWallType
        {
            get
            {
                return selectedWallType;
            }
            set
            {
                selectedWallType = value;
                OnPropertyChanged("SelectedWallType");
            }
        }

        private int wallBaseOffset;
        public int WallBaseOffset
        {
            get
            {
                return wallBaseOffset;
            }
            set
            {
                wallBaseOffset = value;
                OnPropertyChanged("WallBaseOffset");
            }
        }

        private int wallTopOffset;
        public int WallTopOffset
        {
            get
            {
                return wallTopOffset;
            }
            set
            {
                wallTopOffset = value;
                OnPropertyChanged("WallTopOffset");
            }
        }

        private bool isSecondWallGenerating;
        public bool IsSecondWallGenerating
        {
            get
            {
                return isSecondWallGenerating;
            }
            set
            {
                isSecondWallGenerating = value;
                OnPropertyChanged("IsSecondWallGenerating");
            }
        }

        private ObservableCollection<WallType> walls2;
        public ObservableCollection<WallType> Walls2
        {
            get
            {
                return walls2;
            }
            set
            {
                walls2 = value;
                OnPropertyChanged("Walls2");
            }
        }

        private string searchTextSecondWallType;
        public string SearchTextSecondWallType
        {
            get
            {
                return searchTextSecondWallType;
            }
            set
            {
                searchTextSecondWallType = value;
                OnPropertyChanged("SearchTextSecondWallType");
                Walls2 = new ObservableCollection<WallType>(Walls.ToList().Where(x => x.Name.ToLower().Contains(SearchTextSecondWallType.ToLower())));
            }
        }

        private WallType selectedSecondWallType;
        public WallType SelectedSecondWallType
        {
            get
            {
                return selectedSecondWallType;
            }
            set
            {
                selectedSecondWallType = value;
                OnPropertyChanged("SelectedSecondWallType");
            }
        }

        private int secondWallHeight;
        public int SecondWallHeight
        {
            get
            {
                return secondWallHeight;
            }
            set
            {
                secondWallHeight = value;
                OnPropertyChanged("SecondWallHeight");
            }
        }


        #endregion

        #region floor

        private bool isFloorGenerating;
        public bool IsFloorGenerating
        {
            get
            {
                return isFloorGenerating;
            }
            set
            {
                isFloorGenerating = value;
                OnPropertyChanged("IsFloorGenerating");
            }
        }

        private ObservableCollection<FloorType> floors;
        public ObservableCollection<FloorType> Floors
        {
            get
            {
                return floors;
            }
            set
            {
                floors = value;
                OnPropertyChanged("Floors");
            }
        }

        private ObservableCollection<FloorType> floors1;
        public ObservableCollection<FloorType> Floors1
        {
            get
            {
                return floors1;
            }
            set
            {
                floors1 = value;
                OnPropertyChanged("Floors1");
            }
        }

        private string searchTextFloorType;
        public string SearchTextFloorType
        {
            get
            {
                return searchTextFloorType;
            }
            set
            {
                searchTextFloorType = value;
                OnPropertyChanged("SearchTextFloorType");
                Floors1 = new ObservableCollection<FloorType>(Floors.ToList().Where(x => x.Name.ToLower().Contains(SearchTextFloorType.ToLower())));
            }
        }

        private FloorType selectedFloorType;
        public FloorType SelectedFloorType
        {
            get
            {
                return selectedFloorType;
            }
            set
            {
                selectedFloorType = value;
                OnPropertyChanged("SelectedFloorType");
            }
        }

        private int floorBaseOffset;
        public int FloorBaseOffset
        {
            get
            {
                return floorBaseOffset;
            }
            set
            {
                floorBaseOffset = value;
                OnPropertyChanged("FloorBaseOffset");
            }
        }

        #endregion

        #region ceiling

        private bool isCeilingGenerating;
        public bool IsCeilingGenerating
        {
            get
            {
                return isCeilingGenerating;
            }
            set
            {
                isCeilingGenerating = value;
                OnPropertyChanged("IsCeilingGenerating");
            }
        }

        private ObservableCollection<FloorType> floors2;
        public ObservableCollection<FloorType> Floors2
        {
            get
            {
                return floors2;
            }
            set
            {
                floors2 = value;
                OnPropertyChanged("Floors2");
            }
        }

        private string searchTextCeilingType;
        public string SearchTextCeilingType
        {
            get
            {
                return searchTextCeilingType;
            }
            set
            {
                searchTextCeilingType = value;
                OnPropertyChanged("SearchTextCeilingType");
                Floors2 = new ObservableCollection<FloorType>(Floors.ToList().Where(x => x.Name.ToLower().Contains(SearchTextCeilingType.ToLower())));
            }
        }

        private FloorType selectedCeilingType;
        public FloorType SelectedCeilingType
        {
            get
            {
                return selectedCeilingType;
            }
            set
            {
                selectedCeilingType = value;
                OnPropertyChanged("SelectedCeilingType");
            }
        }

        private int ceilingBaseOffset;
        public int CeilingBaseOffset
        {
            get
            {
                return ceilingBaseOffset;
            }
            set
            {
                ceilingBaseOffset = value;
                OnPropertyChanged("CeilingBaseOffset");
            }
        }

        #endregion

        #region plinth

        private bool isPlinthGenerating;
        public bool IsPlinthGenerating
        {
            get
            {
                return isPlinthGenerating;
            }
            set
            {
                isPlinthGenerating = value;
                OnPropertyChanged("IsPlinthGenerating");
            }
        }

        private ObservableCollection<FamilySymbol> plinths;
        public ObservableCollection<FamilySymbol> Plinths
        {
            get
            {
                return plinths;
            }
            set
            {
                plinths = value;
                OnPropertyChanged("Plinths");
            }
        }

        private ObservableCollection<FamilySymbol> plinths1;
        public ObservableCollection<FamilySymbol> Plinths1
        {
            get
            {
                return plinths1;
            }
            set
            {
                plinths1 = value;
                OnPropertyChanged("Plinths1");
            }
        }

        private string searchTextPlinthType;
        public string SearchTextPlinthType
        {
            get
            {
                return searchTextPlinthType;
            }
            set
            {
                searchTextPlinthType = value;
                OnPropertyChanged("SearchTextPlinthType");
                Plinths1 = new ObservableCollection<FamilySymbol>(Plinths.ToList().Where(x => x.Name.ToLower().Contains(SearchTextPlinthType.ToLower())));
            }
        }

        private FamilySymbol selectedPlinthType;
        public FamilySymbol SelectedPlinthType
        {
            get
            {
                return selectedPlinthType;
            }
            set
            {
                selectedPlinthType = value;
                OnPropertyChanged("SelectedPlinthType");
            }
        }

        private int plinthBaseOffset;
        public int PlinthBaseOffset
        {
            get
            {
                return plinthBaseOffset;
            }
            set
            {
                plinthBaseOffset = value;
                OnPropertyChanged("PlinthBaseOffset");
            }
        }

        #endregion

        #region workset

        private bool isAddToWorkset;
        public bool IsAddToWorkset
        {
            get
            {
                return isAddToWorkset;
            }
            set
            {
                isAddToWorkset = value;
                OnPropertyChanged("IsAddToWorkset");
            }
        }

        private ObservableCollection<Workset> worksets;
        public ObservableCollection<Workset> Worksets
        {
            get
            {
                return worksets;
            }
            set
            {
                worksets = value;
                OnPropertyChanged("Worksets");
            }
        }

        private ObservableCollection<Workset> worksets1;
        public ObservableCollection<Workset> Worksets1
        {
            get
            {
                return worksets1;
            }
            set
            {
                worksets1 = value;
                OnPropertyChanged("Worksets1");
            }
        }

        private string searchTextWorkset;
        public string SearchTextWorkset
        {
            get
            {
                return searchTextWorkset;
            }
            set
            {
                searchTextWorkset = value;
                OnPropertyChanged("SearchTextWorkset");
                Worksets1 = new ObservableCollection<Workset>(Worksets.ToList().Where(x => x.Name.ToLower().Contains(SearchTextWorkset.ToLower())));
            }
        }

        private Workset selectedWorkset;
        public Workset SelectedWorkset
        {
            get
            {
                return selectedWorkset;
            }
            set
            {
                selectedWorkset = value;
                OnPropertyChanged("SelectedWorkset");
            }
        }

        #endregion
    }
}
