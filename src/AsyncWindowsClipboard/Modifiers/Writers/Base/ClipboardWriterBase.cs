using System;
using System.Threading.Tasks;
using AsyncClipboardService.Clipboard;
using AsyncWindowsClipboard.Exceptions;
using AsyncWindowsClipboard.Helpers;

namespace AsyncWindowsClipboard.Clipboard.Modifiers.Writers
{
    /// <summary>
    ///     <p>Base class that clipboard writers must implement.</p>
    ///     <p>
    ///         Starts and ends asynchronous <see cref="IWindowsClipboardSession" />'s and send its context to its member
    ///         classes.
    ///     </p>
    ///     <p>Provides helper classes and navigation properties for its members.</p>
    /// </summary>
    /// <typeparam name="TData">Data type that'll be set during this operation.</typeparam>
    /// <seealso cref="IClipboardWritingContext" />
    /// <seealso cref="IClipboardWriter{TResult}" />
    /// <seealso cref="ClipboardModifierBase" />
    internal abstract class ClipboardWriterBase<TData> : ClipboardModifierBase, IClipboardWriter<TData>
    {

        /// <exception cref="ClipboardWindowsApiException">
        ///     <p>Communication with windows API's has failed.</p>
        ///     <p>Connection to the clipboard could not be opened</p>
        /// </exception>
        /// <exception cref="ClipboardTimeoutException">Connection to clipboard fails after timeout</exception>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is <see langword="null"/></exception>
        public Task<bool> WriteAsync(TData data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            return TaskHelper.StartStaTask(() =>
            {
                using (var clipboard = new WindowsClipboardSession())
                {
                    var context = new ClipboardWritingContext(clipboard);
                    EnsureOpenConnection(clipboard);
                    ClearClipboard(clipboard);
                    var result = Write(context, data);
                    ThrowIfNotSuccessful(result);
                    return result.IsSuccessful;
                }
            });
        }

        /// <exception cref="ClipboardWindowsApiException">Communication with windows API's has failed.</exception>
        private static void ThrowIfNotSuccessful(IClipboardOperationResult result)
        {
            if (result.IsSuccessful)
                return;
            if (result.LastError.HasValue)
                throw new ClipboardWindowsApiException(result.LastError.Value);
            var message = result.ResultCode.ToString();
            throw new ClipboardWindowsApiException(message);
        }

        /// <summary>
        ///     Reads the data from specified <see cref="IWindowsClipboardSession" />.
        /// </summary>
        /// <param name="context">Clipboard session context.</param>
        /// <returns>Result of the reading operation.</returns>
        public abstract IClipboardOperationResult Write(IClipboardWritingContext context, TData data);
    }
}