using System.Threading.Tasks;

namespace AsyncWindowsClipboard.Modifiers.Readers.Base
{
    /// <summary>
    ///     Abstraction for different data readers from clipboard.
    /// </summary>
    /// <typeparam name="TResult">Result data type from reading operation.</typeparam>
    /// <seealso cref="IClipboardDataChecker" />
    internal interface IClipboardReader<TResult> : IClipboardDataChecker
        where TResult : class
    {
        /// <summary>
        ///     Reads and returns the <typeparam ref="TResult" />
        /// </summary>
        Task<TResult> ReadAsync();
    }
}