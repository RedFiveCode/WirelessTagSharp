using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WirelessTagClientApp.Utils;

namespace WirelessTagClientApp.Test.Utils
{
    /// <summary>
    /// Unit tests for <see cref="ThrowIf"/> class.
    /// </summary>
    [TestClass]
    public class ThrowIfTest
    {
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void Argument_IsNull_Throws_ArgumentNullException()
        {
            object value = null;

            ThrowIf.Argument.IsNull(value, nameof(value));
        }

        [TestMethod]
        public void Argument_IsNull_DoesNot_Throw_Exception()
        {
            object value = this;

            ThrowIf.Argument.IsNull(value, nameof(value));
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void Argument_IsNotEqual_Throws_ArgumentException()
        {
            bool value = true;

            ThrowIf.Argument.IsNotEqual(value, nameof(value));
        }

        [TestMethod]
        public void Argument_IsNotEqual_DoesNot_Throw_Exceptionn()
        {
            bool value = false;

            ThrowIf.Argument.IsNotEqual(value, nameof(value));
        }
    }
}
