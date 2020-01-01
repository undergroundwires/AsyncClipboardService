using System;
using AsyncWindowsClipboard.Clipboard;
using AsyncWindowsClipboard.Modifiers.Writers.Base;

namespace AsyncWindowsClipboard.Modifiers.Readers.Base
{
    /// <summary>
    ///     <p>A class to be used during read operations.</p>
    ///     <p>A wrapper for <see cref="IWindowsClipboardSession" /></p>
    /// </summary>
    /// <seealso cref="IWindowsClipboardSession" />
    /// <seealso cref="IClipboardWritingContext" />
    internal class ClipboardReadingContext : IClipboardReadingContext
    {
        private readonly IWindowsClipboardSession _session;

        /// <exception cref="T:System.ArgumentNullException"><paramref name="session" /> is <see langword="null" /></exception>
        public ClipboardReadingContext(IWindowsClipboardSession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        /// <inheritdoc />
        public bool IsContentTypeOf(ClipboardDataType dataType) => _session.IsContentTypeOf(dataType);

        /// <inheritdoc />
        public byte[] GetData(ClipboardDataType dataType)
            => _session.GetData(dataType);
    }
}