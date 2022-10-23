using System.Windows;
using System.Windows.Input;
using WirelessTagClientApp.Interfaces;
using WirelessTagClientApp.ViewModels;

namespace WirelessTagClientApp.Views
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();

            KeyDown += OnKeyDown;

            var viewModel = new AboutViewModel();
            viewModel.Initialise();

            DataContext = viewModel;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }

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
