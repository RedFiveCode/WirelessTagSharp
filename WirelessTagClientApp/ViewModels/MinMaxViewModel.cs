using WirelessTagClientApp.Common;
using WirelessTagClientLib;

namespace WirelessTagClientApp.ViewModels
{
    public class MinMaxViewModel : ViewModelBase
    {
        private readonly IWirelessTagAsyncClient client;
        private readonly Options options;

        public MinMaxViewModel(Options options = null)
        {
            this.options = options;

            client = new WirelessTagAsyncClient();
        }

        /// <summary>
        /// Ctor for unit testing
        /// </summary>
        /// <param name="client"></param>
        public MinMaxViewModel(IWirelessTagAsyncClient client)
        {
            this.client = client;
            options = new Options();
        }
    }
}
