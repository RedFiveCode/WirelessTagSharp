using System.Windows;
using System.Windows.Input;
using WirelessTagClientApp.Common;

namespace WirelessTagClientApp.Commands
{
    /// <summary>
    /// Command to close the app.
    /// </summary>
    public class CloseCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CloseCommand"/> class.
        /// </summary>
        public CloseCommand()
        {
            Command = new RelayCommandT<object>(p => Close(p));
        }

        private void Close(object o)
        {
             Application.Current.Shutdown();
        }

        /// <summary>
        /// Get the command object.
        /// </summary>
        public ICommand Command { get; private set; }
    }
}
