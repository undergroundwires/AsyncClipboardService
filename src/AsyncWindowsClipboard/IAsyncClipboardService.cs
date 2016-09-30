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
        ///     Sets unicode bytes as text in the clipboard.
        /// </summary>
        /// <param name="textBytes">Bytes to set.</param>
        /// <returns>If the operation was successful.</returns>
        Task<bool> SetUnicodeBytes(byte[] textBytes);

        /// <summary>
        ///     Gets the clipboard data as a unicode <see cref="byte" /> array.
        /// </summary>
        /// <returns>The data in the clipboard as bytes</returns>
        Task<byte[]> GetAsUnicodeBytes();

        /// <summary>
        ///     Gets the clipboard data as a <see cref="string" />.
        /// </summary>
        /// <returns>
        ///     <p>The data in the clipboard as <see cref="string" /></p>
        ///     <p><see langword="null" /> if there is no string data available in the clipboard</p>
        /// </returns>
        Task<string> GetText();

        /// <summary>
        ///     Sets the text.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>If the operation was successful.</returns>
        Task<bool> SetText(string value);

        /// <summary>
        ///     Sets list of file paths as a file drop list.
        /// </summary>
        /// <param name="filePaths">List of absolute file paths.</param>
        /// <returns>If the operation was successful.</returns>
        Task<bool> SetFileDropList(IEnumerable<string> filePaths);

        /// <summary>
        ///     Gets the file drop list in the clipboard as list of file paths.
        /// </summary>
        /// <returns>
        ///     <p>The list of file paths.</p>
        ///     <p><see langword="null" /> if there is no file drop list  in the clipboard.</p>
        /// </returns>
        Task<IEnumerable<string>> GetFileDropList();
    }
}