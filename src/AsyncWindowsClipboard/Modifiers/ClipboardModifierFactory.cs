using System;

namespace AsyncWindowsClipboard.Modifiers
{
    /// <summary>
    ///     Factory implementation to get the modifier instance implementing abstract <see cref="ClipboardModifierBase" />
    /// </summary>
    /// <seealso cref="IClipboardModifierFactory" />
    /// <seealso cref="ClipboardModifierBase" />
    internal class ClipboardModifierFactory : IClipboardModifierFactory
    {
        public static IClipboardModifierFactory StaticInstance = new ClipboardModifierFactory();

        public TModifier Get<TModifier>(TimeSpan? timeout = null) where TModifier : ClipboardModifierBase, new()
        {
            var result = CreateInstance<TModifier>(timeout);
            return result;
        }

        public TModifier CreateInstance<TModifier>(TimeSpan? timeout = null)
            where TModifier : ClipboardModifierBase, new()
        {
            var instance = new TModifier {Timeout = timeout};
            return instance;
        }
    }
}