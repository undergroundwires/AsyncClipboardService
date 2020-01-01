using System;
using System.Collections.Generic;
using System.Linq;
using AsyncWindowsClipboard.Clipboard.Result;

namespace AsyncWindowsClipboard.Clipboard.Connection
{
    internal sealed class ClipboardOpenerWithTimeout : IClipboardOpenerWithTimeout
    {
        public const int DefaultDelayMilliseconds = 30;

        /// <exception cref="T:System.ArgumentNullException"><paramref name="session" /> is <see langword="null" /></exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     <p><paramref name="timeout" /> is too short. It must be higher than 30.</p>
        ///     <p><paramref name="delayMilliseconds" /> is too short. It must be higher than 15.</p>
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="timeout" /> must is lower than
        ///     <paramref name="delayMilliseconds" />
        /// </exception>
        public IClipboardOperationResult Open(IWindowsClipboardSession session, TimeSpan timeout,
            int delayMilliseconds = DefaultDelayMilliseconds)
        {
            // Validate parameter
            if (session == null) throw new ArgumentNullException(nameof(session));
            if (session.IsOpen)
                return new ClipboardOperationResult(ClipboardOperationResultCode.Success,
                    "A clipboard connection for the current thread is already active.");
            if (timeout.TotalMilliseconds < delayMilliseconds) return session.Open();
            var errorCodes = new List<uint>();
            var counter = 0;
            var result = TimeoutHelper.RetryUntilSuccessOrTimeout(
                func: () => TryOpenSession(session, errorCodes, ref counter),
                timeout: timeout,
                delayInMs: delayMilliseconds);
            return result ? ClipboardOperationResult.SuccessResult : GetErrorResult(errorCodes, counter);
        }

        private static bool TryOpenSession(IWindowsClipboardSession session, ICollection<uint> errorCodes,
            ref int counter)
        {
            counter++;
            var result = session.Open();
            if (result.LastError.HasValue)
                errorCodes.Add(result.LastError.Value);
            return result.IsSuccessful;
        }

        private static ClipboardOperationResult GetErrorResult(IEnumerable<uint> errorCodes, int counter)
        {
            var errors = errorCodes.Distinct().ToArray();
            if (errors.Length == 1)
                return GetResultForSingleError(errors[0], counter);
            if (errors.Any())
                return GetResultForMultipleErrors(errors, counter);
            return new ClipboardOperationResult(
                resultCode: ClipboardOperationResultCode.ErrorOpenClipboard,
                message: $"Clipboard could not be opened after {counter} tries");
        }

        private static ClipboardOperationResult GetResultForSingleError(uint error, int counter)
        {
            return new ClipboardOperationResult(
                resultCode: ClipboardOperationResultCode.ErrorOpenClipboard,
                message:
                $"Clipboard has been tried to be reached {counter} times and all of them returned {error} error code.",
                errorCode: error);
        }

        private static ClipboardOperationResult GetResultForMultipleErrors(IEnumerable<uint> errorList, int counter)
        {
            return new ClipboardOperationResult(
                resultCode: ClipboardOperationResultCode.ErrorOpenClipboard,
                message:
                $"Clipboard has been tried to be reached {counter} times and returned {errorList.Count()} unique errors.");
        }
    }
}