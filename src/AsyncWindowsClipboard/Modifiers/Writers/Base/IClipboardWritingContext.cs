using AsyncWindowsClipboard.Clipboard;
using AsyncWindowsClipboard.Clipboard.Result;
using AsyncWindowsClipboard.Modifiers.Readers.Base;

namespace AsyncWindowsClipboard.Modifiers.Writers.Base
{
    /// <summary>
    ///     Context to be used by clipboard writers in an <see cref="IWindowsClipboardSession" />
    /// </summary>
    /// <seealso cref="IClipboardReadingContext" />
    /// <seealso cref="Writers" />
    internal interface IClipboardWritingContext
    {
        /// <summary>
        ///     Places data on the clipboard in a specified clipboard format.
        /// </summary>
        IClipboardOperationResult SetData(ClipboardDataType dataType, byte[] data);
    }
}