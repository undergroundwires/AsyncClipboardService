using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncWindowsClipboard.Clipboard.Connection;
using AsyncWindowsClipboard.Clipboard.Modifiers;
using AsyncWindowsClipboard.Clipboard.Modifiers.Readers;
using AsyncWindowsClipboard.Clipboard.Modifiers.Writers;
using AsyncWindowsClipboard.Exceptions;

namespace AsyncWindowsClipboard
{
    /// <summary>
    ///     <p>Connects to and controls windows clipboard asynchronously.</p>
    ///     <p>This class is thread-safe, and it's state is depended on <see cref="Timeout" /> property.</p>
    /// </summary>
    /// <remark>
    ///     <p>
    ///         It's recommended to use it with a <see cref="Timeout" /> setting, as it'll then wait for the thread that blocks
    ///         the windows api instead of failing.
    ///     </p>
    /// </remark>
    /// <seealso cref="IAsyncClipboardService" />
    public class WindowsClipboardService : IAsyncClipboardService
    {
        private readonly IClipboardModifierFactory _clipboardModifierFactory;

        /// <summary>
        ///     <p>Initializes a new instance of the <see cref="WindowsClipboardService" /> class with timeout strategy.</p>
        ///     <p>
        ///         This constructor will set <see cref="Timeout" /> property a not-<see langword="null" /> value which will
        ///         eventually activate the time out strategy. In this case if the initial try of opening a connection to clipboard
        ///         fails (might be due
        ///         to another thread / or application locking it), this <see cref="WindowsClipboardService" /> instance will
        ///         then try to connect to the the windows clipboard until <paramref name="timeout" /> is reached.
        ///     </p>
        /// </summary>
        /// <param name="timeout">The timeout to stop trying to access the clipboard.</param>
        /// <seealso cref="Timeout" />
        public WindowsClipboardService(TimeSpan timeout) : this(ClipboardModifierFactory.StaticInstance)
        {
            Timeout = timeout;
        }

        /// <summary>
        ///     <p>Initializes a new instance of the <see cref="WindowsClipboardService" /> class without the timeout strategy.</p>
        ///     <p>
        ///         The instance will try to connect to the windows clipboard and fail if the connection is locked by another
        ///         application.
        ///     </p>
        /// </summary>
        /// <remarks>
        ///     <p>The timeout strategy can be by setting <see cref="Timeout" /> property to a not <see langword="null" /> value.</p>
        /// </remarks>
        /// <seealso cref="WindowsClipboardService(TimeSpan)" />
        /// <seealso cref="Timeout" />
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
        ///     <p>Gets or sets the timeout.</p>
        ///     <p>
        ///         If value is <see langword="null" /> then the  <see cref="WindowsClipboardService" />  instance will have no
        ///         time out strategy. It'll  try to open a connection to the windows clipboard api and returns failed status if
        ///         the initial try fails.
        ///     </p>
        ///     <p>
        ///         If value is not <see langword="null" /> the <see cref="WindowsClipboardService" /> instance will try to connect
        ///         to the windows clipboard until the value of <see cref="Timeout" /> is reached. This might be needed if
        ///         clipboard is locked by another application.
        ///     </p>
        /// </summary>
        /// <seealso cref="ClipboardOpenerWithTimeout" />
        public TimeSpan? Timeout { get; set; }

        /// <summary>
        ///     Gets the clipboard data as <c>UTF-16 little endian</c> bytes asynchronously.
        /// </summary>
        /// <returns>The data in the clipboard as bytes, <see langword="null" /> if no data exists in clipboard.</returns>
        /// <exception cref="ClipboardWindowsApiException">Connection to the clipboard could not be opened.</exception>
        public Task<byte[]> GetAsUnicodeBytesAsync()
        {
            var reader = _clipboardModifierFactory.Get<UnicodeBytesReader>(Timeout);
            return reader.ReadAsync();
        }

        /// <summary>
        ///     Retrieves the text in clipboard as a <see cref="string" /> asynchronously.
        /// </summary>
        /// <returns>
        ///     <p>The data in the clipboard as <see cref="string" /></p>
        ///     <p><see langword="null" /> if there is no string data available in the clipboard.</p>
        /// </returns>
        /// <exception cref="ClipboardWindowsApiException">Connection to the clipboard could not be opened.</exception>
        public Task<string> GetTextAsync()
        {
            var reader = _clipboardModifierFactory.Get<StringReader>(Timeout);
            return reader.ReadAsync();
        }

        /// <summary>
        ///     Sets a <see cref="string" /> as the clipboard data asynchronously.
        /// </summary>
        /// <exception cref="ClipboardWindowsApiException">Connection to the clipboard could not be opened.</exception>
        public Task<bool> SetTextAsync(string value)
        {
            var writer = _clipboardModifierFactory.Get<StringWriter>(Timeout);
            return writer.WriteAsync(value);
        }

        /// <summary>
        ///     Retrieves a collection of file names from the clipboard asynchronously.
        /// </summary>
        /// <returns>
        ///     A <see cref="IEnumerable{String}" /> containing file names or <see langword="null" /> if the clipboard does not
        ///     contain any data that is in the FileDrop format or can be converted to that format.
        /// </returns>
        /// <exception cref="ClipboardWindowsApiException">Connection to the clipboard could not be opened.</exception>
        public Task<IEnumerable<string>> GetFileDropListAsync()
        {
            var reader = _clipboardModifierFactory.Get<FileDropListReader>(Timeout);
            return reader.ReadAsync();
        }

        /// <summary>
        ///     Sets list of file path <see cref="string" />'s as file drop list.
        /// </summary>
        /// <param name="filePaths">List of absolute file paths.</param>
        /// <returns>If the operation was successful.</returns>
        /// <exception cref="ClipboardWindowsApiException">Connection to the clipboard could not be opened.</exception>
        public Task<bool> SetFileDropListAsync(IEnumerable<string> filePaths)
        {
            var writer = _clipboardModifierFactory.Get<FileDropListWriter>(Timeout);
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
        ///     Sets unicode (UTF16 little endian) bytes to the clipboard asynchronously.
        /// </summary>
        /// <param name="textBytes">Unicode (UTF16 little endian) byte representation of the text.</param>
        /// <returns>If the operation was successful.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="textBytes" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException"><paramref name="textBytes" /> is empty.</exception>
        /// <exception cref="ClipboardWindowsApiException">Connection to the clipboard could not be opened.</exception>
        public Task<bool> SetUnicodeBytesAsync(byte[] textBytes)
        {
            var writer = _clipboardModifierFactory.Get<UnicodeBytesWriter>(Timeout);
            return writer.WriteAsync(textBytes);
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