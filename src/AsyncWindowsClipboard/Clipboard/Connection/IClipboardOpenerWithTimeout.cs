using System;
using AsyncWindowsClipboard.Clipboard.Result;

namespace AsyncWindowsClipboard.Clipboard.Connection
{
    internal interface IClipboardOpenerWithTimeout
    {
        IClipboardOperationResult Open(
            IWindowsClipboardSession session,
            TimeSpan timeout,
            int delayMilliseconds = ClipboardOpenerWithTimeout.DefaultDelayMilliseconds);
    }
}