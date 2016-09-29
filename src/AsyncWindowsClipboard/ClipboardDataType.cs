using AsyncWindowsClipboard.Native;

namespace AsyncWindowsClipboard
{
    public enum ClipboardDataType : uint
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
        Wave = NativeMethods.CF_WAVE
    }
}