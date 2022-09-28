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
        private readonly MainWindowViewModel viewModel;
        private readonly Options options;

        public MainWindow()
        {
            InitializeComponent();

            // parse command line
            var args = Environment.GetCommandLineArgs();
            options = Parser.Default.ParseArguments<Options>(args).Value;

            viewModel = new MainWindowViewModel(options);

            DataContext = viewModel;

            Loaded += OnLoaded;

            // allow user to click and drag to move main window around
            // by clicking anywhere on main window, including child controls
            // https://stackoverflow.com/questions/34581188/draggable-wpf-window-with-no-border
            PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;           

            // Persist window state; using the name of type as key
            ((App)Application.Current).WindowPlace.Register(this);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // login and refresh the view model
            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => { viewModel.LoginAndRefresh(); }), DispatcherPriority.Background, null);
        }

        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

    }
}
