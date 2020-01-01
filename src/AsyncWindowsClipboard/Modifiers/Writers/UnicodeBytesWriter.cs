using System;
using System.Linq;
using AsyncWindowsClipboard.Clipboard;
using AsyncWindowsClipboard.Clipboard.Result;
using AsyncWindowsClipboard.Modifiers.Writers.Base;

namespace AsyncWindowsClipboard.Modifiers.Writers
{
    /// <summary>
    ///     Writes a byte array containing unicode characters to a <see cref="IClipboardWritingContext" />.
    /// </summary>
    /// <seealso cref="IClipboardWritingContext" />
    internal class UnicodeBytesWriter : ClipboardWriterBase<byte[]>
    {
        /// <exception cref="ArgumentNullException"><paramref name="context" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="data" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException"><paramref name="data" /> is empty.</exception>
        public override IClipboardOperationResult Write(IClipboardWritingContext context, byte[] data)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (!data.Any()) throw new ArgumentException($"{nameof(data)} cannot be empty.");
            var unicodeData = TransformToUnicodeClipboardBytes(data);
            try
            {
                var result = context.SetData(ClipboardDataType.UnicodeLittleEndianText, unicodeData);
                return result;
            }
            finally
            {
                Array.Clear(unicodeData, 0, unicodeData.Length);
            }
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