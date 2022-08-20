using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;

namespace WirelessTagClientLib.Test
{
    [TestClass]
    public class HttpStatusExceptionTest
    {
        [TestMethod]
        public void Ctor_HttpStatusCode_Sets_Status_Property()
        {
            var target = new HttpStatusException(HttpStatusCode.BadRequest);

            Assert.AreEqual(HttpStatusCode.BadRequest, target.Status);
        }

        [TestMethod]
        public void Ctor_HttpStatusCode_String_Sets_Status_Property()
        {
            var target = new HttpStatusException(HttpStatusCode.BadRequest, "Some message");

            Assert.AreEqual(HttpStatusCode.BadRequest, target.Status);
            Assert.AreEqual("Some message", target.Message);
        }

        [TestMethod]
        public void Ctor_HttpStatusCode_Exception_Sets_Status_Property()
        {
            var inner = new ArgumentException();
            var target = new HttpStatusException(HttpStatusCode.BadRequest, inner);

            Assert.AreEqual(HttpStatusCode.BadRequest, target.Status);
            Assert.AreSame(inner, target.InnerException);
        }
    }
}
