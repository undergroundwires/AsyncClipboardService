using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using AsyncWindowsClipboard.Clipboard.Modifiers.Readers;
using AsyncWindowsClipboard.Clipboard.Modifiers.Writers;
using AsyncWindowsClipboard.Clipboard.Modifiers.Readers;
using AsyncWindowsClipboard.Clipboard.Modifiers.Writers;

namespace AsyncWindowsClipboard
{
    /// <summary>
    ///     Connects to and controls windows clipboard asynchronously.
    /// </summary>
    /// <seealso cref="IAsyncClipboardService" />
    public class WindowsClipboardService : IAsyncClipboardService
    {
        /// <summary>
        ///     Gets the static instance of <see cref="WindowsClipboardService" />.
        /// </summary>
        /// <value>The static instance of <see cref="WindowsClipboardService" />.</value>
        public static IAsyncClipboardService StaticInstance => StaticInstanceLazy.Value;

        private static Lazy<WindowsClipboardService> StaticInstanceLazy => new Lazy<WindowsClipboardService>();


        /// <summary>
        ///     Sets unicode (UTF16 little endian) bytes to the clipboard asynchronously.
        /// </summary>
        /// <param name="textBytes">Unicode (UTF16 little endian) byte representation of the text.</param>
        /// <returns>If the operation was successful.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="textBytes" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException"><paramref name="textBytes" /> is empty.</exception>
        /// <exception cref="Win32Exception">Connection to the clipboard could not be opened.</exception>
        public Task<bool> SetUnicodeBytesAsync(byte[] textBytes)
        {
            var writer = new UnicodeBytesWriter();
            var result = writer.WriteAsync(textBytes);
            return result;
        }

        /// <summary>
        ///     Gets the clipboard data as <c>UTF-16 little endian</c> bytes asynchronously.
        /// </summary>
        /// <returns>The data in the clipboard as bytes, <see langword="null" /> if no data exists in clipboard.</returns>
        /// <exception cref="Win32Exception">Connection to the clipboard could not be opened.</exception>
        public Task<byte[]> GetAsUnicodeBytesAsync()
        {
            var reader = new UnicodeBytesReader();
            return reader.ReadAsync();
        }

        /// <summary>
        ///     Retrieves the text in clipboard as a <see cref="string" /> asynchronously.
        /// </summary>
        /// <returns>
        ///     <p>The data in the clipboard as <see cref="string" /></p>
        ///     <p><see langword="null" /> if there is no string data available in the clipboard.</p>
        /// </returns>
        /// <exception cref="Win32Exception">Connection to the clipboard could not be opened.</exception>
        public async Task<string> GetTextAsync()
        {
            var reader = new StringReader();
            var result = await reader.ReadAsync();
            return result;
        }

        /// <summary>
        ///     Sets a <see cref="string"/> as the clipboard data asynchronously.
        /// </summary>
        /// <exception cref="Win32Exception">Connection to the clipboard could not be opened.</exception>
        public async Task<bool> SetTextAsync(string value)
        {
            var writer = new StringWriter();
            var result = await writer.WriteAsync(value);
            return result;
        }

        /// <summary>
        ///     Retrieves a collection of file names from the clipboard asynchronously.
        /// </summary>
        /// <returns>
        ///     A <see cref="IEnumerable{String}" /> containing file names or <see langword="null" /> if the clipboard does not
        ///     contain any data that is in the FileDrop format or can be converted to that format.
        /// </returns>
        /// <exception cref="Win32Exception">Connection to the clipboard could not be opened.</exception>
        public async Task<IEnumerable<string>> GetFileDropListAsync()
        {
            var reader = new FileDropListReader();
            var result = await reader.ReadAsync();
            return result;
        }

        /// <summary>
        ///     Sets list of file path <see cref="string"/>'s as file drop list.
        /// </summary>
        /// <param name="filePaths">List of absolute file paths.</param>
        /// <returns>If the operation was successful.</returns>
        /// <exception cref="Win32Exception">Connection to the clipboard could not be opened.</exception>
        public async Task<bool> SetFileDropListAsync(IEnumerable<string> filePaths)
        {
            var writer = new FileDropListWriter();
            var result = await writer.WriteAsync(filePaths);
            return result;
        }
    }
}