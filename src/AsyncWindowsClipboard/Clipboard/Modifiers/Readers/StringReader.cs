namespace AsyncWindowsClipboard.Modifiers.Readers
{
    /// <summary>
    ///     Reads a <see cref="string" /> text from an <see cref="IClipboardReadingContext" />.
    /// </summary>
    /// <seealso cref="IClipboardReadingContext" />
    internal class StringReader : ClipboardReaderBase<string>
    {
        public override bool Exists(IClipboardReadingContext context)
        {
            return context.IsContentTypeOf(ClipboardDataType.UnicodeLittleEndianText);
        }
        public override string Read(IClipboardReadingContext context)
        {
            var reader = new UnicodeBytesReader();
            var unicodeBytes = reader.Read(context);
            var result = TextService.GetString(unicodeBytes);
            return result;
        }
    }
}