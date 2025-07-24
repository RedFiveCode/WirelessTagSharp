using System;
using Xunit;
using WirelessTagClientApp.Utils;

namespace WirelessTagClientApp.Test.Utils
{
    /// <summary>
    /// Unit tests for <see cref="ThrowIf"/> class.
    /// </summary>
    
    public class ThrowIfTest
    {
        [Fact]
        public void Argument_IsNull_Throws_ArgumentNullException()
        {
            object value = null;

            Assert.Throws<ArgumentNullException>(() => ThrowIf.Argument.IsNull(value, nameof(value)));
        }

        [Fact]
        public void Argument_IsNull_DoesNot_Throw_Exception()
        {
            object value = this;

            ThrowIf.Argument.IsNull(value, nameof(value));
        }

        [Fact]
        public void Argument_IsNotEqual_Throws_ArgumentException()
        {
            bool value = true;

            Assert.Throws<ArgumentException>(() => ThrowIf.Argument.IsNotEqual(value, nameof(value)));
        }

        [Fact]
        public void Argument_IsNotEqual_DoesNot_Throw_Exceptionn()
        {
            bool value = false;

            ThrowIf.Argument.IsNotEqual(value, nameof(value));
        }
    }
}
