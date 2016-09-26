using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsyncWindowsClipboard.Clipboard;

namespace AsyncWindowsClipboard
{
    /// <summary>
    /// Reveals methods from windows native methods.
    /// </summary>
    internal interface IWindowsClipboard : IDisposable
    {
        /// <summary>
        ///   Opens the clipboard for examination. 
        /// </summary>
        /// <returns>If the operation method is successful.</returns>
        Task<bool> OpenAsync();
        /// <summary>
        ///   Closes the the clipboard.
        /// </summary>
        /// <returns>If the operation method is successful.</returns>
        Task<bool> CloseAsync();
        /// <summary>
        /// Clears the clipboard.
        /// </summary>
        /// <returns><c>true</c> if the function succeeds, <c>false</c> the function fails.</returns>
        Task<bool> ClearAsync();
        /// <summary>
        /// Determines whether the content of the clipboard is unicode text.
        /// </summary>
        /// <returns><c>true</c> if content of the clipboard is unicode text, otherwise; <c>false</c>.</returns>
        Task<bool> IsContentTypeOf(ClipboardDataType dataType);
        Task<bool> SetDataAsync(ClipboardDataType dataType, byte[] data);
        Task<byte[]> GetDataAsync(ClipboardDataType dataType);
    }
}
