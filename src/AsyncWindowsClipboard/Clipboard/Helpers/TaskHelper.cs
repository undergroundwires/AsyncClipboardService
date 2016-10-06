using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncWindowsClipboard.Helpers
{
    internal static class TaskHelper
    {
        /// <summary>
        ///     Runs the given <param name="func"/> as a <see cref="Task"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The function to run.</param>
        /// <returns>A <see cref="Task{TResult}"/> that runs <param name="func"></param></returns>
        /// <exception cref="ArgumentNullException"><paramref name="func" /> is <see langword="null" />.</exception>
        internal static Task<TResult> StartStaTask<TResult>(Func<TResult> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            var tcs = new TaskCompletionSource<TResult>();
            var thread = new Thread(() =>
            {
                try
                {
                    var result = func();
                    tcs.SetResult(result);
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