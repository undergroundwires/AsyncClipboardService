using System;
using System.Collections.Generic;

namespace AsyncWindowsClipboard.Exceptions
{
    /// <summary>
    ///     This type of exception is thrown if the operation fails until timeout is reached. It's depended on a single or
    ///     multiple <see cref="ClipboardWindowsApiException" />s.
    /// </summary>
    /// <seealso cref="ClipboardWindowsApiException"/>
    public sealed class ClipboardTimeoutException : Exception
    {
        public ClipboardTimeoutException(string message, ClipboardWindowsApiException innerException) : base
        (
            $"{message}{Environment.NewLine}Check the inner exception ({nameof(InnerException)} property) for more details."
        )
        {
            if (innerException == null) throw new ArgumentNullException(nameof(innerException));
            InnerException = innerException;
        }

        public ClipboardTimeoutException(string message, IEnumerable<ClipboardWindowsApiException> innerExceptions)
            : base(
                $"{message}{Environment.NewLine}Check the inner exceptions ({nameof(InnerExceptions)} property) for more details."
            )
        {
            if (innerExceptions == null) throw new ArgumentNullException(nameof(innerExceptions));
            InnerExceptions = innerExceptions;
        }

        public ClipboardTimeoutException(string message) : base(message)
        {
        }

        public new ClipboardWindowsApiException InnerException { get; }
        public IEnumerable<ClipboardWindowsApiException> InnerExceptions { get; }
    }
}