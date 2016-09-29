using System.ComponentModel;
using System.Diagnostics;
using AsyncClipboardService.Clipboard;
using AsyncWindowsClipboard.Text;

namespace AsyncWindowsClipboard.Modifiers
{
    /// <summary>
    ///     <p>Provides helper methods to manipulate a clipboard connection.</p>
    ///     <p>Provides helper services.</p>
    /// </summary>
    internal abstract class ClipboardModifierBase
    {
        protected ClipboardModifierBase()
        {
            TextService = UnicodeTextService.StaticInstance;
        }

        protected ITextService TextService { get; }

        /// <summary>
        ///     Clears the clipboard, and asserts if the operation fails.
        /// </summary>
        protected void ClearClipboard(IWindowsClipboardSession clipboard)
        {
            var clearResult = clipboard.Clear();
            if (!clearResult.IsSuccessful) Debug.Assert(false);
        }

        /// <summary>
        ///     Ensures that connection to <see cref="clipboard" /> instance is made, it throws if connection fails.
        /// </summary>
        /// <param name="clipboard">The clipboard object to open connection.</param>
        /// <exception cref="Win32Exception">Connection to the clipboard could not be opened.</exception>
        protected void EnsureOpenConnection(IWindowsClipboardSession clipboard)
        {
            if (clipboard.IsOpen) return;
            var openResult = clipboard.Open();
            if (!openResult.IsSuccessful)
            {
                if (openResult.LastError.HasValue)
                    throw new Win32Exception((int) openResult.LastError.Value);
                throw new Win32Exception("Clipboard could not be reached.");
            }
        }
    }
}