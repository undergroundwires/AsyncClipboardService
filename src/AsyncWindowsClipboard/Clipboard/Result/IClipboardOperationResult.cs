namespace AsyncWindowsClipboard.Clipboard.Result
{
    internal interface IClipboardOperationResult
    {
        ClipboardOperationResultCode ResultCode { get; }
        uint? LastError { get; }
        uint[] LastErrors { get; }
        bool IsSuccessful { get; }
        string Message { get; }
    }
}