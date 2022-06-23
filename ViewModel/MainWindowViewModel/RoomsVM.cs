using System.Collections.Generic;
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

        public static readonly DependencyProperty DataGridProperty = DependencyProperty
            .RegisterAttached("DataGrid", typeof(DataGrid), typeof(MainWindowViewModel), new FrameworkPropertyMetadata(OnDataGridChanged));

        public static void SetDataGrid(DependencyObject element, DataGrid value)
        {
            element.SetValue(DataGridProperty, value);
        }

        public static DataGrid GetDataGrid(DependencyObject element)
        {
            return (DataGrid)element.GetValue(DataGridProperty);
        }

        private static DataGrid dataGrid = null;
        private static void OnDataGridChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            dataGrid = d as DataGrid;
        }

        #endregion

        private ObservableCollection<RoomModel> rooms;
        public ObservableCollection<RoomModel> Rooms
        {
            get
            {
                return rooms;
            }
            set
            {
                rooms = value;
                OnPropertyChanged("Rooms");
            }
        }

        private RoomModel selectedRoom;
        public RoomModel SelectedRoom
        {
            get
            {
                return selectedRoom;
            }
            set
            {
                selectedRoom = value;
                OnPropertyChanged("SelectedRoom");
            }
        }
                
        public ICommand AddRooms => new RelayCommandWithoutParameter(OnAddRooms);
        private void OnAddRooms()
        {
            MainWindowModelService.AddRooms(this);
            mainWindow.Activate();
        }

        public ICommand RemoveRooms => new RelayCommandWithoutParameter(OnRemoveRooms);
        private void OnRemoveRooms()
        {
            var selectedItems = dataGrid.SelectedItems;
            foreach (var room in selectedItems.Cast<RoomModel>().ToList()) Rooms.Remove(room);
        }

        public ICommand WriteVolumes => new RelayCommandWithoutParameter(OnWriteVolumes);
        private void OnWriteVolumes()
        {
            MainWindowModelService.WriteVolumes(this);
        }
    }
}
