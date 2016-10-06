using System;

namespace AsyncWindowsClipboard.Clipboard.Modifiers
{
    /// <summary>
    ///     Generic factory pattern to get the right clipboard modifier instance implementing abstract
    ///     <see cref="ClipboardModifierBase" />.
    /// </summary>
    /// <seealso cref="ClipboardModifierBase" />
    internal interface IClipboardModifierFactory
    {
        /// <summary>
        ///     Gets the specified timeout.
        /// </summary>
        /// <typeparam name="TModifier">The type of the modifier.</typeparam>
        /// <param name="timeout">
        ///     <p>If time out is <see langword="null" /> then the result will be a regular clipboard modifier.</p>
        ///     <p>
        ///         If time out parameter is given and is not <see langword="null" />. Then it'll return a clipboard
        ///         modifier with time-out strategy. The modifier will keep on trying to connect to the clipboard until it's
        ///         unlocked or timeout is reached.
        ///     </p>
        /// </param>
        /// <returns>The related modifier instance.</returns>
        TModifier Get<TModifier>(TimeSpan? timeout = null) where TModifier : ClipboardModifierBase, new();
    }
}