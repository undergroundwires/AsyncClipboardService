using System;
using AsyncWindowsClipboard.Clipboard;
using AsyncWindowsClipboard.Clipboard.Result;

namespace AsyncWindowsClipboard.Modifiers.Writers.Base
{
    /// <summary>
    ///     <p>A class to be used during write operations.</p>
    ///     <p>A wrapper for <see cref="IWindowsClipboardSession" /></p>
    /// </summary>
    /// <seealso cref="IWindowsClipboardSession" />
    /// <seealso cref="IClipboardWritingContext" />
    internal class ClipboardWritingContext : IClipboardWritingContext
    {
        private readonly IWindowsClipboardSession _session;

        /// <exception cref="T:System.ArgumentNullException"><paramref name="session"/> is <see langword="null"/></exception>
        public ClipboardWritingContext(IWindowsClipboardSession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        /// <inheritdoc />
        public IClipboardOperationResult SetData(ClipboardDataType dataType, byte[] data) =>
            _session.SetData(dataType, data);
    }
}