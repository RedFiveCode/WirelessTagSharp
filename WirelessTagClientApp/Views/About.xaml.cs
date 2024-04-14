using System.Windows;
using System.Windows.Input;
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
}
