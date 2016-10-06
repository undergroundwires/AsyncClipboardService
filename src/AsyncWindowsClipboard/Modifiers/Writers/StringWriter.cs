﻿using System;
using System.Threading.Tasks;
using AsyncClipboardService.Clipboard;
using AsyncWindowsClipboard.Clipboard.Modifiers.Readers;

namespace AsyncWindowsClipboard.Clipboard.Modifiers.Writers
{
    /// <summary>
    ///     Writes a <see cref="string" /> text to a <see cref="IClipboardWritingContext" />.
    /// </summary>
    /// <seealso cref="IClipboardWritingContext" />
    internal class StringWriter : ClipboardWriterBase<string>
    {
        public override IClipboardOperationResult Write(IClipboardWritingContext context, string data)
        {
            var bytes = TextService.GetBytes(data);
            var unicodeBytesWriter = Factory.Get<UnicodeBytesWriter>();
            var result = unicodeBytesWriter.Write(context, bytes);
            return result;
        }
    }
}