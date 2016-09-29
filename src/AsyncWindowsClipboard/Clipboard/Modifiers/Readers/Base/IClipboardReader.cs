using System.Threading.Tasks;
using AsyncWindowsClipboard.Modifiers;

namespace AsyncWindowsClipboard.Modifiers.Readers
{
    /// <summary>
    ///     Abstraction for different data readers from clipboard.
    /// </summary>
    /// <typeparam name="TResult">Result data type from reading operation.</typeparam>
    internal interface IClipboardReader<TResult>
        where TResult : class
    {
        Task<TResult> ReadAsync();
        Task<bool> ExistsAsync();
    }

}