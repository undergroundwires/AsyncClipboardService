namespace AsyncWindowsClipboard
{
    /// <summary>
    ///     An enum representing clipboard data formats.
    /// </summary>
    /// <remarks>
    ///     This enum is intended for public usage. Check <see cref="ClipboardDataType" /> for internal wrapper of windows
    ///     clipboard.
    /// </remarks>
    public enum ClipboardDataFormat
    {
        /// <summary>
        ///     Represents text clipboard format.
        /// </summary>
        Text,

        /// <summary>
        ///     Represents file drop list clipboard format.
        /// </summary>
        FileDropList
    }
}