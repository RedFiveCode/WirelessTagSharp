using CommandLine;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using WirelessTagClientApp.ViewModels;

namespace WirelessTagClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AllTagsViewModel viewModel;
        private readonly Options options;

        public MainWindow()
        {
            InitializeComponent();

            // parse command line
            var args = Environment.GetCommandLineArgs();
            options = Parser.Default.ParseArguments<Options>(args).Value;

            viewModel = new AllTagsViewModel(options);

            DataContext = viewModel;

            Loaded += OnLoaded;
            MouseDown += OnMouseDown;
            KeyDown += OnKeyDown;

            // Persist window state; using the name of type as key
            ((App)Application.Current).WindowPlace.Register(this);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // refresh view model
            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => { viewModel.Refresh(); }), DispatcherPriority.Background, null);
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
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
