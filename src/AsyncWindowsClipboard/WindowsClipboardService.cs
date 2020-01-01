using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncWindowsClipboard.Clipboard.Connection;
using AsyncWindowsClipboard.Clipboard.Exceptions;
using AsyncWindowsClipboard.Modifiers;
using AsyncWindowsClipboard.Modifiers.Readers;
using AsyncWindowsClipboard.Modifiers.Readers.Base;
using AsyncWindowsClipboard.Modifiers.Writers;

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

        /// <inheritdoc />
        /// <seealso cref="ClipboardOpenerWithTimeout" />
        public TimeSpan? Timeout { get; set; }

        /// <exception cref="ClipboardWindowsApiException">Connection to the clipboard could not be opened.</exception>
        public Task<byte[]> GetAsUnicodeBytesAsync()
        {
            var reader = _clipboardModifierFactory.Get<UnicodeBytesReader>(Timeout);
            return reader.ReadAsync();
        }

        /// <inheritdoc />
        /// <exception cref="ClipboardWindowsApiException">Connection to the clipboard could not be opened.</exception>
        public Task<string> GetTextAsync()
        {
            var reader = _clipboardModifierFactory.Get<StringReader>(Timeout);
            return reader.ReadAsync();
        }

        /// <inheritdoc />
        /// <exception cref="ClipboardWindowsApiException">Connection to the clipboard could not be opened.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/></exception>
        /// <exception cref="ClipboardTimeoutException">Connection to clipboard fails after timeout</exception>
        public Task<bool> SetTextAsync(string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            var writer = _clipboardModifierFactory.Get<StringWriter>(Timeout);
            return writer.WriteAsync(value);
        }

        /// <inheritdoc />
        /// <exception cref="ClipboardWindowsApiException">Connection to the clipboard could not be opened.</exception>
        public Task<IEnumerable<string>> GetFileDropListAsync()
        {
            var reader = _clipboardModifierFactory.Get<FileDropListReader>(Timeout);
            return reader.ReadAsync();
        }

        /// <inheritdoc />
        /// <exception cref="ClipboardWindowsApiException">Connection to the clipboard could not be opened.</exception>
        public Task<bool> SetFileDropListAsync(IEnumerable<string> filePaths)
        {
            var writer = _clipboardModifierFactory.Get<FileDropListWriter>(Timeout);
            return writer.WriteAsync(filePaths);
        }

        /// <inheritdoc />
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
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="format" /> is unknown.</exception>
        /// <seealso cref="ClipboardDataFormat" />
        public Task<bool> ContainsAsync(ClipboardDataFormat format)
        {
            var checker = GetDataChecker(format);
            return checker.ExistsAsync();
        }

        /// <inheritdoc />
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
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="format" /> is unknown.</exception>
        private IClipboardDataChecker GetDataChecker(ClipboardDataFormat format)
        {
            return format switch
            {
                ClipboardDataFormat.Text => _clipboardModifierFactory.Get<StringReader>(),
                ClipboardDataFormat.FileDropList => _clipboardModifierFactory.Get<StringReader>(),
                _ => throw new ArgumentOutOfRangeException(nameof(format), format, $"{nameof(format)} is unknown.")
            };
        }
    }
}