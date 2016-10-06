using System;
using System.Diagnostics;
using System.Linq;
using AsyncClipboardService.Clipboard;
using AsyncWindowsClipboard.Clipboard.Connection;
using AsyncWindowsClipboard.Exceptions;
using AsyncWindowsClipboard.Text;

namespace AsyncWindowsClipboard.Clipboard.Modifiers
{
    /// <summary>
    ///     <p>Provides helper methods/services to manipulate a clipboard connection.</p>
    ///     <p>Provides strategies for connections with timeout.</p>
    /// </summary>
    internal abstract class ClipboardModifierBase
    {
        private const int DelayLength = 30; //delay in milliseconds
        private readonly IClipboardOpenerWithTimeout _clipboardOpenerWithTimeout;

        protected ClipboardModifierBase()
        {
            Factory = ClipboardModifierFactory.StaticInstance;
            TextService = UnicodeTextService.StaticInstance;
            _clipboardOpenerWithTimeout = new ClipboardOpenerWithTimeout();
        }

        /// <summary>
        ///     <p>Gets or sets the timeout.</p>
        ///     <p>
        ///         If value is <see langword="null" /> then the  <see cref="ClipboardModifierBase" />  instance will have no time
        ///         out strategy. It'll  try to open a connection to the windows clipboard api and returns failed status if the initial
        ///         try fails.
        ///     </p>
        ///     <p>
        ///         If value is not <see langword="null" /> the <see cref="ClipboardModifierBase" /> instance will try to connect
        ///         to the windows clipboard until the value of <see cref="Timeout" /> is reached. This might be needed if clipboard is
        ///         locked by another application.
        ///     </p>
        /// </summary>
        /// <seealso cref="ClipboardOpenerWithTimeout" />
        public TimeSpan? Timeout { get; set; }

        protected IClipboardModifierFactory Factory { get; }
        protected ITextService TextService { get; }

        /// <summary>
        ///     Clears the clipboard, and asserts if the operation fails.
        /// </summary>
        protected void ClearClipboard(IWindowsClipboardSession session)
        {
            var result = session.Clear();
            if (!result.IsSuccessful) Debug.Assert(false);
        }

        /// <summary>
        ///     Opens a connection in given <see cref="IWindowsClipboardSession" />.
        ///     If the connection fails then it throws exception.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <exception cref="ClipboardWindowsApiException">
        ///     Connection to the clipboard could not be established.
        /// </exception>
        /// <exception cref="ClipboardTimeoutException">
        /// </exception>
        protected void EnsureOpenConnection(IWindowsClipboardSession session)
        {
            if (Timeout.HasValue)
            {
                var result = _clipboardOpenerWithTimeout.Open(session, Timeout.Value, DelayLength);
                if (!result.IsSuccessful) throw GetTimeOutException(result);
            }
            else
            {
                var result = session.Open();
                if (!result.IsSuccessful) throw GetException(result);
            }
        }

        private ClipboardWindowsApiException GetException(IClipboardOperationResult result)
        {
            var message = string.IsNullOrEmpty(result.Message)
                ? "Connection to the clipboard could not be established."
                : result.Message;
            if (result.LastError.HasValue)
                return new ClipboardWindowsApiException(result.LastError.Value, message);
            return new ClipboardWindowsApiException(message);
        }

        private static ClipboardTimeoutException GetTimeOutException(IClipboardOperationResult result)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));
            if (result.IsSuccessful) throw new ArgumentException($"{nameof(result)} is successful.");
            var message = string.IsNullOrEmpty(result.Message)
                ? "Connection has been time out without being able to connect to the windows clipboard."
                : result.Message;
            if (result.LastError.HasValue)
            {
                var exception = new ClipboardWindowsApiException(result.LastError.Value);
                return new ClipboardTimeoutException
                (
                    message,
                    exception
                );
            }
            if ((result.LastErrors != null) && result.LastErrors.Any())
            {
                var innerExceptions = result.LastErrors.Select(e => new ClipboardWindowsApiException(e));
                throw new ClipboardTimeoutException(message, innerExceptions);
            }
            return new ClipboardTimeoutException(message);
        }
    }
}