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
    }
}