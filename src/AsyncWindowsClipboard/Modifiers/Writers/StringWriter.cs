using AsyncWindowsClipboard.Clipboard.Result;
using AsyncWindowsClipboard.Modifiers.Writers.Base;
using System;

namespace AsyncWindowsClipboard.Modifiers.Writers
{
    /// <summary>
    ///     Writes a <see cref="string" /> text to a <see cref="IClipboardWritingContext" />.
    /// </summary>
    /// <seealso cref="IClipboardWritingContext" />
    internal class StringWriter : ClipboardWriterBase<string>
    {
        /// <exception cref="T:System.ArgumentNullException"><paramref name="context"/> is <see langword="null"/></exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="data"/> is <see langword="null"/></exception>
        public override IClipboardOperationResult Write(IClipboardWritingContext context, string data)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (data == null) throw new ArgumentNullException(nameof(data));
            var bytes = TextService.GetBytes(data);
            var unicodeBytesWriter = Factory.Get<UnicodeBytesWriter>();
            var result = unicodeBytesWriter.Write(context, bytes);
            return result;
        }
    }
}