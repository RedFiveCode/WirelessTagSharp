using System.Linq;
using System.Windows.Input;
using WirelessTagClientApp.Common;
using WirelessTagClientApp.Interfaces;
using WirelessTagClientApp.Utils;
using WirelessTagClientApp.ViewModels;

namespace WirelessTagClientApp.Commands
{
    public class CopyAllTagsCommand
    {
        private readonly IClipboardWriter clipboardWriter;

        /// <summary>
        /// Get the command object.
        /// </summary>
        public ICommand Command { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyAllTagsCommand"/> class
        /// </summary>
        /// <param name="mode"></param>
        public CopyAllTagsCommand() : this(new ClipboardWriter())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyAllTagsCommand"/> class for unit testing
        /// </summary>
        /// <param name="clipboardWriter">Clipboard writer</param>
        public CopyAllTagsCommand(IClipboardWriter clipboardWriter)
        {
            Command = new RelayCommandT<AllTagsViewModel>(p => Copy(p), p => CanCopy(p));
            this.clipboardWriter = clipboardWriter;
        }

        private bool CanCopy(AllTagsViewModel viewModel)
        {
            return viewModel != null && viewModel.Tags != null && viewModel.Tags.Any();
        }

        private void Copy(AllTagsViewModel viewModel)
        {
            if (!CanCopy(viewModel))
            {
                return;
            }

            var csv = GetCSVData(viewModel);

            clipboardWriter.WriteText(csv);
        }

        private string GetCSVData(AllTagsViewModel viewModel)
        {
            var writer = new CSVWriter<TagViewModel>();
            writer.AddColumn(x => x.Id.ToString(), "Id");
            writer.AddColumn(x => x.Name, "Name");
            writer.AddColumn(x => x.Description, "Description");
            writer.AddColumn(x => x.Uuid.ToString(), "Uuid");
            writer.AddColumn(x => x.Temperature.ToString("f1"), "Temperature (C)");           
            writer.AddColumn(x => x.RelativeHumidity.ToString("f1"), "Relative Humidity");
            writer.AddColumn(x => x.SignalStrength.ToString(), "Signal Strength (dBm)");
            writer.AddColumn(x => x.BatteryVoltage.ToString("f2"), "Battery Voltage (V)");
            writer.AddColumn(x => x.BatteryRemaining.ToString("f1"), "Battery Remaining (%)");
            writer.AddColumn(x => x.LastCommunication.ToString("dd-MMM-yyyy HH:mm:ss"), "Last Communication");
            writer.AddColumn(x => x.IsHumidityTag.ToString(), "IsHumidityTag");

            return writer.WriteCSV(viewModel.Tags);
        }
    }
}
