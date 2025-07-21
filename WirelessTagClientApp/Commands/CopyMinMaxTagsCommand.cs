using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using WirelessTagClientApp.Common;
using WirelessTagClientApp.Interfaces;
using WirelessTagClientApp.Utils;
using WirelessTagClientApp.ViewModels;
using WirelessTagClientLib.DTO;

namespace WirelessTagClientApp.Commands
{
    public class CopyMinMaxTagsCommand
    {
        private readonly IClipboardWriter _clipboardWriter;
        private readonly DataSource _dataSource;

        public enum DataSource { MinMaxSummary, RawData }

        /// <summary>
        /// Get the command object.
        /// </summary>
        public ICommand Command { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyMinMaxTagsCommand"/> class
        /// </summary>
        /// <param name="dataSource">Data source to copy from</param>
        public CopyMinMaxTagsCommand(DataSource dataSource) : this(dataSource, new ClipboardWriter())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyMinMaxTagsCommand"/> class for unit testing
        /// </summary>
        /// <param name="dataSource">Data source to copy from</param>
        /// <param name="clipboardWriter">Clipboard writer</param>
        public CopyMinMaxTagsCommand(DataSource dataSource, IClipboardWriter clipboardWriter)
        {
            _dataSource = dataSource;
            _clipboardWriter = clipboardWriter;

            Command = new RelayCommandT<MinMaxViewModel>(p => Copy(p), p => CanCopy(p));
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

            _clipboardWriter.WriteText(csv);
        }

        private string GetCSVData(MinMaxViewModel viewModel)
        {
            if (_dataSource == DataSource.MinMaxSummary)
            {
                return GetCSVData(viewModel.Data);
            }
            else if (_dataSource == DataSource.RawData)
            {
                return GetCSVData(viewModel.RawDataCache.GetAllData());
            }

            throw new InvalidOperationException($"Unknown data source ({_dataSource}) for CopyMinMaxTagsCommand");
        }

        private string GetCSVData(IEnumerable<MinMaxMeasurementViewModel> data)
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

            return writer.WriteCSV(data.ToList());
        }

        private string GetCSVData(IEnumerable<TagMeasurementDataPoint> data)
        {
            var writer = new CSVWriter<TagMeasurementDataPoint>();

            writer.AddColumn(x => x.TagId.ToString(), "Id");
            writer.AddColumn(x => x.Time.ToString("dd-MMM-yyyy HH:mm:ss"), "Timestamp");
            writer.AddColumn(x => x.Temperature.ToString("f1"), "Temperature");
            writer.AddColumn(x => x.Humidity.ToString("f1"), "Humidity");
            writer.AddColumn(x => x.Lux.ToString("f1"), "Lux");
            writer.AddColumn(x => x.Battery.ToString("f2"), "Battery");

            return writer.WriteCSV(data.ToList());
        }
    }
}
