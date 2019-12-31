namespace AsyncWindowsClipboard.Clipboard.Text
{
    /// <summary>
    ///     Defines an interface to convert between a <see cref="byte" /> array and <see cref="string" /> with an constant
    ///     encoding.
    /// </summary>
    internal interface ITextService
    {
        /// <summary>
        ///     Decodes a sequence of bytes into a string.
        /// </summary>
        /// <param name="bytes">The byte array containing the sequence of bytes to decode.</param>
        /// <returns>A string that contains the results of decoding the specified sequence of bytes.</returns>
        string GetString(byte[] bytes);

        /// <summary>
        ///     Encodes all the characters in the specified string into a sequence of bytes.
        /// </summary>
        /// <param name="text">The string containing the characters to encode.</param>
        /// <returns>A byte array containing the results of encoding the specified set of characters.</returns>
        byte[] GetBytes(string text);
    }
}