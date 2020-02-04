using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace AsyncWindowsClipboard.Clipboard.Exceptions
{
    /// <summary>
    ///     This type of exception is thrown if there has been any errors during communication with windows api.
    /// </summary>
    /// <seealso cref="Win32Exception" />
    [Serializable]
    public sealed class ClipboardWindowsApiException : Win32Exception
    {
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="error" /> is not unsigned</exception>
        public ClipboardWindowsApiException(uint error) : base((int) error)
        {
            if (error <= 0) throw new ArgumentOutOfRangeException(nameof(error));
        }

        /// <exception cref="ArgumentException"><paramref name="message" /> cannot be null or empty.</exception>
        public ClipboardWindowsApiException(string message) : base(message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Value cannot be null or empty.", nameof(message));
        }

        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="info"/> parameter is <c>null</c>.</exception>
        /// <exception cref="SerializationException">The class name is <c>null</c> or <see cref="Exception.HResult"/> is zero (0).</exception>
        private ClipboardWindowsApiException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}