using System.Windows;
using WirelessTagClientApp.Interfaces;

namespace WirelessTagClientApp.Views
{
    public class AboutDialogService : IDialogService
    {
        public bool ShowDialog(Window ownerWindow)
        {
            var dlg = new Views.About()
            {
                Owner = ownerWindow
            };

            var dialogResult = dlg.ShowDialog();

            return dialogResult.HasValue && dialogResult.Value;
        }
    }
}
