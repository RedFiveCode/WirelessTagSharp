using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Windows.Input;
using WirelessTagClientApp.Commands;
using WirelessTagClientApp.Interfaces;
using WirelessTagClientApp.ViewModels;
using WirelessTagClientLib.DTO;

namespace WirelessTagClientApp.Test.Commands
{
    /// <summary>
    /// Unit tests for the <see cref="CopyMinMaxTagsCommand"/> class
    /// </summary>
    
    public class CopyMinMaxTagsCommandTest
    {
        [Fact]
        public void Command_Implements_ICommand()
        {
            var target = new CopyMinMaxTagsCommand(CopyMinMaxTagsCommand.DataSource.MinMaxSummary);

            Assert.IsAssignableFrom<ICommand>(target.Command);
        }

        [Fact]
        public void CanExecute_Null_Should_Return_False()
        {
            var target = new CopyMinMaxTagsCommand(CopyMinMaxTagsCommand.DataSource.MinMaxSummary);
            MinMaxViewModel viewModel = null;

            var result = target.Command.CanExecute(viewModel);

            Assert.False(result);
        }

        [Fact]
        public void CanExecute_Data_Null_Should_Return_False()
        {
            var target = new CopyMinMaxTagsCommand(CopyMinMaxTagsCommand.DataSource.MinMaxSummary);
            var viewModel = new MinMaxViewModel()
            {
                Data = null
            };

            var result = target.Command.CanExecute(viewModel);

            Assert.False(result);
        }

        [Fact]
        public void CanExecute_Data_Empty_Should_Return_False()
        {
            var target = new CopyMinMaxTagsCommand(CopyMinMaxTagsCommand.DataSource.MinMaxSummary);
            var viewModel = new MinMaxViewModel();

            Assert.Equal(0, viewModel.Data.Count);

            var result = target.Command.CanExecute(viewModel);

            Assert.False(result);
        }

        [Fact]
        public void Execute_Should_WriteToClipboard()
        {
            var mock = CreateMockClipboardWriter();
            var target = new CopyMinMaxTagsCommand(CopyMinMaxTagsCommand.DataSource.MinMaxSummary, mock.Object);
            var viewModel = CreateMinMaxViewModel();

            target.Command.Execute(viewModel);

            mock.Verify(x => x.WriteText(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void Execute_Should_WriteExpectedMinMaxDataToClipboard()
        {
            var mock = CreateMockClipboardWriter();
            var target = new CopyMinMaxTagsCommand(CopyMinMaxTagsCommand.DataSource.MinMaxSummary, mock.Object);
            var viewModel = CreateMinMaxViewModel();

            target.Command.Execute(viewModel);

            // only check header, so we can avoid timestamp comparisons
            string expectedDataHeader = "#Id, Tag, Interval, From, IntervalTo, MinimumTemperature, MinimumTimestamp, MaximumTemperature, MaximumTimestamp, Difference, Measurements";

            mock.Verify(x => x.WriteText(It.Is<string>(csv => csv.StartsWith(expectedDataHeader))), Times.Once());
        }

        [Fact]
        public void Execute_Should_WriteExpectedRawDataToClipboard()
        {
            var mock = CreateMockClipboardWriter();
            var target = new CopyMinMaxTagsCommand(CopyMinMaxTagsCommand.DataSource.RawData, mock.Object);
            var viewModel = CreateMinMaxViewModel();

            target.Command.Execute(viewModel);

            // only check header, so we can avoid timestamp comparisons
            string expectedDataHeader = "#Id, Timestamp, Temperature, Humidity, Lux, Battery";

            mock.Verify(x => x.WriteText(It.Is<string>(csv => csv.StartsWith(expectedDataHeader))), Times.Once());
        }

        private MinMaxViewModel CreateMinMaxViewModel()
        {
            var viewModel = new MinMaxViewModel();

            const int tagId = 42;

            viewModel.Data.Add(new MinMaxMeasurementViewModel()
            {
                TagId = tagId,
                TagName = "My tag",
                Interval = TimeInterval.ThisYear,
                Minimum = new TemperatureMeasurement(2d, new DateTime(2022, 1, 1, 12, 0, 0)),
                Maximum = new TemperatureMeasurement(25d, new DateTime(2022, 7, 1, 15, 0, 0)),
                IntervalFrom = new DateTime(2022, 1, 1),
                IntervalTo = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59),
                Count = 42
            });

            var data = new List<Measurement>
            {
                new Measurement(new DateTime(2022, 1, 1, 12, 0, 0), 2d),
                new Measurement(new DateTime(2022, 7, 1, 15, 0, 0), 25d)
            };

            viewModel.RawDataCache.Update(tagId, data);

            return viewModel;
        }

        private Mock<IClipboardWriter> CreateMockClipboardWriter()
        {
            var clipboardWriterMock = new Mock<IClipboardWriter>();

            clipboardWriterMock.Setup(x => x.WriteText(It.IsAny<string>()));

            return clipboardWriterMock;
        }
    }
}
