using System.Threading.Tasks;

namespace AsyncWindowsClipboard.Modifiers.Readers.Base
{
    /// <summary>
    ///     Abstraction for a class that can check if its data type exists in the clipboard.
    /// </summary>
    /// <seealso cref="IClipboardReader{TResult}" />
    internal interface IClipboardDataChecker
    {
        Task<bool> ExistsAsync();
    }
}