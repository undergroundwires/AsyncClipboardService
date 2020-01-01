﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AsyncWindowsClipboard.Clipboard;
using AsyncWindowsClipboard.Clipboard.Result;
using AsyncWindowsClipboard.Modifiers.Writers.Base;

namespace AsyncWindowsClipboard.Modifiers.Writers
{
    internal class FileDropListWriter : ClipboardWriterBase<IEnumerable<string>>
    {
        private const string Header = "\u0014\0\0\0\0\0\0\0\u0001\0";
        private const string Terminator = "\0";

        /// <exception cref="T:System.ArgumentNullException"><paramref name="data" /> is <see langword="null" /></exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="data" /> is empty list.</exception>
        public override IClipboardOperationResult Write(IClipboardWritingContext context, IEnumerable<string> data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            var filePaths = data as string[] ?? data.ToArray();
            if (!filePaths.Any()) throw new ArgumentException($"{nameof(data)} is empty list");
            //    https://msdn.microsoft.com/en-us/library/windows/desktop/bb776902(v=vs.85).aspx#CF_HDROP
            //  The file name array consists of a series of strings, each containing one file's fully qualified path, including
            //  the terminating NULL character. An additional null character is appended to the final string to terminate the
            //  array. For example, if the files c:\temp1.txt and c:\temp2.txt are being transferred, the character array looks
            //  like this:
            //    c:\temp1.txt'\0'c:\temp2.txt'\0''\0'
            var sb = new StringBuilder();
            sb.Append(Header);
            foreach (var filePath in filePaths) sb.Append(filePath + '\0');
            sb.Append(Terminator);
            var stringData = sb.ToString();
            var bytes = TextService.GetBytes(stringData);
            var result = context.SetData(ClipboardDataType.FileDropList, bytes);
            return result;
        }
    }
}