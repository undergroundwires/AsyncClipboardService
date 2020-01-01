namespace AsyncWindowsClipboard.Clipboard
{
    /// <summary>
    /// Result of the Clipboard operation
    /// </summary>
    public enum ClipboardOperationResultCode
    {
        /// <summary>
        /// Clipboard operations was successful
        /// </summary>
        Success = 0,
        /// <summary>
        /// Clipboard operation failed due to an error during opening clipboard.
        /// </summary>
        ErrorOpenClipboard,
        /// <summary>
        /// Clipboard operation failed due to an error during closing clipboard.
        /// </summary>
        ErrorCloseClipboard,
        /// <summary>
        /// Clipboard operation failed due to an error during clearing clipboard.
        /// </summary>
        ErrorClearClipboard,
        /// <summary>
        /// Clipboard operation when allocating native bytes for the clipboard.
        /// </summary>
        ErrorGlobalAlloc,
        /// <summary>
        /// Clipboard operation when communicating native bytes with the clipboard.
        /// </summary>
        ErrorGlobalLock,
        /// <summary>
        /// Clipboard operation when communicating wit the clipboard.
        /// </summary>
        ErrorSetClipboardData
    };
}
