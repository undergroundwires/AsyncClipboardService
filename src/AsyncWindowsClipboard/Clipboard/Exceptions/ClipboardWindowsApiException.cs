using System;
using System.ComponentModel;

namespace AsyncWindowsClipboard.Clipboard.Exceptions
{
    /// <summary>
    ///     This type of exception is thrown if there has been any errors during communication with windows api.
    /// </summary>
    /// <seealso cref="Win32Exception" />
    public sealed class ClipboardWindowsApiException : Win32Exception
    {
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="error"/> is not unsigned</exception>
        public ClipboardWindowsApiException(uint error) : base((int)error)
        {
            if (error <= 0) throw new ArgumentOutOfRangeException(nameof(error));
        }

        /// <exception cref="ArgumentException"><paramref name="message"/> cannot be null or empty.</exception>
        public ClipboardWindowsApiException(string message) : base(message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Value cannot be null or empty.", nameof(message));
        }
    }
}