using System;
using System.Threading.Tasks;

namespace AsyncWindowsClipboard.Helpers
{
    public static class TimeoutHelper
    {
        /// <summary>
        ///     Runs the given <see cref="Func{Boolean}" /> until it returns true.
        ///     It keeps track of time and only runs it until given <see cref="TimeSpan" /> is reached.
        /// </summary>
        /// <param name="task">Task to run.</param>
        /// <param name="timeOut">Time out for the trying cycle.</param>
        /// <param name="delayMilliseconds">Delay to wait after each execution of the function.</param>
        /// <returns>
        ///     <c>TRUE</c> if the <paramref name="task" /> returns true before <paramref name="timeOut" /> otherwise;<c>FALSE</c>
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <p><paramref name="timeOut" /> is too short. It must be higher than 30.</p>
        ///     <p><paramref name="delayMilliseconds" /> is too short. It must be higher than 15.</p>
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="timeOut" /> must is lower than <paramref name="delayMilliseconds" />
        /// </exception>
        public static bool RetryUntilSuccessOrTimeout(this Func<bool> task, TimeSpan timeOut, int delayMilliseconds = 0)
        {
            //validate parameters
            if (timeOut.TotalMilliseconds < 30)
                throw new ArgumentOutOfRangeException(nameof(timeOut),
                    $"{timeOut} is too short. It must be heigher than {30}");
            if (delayMilliseconds < 15)
                throw new ArgumentOutOfRangeException(nameof(delayMilliseconds),
                    $"{delayMilliseconds} is too short. It must be heigher than {30}");
            if (timeOut.TotalMilliseconds < delayMilliseconds)
                throw new ArgumentException(
                    $"{nameof(timeOut)} ({timeOut}) must be longer than {nameof(delayMilliseconds)} ({delayMilliseconds})");
            //initialize
            var isFirstRun = true;
            var timeOutDate = DateTime.UtcNow.Add(timeOut);
            var success = false;
            //run
            while (!success && (DateTime.UtcNow < timeOutDate))
            {
                if (isFirstRun)
                {
                    isFirstRun = false;
                }
                else
                {
                    if (delayMilliseconds != 0)
                        if (DateTime.Now < timeOutDate.AddMilliseconds(delayMilliseconds))
                            Task.Delay(delayMilliseconds);
                }
                success = task();
            }
            //return
            return success;
        }
    }
}