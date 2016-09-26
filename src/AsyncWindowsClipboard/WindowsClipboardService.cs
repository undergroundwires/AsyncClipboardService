using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AsyncWindowsClipboard.Clipboard;
using AsyncWindowsClipboard.Text;

namespace AsyncWindowsClipboard
{
    /// <summary>
    ///     Connects to and controls windows clipboard asynchronously using <see cref="WindowsClipboard" />
    /// </summary>
    /// <remarks>Use <see cref="WindowsClipboard" /> to manipulate Windows clipboard in a lower level.</remarks>
    /// <seealso cref="IClipboardService" />
    /// <seealso cref="WindowsClipboard" />
    public class WindowsClipboardService : IClipboardService
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
        /// Gets the static instance of <see cref="WindowsClipboardService"/>.
        /// </summary>
        /// <value>The static instance of <see cref="WindowsClipboardService"/>.</value>
        public static IClipboardService StaticInstance => StaticInstanceLazy.Value;
        private static Lazy<WindowsClipboardService> StaticInstanceLazy => new Lazy<WindowsClipboardService>();

        /// <summary>
        ///     Sets unicode (UTF16 little endian) bytes to the clipboard.
        /// </summary>
        /// <param name="textBytes">Unicode (UTF16 little endian) byte representation of the text.</param>
        /// <returns>If the operation was successful.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="textBytes" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException"><paramref name="textBytes" /> is empty.</exception>
        /// <exception cref="OperationCanceledException">Clipboard could not be reached.</exception>
        public async Task<bool> SetUnicodeBytes(byte[] textBytes)
        {
            if (textBytes == null) throw new ArgumentNullException(nameof(textBytes));
            if (!textBytes.Any()) throw new ArgumentException($"{nameof(textBytes)} cannot be empty.");
            using (var clipboard = new WindowsClipboard())
            {
                if (!await clipboard.OpenAsync())
                    throw new OperationCanceledException("Clipboard could not be reached.");
                if (!await clipboard.ClearAsync()) Debug.Assert(false);
                var unicodeData = TransformToUnicodeClipboardBytes(textBytes);
                try
                {
                    var result = await clipboard.SetDataAsync(ClipboardDataType.UnicodeLittleEndianText, unicodeData);
                    return result;
                }
                finally
                {
                    Array.Clear(unicodeData, 0, unicodeData.Length);
                }
            }
        }

        /// <summary>
        ///     Gets the clipboard data as <c>UTF-16 little endian</c> bytes.
        /// </summary>
        /// <exception cref="OperationCanceledException">Clipboard could not be reached.</exception>
        public async Task<byte[]> GetAsUnicodeBytes()
        {
            var clipboardData = (byte[]) null;
            using (var clipboard = new WindowsClipboard())
            {
                var isUnicode = await clipboard.IsContentTypeOf(ClipboardDataType.UnicodeLittleEndianText);
                if (!isUnicode) return null;
                if (!await clipboard.OpenAsync())
                    throw new OperationCanceledException("Clipboard could not be reached.");
                clipboardData = await clipboard.GetDataAsync(ClipboardDataType.UnicodeLittleEndianText);
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
        }

        /// <summary>
        ///     Gets the clipboard data as string.
        /// </summary>
        /// <exception cref="OperationCanceledException">Clipboard could not be reached.</exception>
        public async Task<string> GetAsString()
        {
            var unicodeBytes = await GetAsUnicodeBytes();
            if ((unicodeBytes == null) || !unicodeBytes.Any()) return null;
            return _textService.GetString(unicodeBytes);
        }

        /// <summary>
        ///     Sets a string as the clipboard data.
        /// </summary>
        /// <exception cref="OperationCanceledException">Clipboard could not be reached.</exception>
        public async Task<bool> SetText(string value)
        {
            var bytes = _textService.GetBytes(value);
            return await SetUnicodeBytes(bytes);
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