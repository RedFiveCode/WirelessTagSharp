using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Windows.Input;
using WirelessTagClientApp.Commands;
using WirelessTagClientApp.Interfaces;
using WirelessTagClientApp.ViewModels;

namespace WirelessTagClientApp.Test.Commands
{
    /// <summary>
    /// Unit tests for the <see cref="CopyMinMaxTagsComand"/> class
    /// </summary>
    [TestClass]
    public class CopyMinMaxTagsComandTest
    {
        [TestMethod]
        public void Command_Implements_ICommand()
        {
            var target = new CopyMinMaxTagsComand();

            Assert.IsInstanceOfType(target.Command, typeof(ICommand));
        }

        [TestMethod]
        public void CanExecute_Null_Should_Return_False()
        {
            var target = new CopyMinMaxTagsComand();
            MinMaxViewModel viewModel = null;

            var result = target.Command.CanExecute(viewModel);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CanExecute_Data_Null_Should_Return_False()
        {
            var target = new CopyMinMaxTagsComand();
            var viewModel = new MinMaxViewModel()
            {
                Data = null
            };

            var result = target.Command.CanExecute(viewModel);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CanExecute_Data_Empty_Should_Return_False()
        {
            var target = new CopyMinMaxTagsComand();
            var viewModel = new MinMaxViewModel();

            Assert.AreEqual(0, viewModel.Data.Count);

            var result = target.Command.CanExecute(viewModel);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Execute_Should_WriteToClipboard()
        {
            var mock = CreateMockClipboardWriter();
            var target = new CopyMinMaxTagsComand(mock.Object);
            var viewModel = CreateMinMaxViewModel();

            target.Command.Execute(viewModel);

            mock.Verify(x => x.WriteText(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void Execute_Should_WriteExpectedDataToClipboard()
        {
            var mock = CreateMockClipboardWriter();
            var target = new CopyMinMaxTagsComand(mock.Object);
            var viewModel = CreateMinMaxViewModel();

            target.Command.Execute(viewModel);

            // only check header, so we can avoid timestamp comparisons
            string expectedDataHeader = "#Id,Tag,Interval,From,IntervalTo,MinimumTemperature,MinimumTimestamp,MaximumTemperature,MaximumTimestamp,Difference";

            mock.Verify(x => x.WriteText(It.Is<string>(csv => csv.StartsWith(expectedDataHeader))), Times.Once());
        }

        private MinMaxViewModel CreateMinMaxViewModel()
        {
            var viewModel = new MinMaxViewModel();

            viewModel.Data.Add(new MinMaxMeasurementViewModel()
            {
                TagId = 42,
                TagName = "My tag",
                Interval = TimeInterval.ThisYear,
                Minimum = new Measurement() { Temperature = 2.0, Timestamp = new DateTime(2022, 1, 1, 12, 0, 0) },
                Maximum = new Measurement() { Temperature = 25.0, Timestamp = new DateTime(2022, 7, 1, 15, 0, 0) },
                IntervalFrom = new DateTime(2022, 1, 1),
                IntervalTo = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59)
            });

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
