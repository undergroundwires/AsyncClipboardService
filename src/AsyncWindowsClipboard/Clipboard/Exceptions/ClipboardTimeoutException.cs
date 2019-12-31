using System;
using System.Collections.Generic;
using AsyncWindowsClipboard.Exceptions;

namespace AsyncWindowsClipboard.Clipboard.Exceptions
{
    /// <summary>
    ///     This type of exception is thrown if the operation fails until timeout is reached. It's depended on a single or
    ///     multiple <see cref="ClipboardWindowsApiException" />s.
    /// </summary>
    /// <seealso cref="ClipboardWindowsApiException"/>
    public sealed class ClipboardTimeoutException : Exception
    {
        /// <exception cref="ArgumentNullException"><paramref name="innerException"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="message"/> cannot be null or empty.</exception>
        public ClipboardTimeoutException(string message, ClipboardWindowsApiException innerException) : base
        (
            $"{message}{Environment.NewLine}Check the inner exception ({nameof(InnerException)} property) for more details."
        )
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Value cannot be null or empty.", nameof(message));
            InnerException = innerException ?? throw new ArgumentNullException(nameof(innerException));
        }

        /// <exception cref="ArgumentNullException"><paramref name="innerExceptions"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="message"/> cannot be null or empty.</exception>
        public ClipboardTimeoutException(string message, IEnumerable<ClipboardWindowsApiException> innerExceptions)
            : base(
                $"{message}{Environment.NewLine}Check the inner exceptions ({nameof(InnerExceptions)} property) for more details."
            )
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Value cannot be null or empty.", nameof(message));
            InnerExceptions = innerExceptions ?? throw new ArgumentNullException(nameof(innerExceptions));
        }

        /// <exception cref="ArgumentException"><paramref name="message"/> cannot be null or empty.</exception>
        public ClipboardTimeoutException(string message) : base(message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Value cannot be null or empty.", nameof(message));
        }

        public new ClipboardWindowsApiException InnerException { get; }
        public IEnumerable<ClipboardWindowsApiException> InnerExceptions { get; }
    }
}