using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncClipboardService.Clipboard
{
    public interface IClipboardOperationResult
    {
        ClipboardOperationResultCode ResultCode { get; }
        uint? LastError { get;  }
        bool IsSuccessful { get; }
    }
}
