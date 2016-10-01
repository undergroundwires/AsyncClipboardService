using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using AsyncWindowsClipboard.Clipboard.Modifiers;
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
        private readonly IClipboardModifierFactory _clipboardModifierFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WindowsClipboardService" /> class.
        /// </summary>
        public WindowsClipboardService() : this(ClipboardModifierFactory.StaticInstance)
        {
        }

        /// <summary>
        ///     Internal constructor for dependency injections.
        /// </summary>
        internal WindowsClipboardService(IClipboardModifierFactory clipboardModifierFactory)
        {
            _clipboardModifierFactory = clipboardModifierFactory;
        }

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
            var writer = _clipboardModifierFactory.Get<UnicodeBytesWriter>();
            return writer.WriteAsync(textBytes);
        }

        /// <summary>
        ///     Gets the clipboard data as <c>UTF-16 little endian</c> bytes asynchronously.
        /// </summary>
        /// <returns>The data in the clipboard as bytes, <see langword="null" /> if no data exists in clipboard.</returns>
        /// <exception cref="Win32Exception">Connection to the clipboard could not be opened.</exception>
        public Task<byte[]> GetAsUnicodeBytesAsync()
        {
            var reader = _clipboardModifierFactory.Get<UnicodeBytesReader>();
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
        public Task<string> GetTextAsync()
        {
            var reader = _clipboardModifierFactory.Get<StringReader>();
            return reader.ReadAsync();
        }

        /// <summary>
        ///     Sets a <see cref="string" /> as the clipboard data asynchronously.
        /// </summary>
        /// <exception cref="Win32Exception">Connection to the clipboard could not be opened.</exception>
        public Task<bool> SetTextAsync(string value)
        {
            var writer = _clipboardModifierFactory.Get<StringWriter>();
            return writer.WriteAsync(value);
        }

        /// <summary>
        ///     Retrieves a collection of file names from the clipboard asynchronously.
        /// </summary>
        /// <returns>
        ///     A <see cref="IEnumerable{String}" /> containing file names or <see langword="null" /> if the clipboard does not
        ///     contain any data that is in the FileDrop format or can be converted to that format.
        /// </returns>
        /// <exception cref="Win32Exception">Connection to the clipboard could not be opened.</exception>
        public Task<IEnumerable<string>> GetFileDropListAsync()
        {
            var reader = _clipboardModifierFactory.Get<FileDropListReader>();
            return reader.ReadAsync();
        }

        /// <summary>
        ///     Sets list of file path <see cref="string" />'s as file drop list.
        /// </summary>
        /// <param name="filePaths">List of absolute file paths.</param>
        /// <returns>If the operation was successful.</returns>
        /// <exception cref="Win32Exception">Connection to the clipboard could not be opened.</exception>
        public Task<bool> SetFileDropListAsync(IEnumerable<string> filePaths)
        {
            var writer = _clipboardModifierFactory.Get<FileDropListWriter>();
            return writer.WriteAsync(filePaths);
        }

        /// <summary>
        ///     Indicates whether there is data on the clipboard that is in the specified format or can be converted to that
        ///     format.
        /// </summary>
        /// <remarks>
        ///     The alternative way of checking if the data format exists can be using one of Get methods and check if the result
        ///     is <see langword="null" />.
        ///     Because get methods (<see cref="GetTextAsync" />, <see cref="GetFileDropListAsync" />,
        ///     <see cref="GetAsUnicodeBytesAsync" />)
        ///     returns <see langword="null" />, if the clipboard does not contain any data that is in the wanted format or
        ///     can be converted to that format.
        ///     <example>
        ///         <code>
        ///            var result = await ContainsAsync(ClipboardDataFormat.Text); //returns if text format exists in the clipboard
        ///            var same = await GetTextAsync() == null; //gives same result
        ///         </code>
        ///     </example>
        /// </remarks>
        /// <param name="format">The format of the data to look for.</param>
        /// <returns>
        ///     TRUE if there is data on the clipboard that is in the specified <see cref="format" /> or can be converted to
        ///     that format; otherwise, false.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"><see cref="format" /> is unknown.</exception>
        /// <seealso cref="ClipboardDataFormat" />
        public Task<bool> ContainsAsync(ClipboardDataFormat format)
        {
            var checker = GetDataChecker(format);
            return checker.ExistsAsync();
        }

        /// <summary>
        ///     Gets right <seealso cref="IClipboardDataChecker" /> instance for the given format.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><see cref="format" /> is unknown.</exception>
        private IClipboardDataChecker GetDataChecker(ClipboardDataFormat format)
        {
            switch (format)
            {
                case ClipboardDataFormat.Text:
                    return _clipboardModifierFactory.Get<StringReader>();
                case ClipboardDataFormat.FileDropList:
                    return _clipboardModifierFactory.Get<StringReader>();
                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, $"{nameof(format)} is unknown.");
            }
        }
    }
}