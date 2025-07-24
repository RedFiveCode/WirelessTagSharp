using Xunit;
using System;
using WirelessTagClientApp.Utils;

namespace WirelessTagClientApp.Test.Utils
{
    
    public class EnumHelperTest
    {
        internal enum ExampleEnumeration { Hearts = 0, Spades = 1, Clubs = 3, Diamonds = 7 }

        [Fact]
        public void NextEnum_ValueIsNotAnEnum_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => EnumHelper.NextEnum<int>(42));
        }

        [Fact]
        public void NextEnum_Value_ShouldReturnNextValue()
        {
            var result = EnumHelper.NextEnum<ExampleEnumeration>(ExampleEnumeration.Spades);

            Assert.Equal(ExampleEnumeration.Clubs, result);
        }

        [Fact]
        public void NextEnum_ValueAtEnd_ShouldReturnFirstValue()
        {
            var result = EnumHelper.NextEnum<ExampleEnumeration>(ExampleEnumeration.Diamonds);

            Assert.Equal(ExampleEnumeration.Hearts, result);
        }

        [Fact]
        public void PreviousEnum_Value_ShouldReturnPreviousValue()
        {
            var result = EnumHelper.PreviousEnum<ExampleEnumeration>(ExampleEnumeration.Spades);

            Assert.Equal(ExampleEnumeration.Hearts, result);
        }

        [Fact]
        public void PreviousEnum_ValueAtStart_ShouldReturnLastValue()
        {
            var result = EnumHelper.PreviousEnum<ExampleEnumeration>(ExampleEnumeration.Hearts);

            Assert.Equal(ExampleEnumeration.Diamonds, result);
        }
    }
}
