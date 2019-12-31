using NativeMethods = AsyncWindowsClipboard.Clipboard.Native.NativeMethods;

namespace AsyncWindowsClipboard.Clipboard
{
    /// <summary>
    ///     Internal enum that wraps native clipboard data types
    /// </summary>
    /// <remarks>
    ///     The clipboard formats defined by the system are called standard clipboard formats.
    ///     See more at : https://msdn.microsoft.com/en-us/library/windows/desktop/ff729168%28v=vs.85%29.aspx?f=255&MSPPError=-2147217396
    /// </remarks>
    /// <seealso cref="Clipboard.Native.NativeMethods"/>
    internal enum ClipboardDataType : uint
    {
        /// <summary>
        ///     The unicode little endian text.
        /// </summary>
        UnicodeLittleEndianText = NativeMethods.CF_UNICODETEXT,

        /// <summary>
        ///     Bitmap image file.
        /// </summary>
        Bitmap = NativeMethods.CF_BITMAP,

        /// <summary>
        ///     Software Arts' Data Interchange Format.
        /// </summary>
        Dif = NativeMethods.CF_DIF,

        /// <summary>
        ///     Represents audio data in one of the standard wave formats, such as 11 kHz or 22 kHz PCM.
        /// </summary>
        Wave = NativeMethods.CF_WAVE,

        /// <summary>
        ///     Collection of file names.
        /// </summary>
        FileDropList = NativeMethods.CF_HDROP
    }
}