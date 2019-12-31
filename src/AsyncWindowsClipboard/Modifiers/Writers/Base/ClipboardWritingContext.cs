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

        public ClipboardWritingContext(IWindowsClipboardSession session)
        {
            if (session == null) throw new ArgumentNullException(nameof(session));
            _session = session;
        }

        public IClipboardOperationResult SetData(ClipboardDataType dataType, byte[] data) => _session.SetData(dataType, data);
    }
}