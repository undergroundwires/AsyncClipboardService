using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using AsyncWindowsClipboard.Clipboard;
using AsyncWindowsClipboard.Native;

namespace AsyncClipboardService.Clipboard
{
    /// <summary>
    ///     A wrapper for native windows methods.
    /// </summary>
    /// <remarks>
    ///     <p>This class is not thread safe and should be consumed in the same thread.</p>
    /// </remarks>
    /// <seealso cref="IWindowsClipboard" />
    /// <seealso cref="IDisposable" />
    public class WindowsClipboard : IWindowsClipboard
    {
        /// <summary>
        ///     A <see cref="bool" /> representing whether this <seealso cref="WindowsClipboard" /> instance has an open
        ///     communication with the windows clipboard.
        /// </summary>
        /// <remarks>
        ///     <p>Use <see cref="Open" /> method to open the communication</p>
        ///     <p>Use <see cref="Close" /> method to close the communication</p>
        /// </remarks>
        /// <seealso cref="Open" />
        /// <seealso cref="Close" />
        public bool IsOpen { get; private set; }

        /// <summary>
        ///     Empties the clipboard and frees handles to data in the clipboard.
        /// </summary>
        /// <returns><c>true</c> if the function succeeds, <c>false</c> the function fails.</returns>
        /// <exception cref="ArgumentException">Throws if clipboard is closed.</exception>
        /// <remarks>
        ///     <p>
        ///         Before calling <seealso cref="Clear" />, an application must open the clipboard by using the
        ///         <seealso cref="Open" /> function in the same thread.
        ///     </p>
        /// </remarks>
        /// <exception cref="ArgumentException">Throws if clipboard is closed.</exception>
        /// <seealso cref="Open" />
        public IClipboardOperationResult Clear()
        {
            ThrowIfNotOpen();
            var result = NativeMethods.EmptyClipboard();
            if (result) return ClipboardOperationResult.SuccesResult;
            return new ClipboardOperationResult(ClipboardOperationResultCode.ErrorClearClipboard);
        }

        /// <summary>
        ///     Calls <see cref="Clear" />
        /// </summary>
        public void Dispose()
        {
            if (IsOpen)
                Close();
        }

        /// <summary>
        ///     Places data on the clipboard in a specified clipboard format.
        /// </summary>
        /// <remarks>
        ///     <p>
        ///         A connection must be made using <see cref="Open" /> method in the same thread.
        ///     </p>
        ///     <p>
        ///         Data might not be set before <see cref="Close" /> or <see cref="Dispose" /> methods are called.
        ///     </p>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="data" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="dataType" /> should be defined in
        ///     <seealso cref="ClipboardDataType" /> enum.
        /// </exception>
        /// <exception cref="ArgumentException">Throws if clipboard is closed.</exception>
        public IClipboardOperationResult SetData(ClipboardDataType dataType, byte[] data)
        {
            ThrowIfNotOpen();
            if (data == null) throw new ArgumentNullException(nameof(data));
            ThrowIfNotInRange(dataType);
            //prepare variable to retrieve clipboard data
            var sizePtr = new UIntPtr((uint) data.Length);

            var dataPtr = NativeMethods.GlobalAlloc(NativeMethods.GHND, sizePtr);
            if (dataPtr == IntPtr.Zero)
            {
                Debug.Assert(false);
                return new ClipboardOperationResult(ClipboardOperationResultCode.ErrorGlobalAlloc);
            }

            Debug.Assert(NativeMethods.GlobalSize(dataPtr).ToUInt64() >= (ulong) data.Length); // Might be larger

            var lockedMemoryPtr = NativeMethods.GlobalLock(dataPtr);
            if (lockedMemoryPtr == IntPtr.Zero)
            {
                Debug.Assert(false);
                NativeMethods.GlobalFree(dataPtr);
                return new ClipboardOperationResult(ClipboardOperationResultCode.ErrorGlobalLock);
            }

            Marshal.Copy(data, 0, lockedMemoryPtr, data.Length);
            NativeMethods.GlobalUnlock(dataPtr); // May return false on success
            //retrieve clipboard data
            var uFormat = (uint) dataType;
            if (NativeMethods.SetClipboardData(uFormat, dataPtr) == IntPtr.Zero)
            {
                Debug.Assert(false);
                NativeMethods.GlobalFree(dataPtr);
                return new ClipboardOperationResult(ClipboardOperationResultCode.ErrorSetClipboardData);
            }
            // SetClipboardData takes ownership of dataPtr upon success.
            dataPtr = IntPtr.Zero;
            return ClipboardOperationResult.SuccesResult;
        }

        /// <summary>
        ///     Retrieves data from the clipboard in a specified format. The clipboard must have been opened previously.
        /// </summary>
        /// <remarks>
        ///     <p>
        ///         A connection must be made using <see cref="Open" /> method in the same thread.
        ///     </p>
        /// </remarks>
        /// <exception cref="ArgumentException">Throws if clipboard is closed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="dataType" /> should be defined in
        ///     <seealso cref="ClipboardDataType" /> enum.
        /// </exception>
        /// <exception cref="Win32Exception">
        ///     <p>If the <c>GlobalSize</c> function of windows api for the size of the clipboard handle fails.</p>
        ///     <p>If the <c>GlobalLock</c> function of windows api for the size of the clipboard handle fails.</p>
        /// </exception>
        public byte[] GetData(ClipboardDataType dataType)
        {
            ThrowIfNotOpen();
            ThrowIfNotInRange(dataType);
            var format = (uint) dataType;

            var dataPtr = NativeMethods.GetClipboardData(format);
            if (dataPtr == IntPtr.Zero) return null;

            var sizePtr = NativeMethods.GlobalSize(dataPtr);
            if (sizePtr == UIntPtr.Zero) throw new Win32Exception(Marshal.GetLastWin32Error());

            var lockedMemoryPtr = NativeMethods.GlobalLock(dataPtr);
            if (lockedMemoryPtr == IntPtr.Zero) throw new Win32Exception(Marshal.GetLastWin32Error());

            var buffer = new byte[sizePtr.ToUInt64()];
            Marshal.Copy(lockedMemoryPtr, buffer, 0, buffer.Length);

            NativeMethods.GlobalUnlock(dataPtr);

            return buffer;
        }

        /// <summary>
        ///     Opens the clipboard for examination and prevents other applications from modifying the clipboard content
        /// </summary>
        /// <remarks>
        ///     The connection will only be usable in the same thread.
        /// </remarks>
        /// <returns><c>true</c> if successful operation or connection is already open, <c>false</c> otherwise.</returns>
        /// <seealso cref="Clear" />
        /// <seealso cref="Close" />
        /// <seealso cref="SetData" />
        public IClipboardOperationResult Open()
        {
            if (IsOpen) return ClipboardOperationResult.SuccesResult;
            var hOwner = NativeMethods.GetConsoleWindow();
            var result = NativeMethods.OpenClipboard(hOwner);
            if (result)
            {
                IsOpen = true;
                return ClipboardOperationResult.SuccesResult;
            }
            return new ClipboardOperationResult(ClipboardOperationResultCode.ErrorOpenClipboard);
        }

        /// <summary>
        ///     Closes the clipboard.
        /// </summary>
        /// <returns><c>true</c> if the function succeeds, <c>false</c> the function fails.</returns>
        /// <exception cref="ArgumentException">Throws if clipboard is closed.</exception>
        /// <remarks>
        ///     <p>
        ///         Before calling <seealso cref="Clear" />, an application must open the clipboard by using the
        ///         <seealso cref="Open" /> function in the same thread.
        ///     </p>
        ///     ///
        ///     <p>
        ///         When the window has finished examining or changing the clipboard, close the clipboard by calling
        ///         <see cref="Close" />. This enables other windows to access the clipboard.
        ///     </p>
        ///     <p>
        ///         Do not place an object using <see cref="SetData" /> on the clipboard after calling
        ///         <see cref="Close" />.
        ///     </p>
        /// </remarks>
        /// <exception cref="ArgumentException">Throws if clipboard is closed.</exception>
        /// <seealso cref="Open" />
        /// <seealso cref="SetData" />
        public IClipboardOperationResult Close()
        {
            ThrowIfNotOpen();
            var result = NativeMethods.CloseClipboard();
            if (result)
            {
                IsOpen = false;
                return ClipboardOperationResult.SuccesResult;
            }
            return new ClipboardOperationResult(ClipboardOperationResultCode.ErrorCloseClipboard);
        }

        /// <summary>
        ///     Determines whether the content of the clipboard is <paramref name="dataType" />.
        /// </summary>
        /// <param name="dataType">Clipboard data format.</param>
        /// <returns><c>true</c> if content of the clipboard data is <paramref name="dataType" />, otherwise; <c>false</c>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="dataType" /> should be defined in
        ///     <seealso cref="ClipboardDataType" /> enum.
        /// </exception>
        /// <seealso cref="ClipboardDataType" />
        public bool IsContentTypeOf(ClipboardDataType dataType)
        {
            ThrowIfNotInRange(dataType);
            var format = (uint) dataType;
            var result = NativeMethods.IsClipboardFormatAvailable(format);
            return result;
        }

        /// <exception cref="ArgumentException">Throws if clipboard is closed.</exception>
        private void ThrowIfNotOpen()
        {
            if (!IsOpen)
                throw new ArgumentException($"Clipboard is closed. Open a connection using {nameof(Open)}");
        }

        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="dataType" /> should be defined in
        ///     <seealso cref="ClipboardDataType" /> enum.
        /// </exception>
        private static void ThrowIfNotInRange(ClipboardDataType dataType)
        {
            if (!Enum.IsDefined(typeof(ClipboardDataType), dataType))
                throw new ArgumentOutOfRangeException(nameof(dataType),
                    $"Value must be defined in the {nameof(ClipboardDataType)} enum.");
        }
    }
}