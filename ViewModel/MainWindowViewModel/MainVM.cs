using Autodesk.Revit.UI;
using System;
using System.Windows.Input;

namespace RoomFinishing
{
    public partial class MainWindowViewModel : ModelBase
    {
        public Action CloseAction { get; set; }

        private MainWindowModelService MainWindowModelService;

        private MainWindow mainWindow;

        public MainWindowViewModel(UIApplication app, MainWindow mainWindow)
        {
            MainWindowModelService = new MainWindowModelService(app);
            this.mainWindow = mainWindow;

            MainWindowModelService.PopulateWalls(this);
            Walls1 = Walls;
            Walls2 = Walls;
            Walls3 = Walls;
            Walls4 = Walls;
            Walls5 = Walls;
            MainWindowModelService.PopulateFloors(this);
            Floors1 = Floors;
            Floors2 = Floors;
            MainWindowModelService.PopulatePlinths(this);
            Plinths1 = Plinths;
            MainWindowModelService.PopulateWorksets(this);
            Worksets1 = Worksets;
        }

        public ICommand btnOK => new RelayCommandWithoutParameter(OnbtnOK);
        private void OnbtnOK()
        {
            MainWindowModelService.CreateFinishing(this);
        }

        public ICommand btnCancel => new RelayCommandWithoutParameter(OnbtnCancel);
        private void OnbtnCancel()
        {
            CloseAction();
        }
    }
}
