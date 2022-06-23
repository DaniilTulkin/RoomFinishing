using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using System;

namespace RoomFinishing.Model
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class RoomFinishing : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            try
            {
                MainWindow mainWindow = new MainWindow(app);
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: Failed to run main window of the plugin.\n{ex.Message}");
            }
        }

        public string GetName() => nameof(RoomFinishing);
    }
}
