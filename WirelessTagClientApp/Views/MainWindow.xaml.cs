﻿using CommandLine;
using System;
using System.Windows;
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
            // Persist window state; using the name of type as key
            ((App)Application.Current).WindowPlace.Register(this);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // refresh view model
            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => { viewModel.Refresh(); }), DispatcherPriority.Background, null);
        }
    }
}