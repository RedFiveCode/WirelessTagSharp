using System;
using System.Net;
using Xunit;

namespace WirelessTagClientLib.Test
{
    public class HttpStatusExceptionTest
    {
        [Fact]
        public void Ctor_HttpStatusCode_Sets_Status_Property()
        {
            var target = new HttpStatusException(HttpStatusCode.BadRequest);

            Assert.Equal(HttpStatusCode.BadRequest, target.Status);
        }

        [Fact]
        public void Ctor_HttpStatusCode_String_Sets_Status_Property()
        {
            var target = new HttpStatusException(HttpStatusCode.BadRequest, "Some message");

            Assert.Equal(HttpStatusCode.BadRequest, target.Status);
            Assert.Equal("Some message", target.Message);
        }

        [Fact]
        public void Ctor_HttpStatusCode_Exception_Sets_Status_Property()
        {
            var inner = new ArgumentException();
            var target = new HttpStatusException(HttpStatusCode.BadRequest, inner);

            Assert.Equal(HttpStatusCode.BadRequest, target.Status);
            Assert.Same(inner, target.InnerException);
        }
    }
}
