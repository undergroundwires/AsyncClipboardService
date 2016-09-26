using System.Threading.Tasks;

namespace AsyncWindowsClipboard
{
    /// <summary>
    ///     Represents a service capable of transferring data to the system clipboard.
    /// </summary>
    public interface IClipboardService
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
        /// <returns>The data in the clipboard as <see cref="string" /></returns>
        Task<string> GetAsString();

        /// <summary>
        ///     Sets the text.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>If the operation was successful.</returns>
        Task<bool> SetText(string value);
    }
}