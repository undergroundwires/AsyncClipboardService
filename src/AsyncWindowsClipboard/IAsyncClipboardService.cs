using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncWindowsClipboard
{
    /// <summary>
    ///     Represents a service capable of transferring data to the system clipboard asynchronously.
    /// </summary>
    public interface IAsyncClipboardService
    {
        /// <summary>
        ///     <p>
        ///         If value is <see langword="null" /> then the instance will have no time out strategy. It'll  try to open a connection to
        ///         the windows clipboard api and returns failed status if the initial try fails.
        ///     </p>
        ///     <p>
        ///         If value is not <see langword="null" /> the instance will try to connect to the windows
        ///         clipboard until the value of <see cref="Timeout" /> is reached. This might be needed if clipboard is locked by
        ///         another application.
        ///     </p>
        /// </summary>
        TimeSpan? Timeout { get; set; }

        /// <summary>
        ///     Sets unicode bytes as text in the clipboard asynchronously.
        /// </summary>
        /// <param name="textBytes">Bytes to set.</param>
        /// <returns>If the operation was successful.</returns>
        Task<bool> SetUnicodeBytesAsync(byte[] textBytes);

        /// <summary>
        ///     Gets the clipboard data as a unicode <see cref="byte" /> array asynchronously.
        /// </summary>
        /// <returns>The data in the clipboard as bytes</returns>
        Task<byte[]> GetAsUnicodeBytesAsync();

        /// <summary>
        ///     Gets the clipboard data as a <see cref="string" /> asynchronously.
        /// </summary>
        /// <returns>
        ///     <p>The data in the clipboard as <see cref="string" /></p>
        ///     <p><see langword="null" /> if there is no string data available in the clipboard</p>
        /// </returns>
        Task<string> GetTextAsync();

        /// <summary>
        ///     Sets the text asynchronously.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>If the operation was successful.</returns>
        Task<bool> SetTextAsync(string value);

        /// <summary>
        ///     Sets list of file paths as a file drop list asynchronously.
        /// </summary>
        /// <param name="filePaths">List of absolute file paths.</param>
        /// <returns>If the operation was successful.</returns>
        Task<bool> SetFileDropListAsync(IEnumerable<string> filePaths);

        /// <summary>
        ///     Gets the file drop list in the clipboard as list of file paths asynchronously.
        /// </summary>
        /// <returns>
        ///     <p>The list of file paths.</p>
        ///     <p><see langword="null" /> if there is no file drop list  in the clipboard.</p>
        /// </returns>
        Task<IEnumerable<string>> GetFileDropListAsync();

        /// <summary>
        ///     Indicates whether there is data on the clipboard that is in the specified format or can be converted to that
        ///     format.
        /// </summary>
        /// <param name="format">The format of the data to look for.</param>
        /// <returns>
        ///     TRUE if there is data on the clipboard that is in the specified <see cref="format" /> or can be converted to
        ///     that format; otherwise, false.
        /// </returns>
        /// <seealso cref="ClipboardDataFormat" />
        Task<bool> ContainsAsync(ClipboardDataFormat format);
    }
}