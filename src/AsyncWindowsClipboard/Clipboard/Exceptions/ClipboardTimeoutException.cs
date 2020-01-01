using System;
using System.Collections.Generic;

namespace AsyncWindowsClipboard.Clipboard.Exceptions
{
    /// <summary>
    ///     This type of exception is thrown if the operation fails until timeout is reached. It's depended on a single or
    ///     multiple <see cref="ClipboardWindowsApiException" />s.
    /// </summary>
    /// <seealso cref="ClipboardWindowsApiException" />
    public sealed class ClipboardTimeoutException : Exception
    {
        /// <exception cref="ArgumentNullException"><paramref name="innerException" /> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="message" /> cannot be null or empty.</exception>
        public ClipboardTimeoutException(string message, ClipboardWindowsApiException innerException)
            : base(GetExceptionMessageWithInnerPropertyReference(message, nameof(InnerException)))
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Value cannot be null or empty.", nameof(message));
            InnerException = innerException ?? throw new ArgumentNullException(nameof(innerException));
        }

        /// <exception cref="ArgumentNullException"><paramref name="innerExceptions" /> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="message" /> cannot be null or empty.</exception>
        public ClipboardTimeoutException(string message, IEnumerable<ClipboardWindowsApiException> innerExceptions)
            : base(GetExceptionMessageWithInnerPropertyReference(message, nameof(InnerExceptions)))
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Value cannot be null or empty.", nameof(message));
            InnerExceptions = innerExceptions ?? throw new ArgumentNullException(nameof(innerExceptions));
        }

        /// <exception cref="ArgumentException"><paramref name="message" /> cannot be null or empty.</exception>
        public ClipboardTimeoutException(string message) : base(message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Value cannot be null or empty.", nameof(message));
        }

        /// <summary>
        ///     The inner exception that caused this exception.
        /// </summary>
        public new ClipboardWindowsApiException InnerException { get; }

        /// <summary>
        ///     List of innerexceptions that caused this exception.
        /// </summary>
        public IEnumerable<ClipboardWindowsApiException> InnerExceptions { get; }

        private static string GetExceptionMessageWithInnerPropertyReference(string message, string propertyName)
            => $"{message}{Environment.NewLine}Check the inner exceptions ({propertyName} property) for more details.";
    }
}