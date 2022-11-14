using System;

namespace WirelessTagClientApp.Utils
{
    /// <summary>
    /// Dedicated exception helper class.
    /// </summary>
    /// <remarks>
    /// From http://blog.mariusschulz.com/2014/05/16/implementing-an-exception-helper-class-for-parameter-null-checking
    /// Thanks Marius!
    /// </remarks>
    public static class ThrowIf
    {
        public static class Argument
        {
            /// <summary>
            /// Throws an <see cref="ArgumentNullException"/> if the supplied object is null.
            /// </summary>
            /// <param name="argument"></param>
            /// <param name="argumentName"></param>
            public static void IsNull(object argument, string argumentName)
            {
                if (argument == null)
                {
                    throw new ArgumentNullException(argumentName);
                }
            }

            /// <summary>
            /// Throws an <see cref="ArgumentException"/> if the supplied object is not equal.
            /// </summary>
            /// <param name="argument"></param>
            /// <param name="argumentName"></param>
            public static void IsNotEqual(bool argument, string argumentName)
            {
                if (argument)
                {
                    throw new ArgumentException(argumentName);
                }
            }
        }
    }
}