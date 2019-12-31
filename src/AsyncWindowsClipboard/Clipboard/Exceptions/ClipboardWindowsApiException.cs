using System.ComponentModel;

namespace AsyncWindowsClipboard.Exceptions
{
    /// <summary>
    ///     This type of exception is thrown if there has been any errors during communication with windows api.
    /// </summary>
    /// <seealso cref="Win32Exception" />
    public sealed class ClipboardWindowsApiException : Win32Exception
    {
        public ClipboardWindowsApiException(uint error) : base((int) error)
        {
        }

        public ClipboardWindowsApiException(string message) : base(message)
        {
        }
    }
}