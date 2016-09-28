using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AsyncClipboardService.Clipboard;
using AsyncWindowsClipboard.Clipboard;
using AsyncWindowsClipboard.Helpers;
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
        private readonly ITextService _textService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WindowsClipboardService" /> class.
        /// </summary>
        public WindowsClipboardService() : this(UnicodeTextService.StaticInstance)
        {
        }

        internal WindowsClipboardService(ITextService textService)
        {
            if (textService == null) throw new ArgumentNullException(nameof(textService));
            _textService = textService;
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
            if (textBytes == null) throw new ArgumentNullException(nameof(textBytes));
            if (!textBytes.Any()) throw new ArgumentException($"{nameof(textBytes)} cannot be empty.");
            return TaskHelper.StartStaTask(() =>
            {
                using (var clipboard = new WindowsClipboard())
                {
                    OpenConnection(clipboard);
                    ClearClipboard(clipboard);
                    var unicodeData = TransformToUnicodeClipboardBytes(textBytes);
                    try
                    {
                        var result = clipboard.SetData(ClipboardDataType.UnicodeLittleEndianText, unicodeData);
                        return result.IsSuccessful;
                    }
                    finally
                    {
                        Array.Clear(unicodeData, 0, unicodeData.Length);
                    }
                }
            });
        }

        /// <summary>
        ///     Gets the clipboard data as <c>UTF-16 little endian</c> bytes.
        /// </summary>
        /// <exception cref="Win32Exception">Connection to the clipboard could not be opened.</exception>
        public Task<byte[]> GetAsUnicodeBytes()
        {
            var clipboardData = (byte[]) null;
            return TaskHelper.StartStaTask(() =>
            {
                using (var clipboard = new WindowsClipboard())
                {
                    var isUnicode = clipboard.IsContentTypeOf(ClipboardDataType.UnicodeLittleEndianText);
                    if (!isUnicode) return null;
                    OpenConnection(clipboard);
                    clipboardData = clipboard.GetData(ClipboardDataType.UnicodeLittleEndianText);
                }
                try
                {
                    if ((clipboardData == null) || (clipboardData.Length <= 2))
                        return clipboardData;
                    return GetBytes(clipboardData, true);
                }
                finally
                {
                    if (clipboardData != null) Array.Clear(clipboardData, 0, clipboardData.Length);
                }
            });
        }

        /// <summary>
        ///     Gets the clipboard data as string.
        /// </summary>
        /// <exception cref="Win32Exception">Connection to the clipboard could not be opened.</exception>
        public async Task<string> GetText()
        {
            var unicodeBytes = await GetAsUnicodeBytes();
            if ((unicodeBytes == null) || !unicodeBytes.Any()) return null;
            return _textService.GetString(unicodeBytes);
        }

        /// <summary>
        ///     Sets a string as the clipboard data.
        /// </summary>
        /// <exception cref="Win32Exception">Connection to the clipboard could not be opened.</exception>
        public async Task<bool> SetText(string value)
        {
            var bytes = _textService.GetBytes(value);
            return await SetUnicodeBytes(bytes);
        }

        /// <summary>
        ///     Clears the clipboard.
        /// </summary>
        private static void ClearClipboard(IWindowsClipboard clipboard)
        {
            var clearResult = clipboard.Clear();
            if (!clearResult.IsSuccessful) Debug.Assert(false);
        }

        /// <summary>
        ///     Opens the connection.
        /// </summary>
        /// <param name="clipboard">The clipboard.</param>
        /// <exception cref="Win32Exception">Connection to the clipboard could not be opened.</exception>
        private static void OpenConnection(IWindowsClipboard clipboard)
        {
            var openResult = clipboard.Open();
            if (!openResult.IsSuccessful)
            {
                if (openResult.LastError.HasValue)
                    throw new Win32Exception((int) openResult.LastError.Value);
                throw new Win32Exception("Clipboard could not be reached.");
            }
        }

        /// <summary>
        ///     Clears the extra zeros that's created by windows clipboard api.
        ///     Clipboard data returns text bytes with extra zeros. 2 zeros bytes in the end for unicode.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="clipboardData" /> is <see langword="null" />.</exception>
        private static byte[] GetBytes(byte[] clipboardData, bool isUnicode)
        {
            var nBytes = 0;
            for (var i = 0; i < clipboardData.Length; i += isUnicode ? 2 : 1)
            {
                if (isUnicode && (i == clipboardData.Length - 1))
                {
                    Debug.Assert(false);
                    return null;
                }
                var uValue = isUnicode
                    ? (ushort) ((clipboardData[i] << 8) |
                                clipboardData[i + 1])
                    : clipboardData[i];
                if (uValue == 0) break;

                nBytes += isUnicode ? 2 : 1;
            }
            var bytesCharsOnly = new byte[nBytes];
            Array.Copy(clipboardData, bytesCharsOnly, nBytes);
            return bytesCharsOnly;
        }

        /// <summary>
        ///     Clipboard text data must have extra zeros in the ends. 2 zeros bytes in the end for unicode.
        ///     This method adds the extra zero bytes.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="textBytes" /> is <see langword="null" />.</exception>
        private static byte[] TransformToUnicodeClipboardBytes(byte[] textBytes)
        {
            if (textBytes == null) throw new ArgumentNullException(nameof(textBytes));
            const bool areUnicodeBytes = true;
            var withZeroBytes = new byte[textBytes.Length + (areUnicodeBytes ? 2 : 1)];
            Array.Copy(textBytes, withZeroBytes, textBytes.Length);
            withZeroBytes[textBytes.Length] = 0;
            if (areUnicodeBytes) withZeroBytes[textBytes.Length + 1] = 0;
            return withZeroBytes;
        }
    }
}