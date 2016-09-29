namespace AsyncWindowsClipboard.Native
{
    internal static partial class NativeMethods
    {
        /// <summary>
        ///     Combines GMEM_MOVEABLE and GMEM_ZEROINIT.
        ///     GMEM_MOVEABLE : Allocates movable memory. Memory blocks are never moved in physical memory, but they can be moved
        ///     within the default heap.
        ///     The return value is a handle to the memory object. To translate the handle into a pointer, use the GlobalLock
        ///     function.
        ///     This value cannot be combined with GMEM_FIXED.
        ///     GMEM_ZEROINIT : Initializes memory contents to zero.
        /// </summary>
        /// <remarks>https://msdn.microsoft.com/en-us/library/windows/desktop/aa366574%28v=vs.85%29.aspx</remarks>
        internal const uint GHND = 0x0042;

        #region [Standard Clipboard Formats]

        //The clipboard formats defined by the system are called standard clipboard formats.
        //https://msdn.microsoft.com/en-us/library/windows/desktop/ff729168%28v=vs.85%29.aspx?f=255&MSPPError=-2147217396

        /// <summary>
        ///     A handle to a bitmap (<c>HBITMAP</c>).
        /// </summary>
        public const uint CF_BITMAP = 2;

        /// <summary>
        ///     A memory object containing a <c>BITMAPINFO</c> structure followed by the bitmap bits.
        /// </summary>
        public const uint CF_DIB = 8;

        /// <summary>
        ///     A memory object containing a <c>BITMAPV5HEADER</c> structure followed by the bitmap color space information and the
        ///     bitmap bits.
        /// </summary>
        public const uint CF_DIBV5 = 17;

        /// <summary>
        ///     Software Arts' Data Interchange Format.
        /// </summary>
        public const uint CF_DIF = 5;

        /// <summary>
        ///     Bitmap display format associated with a private format.
        ///     The <c>hMem</c> parameter must be a handle to data that can be displayed in bitmap format in lieu of the privately
        ///     formatted data.
        /// </summary>
        public const uint CF_DSPBITMAP = 0x0082;

        /// <summary>
        ///     Enhanced metafile display format associated with a private format.
        ///     The <c>hMem</c> parameter must be a handle to data that can be displayed in enhanced metafile format in lieu of the
        ///     privately formatted data.
        /// </summary>
        public const uint CF_DSPENHMETAFILE = 0x008E;

        /// <summary>
        ///     Metafile-picture display format associated with a private format. The <c>hMem</c> parameter must be a handle to
        ///     data that can be displayed in metafile-picture format in lieu of the privately formatted data.
        /// </summary>
        public const uint CF_DSPMETAFILEPICT = 0x0083;

        /// <summary>
        ///     Text display format associated with a private format. The <c>hMem</c> parameter must be a handle to data that can
        ///     be displayed in text format in lieu of the privately formatted data.
        /// </summary>
        public const uint CF_DSPTEXT = 0x0081;

        /// <summary>
        ///     A handle to an enhanced metafile (<c>HENHMETAFILE</c>).
        /// </summary>
        public const uint CF_ENHMETAFILE = 14;

        /// <summary>
        ///     <p>
        ///         Start of a range of integer values for application-defined GDI object clipboard formats. The end of the range
        ///         is
        ///         <see cref="CF_GDIOBJLAST" />.
        ///     </p>
        ///     <p>
        ///         Handles associated with clipboard formats in this range are not automatically deleted using the
        ///         <see cref="GlobalFree" />
        ///         function when the clipboard is emptied. Also, when using values in this range, the <c>hMem</c> parameter is not
        ///         a
        ///         handle to a GDI object, but is a handle allocated by the <see cref="GlobalAlloc" /> function with the
        ///         <c>GMEM_MOVEABLE</c> flag.
        ///     </p>
        /// </summary>
        public const uint CF_GDIOBJFIRST = 0x0300;

        /// <summary>
        ///     See <see cref="CF_GDIOBJFIRST" />.
        /// </summary>
        public const uint CF_GDIOBJLAST = 0x03FF;

        /// <summary>
        ///     A handle to type <c>HDROP</c> that identifies a list of files.
        ///     An application can retrieve information about the files by passing the handle to the <c>DragQueryFile</c> function.
        /// </summary>
        public const uint CF_HDROP = 15;

        /// <summary>
        ///     <p>
        ///         The data is a handle to the locale identifier associated with text in the clipboard. When you close the
        ///         clipboard, if it contains <see cref="CF_TEXT" /> data but no <see cref="CF_LOCALE" /> data, the system
        ///         automatically sets the <see cref="CF_LOCALE" /> format to the current input language. You can use the
        ///         <see cref="CF_LOCALE" /> format to associate a different locale with the clipboard text.
        ///     </p>
        ///     <p>
        ///         An application that pastes text from the clipboard can retrieve this format to determine which character set
        ///         was used to generate the text.
        ///     </p>
        ///     <p>
        ///         Note that the clipboard does not support plain text in multiple character sets. To achieve this, use a
        ///         formatted text data type such as RTF instead.
        ///     </p>
        ///     <p>
        ///         The system uses the code page associated with <see cref="CF_LOCALE" /> to implicitly convert from
        ///         <see cref="CF_TEXT" /> to <see cref="CF_UNICODETEXT" />. Therefore, the correct code page table is used
        ///         for the conversion.
        ///     </p>
        /// </summary>
        public const uint CF_LOCALE = 16;

        /// <summary>
        ///     Handle to a metafile picture format as defined by the <c>METAFILEPICT</c> structure. When passing a
        ///     <see cref="CF_METAFILEPICT" />  handle by means of DDE, the application responsible for deleting <c>hMem</c> should
        ///     also free the metafile referred to by the <see cref="CF_METAFILEPICT" /> handle.
        /// </summary>
        public const uint CF_METAFILEPICT = 3;

        /// <summary>
        ///     Text format containing characters in the OEM character set. Each line ends with a carriage return/linefeed (CR-LF)
        ///     combination. A null character signals the end of the data.
        /// </summary>
        public const uint CF_OEMTEXT = 7;

        /// <summary>
        ///     Owner-display format. The clipboard owner must display and update the clipboard viewer window, and receive the
        ///     <c>WM_ASKCBFORMATNAME</c>, <c>WM_HSCROLLCLIPBOARD</c>, <c>WM_PAINTCLIPBOARD</c>, <c>WM_SIZECLIPBOARD</c>, and
        ///     <c>WM_VSCROLLCLIPBOARD</c> messages. The <c>hMem</c> parameter must be <see langword="null" />.
        /// </summary>
        internal const uint CF_OWNERDISPLAY = 0x0080;

        /// <summary>
        ///     <p>
        ///         Handle to a color palette. Whenever an application places data in the clipboard that depends on or assumes a
        ///         color palette, it should place the palette on the clipboard as well.
        ///     </p>
        ///     <p>
        ///         If the clipboard contains data in the <see cref="CF_PALETTE" /> (logical color palette) format, the application
        ///         should use the <c>SelectPalette</c> and <c>RealizePalette</c> functions to realize (compare) any other data in
        ///         the clipboard against that logical palette.
        ///     </p>
        ///     <p>
        ///         When displaying clipboard data, the clipboard always uses as its current palette any object on the clipboard
        ///         that is in the <see cref="CF_PALETTE" /> format.
        ///     </p>
        /// </summary>
        internal const uint CF_PALETTE = 9;

        /// <summary>
        ///     Data for the pen extensions to the Microsoft Windows for Pen Computing.
        /// </summary>
        internal const uint CF_PENDATA = 10;

        /// <summary>
        ///     Start of a range of integer values for private clipboard formats. The range ends with <see cref="CF_PRIVATELAST" />
        ///     . Handles associated with private clipboard formats are not freed automatically; the clipboard owner must free such
        ///     handles, typically in response to the <c>WM_DESTROYCLIPBOARD</c> message.
        /// </summary>
        internal const uint CF_PRIVATEFIRST = 0x0200;

        /// <summary>
        ///     See <see cref="CF_PRIVATEFIRST" />.
        /// </summary>
        internal const uint CF_PRIVATELAST = 0x02FF;

        /// <summary>
        ///     Represents audio data more complex than can be represented in a <see cref="CF_WAVE" /> standard wave format.
        /// </summary>
        internal const uint CF_RIFF = 11;

        /// <summary>
        ///     Microsoft Symbolic Link (SYLK) format.
        /// </summary>
        internal const uint CF_SYLK = 4;

        /// <summary>
        ///     Text format. Each line ends with a carriage return/linefeed (CR-LF) combination. A null character signals the end
        ///     of the data. Use this format for ANSI text.
        /// </summary>
        internal const uint CF_TEXT = 1;

        /// <summary>
        ///     Tagged-image file format.
        /// </summary>
        internal const uint CF_TIFF = 6;

        /// <summary>
        ///     Unicode text format. Each line ends with a carriage return/linefeed (CR-LF) combination. A null character signals
        ///     the end of the data.
        /// </summary>
        internal const uint CF_UNICODETEXT = 13;

        /// <summary>
        ///     Represents audio data in one of the standard wave formats, such as 11 kHz or 22 kHz PCM.
        /// </summary>
        internal const uint CF_WAVE = 12;

        #endregion
    }
}