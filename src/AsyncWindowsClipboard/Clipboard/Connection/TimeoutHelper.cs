using System;
using System.Threading.Tasks;

namespace AsyncWindowsClipboard.Clipboard.Connection
{
    /// <summary>
    ///     Utility class to help with retry and timeout logic.
    /// </summary>
    public static class TimeoutHelper
    {
        /// <summary>
        ///     Runs the given <see cref="Func{Boolean}" /> until it returns true.
        ///     It keeps track of time and only runs it until given <see cref="TimeSpan" /> is reached.
        /// </summary>
        /// <param name="func">Task to run.</param>
        /// <param name="timeout">Time out for the trying cycle.</param>
        /// <param name="delayInMs">Delay to wait after each execution of the function.</param>
        /// <returns>
        ///     <c>TRUE</c> if the <paramref name="func" /> returns true before <paramref name="timeout" /> otherwise;<c>FALSE</c>
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <p><paramref name="timeout" /> is too short. It must be higher than 30.</p>
        ///     <p><paramref name="delayInMs" /> is too short. It must be higher than 15.</p>
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="timeout" /> must is lower than <paramref name="delayInMs" />
        /// </exception>
        public static bool RetryUntilSuccessOrTimeout(this Func<bool> func, TimeSpan timeout, int delayInMs = 0)
        {
            ValidateParameters(timeout, delayInMs);
            // Validate parameters
            var session = new RetrySession(timeout, delayInMs);
            while (session.CanRun)
                session.Run(func);
            return session.IsSuccess;
        }

        /// <exception cref="ArgumentOutOfRangeException">
        ///     <p><paramref name="timeout" /> is too short. It must be higher than 30.</p>
        ///     <p><paramref name="delayInMs" /> is too short. It must be higher than 15.</p>
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="timeout" /> must is lower than <paramref name="delayInMs" />
        /// </exception>
        private static void ValidateParameters(TimeSpan timeout, int delayInMs)
        {
            if (timeout.TotalMilliseconds < 30)
                throw new ArgumentOutOfRangeException(nameof(timeout),
                    $"{timeout} is too short. It must be higher than {30}");
            if (delayInMs < 15)
                throw new ArgumentOutOfRangeException(nameof(delayInMs),
                    $"{delayInMs} is too short. It must be higher than {30}");
            if (timeout.TotalMilliseconds < delayInMs)
                throw new ArgumentException(
                    $"{nameof(timeout)} ({timeout}) must be longer than {nameof(delayInMs)} ({delayInMs})");
        }

        private class RetrySession
        {
            private readonly int _delayInMs;
            private readonly DateTime _finishDate;
            private bool isFirstRun = true;

            public RetrySession(TimeSpan timeout, int delayInMs)
            {
                _delayInMs = delayInMs;
                _finishDate = DateTime.UtcNow.Add(timeout);
            }

            public bool IsSuccess { get; private set; }
            public bool CanRun => !IsSuccess && DateTime.UtcNow < _finishDate;

            public void Run(Func<bool> func)
            {
                if (isFirstRun)
                {
                    isFirstRun = false;
                }
                else
                {
                    if (_delayInMs != 0)
                        if (DateTime.Now < _finishDate.AddMilliseconds(_delayInMs))
                            Task.Delay(_delayInMs);
                }

                IsSuccess = func();
            }
        }
    }
}