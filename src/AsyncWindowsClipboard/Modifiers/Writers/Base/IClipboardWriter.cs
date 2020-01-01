using System.Threading.Tasks;

namespace AsyncWindowsClipboard.Modifiers.Writers.Base
{
    /// <summary>
    ///     Abstraction for a clipboard data setter.
    /// </summary>
    /// <typeparam name="TData">Type of the data which will be set.</typeparam>
    public interface IClipboardWriter<in TData>
    {
        /// <summary>
        ///     Writes data to the clipboard.
        /// </summary>
        /// <param name="data">Data to write</param>
        /// <returns>Whether the data was written successfully</returns>
        Task<bool> WriteAsync(TData data);
    }
}