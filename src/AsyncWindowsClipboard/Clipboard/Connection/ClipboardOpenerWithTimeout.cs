using System;
using System.Collections.Generic;
using System.Linq;
using AsyncClipboardService.Clipboard;
using AsyncWindowsClipboard.Helpers;

namespace AsyncWindowsClipboard.Clipboard.Connection
{
    internal sealed class ClipboardOpenerWithTimeout : IClipboardOpenerWithTimeout
    {
        public const int DefaultDelayMilliseconds = 30;

        public static ClipboardOperationResult ConnectionAlreadyExistsResult =
            new ClipboardOperationResult(ClipboardOperationResultCode.Success,
                "A clipboard connection for the current thread is already active.");

        public IClipboardOperationResult Open(IWindowsClipboardSession session, TimeSpan timeout,
            int delayMilliseconds = DefaultDelayMilliseconds)
        {
            //validate parameter
            if (session == null) throw new ArgumentNullException(nameof(session));
            if (session.IsOpen)
                return new ClipboardOperationResult(ClipboardOperationResultCode.Success,
                    "A clipboard connection for the current thread is already active.");
            if (timeout.TotalMilliseconds < delayMilliseconds) return session.Open();
            var errorCodes = new List<uint>();
            var counter = 0;
            var getResult = new Func<bool>(() =>
            {
                counter++;
                var funcResult = session.Open();
                if (funcResult.LastError.HasValue)
                    errorCodes.Add(funcResult.LastError.Value);
                return funcResult.IsSuccessful;
            });
            var result = getResult.RetryUntilSuccessOrTimeout(timeout, delayMilliseconds);
            //return result;
            if (result) return ClipboardOperationResult.SuccessResult;
            var errors = errorCodes.Distinct().ToArray();
            if (errors.Length == 1)
                return GetResultForSingleError(errors.First(), counter);
            if (errors.Any())
                return GetResultForMultipleErrors(errors, counter);
           return new ClipboardOperationResult(ClipboardOperationResultCode.ErrorOpenClipboard, $"Clipboard could not be opened after {counter} tries");
        }

        private static ClipboardOperationResult GetResultForSingleError(uint error, int counter)
        {
            return new ClipboardOperationResult(ClipboardOperationResultCode.ErrorOpenClipboard,
                $"Clipboard has been tried to be reached {counter} times and all of them returned {error} error code.",
                error);
        }

        private static ClipboardOperationResult GetResultForMultipleErrors(IEnumerable<uint> errorList, int counter)
        {
            return new ClipboardOperationResult(ClipboardOperationResultCode.ErrorOpenClipboard,
                $"Clipboard has been tried to be reached {counter} times and returned {errorList.Count()} unique errors.");
        }
    }
}