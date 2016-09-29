using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsyncClipboardService.Clipboard;
using AsyncWindowsClipboard.Helpers;
using AsyncWindowsClipboard.Modifiers.Readers;

namespace AsyncWindowsClipboard.Modifiers.Writers
{
    /// <summary>
    ///     <p>Base class that clipboard writers must implement.</p>
    ///     <p>Starts and ends asynchronous <see cref="IWindowsClipboardSession" />'s and send its context to its member classes.</p>
    ///     <p>Provides helper classes and navigation properties for its members.</p>
    /// </summary>
    /// <typeparam name="TData">Data type that'll be set during this operation.</typeparam>
    /// <seealso cref="IClipboardWritingContext"/>
    /// <seealso cref="IClipboardWriter{TResult}" />
    /// <seealso cref="ClipboardModifierBase" />
    internal abstract class ClipboardWriterBase<TData> : ClipboardModifierBase, IClipboardWriter<TData>
    {

        /// <exception cref="Win32Exception">Connection to the clipboard could not be opened.</exception>
        public Task<bool> WriteAsync(TData data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            return TaskHelper.StartStaTask(() =>
            {
                using (var clipboard = new WindowsClipboardSession())
                {
                    var context = new ClipboardWritingContext(clipboard);
                    base.EnsureOpenConnection(clipboard);
                    base.ClearClipboard(clipboard);
                    var result = Write(context, data);
                    return result;
                }
            });
        }

        /// <summary>
        ///     Reads the data from specified <see cref="IWindowsClipboardSession" />.
        /// </summary>
        /// <param name="context">Clipboard session context.</param>
        /// <returns>Result of the reading operation.</returns>
        public abstract bool Write(IClipboardWritingContext context, TData data);
    }
}
