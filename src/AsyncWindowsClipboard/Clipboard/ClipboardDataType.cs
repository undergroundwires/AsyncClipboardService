using AsyncWindowsClipboard.Native;

namespace AsyncWindowsClipboard.Clipboard
{
    public enum ClipboardDataType : uint
    {
        /// <summary>
        ///     The unicode little endian text
        /// </summary>
        UnicodeLittleEndianText = NativeMethods.CF_UNICODETEXT
    }
}