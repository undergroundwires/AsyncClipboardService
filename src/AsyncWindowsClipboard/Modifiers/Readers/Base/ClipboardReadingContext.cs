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

        public ClipboardReadingContext(IWindowsClipboardSession session)
        {
            if (session == null) throw new ArgumentNullException(nameof(session));
            _session = session;
        }

        public bool IsContentTypeOf(ClipboardDataType dataType) => _session.IsContentTypeOf(dataType);

        public byte[] GetData(ClipboardDataType dataType)
            => _session.GetData(dataType);
    }
}