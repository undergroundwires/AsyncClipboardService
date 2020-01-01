using System.Threading.Tasks;
using AsyncWindowsClipboard.Clipboard;
using AsyncWindowsClipboard.Clipboard.Exceptions;
using AsyncWindowsClipboard.Modifiers.Helpers;

namespace AsyncWindowsClipboard.Modifiers.Readers.Base
{
    /// <summary>
    ///     <p>Base class that clipboard readers must implement.</p>
    ///     <p>
    ///         Starts and ends asynchronous <see cref="IWindowsClipboardSession" />'s and send its context to its member
    ///         classes.
    ///     </p>
    ///     <p>Provides helper classes for its members.</p>
    /// </summary>
    /// <typeparam name="TResult">Result data type from reading operation.</typeparam>
    /// <seealso cref="IClipboardReadingContext" />
    /// <seealso cref="IClipboardReader{TResult}" />
    /// <seealso cref="ClipboardModifierBase" />
    internal abstract class ClipboardReaderBase<TResult> : ClipboardModifierBase, IClipboardReader<TResult>
        where TResult : class
    {
        /// <remarks>
        ///     <p>Starts a <see cref="WindowsClipboardSession" /> in an async contexts.</p>
        ///     <p>Sends the session to abstract <see cref="Exists" /> and <see cref="Read" /> methods.</p>
        /// </remarks>
        /// <inheritdoc />
        /// <returns>Null if <see cref="Exists" /> method returns <c>FALSE</c>; otherwise result from <see cref="Read" /></returns>
        /// <seealso cref="Exists" />
        /// <seealso cref="Read" />
        /// <exception cref="ClipboardWindowsApiException">Connection to the clipboard could not be opened.</exception>
        /// <exception cref="ClipboardTimeoutException">Connection to clipboard fails after timeout</exception>
        public Task<TResult> ReadAsync()
        {
            return TaskHelper.StartStaTask(() =>
            {
                using var session = new WindowsClipboardSession();
                var context = new ClipboardReadingContext(session);
                if (!Exists(context)) return null;
                EnsureOpenConnection(session);
                var result = Read(context);
                return result;
            });
        }

        /// <summary>
        ///     Returns if the data type exists in the clipboard.
        /// </summary>
        /// <returns><c>TRUE</c> if exists, <c>False</c> if it does not.</returns>
        /// <exception cref="ClipboardWindowsApiException">Connection to the clipboard could not be opened.</exception>
        public async Task<bool> ExistsAsync() => 
            await ReadAsync() == null;

        /// <summary>
        ///     Returns if the reading object type exists in the given <see cref="context" />.
        /// </summary>
        /// <param name="context">Clipboard session context.</param>
        /// <returns><c>true</c> if the object type is in clipboard data, <c>false</c> otherwise.</returns>
        public abstract bool Exists(IClipboardReadingContext context);

        /// <summary>
        ///     Reads the data from specified <see cref="IWindowsClipboardSession" />.
        /// </summary>
        /// <param name="context">Clipboard session context.</param>
        /// <returns>Result of the reading operation.</returns>
        public abstract TResult Read(IClipboardReadingContext context);
    }
}