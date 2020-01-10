using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AsyncWindowsClipboard.Clipboard.Exceptions
{
    /// <summary>
    ///     This type of exception is thrown if the operation fails until timeout is reached. It's depended on a single or
    ///     multiple <see cref="ClipboardWindowsApiException" />s.
    /// </summary>
    /// <seealso cref="ClipboardWindowsApiException" />
    [Serializable]
    public sealed class ClipboardTimeoutException : AggregateException, ISerializable
    {
        /// <exception cref="ArgumentNullException"><paramref name="innerException" /> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="message" /> cannot be null or empty.</exception>
        public ClipboardTimeoutException(string message, ClipboardWindowsApiException innerException)
            : this(GetExceptionMessageWithInnerPropertyReference(message, nameof(InnerException)), new []{innerException})
        {
            if (innerException == null) throw new ArgumentNullException(nameof(innerException));
        }

        /// <exception cref="ArgumentNullException"><paramref name="innerExceptions" /> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="message" /> cannot be null or empty.</exception>
        public ClipboardTimeoutException(string message, IEnumerable<ClipboardWindowsApiException> innerExceptions)
            : base(GetExceptionMessageWithInnerPropertyReference(message, nameof(InnerExceptions)), innerExceptions)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Value cannot be null or empty.", nameof(message));
        }

        /// <exception cref="ArgumentException"><paramref name="message" /> cannot be null or empty.</exception>
        public ClipboardTimeoutException(string message) : base(message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Value cannot be null or empty.", nameof(message));
        }

        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="info"/> parameter is <c>null</c>.</exception>
        /// <exception cref="SerializationException">The class name is <c>null</c> or <see cref="Exception.HResult"/> is zero (0).</exception>
        private ClipboardTimeoutException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        private static string GetExceptionMessageWithInnerPropertyReference(string message, string propertyName)
            => $"{message}{Environment.NewLine}Check the inner exceptions ({propertyName} property) for more details.";
    }
}