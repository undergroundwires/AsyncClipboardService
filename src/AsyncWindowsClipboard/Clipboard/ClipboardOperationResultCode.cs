namespace AsyncClipboardService.Clipboard
{
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

        ErrorGlobalAlloc,
        ErrorGlobalLock,
        ErrorSetClipboardData
    };
}
