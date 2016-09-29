using System;
using System.ComponentModel;
using System.Threading.Tasks;
using AsyncWindowsClipboard.Modifiers.Readers;
using AsyncWindowsClipboard.Modifiers.Writers;
using AsyncWindowsClipboard.Text;

namespace AsyncWindowsClipboard
{
    /// <summary>
    ///     Connects to and controls windows clipboard asynchronously using <see cref="WindowsClipboard" />
    /// </summary>
    /// <remarks>Use <see cref="WindowsClipboard" /> to manipulate Windows clipboard in a lower level.</remarks>
    /// <seealso cref="IAsyncClipboardService" />
    /// <seealso cref="WindowsClipboard" />
    public class WindowsClipboardService : IAsyncClipboardService
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="WindowsClipboardService" /> class.
        /// </summary>
        public WindowsClipboardService()
        {
        }

        /// <summary>
        ///     Gets the static instance of <see cref="WindowsClipboardService" />.
        /// </summary>
        /// <value>The static instance of <see cref="WindowsClipboardService" />.</value>
        public static IAsyncClipboardService StaticInstance => StaticInstanceLazy.Value;

        private static Lazy<WindowsClipboardService> StaticInstanceLazy => new Lazy<WindowsClipboardService>();


        /// <summary>
        ///     Sets unicode (UTF16 little endian) bytes to the clipboard.
        /// </summary>
        /// <param name="textBytes">Unicode (UTF16 little endian) byte representation of the text.</param>
        /// <returns>If the operation was successful.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="textBytes" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException"><paramref name="textBytes" /> is empty.</exception>
        /// <exception cref="Win32Exception">Connection to the clipboard could not be opened.</exception>
        public Task<bool> SetUnicodeBytes(byte[] textBytes)
        {
            var writer = new UnicodeBytesWriter();
            var result = writer.WriteAsync(textBytes);
            return result;
        }

        /// <summary>
        ///     Gets the clipboard data as <c>UTF-16 little endian</c> bytes.
        /// </summary>
        /// <returns>The data in the clipboard as bytes</returns>
        /// <exception cref="Win32Exception">Connection to the clipboard could not be opened.</exception>
        public Task<byte[]> GetAsUnicodeBytes()
        {
            var reader = new UnicodeBytesReader();
            return reader.ReadAsync();
        }

        /// <summary>
        ///     Gets the clipboard data as a <see cref="string" />.
        /// </summary>
        /// <returns>
        ///     <p>The data in the clipboard as <see cref="string" /></p>
        ///     <p><see langword="null" /> if there is no string data available in the clipboard</p>
        /// </returns>
        /// <exception cref="Win32Exception">Connection to the clipboard could not be opened.</exception>
        public async Task<string> GetText()
        {
            var reader = new StringReader();
            var result = await reader.ReadAsync();
            return result;
        }


        /// <summary>
        ///     Sets a string as the clipboard data.
        /// </summary>
        /// <exception cref="Win32Exception">Connection to the clipboard could not be opened.</exception>
        public async Task<bool> SetText(string value)
        {
            var writer = new StringWriter();
            var result = await writer.WriteAsync(value);
            return result;
        }
    }
}