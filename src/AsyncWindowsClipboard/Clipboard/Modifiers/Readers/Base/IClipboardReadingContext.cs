using AsyncClipboardService.Clipboard;
using AsyncWindowsClipboard.Clipboard.Modifiers.Writers;

namespace AsyncWindowsClipboard.Clipboard.Modifiers.Readers
{
    /// <summary>
    /// Context to be used by clipboard readers in an <see cref="IWindowsClipboardSession"/>
    /// </summary>
    /// <seealso cref="IClipboardWritingContext"/>
    /// <seealso cref="Writers"/>
    internal interface IClipboardReadingContext
    {
        /// <summary>
        ///     Determines whether the content of the clipboard is unicode text.
        /// </summary>
        /// <returns><c>true</c> if content of the clipboard is unicode text, otherwise; <c>false</c>.</returns>
        bool IsContentTypeOf(ClipboardDataType dataType);

        /// <summary>
        ///     Retrieves data from the clipboard in a specified format.
        /// </summary>
        byte[] GetData(ClipboardDataType dataType);
    }
}