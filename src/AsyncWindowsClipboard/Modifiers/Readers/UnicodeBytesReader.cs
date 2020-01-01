using System;
using System.Collections.Generic;
using System.Diagnostics;
using AsyncWindowsClipboard.Clipboard;
using AsyncWindowsClipboard.Modifiers.Readers.Base;

namespace AsyncWindowsClipboard.Modifiers.Readers
{
    /// <summary>
    ///     Reads byte array containing unicode text from an <see cref="IClipboardReadingContext" />.
    /// </summary>
    /// <seealso cref="IClipboardReadingContext" />
    internal class UnicodeBytesReader : ClipboardReaderBase<byte[]>
    {
        public override bool Exists(IClipboardReadingContext context)
        {
            return context.IsContentTypeOf(ClipboardDataType.UnicodeLittleEndianText);
        }

        public override byte[] Read(IClipboardReadingContext context)
        {
            var clipboardData = context.GetData(ClipboardDataType.UnicodeLittleEndianText);
            try
            {
                if (clipboardData == null)
                    return null;
                if (clipboardData.Length <= 2)
                    return clipboardData;
                return GetBytes(clipboardData, true);
            }
            finally
            {
                if (clipboardData != null)
                    Array.Clear(clipboardData, 0, clipboardData.Length);
            }
        }
        /// <summary>
        ///     Clears the extra zeros that's created by windows clipboard api.
        ///     Clipboard data returns text bytes with extra zeros. 2 zeros bytes in the end for unicode.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="clipboardData" /> is <see langword="null" />.</exception>
        protected static byte[] GetBytes(byte[] clipboardData, bool isUnicode)
        {
            if (clipboardData == null) throw new ArgumentNullException(nameof(clipboardData));
            var charBytesTotal = CountTotalCharacterBytes(clipboardData, isUnicode);
            var bytesCharsOnly = new byte[charBytesTotal];
            Array.Copy(clipboardData, bytesCharsOnly, charBytesTotal);
            return bytesCharsOnly;
        }
        private static int CountTotalCharacterBytes(IReadOnlyList<byte> clipboardData, bool isUnicode)
        {
            var charBytes = 0;
            for (var i = 0; i < clipboardData.Count; i += isUnicode ? 2 : 1)
            {
                var uValue = isUnicode ? (ushort)((clipboardData[i] << 8) |
                                      clipboardData[i + 1]) : clipboardData[i];
                if (uValue == 0) break;
                charBytes += isUnicode ? 2 : 1;
            }
            return charBytes;
        }
    }
}