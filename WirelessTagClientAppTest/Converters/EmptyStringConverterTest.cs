using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Data;
using WirelessTagClientApp.Converters;

namespace WirelessTagClientAppTest.Converters
{
    /// <summary>
    /// Unit tests for the <see cref="EmptyStringConverter"/> class.
    /// </summary>
    [TestClass]
    public class EmptyStringConverterTest
    {
        [TestMethod]
        public void Class_Should_Implement_IValueConverter_Interface()
        {
            var target = new EmptyStringConverter();

            Assert.IsInstanceOfType(target, typeof(IValueConverter));
        }

        [TestMethod]
        public void Convert_Null_Should_Return_none()
        {
            // arrange
            var target = new EmptyStringConverter();

            // act
            var result = target.Convert(null, null, null, null);

            // assert
            Assert.IsInstanceOfType(result, typeof(string));

            Assert.AreEqual("(none)", result);
        }

        [TestMethod]
        public void Convert_Empty_String_Should_Return_none()
        {
            // arrange
            var target = new EmptyStringConverter();

            // act
            var result = target.Convert("", null, null, null);

            // assert
            Assert.IsInstanceOfType(result, typeof(string));

            Assert.AreEqual("(none)", result);
        }

        [TestMethod]
        public void Convert_Non_Empty_String_Should_Return_Original_String()
        {
            // arrange
            var target = new EmptyStringConverter();

            // act
            var result = target.Convert("hello", null, null, null);

            // assert
            Assert.IsInstanceOfType(result, typeof(string));

            Assert.AreEqual("hello", result);
        }

        [TestMethod]
        public void Convert_Null_Should_Return_UnavailableText()
        {
            // arrange
            var target = new EmptyStringConverter()
            {
                UnavailableText = "Value not available"
            };

            // act
            var result = target.Convert(null, null, null, null);

            // assert
            Assert.IsInstanceOfType(result, typeof(string));

            Assert.AreEqual("Value not available", result);
        }
    }
}
