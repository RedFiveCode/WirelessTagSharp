using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WirelessTagClientApp.Utils;

namespace WirelessTagClientApp.Test.Utils
{
    [TestClass]
    public class EnumHelperTest
    {
        internal enum ExampleEnumeration { Hearts = 0, Spades = 1, Clubs = 3, Diamonds = 7 }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NextEnum_ValueIsNotAnEnum_ShouldThrowException()
        {
            EnumHelper.NextEnum<int>(42);
        }

        [TestMethod]
        public void NextEnum_Value_ShouldReturnNextValue()
        {
            var result = EnumHelper.NextEnum<ExampleEnumeration>(ExampleEnumeration.Spades);

            Assert.AreEqual(ExampleEnumeration.Clubs, result);
        }

        [TestMethod]
        public void NextEnum_ValueAtEnd_ShouldReturnFirstValue()
        {
            var result = EnumHelper.NextEnum<ExampleEnumeration>(ExampleEnumeration.Diamonds);

            Assert.AreEqual(ExampleEnumeration.Hearts, result);
        }

        [TestMethod]
        public void PreviousEnum_Value_ShouldReturnPreviousValue()
        {
            var result = EnumHelper.PreviousEnum<ExampleEnumeration>(ExampleEnumeration.Spades);

            Assert.AreEqual(ExampleEnumeration.Hearts, result);
        }

        [TestMethod]
        public void PreviousEnum_ValueAtStart_ShouldReturnLastValue()
        {
            var result = EnumHelper.PreviousEnum<ExampleEnumeration>(ExampleEnumeration.Hearts);

            Assert.AreEqual(ExampleEnumeration.Diamonds, result);
        }
    }
}
