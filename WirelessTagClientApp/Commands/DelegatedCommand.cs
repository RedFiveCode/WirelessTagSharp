using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WirelessTagClientApp.Common;
using WirelessTagClientApp.Utils;
using WirelessTagClientApp.ViewModels;

namespace WirelessTagClientApp.Commands
{
    /// <summary>
    /// Command to delegate to command object on the inner view-model
    /// according to the active view.
    /// </summary>
    public class DelegatedCommand
    {
        private readonly Dictionary<MainWindowViewModel.ViewMode, ICommand> commandMap;

        /// <summary>
        /// Get the command object
        /// </summary>
        public ICommand Command { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegatedCommand"/> class
        /// </summary>
        /// <param name="mode"></param>
        public DelegatedCommand()
        {
            commandMap = new Dictionary<MainWindowViewModel.ViewMode, ICommand>();
            Command = new RelayCommandT<MainWindowViewModel>(p => Execute(p), p => CanExecute(p));
        }

        /// <summary>
        /// Register the command for the specified view
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="command"></param>
        public void Register(MainWindowViewModel.ViewMode mode, ICommand command)
        {
            ThrowIf.Argument.IsNull(command, nameof(command));

            commandMap[mode] = command;
        }

        private bool CanExecute(MainWindowViewModel p)
        {
            var command = GetCommandForViewMode(p);

            if (command == null)
            {
                return false; // no registered command for the view mode 
            }

            // pass inner view-model to the command
            return command.CanExecute(p.ActiveViewModel);
        }

        private void Execute(MainWindowViewModel p)
        {
            if (p == null)
            {
                return;
            }

            var command = GetCommandForViewMode(p);
            if (command == null)
            {
                throw new InvalidOperationException($"No comamnd is registered for view-model mode {p.Mode}");
            }

            // pass inner view-model to the command
            command.Execute(p.ActiveViewModel);
        }

        private ICommand GetCommandForViewMode(MainWindowViewModel p)
        {
            if (p == null)
            {
                return null;
            }

            var mode = p.Mode;
            if (commandMap.ContainsKey(mode))
            {
                return commandMap[mode];
            }

            return null;
        }
    }
}
