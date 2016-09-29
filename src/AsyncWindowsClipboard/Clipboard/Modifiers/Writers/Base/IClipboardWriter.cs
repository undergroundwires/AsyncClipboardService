using System.Threading.Tasks;

namespace AsyncWindowsClipboard.Modifiers.Writers
{
    /// <summary>
    ///     Abstraction for a clipboard data setter.
    /// </summary>
    /// <typeparam name="TData">Type of the data that'll be set.</typeparam>
    public interface IClipboardWriter<in TData>
    {
        Task<bool> WriteAsync(TData data);
    }
}