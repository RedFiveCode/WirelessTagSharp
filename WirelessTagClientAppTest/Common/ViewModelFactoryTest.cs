using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using WirelessTagClientApp.Common;
using WirelessTagClientLib.DTO;

namespace WirelessTagClientApp.Test.Common
{
    [TestClass]
    public class ViewModelFactoryTest
    {
        [TestMethod]
        public void CreateTagViewModel_Should_Return_ValidObject()
        {
            var dto = CreateTagInfo();

            var result = ViewModelFactory.CreateTagViewModel(dto);

            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("My tag name", result.Name);
            Assert.AreEqual("Description text", result.Description);
            Assert.AreEqual(Guid.Parse("11111111-1111-1111-1111-111111111111"), result.Uuid);
            Assert.AreEqual(21.5, result.Temperature);
            Assert.AreEqual(50, result.RelativeHumidity);
            Assert.AreEqual(1.5, result.BatteryVoltage);
            Assert.AreEqual(75, result.BatteryRemaining);
            Assert.AreEqual(42, result.SignalStrength);
            Assert.AreEqual(new DateTime(2022, 1, 1), result.LastCommunication);
        }


        [TestMethod]
        public void CreateTagViewModelList_Empty_Should_Return_ValidObject()
        {
            List<TagInfo> tags = null;

            var result = ViewModelFactory.CreateTagViewModelList(tags);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void CreateTagViewModelList_Should_Return_ValidObject()
        {
            List<TagInfo> tags = new List<TagInfo>()
            {
                CreateTagInfo()
            };

            var result = ViewModelFactory.CreateTagViewModelList(tags);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual("My tag name", result[0].Name);
            Assert.AreEqual("Description text", result[0].Description);
            Assert.AreEqual(Guid.Parse("11111111-1111-1111-1111-111111111111"), result[0].Uuid);
            Assert.AreEqual(21.5, result[0].Temperature);
            Assert.AreEqual(50, result[0].RelativeHumidity);
            Assert.AreEqual(1.5, result[0].BatteryVoltage);
            Assert.AreEqual(75, result[0].BatteryRemaining);
            Assert.AreEqual(42, result[0].SignalStrength);
            Assert.AreEqual(new DateTime(2022, 1, 1), result[0].LastCommunication);
        }


        private TagInfo CreateTagInfo()
        {
            return new TagInfo()
            {
                SlaveId = 1,
                Name = "My tag name",
                Comment = "Description text",
                Uuid = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Temperature = 21.5,
                RelativeHumidity = 50,
                BatteryVoltage = 1.5,
                BatteryRemaining = 75,
                SignalStrength = 42,
                LastCommunication = new DateTime(2022, 1, 1)
            };
        }
    }
}
