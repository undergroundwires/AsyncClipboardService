using System;
using System.Linq;
using System.Text;

namespace AsyncWindowsClipboard.Text
{
    /// <summary>
    ///     <see cref="UnicodeTextService" /> is a wrapper around <see cref="System.Text.Encoding" /> using
    ///     <seealso cref="Encoding.Unicode" /> as encoding object.
    /// </summary>
    /// <seealso cref="ITextService" />
    public class UnicodeTextService : ITextService
    {
        public static ITextService StaticInstance => StaticInstanceLazy.Value;
        private static Lazy<UnicodeTextService> StaticInstanceLazy => new Lazy<UnicodeTextService>();

        /// <summary>
        ///     Decodes a sequence of <c>unicode</c> bytes into a string.
        /// </summary>
        /// <param name="bytes">The byte array containing the sequence of bytes to decode.</param>
        /// <exception cref="ArgumentNullException"><paramref name="bytes" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">The byte array contains invalid encoding code points.</exception>
        /// <returns>A string that contains the results of decoding the specified sequence of bytes.</returns>
        public string GetString(byte[] bytes)
        {
            if ((bytes == null) || !bytes.Any()) throw new ArgumentNullException(nameof(bytes));
            return Encoding.Unicode.GetString(bytes);
        }

        /// <summary>
        ///     Encodes all the <c>unicode</c> characters in the specified string into a sequence of bytes.
        /// </summary>
        /// <param name="text">The string containing the <c>unicode</c> characters to encode.</param>
        /// <returns>A byte array containing the results of encoding the specified set of characters.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="text" /> is <see langword="null" /> or empty.</exception>
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