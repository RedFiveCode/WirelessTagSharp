using System.Windows.Input;
using WirelessTagClientApp.Common;
using WirelessTagClientApp.Interfaces;
using WirelessTagClientApp.Utils;

namespace WirelessTagClientApp.Commands
{
    public class NavigateHyperlinkCommand
    {
        private readonly IProcessStarter processStarter;

        public NavigateHyperlinkCommand() : this(new ProcessStarter())
        { }

        public NavigateHyperlinkCommand(IProcessStarter processStarter)
        {
            this.processStarter = processStarter;
            Command = new RelayCommandT<string>(p => Navigate(p));
        }

        /// <summary>
        /// Get the command object.
        /// </summary>
        public ICommand Command { get; private set; }

        private void Navigate(string uri)
        {
            processStarter.Start(uri);
        }
    }
}
