using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsyncWindowsClipboard.Modifiers.Readers;

namespace AsyncWindowsClipboard.Clipboard.Modifiers.Readers
{
    /// <summary>
    /// Reads the clipboard data as a list of file paths
    /// </summary>
    /// 
    internal class FileDropListReader : ClipboardReaderBase<IEnumerable<string>>
    {
        public override bool Exists(IClipboardReadingContext context)
        {
            return context.IsContentTypeOf(ClipboardDataType.FileDropList);
        }

        public override IEnumerable<string> Read(IClipboardReadingContext context)
        {
            var clipboardData = context.GetData(ClipboardDataType.FileDropList);
            if (clipboardData == null || !clipboardData.Any()) return null;
            //    From windows api documentation : https://msdn.microsoft.com/en-us/library/windows/desktop/bb776902(v=vs.85).aspx#CF_HDROP
            //  The file name array consists of a series of strings, each containing one file's fully qualified path, including
            //  the terminating NULL character. An additional null character is appended to the final string to terminate the
            //  array. For example, if the files c:\temp1.txt and c:\temp2.txt are being transferred, the character array looks
            //  like this:
            //    c:\temp1.txt'\0'c:\temp2.txt'\0''\0'
            var text = TextService.GetString(clipboardData);
            text = RemoveHeader(text); //header that's added but not described in api documentation above.
            text = text.Substring(0, text.Length - 2); //remove last two nulls
            var result = text.Split('\0'); //split them into a list
            return result;
        }

        private static string RemoveHeader(string text) => text.Substring(10);
    }
}
