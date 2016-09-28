using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsyncWindowsClipboard.Native;

namespace AsyncClipboardService.Clipboard
{
    public class ClipboardOperationResult : IClipboardOperationResult
    {
        public static IClipboardOperationResult SuccesResult = new ClipboardOperationResult(ClipboardOperationResultCode.Success);
        public ClipboardOperationResult(ClipboardOperationResultCode resultCode)
        {
            ResultCode = resultCode;
            LastError = NativeMethods.GetLastError();
        }
        public ClipboardOperationResultCode ResultCode { get; }
        public uint? LastError { get; }
        public bool IsSuccessful => ResultCode == ClipboardOperationResultCode.Success;
    }
}
