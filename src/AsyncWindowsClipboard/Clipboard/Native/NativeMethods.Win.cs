using System;
using System.Runtime.InteropServices;

namespace AsyncWindowsClipboard.Native
{
    // Documentations from MSDN are updated 2016-09-26
    internal static partial class NativeMethods
    {
        /// <summary>
        ///     Retrieves the current size of the specified global memory object, in bytes.
        /// </summary>
        /// <param name="hMem">
        ///     A handle to the global memory object. This handle is returned by either the
        ///     <see cref="GlobalAlloc" /> or <c>GlobalReAlloc</c> function.
        /// </param>
        /// <returns>
        ///     <p>If the function succeeds, the return value is the size of the specified global memory object, in bytes.</p>
        ///     <p>
        ///         If the specified handle is not valid or if the object has been discarded, the return value is zero. To get
        ///         extended error information, call <see cref="GetLastError" />.
        ///     </p>
        /// </returns>
        /// <remarks>
        ///     <p>The size of a memory block may be larger than the size requested when the memory was allocated.</p>
        /// </remarks>
        [DllImport(Dlls.Kernel32)]
        internal static extern UIntPtr GlobalSize(IntPtr hMem);

        /// <summary>
        ///     Locks a global memory object and returns a pointer to the first byte of the object's memory block.
        /// </summary>
        /// <param name="hMem">
        ///     A handle to the global memory object. This handle is returned by either the
        ///     <see cref="GlobalAlloc" /> or <c>GlobalReAlloc</c> function.
        /// </param>
        /// <returns>
        ///     <p>If the function succeeds, the return value is a pointer to the first byte of the memory block.</p>
        ///     <p>
        ///         If the function fails, the return value is <see langword="null" />. To get extended error information,  call
        ///         <see cref="GetLastError" />.
        ///     </p>
        /// </returns>
        /// <remarks>
        ///     <p>Discarded objects always have a lock count of zero.</p>
        ///     <p>
        ///         If the specified memory block has been discarded or if the memory block has a zero-byte size, this function
        ///         returns <see langword="null" />.
        ///     </p>
        ///     <p>
        ///         Memory objects allocated with <c>GMEM_FIXED</c> always have a lock count of zero. For these objects, the value
        ///         of the returned pointer is equal to the value of the specified handle.
        ///     </p>
        ///     <p>
        ///         The internal data structures for each memory object include a lock count that is initially zero. For movable
        ///         memory objects, <see cref="GlobalLock"/> increments the count by one, and the <see cref="GlobalUnlock"/> function decrements the count by
        ///         one. Each successful call that a process makes to <see cref="GlobalLock"/> for an object must be matched by a corresponding
        ///         call to <see cref="GlobalUnlock"/>. Locked memory will not be moved or discarded, unless the memory object is reallocated by
        ///         using the <c>GlobalReAlloc</c> function. The memory block of a locked memory object remains locked until its lock
        ///         count is decremented to zero, at which time it can be moved or discarded.
        ///     </p>
        /// </remarks>
        [DllImport(Dlls.Kernel32)]
        internal static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport(Dlls.Kernel32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GlobalUnlock(IntPtr hMem);

        [DllImport(Dlls.Kernel32)]
        internal static extern IntPtr GlobalFree(IntPtr hMem);

        [DllImport(Dlls.Kernel32)]
        internal static extern IntPtr GlobalAlloc(uint uFlags, UIntPtr dwBytes);

        /// <summary>
        ///     Retrieves data from the clipboard in a specified format. The clipboard must have been opened previously.
        /// </summary>
        /// <param name="uFormat">
        ///     A clipboard format. For a description of the standard clipboard formats, see
        ///     https://msdn.microsoft.com/en-us/library/windows/desktop/ms649013(v=vs.85).aspx#_win32_Standard_Clipboard_Formats .
        /// </param>
        /// <returns>
        ///     <p>If the function succeeds, the return value is the handle to a clipboard object in the specified format.</p>
        ///     <p>
        ///         If the function fails, the return value is <see langword="null" />. To get extended error information, call
        ///         <see cref="GetLastError" />.
        ///     </p>
        /// </returns>
        /// <remarks>
        ///     Caution : Clipboard data is not trusted. Parse the data carefully before using it in your application.
        /// </remarks>
        /// <seealso cref="CloseClipboard" />
        /// <seealso cref="EmptyClipboard" />
        /// <seealso cref="SetClipboardData" />
        [DllImport(Dlls.User32, SetLastError = true)]
        internal static extern IntPtr GetClipboardData(uint uFormat);

        /// <summary>
        ///     Opens the clipboard for examination and prevents other applications from modifying the clipboard content
        /// </summary>
        /// <param name="hWndNewOwner">
        ///     A handle to the window to be associated with the open clipboard.
        ///     If this parameter is <see langword="null" />, the open clipboard is associated with the current task.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the function succeeds, <c>false</c> the function fails.
        /// </returns>
        /// <remarks>
        ///     <p><see cref="OpenClipboard" /> fails if another window has the clipboard open.</p>
        ///     <p>
        ///         An application should call the <see cref="CloseClipboard" /> function after every successful call to
        ///         <see cref="OpenClipboard" />.
        ///     </p>
        ///     <p>
        ///         The window identified by the <paramref name="hWndNewOwner" /> parameter does not become the clipboard owner
        ///         unless the <seealso cref="EmptyClipboard" />
        ///         function is called.
        ///     </p>
        ///     <p>
        ///         If an application calls <seealso cref="OpenClipboard" /> with hwnd set to <see langword="null" />,
        ///         <seealso cref="EmptyClipboard" /> sets the clipboard
        ///         owner to <see langword="null" />; this
        ///         causes <seealso cref="SetClipboardData" /> to fail.
        ///     </p>
        /// </remarks>
        /// <seealso cref="CloseClipboard" />
        /// <seealso cref="EmptyClipboard" />
        /// <seealso cref="SetClipboardData" />
        [DllImport(Dlls.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool OpenClipboard(IntPtr hWndNewOwner);

        /// <summary>
        ///     Closes the clipboard.
        /// </summary>
        /// <returns>
        ///     <p><c>true</c> if the function succeeds, <c>false</c> the function fails.</p>
        ///     <p>If the function fail. To get extended error information, call <see cref="GetLastError" />.</p>
        /// </returns>
        /// <remarks>
        ///     <p>
        ///         When the window has finished examining or changing the clipboard, close the clipboard by calling
        ///         <see cref="CloseClipboard" />. This enables other windows to access the clipboard.
        ///     </p>
        ///     <p>Do not place an object on the clipboard after calling <see cref="CloseClipboard" />.</p>
        /// </remarks>
        /// <seealso cref="OpenClipboard" />
        [DllImport(Dlls.User32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CloseClipboard();

        /// <summary>
        ///     <p>Places data on the clipboard in a specified clipboard format.</p>
        ///     <p>
        ///         The window must be the current clipboard owner, and the application must have called the
        ///         <see cref="OpenClipboard" /> function.
        ///     </p>
        ///     <p>Read more at : https://msdn.microsoft.com/en-us/library/windows/desktop/ms649051(v=vs.85).aspx </p>
        /// </summary>
        /// <param name="uFormat">
        ///     The clipboard format. This parameter can be a registered format or any of the standard clipboard formats.
        ///     For more information
        ///     <p>https://msdn.microsoft.com/en-us/library/windows/desktop/ff729168(v=vs.85).aspx</p>
        ///     <p>https://msdn.microsoft.com/en-us/library/windows/desktop/ms649013(v=vs.85).aspx#_win32_Registered_Clipboard_Formats</p>
        /// </param>
        /// <param name="hMem">
        ///     <p>
        ///         A handle to the data in the specified format. This parameter can be <see langword="null" />, indicating that
        ///         the window provides
        ///         data in the specified clipboard format (renders the format) upon request. If a window delays rendering, it must
        ///         process the <c>WM_RENDERFORMAT</c> and WM_RENDERALLFORMATS messages.
        ///     </p>
        ///     <p>
        ///         If <see cref="SetClipboardData" /> succeeds, the system owns the object identified by the <c>hMem</c>
        ///         parameter. The
        ///         application may
        ///         not write to or free the data once ownership has been transferred to the system, but it can lock and read from
        ///         the data until the <see cref="CloseClipboard" /> function is called. (The memory must be unlocked before the
        ///         Clipboard is
        ///         closed.) If the hMem parameter identifies a memory object, the object must have been allocated using the
        ///         function with the <c>GMEM_MOVEABLE</c> flag.
        ///     </p>
        /// </param>
        /// <returns>
        ///     <p>If the function succeeds, the return value is the handle to the data.</p>
        ///     <p>
        ///         If the function fails, the return value is <see langword="null" />. To get extended error information, call
        ///         <see cref="GetLastError" />.
        ///     </p>
        /// </returns>
        /// <remarks>
        ///     <p>
        ///         If an application calls <exception cref="OpenClipboard"></exception> with hwnd set to <see langword="null" />,
        ///         <seealso cref="EmptyClipboard" /> sets the clipboard owner to NULL; this causes SetClipboardData to fail.
        ///     </p>
        /// </remarks>
        [DllImport(Dlls.User32, SetLastError = true)]
        internal static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);

        /// <summary>
        ///     Empties the clipboard and frees handles to data in the clipboard. The function then assigns ownership of the
        ///     clipboard to the window that currently has the clipboard open.
        /// </summary>
        /// <returns>
        ///     <p><c>true</c> if the function succeeds, <c>false</c> the function fails.</p>
        ///     <p>If the function fail. To get extended error information, call <see cref="GetLastError" />.</p>
        /// </returns>
        /// <remarks>
        ///     Before calling <seealso cref="EmptyClipboard" />, an application must open the clipboard by using the
        ///     <seealso cref="OpenClipboard" /> function. If
        ///     the application specifies a <see langword="null" /> window handle when opening the clipboard,
        ///     <seealso cref="EmptyClipboard" /> succeeds but sets the
        ///     clipboard owner to <see langword="null" />. Note that this causes <seealso cref="SetClipboardData" /> to fail.
        /// </remarks>
        [DllImport(Dlls.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool EmptyClipboard();

        /// <summary>
        ///     Determines whether the clipboard contains data in the specified format.
        /// </summary>
        /// <param name="format">
        ///     <p>A standard or registered clipboard format. For a description of the standard clipboard formats</p>
        ///     <p>Read more : https://msdn.microsoft.com/en-us/library/windows/desktop/ff729168(v=vs.85).aspx </p>
        /// </param>
        /// <returns><c>true</c> if the clipboard format is available; otherwise, <c>false</c>.</returns>
        /// <remarks>
        ///     Typically, an application that recognizes only one clipboard format would call this function when processing
        ///     the <c>WM_INITMENU</c> or <c>WM_INITMENUPOPUP</c> message. The application would then enable or disable the Paste
        ///     menu item,
        ///     depending on the return value. Applications that recognize more than one clipboard format should use the
        ///     <c>GetPriorityClipboardFormat</c> function for this purpose.
        /// </remarks>
        [DllImport(Dlls.User32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsClipboardFormatAvailable(uint format);

        /// <summary>
        ///     Retrieves the calling thread's last-error code value.
        ///     The last-error code is maintained on a per-thread basis. Multiple threads do not overwrite each other's last-error
        ///     code.
        /// </summary>
        /// <returns>
        ///     <p>The return value is the calling thread's last-error code.</p>
        ///     <p>
        ///         The Return Value section of the documentation for each function that sets the last-error code notes the
        ///         conditions under which the function sets the last-error code. Most functions that set the thread's last-error
        ///         code set it when they fail. However, some functions also set the last-error code when they succeed. If the
        ///         function is not documented to set the last-error code, the value returned by this function is simply the most
        ///         recent last-error code to have been set; some functions set the last-error code to 0 on success and others do
        ///         not.
        ///     </p>
        /// </returns>
        /// <remarks>
        ///     <p>
        ///         Functions executed by the calling thread set this value by calling the <c>SetLastError</c> function. You should
        ///         call
        ///         the <see cref="GetLastError" /> function immediately when a function's return value indicates that such a call
        ///         will return
        ///         useful data. That is because some functions call <c>SetLastError</c> with a zero when they succeed, wiping out
        ///         the
        ///         error code set by the most recently failed function.
        ///     </p>
        ///     <p>To obtain an error string for system error codes, use the FormatMessage function.</p>
        ///     <p>
        ///         The error codes returned by a function are not part of the Windows API specification and can vary by operating
        ///         system or device driver. For this reason, we cannot provide the complete list of error codes that can be
        ///         returned by each function. There are also many functions whose documentation does not include even a partial
        ///         list of error codes that can be returned.
        ///     </p>
        ///     <p>
        ///         Error codes are 32-bit values (bit 31 is the most significant bit). Bit 29 is reserved for application-defined
        ///         error codes; no system error code has this bit set. If you are defining an error code for your application, set
        ///         this bit to one. That indicates that the error code has been defined by an application, and ensures that your
        ///         error code does not conflict with any error codes defined by the system.
        ///     </p>
        /// </remarks>
        [DllImport(Dlls.Kernel32)]
        internal static extern uint GetLastError();
    }
}