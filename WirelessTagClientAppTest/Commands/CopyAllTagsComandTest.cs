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
    /// Unit tests for teh <see cref="CopyAllTagsCommand"/> class.
    /// </summary>
    [TestClass]
    public class CopyAllTagsComandTest
    {
        [TestMethod]
        public void Command_Implements_ICommand()
        {
            var target = new CopyAllTagsCommand();

            Assert.IsInstanceOfType(target.Command, typeof(ICommand));
        }

        [TestMethod]
        public void CanExecute_Null_Should_Return_False()
        {
            var target = new CopyAllTagsCommand();
            AllTagsViewModel viewModel = null;

            var result = target.Command.CanExecute(viewModel);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CanExecute_Tags_Null_Should_Return_False()
        {
            var target = new CopyAllTagsCommand();
            var viewModel = new AllTagsViewModel()
            {
                Tags = null
            };

            var result = target.Command.CanExecute(viewModel);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CanExecute_Data_Empty_Should_Return_False()
        {
            var target = new CopyAllTagsCommand();
            var viewModel = new AllTagsViewModel();

            Assert.AreEqual(0, viewModel.Tags.Count);

            var result = target.Command.CanExecute(viewModel);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Execute_Should_WriteToClipboard()
        {
            var mock = CreateMockClipboardWriter();
            var target = new CopyAllTagsCommand(mock.Object);
            var viewModel = CreateAllTagsViewModel();

            target.Command.Execute(viewModel);

            mock.Verify(x => x.WriteText(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void Execute_Should_WriteExpectedDataToClipboard()
        {
            var mock = CreateMockClipboardWriter();
            var target = new CopyAllTagsCommand(mock.Object);
            var viewModel = CreateAllTagsViewModel();

            target.Command.Execute(viewModel);

            // only check header, so we can avoid timestamp comparisons
            string expectedData =
@"#Id, Name, Description, Uuid, Temperature (C), RelativeHumidity, SignalStrength (dBm), BatteryVoltage (V), BatteryRemaining (%), Last Communication, IsHumidityTag
42, My tag, Tag description, 00000000-0000-0000-0000-000000000000, 25.0, 50.0, -20, 1.2, 0.9, 01-Jan-2022 00:00:00, True";


            mock.Verify(x => x.WriteText(It.Is<string>(csv => csv == expectedData)), Times.Once());
        }

        private AllTagsViewModel CreateAllTagsViewModel()
        {
            var viewModel = new AllTagsViewModel();

            viewModel.Tags.Add(new TagViewModel()
            {
                Id = 42,
                Name = "My tag",
                Description = "Tag description",
                Uuid = Guid.Empty,
                Temperature = 25.0,
                RelativeHumidity = 50.0,
                SignalStrength = -20,
                BatteryVoltage = 1.2,
                BatteryRemaining = 0.9,
                LastCommunication = new DateTime(2022, 1, 1),
                IsHumidityTag = true

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
