using System;
using System.Linq;
using System.Windows.Input;
using WirelessTagClientApp.Common;
using WirelessTagClientApp.Interfaces;
using WirelessTagClientApp.Utils;
using WirelessTagClientApp.ViewModels;

namespace WirelessTagClientApp.Commands
{
    public class CopyMinMaxTagsComand
    {
        private readonly IClipboardWriter clipboardWriter;

        /// <summary>
        /// Get the command object.
        /// </summary>
        public ICommand Command { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyMinMaxTagsComand"/> class
        /// </summary>
        /// <param name="mode"></param>
        public CopyMinMaxTagsComand() : this(new ClipboardWriter())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyMinMaxTagsComand"/> class for unit testing
        /// </summary>
        /// <param name="clipboardWriter">Clipboard writer</param>
        public CopyMinMaxTagsComand(IClipboardWriter clipboardWriter)
        {
            Command = new RelayCommandT<MinMaxViewModel>(p => Copy(p), p => CanCopy(p));
            this.clipboardWriter = clipboardWriter;
        }

        private bool CanCopy(MinMaxViewModel viewModel)
        {
            return viewModel != null && viewModel.Data != null && viewModel.Data.Any();
        }

        private void Copy(MinMaxViewModel viewModel)
        {
            if (!CanCopy(viewModel))
            {
                return;
            }

            var csv = GetCSVData(viewModel);

            clipboardWriter.WriteText(csv);
        }

        private string GetCSVData(MinMaxViewModel viewModel)
        {
            var writer = new CSVWriter<MinMaxMeasurementViewModel>();
            writer.AddColumn(x => x.TagId.ToString(), "Id");
            writer.AddColumn(x => x.TagName, "Tag");
            writer.AddColumn(x => x.Interval.ToString(), "Interval");
            writer.AddColumn(x => x.IntervalFrom.ToString(), "From");
            writer.AddColumn(x => x.IntervalTo.ToString(), "IntervalTo");
            writer.AddColumn(x => x.Minimum.Temperature.ToString("f1"), "MinimumTemperature");
            writer.AddColumn(x => x.Minimum.Timestamp.ToString("dd-MMM-yyyy HH:mm:ss"), "MinimumTimestamp");
            writer.AddColumn(x => x.Maximum.Temperature.ToString("f1"), "MaximumTemperature");
            writer.AddColumn(x => x.Maximum.Timestamp.ToString("dd-MMM-yyyy HH:mm:ss"), "MaximumTimestamp");
            writer.AddColumn(x => x.Difference.ToString("f1"), "Difference");
            writer.AddColumn(x => x.Count.ToString(), "Measurements");

            return writer.WriteCSV(viewModel.Data);
        }
    }
}
