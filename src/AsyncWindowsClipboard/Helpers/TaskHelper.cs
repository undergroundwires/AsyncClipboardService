using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncWindowsClipboard.Helpers
{
    internal class TaskHelper
    {
        /// <summary>
        ///     Runs the func in a STA thread.
        /// </summary>
        public static Task<T> StartStaTask<T>(Func<T> func)
        {
            var tcs = new TaskCompletionSource<T>();
            var thread = new Thread(() =>
            {
                try
                {
                    tcs.SetResult(func());
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return tcs.Task;
        }
    }
}