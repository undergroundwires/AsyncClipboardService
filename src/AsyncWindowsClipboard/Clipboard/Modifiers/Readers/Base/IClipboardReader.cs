using System.Threading.Tasks;

namespace AsyncWindowsClipboard.Clipboard.Modifiers.Readers
{
    /// <summary>
    ///     Abstraction for different data readers from clipboard.
    /// </summary>
    /// <typeparam name="TResult">Result data type from reading operation.</typeparam>
    /// <seealso cref="IClipboardDataChecker" />
    internal interface IClipboardReader<TResult> : IClipboardDataChecker
        where TResult : class
    {
        Task<TResult> ReadAsync();
    }
}