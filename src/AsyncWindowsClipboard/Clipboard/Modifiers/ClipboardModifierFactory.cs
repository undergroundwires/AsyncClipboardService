namespace AsyncWindowsClipboard.Clipboard.Modifiers
{
    /// <summary>
    ///     Generic factory pattern to get the right clipboard modifier instance implementing abstract <see cref="ClipboardModifierBase"/>.
    /// </summary>
    /// <seealso cref="ClipboardModifierBase"/>
    internal interface IClipboardModifierFactory
    {
        TModifier Get<TModifier>() where TModifier : ClipboardModifierBase, new();
    }
    /// <summary>
    /// Factory implementation to get the modifier instance implementing abstract <see cref="ClipboardModifierBase"/>
    /// </summary>
    /// <seealso cref="IClipboardModifierFactory" />
    /// <seealso cref="ClipboardModifierBase"/>
    internal class ClipboardModifierFactory : IClipboardModifierFactory
    {
        public static IClipboardModifierFactory StaticInstance = new ClipboardModifierFactory();
        public TModifier Get<TModifier>() where TModifier : ClipboardModifierBase, new()
        {
            return new TModifier();
        }
    }
}