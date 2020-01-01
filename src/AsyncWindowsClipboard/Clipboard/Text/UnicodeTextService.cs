using System;
using System.Linq;
using System.Text;

namespace AsyncWindowsClipboard.Clipboard.Text
{
    /// <summary>
    ///     <see cref="UnicodeTextService" /> is a wrapper around <see cref="System.Text.Encoding" /> using
    ///     <seealso cref="Encoding.Unicode" /> as encoding object.
    /// </summary>
    /// <seealso cref="ITextService" />
    internal class UnicodeTextService : ITextService
    {
        public static ITextService StaticInstance => StaticInstanceLazy.Value;
        private static Lazy<UnicodeTextService> StaticInstanceLazy => new Lazy<UnicodeTextService>();

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="bytes" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">The byte array contains invalid encoding code points.</exception>
        /// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Character Encoding in .NET for complete explanation)  
        ///  -and-  
        ///  <see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
        public string GetString(byte[] bytes)
        {
            if ((bytes == null) || !bytes.Any())
                throw new ArgumentNullException(nameof(bytes));
            return Encoding.Unicode.GetString(bytes);
        }

        /// <inheritdoc />
        /// <exception cref="T:System.ArgumentException"><paramref name="text" /> is <see langword="null" /> or empty.</exception>
        /// <exception cref="System.Text.EncoderFallbackException">
        ///     A fallback occurred (see https://msdn.microsoft.com/en-us/library/ms404377(v=vs.110).aspx , Character Encoding in
        ///     the .NET Framework)-and-<see cref="P:System.Text.Encoding.EncoderFallback" /> is set to
        ///     <see cref="T:System.Text.EncoderExceptionFallback" />.
        /// </exception>
        public byte[] GetBytes(string text)
        {
            if (string.IsNullOrEmpty(text)) throw new ArgumentException("Value cannot be null or empty.", nameof(text));
            return Encoding.Unicode.GetBytes(text);
        }
    }
}