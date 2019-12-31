using AsyncWindowsClipboard.Clipboard.Result;

namespace AsyncWindowsClipboard.Clipboard
{
    /// <summary>
    ///     Reveals bunch of methods for synchronous communication with the windows native functions.
    /// </summary>
    internal interface IWindowsClipboardSession
    {
        /// <summary>
        ///     A <see cref="bool" /> representing whether the instance has an open communication with the windows clipboard.
        /// </summary>
        /// <remarks>
        ///     <p>Use <see cref="Open" /> method to open the communication</p>
        ///     <p>Use <see cref="Close" /> method to close the communication</p>
        /// </remarks>
        /// <seealso cref="Open" />
        /// <seealso cref="Close" />
        bool IsOpen { get; }

        /// <summary>
        ///     Opens the clipboard for examination.
        /// </summary>
        /// <returns>If the operation method is successful.</returns>
        IClipboardOperationResult Open();

        /// <summary>
        ///     Closes the the clipboard.
        /// </summary>
        /// <returns>If the operation method is successful.</returns>
        IClipboardOperationResult Close();

        /// <summary>
        ///     Clears the clipboard.
        /// </summary>
        /// <returns><c>true</c> if the function succeeds, <c>false</c> the function fails.</returns>
        IClipboardOperationResult Clear();

        /// <summary>
        ///     Determines whether the content of the clipboard is unicode text.
        /// </summary>
        /// <returns><c>true</c> if content of the clipboard is unicode text, otherwise; <c>false</c>.</returns>
        bool IsContentTypeOf(ClipboardDataType dataType);

        /// <summary>
        ///     Places data on the clipboard in a specified clipboard format.
        /// </summary>
        IClipboardOperationResult SetData(ClipboardDataType dataType, byte[] data);

        /// <summary>
        ///     Retrieves data from the clipboard in a specified format.
        /// </summary>
        byte[] GetData(ClipboardDataType dataType);
    }
}