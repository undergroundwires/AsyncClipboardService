using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncClipboardService.Clipboard
{
    internal interface IClipboardOperationResult
    {
        ClipboardOperationResultCode ResultCode { get; }
        uint? LastError { get;  }
        uint[] LastErrors { get; }
        bool IsSuccessful { get; }
        string Message { get; }
    }
}
