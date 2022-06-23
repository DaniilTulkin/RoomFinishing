using Autodesk.Revit.DB;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RoomFinishing
{
    public partial class MainWindowViewModel : ModelBase
    {
        #region datagrid property

        public static readonly DependencyProperty OptionDataGridProperty = DependencyProperty
            .RegisterAttached("OptionDataGrid", typeof(DataGrid), typeof(MainWindowViewModel), new FrameworkPropertyMetadata(OnOptionDataGridChanged));

        public static void SetOptionDataGrid(DependencyObject element, DataGrid value)
        {
            element.SetValue(OptionDataGridProperty, value);
        }

        public static DataGrid GetOptionDataGrid(DependencyObject element)
        {
            return (DataGrid)element.GetValue(OptionDataGridProperty);
        }

        private static DataGrid optionDataGrid = null;
        private static void OnOptionDataGridChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            optionDataGrid = d as DataGrid;
        }

        #endregion

        private ObservableCollection<WallType> walls3;
        public ObservableCollection<WallType> Walls3
        {
            get
            {
                return walls3;
            }
            set
            {
                walls3 = value;
                OnPropertyChanged("Walls3");
            }
        }

        private string searchTextOriginWall;
        public string SearchTextOriginWall
        {
            get
            {
                return searchTextOriginWall;
            }
            set
            {
                searchTextOriginWall = value;
                OnPropertyChanged("SearchTextOriginWall");
                Walls3 = new ObservableCollection<WallType>(Walls.ToList().Where(x => x.Name.ToLower().Contains(SearchTextOriginWall.ToLower())));
            }
        }

        private WallType selectedOriginWall;
        public WallType SelectedOriginWall
        {
            get
            {
                return selectedOriginWall;
            }
            set
            {
                selectedOriginWall = value;
                OnPropertyChanged("SelectedOriginWall");
            }
        }

        private ObservableCollection<WallType> walls4;
        public ObservableCollection<WallType> Walls4
        {
            get
            {
                return walls4;
            }
            set
            {
                walls4 = value;
                OnPropertyChanged("Walls4");
            }
        }

        private string searchTextFinishingWall1;
        public string SearchTextFinishingWall1
        {
            get
            {
                return searchTextFinishingWall1;
            }
            set
            {
                searchTextFinishingWall1 = value;
                OnPropertyChanged("SearchTextFinishingWall1");
                Walls4 = new ObservableCollection<WallType>(Walls.ToList().Where(x => x.Name.ToLower().Contains(SearchTextFinishingWall1.ToLower())));
            }
        }

        private WallType selectedFinishingWall1;
        public WallType SelectedFinishingWall1
        {
            get
            {
                return selectedFinishingWall1;
            }
            set
            {
                selectedFinishingWall1 = value;
                OnPropertyChanged("SelectedFinishingWall1");
            }
        }

        private ObservableCollection<WallType> walls5;
        public ObservableCollection<WallType> Walls5
        {
            get
            {
                return walls5;
            }
            set
            {
                walls5 = value;
                OnPropertyChanged("Walls5");
            }
        }

        private string searchTextFinishingWall2;
        public string SearchTextFinishingWall2
        {
            get
            {
                return searchTextFinishingWall2;
            }
            set
            {
                searchTextFinishingWall2 = value;
                OnPropertyChanged("SearchTextFinishingWall2");
                Walls4 = new ObservableCollection<WallType>(Walls.ToList().Where(x => x.Name.ToLower().Contains(SearchTextFinishingWall2.ToLower())));
            }
        }

        private WallType selectedFinishingWall2;
        public WallType SelectedFinishingWall2
        {
            get
            {
                return selectedFinishingWall2;
            }
            set
            {
                selectedFinishingWall2 = value;
                OnPropertyChanged("SelectedFinishingWall2");
            }
        }

        private ObservableCollection<WallsPairsModel> wallsPairs;
        public ObservableCollection<WallsPairsModel> WallsPairs
        {
            get
            {
                return wallsPairs;
            }
            set
            {
                wallsPairs = value;
                OnPropertyChanged("WallsPairs");
            }
        }

        private WallsPairsModel selectedWallsPair;
        public WallsPairsModel SelectedWallsPair
        {
            get
            {
                return selectedWallsPair;
            }
            set
            {
                selectedWallsPair = value;
                OnPropertyChanged("SelectedWallsPair");
            }
        }

        public ICommand AddWallsPair => new RelayCommandWithoutParameter(OnAddWallsPair);
        private void OnAddWallsPair()
        {
            MainWindowModelService.AddWallsPair(this);
        }

        public ICommand RemoveWallsPair => new RelayCommandWithoutParameter(OnRemoveWallsPair);
        private void OnRemoveWallsPair()
        {
            WallsPairs.Remove(SelectedWallsPair);
        }

        public ICommand SaveWallsPairs => new RelayCommandWithoutParameter(OnSaveWallsPairs);
        private void OnSaveWallsPairs()
        {
            MainWindowModelService.SaveWallsPairs(this);
        }

        public ICommand LoadWallsPairs => new RelayCommandWithoutParameter(OnLoadWallsPairs);
        private void OnLoadWallsPairs()
        {
            MainWindowModelService.LoadWallsPairs(this);
        }
    }
}
