using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AsyncWindowsClipboard.Clipboard;
using AsyncWindowsClipboard.Helpers;
using AsyncWindowsClipboard.Native;

namespace AsyncWindowsClipboard
{
    /// <summary>
    /// A wrapper for native windows methods
    /// </summary>
    /// <seealso cref="IWindowsClipboard" />
    /// <seealso cref="IDisposable" />
    internal class WindowsClipboard : IWindowsClipboard
    {
        /// <summary>
        /// A <see cref="bool"/> representing whether this <seealso cref="WindowsClipboard"/> instance has an open communication with the windows clipboard.
        /// </summary>
        private volatile bool _isOpen;
        /// <summary>
        ///  Opens the clipboard for examination and prevents other applications from modifying the clipboard content
        /// </summary>
        /// <returns><c>true</c> if successful operation, <c>false</c> otherwise.</returns>
        /// <seealso cref="ClearAsync"/>
        /// <seealso cref="CloseAsync"/>
        /// <seealso cref="SetDataAsync"/>
        public Task<bool> OpenAsync()
        {
            return TaskHelper.StartStaTask(() =>
            {
                var hOwner = NativeMethods.GetConsoleWindow();
                var result = NativeMethods.OpenClipboard(hOwner);
                if (result) _isOpen = true;
                return result;
            });
        }
        /// <summary>
        /// Closes the clipboard.
        /// </summary>
        /// <returns><c>true</c> if the function succeeds, <c>false</c> the function fails.</returns>
        /// <exception cref="ArgumentException">Throws if clipboard is closed.</exception>
        /// <remarks>
        ///     <p>Before calling <seealso cref="ClearAsync" />, an application must open the clipboard by using the
        ///     <seealso cref="OpenAsync" /> function.</p>
        ///         ///     <p>
        ///         When the window has finished examining or changing the clipboard, close the clipboard by calling
        ///         <see cref="CloseAsync" />. This enables other windows to access the clipboard.
        ///     </p>
        ///     <p>Do not place an object using <see cref="SetDataAsync"/> on the clipboard after calling <see cref="CloseAsync" />.</p>
        /// </remarks>
        /// <exception cref="ArgumentException">Throws if clipboard is closed.</exception>
        /// <seealso cref="OpenAsync"/>
        /// <seealso cref="SetDataAsync"/>
        public Task<bool> CloseAsync()
        {
            ThrowIfNotOpen();
            return TaskHelper.StartStaTask(() =>
            {
                var result = NativeMethods.CloseClipboard();
                return result;
            });
        }
        /// <summary>
        /// Empties the clipboard and frees handles to data in the clipboard.
        /// </summary>
        /// <returns><c>true</c> if the function succeeds, <c>false</c> the function fails.</returns>
        /// <exception cref="ArgumentException">Throws if clipboard is closed.</exception>
        /// <remarks>
        ///     Before calling <seealso cref="ClearAsync" />, an application must open the clipboard by using the
        ///     <seealso cref="OpenAsync" /> function.
        /// </remarks>
        /// <exception cref="ArgumentException">Throws if clipboard is closed.</exception>
        /// <seealso cref="OpenAsync"/>
        public Task<bool> ClearAsync()
        {
            ThrowIfNotOpen();
            return TaskHelper.StartStaTask(() =>
            {
                var result = NativeMethods.EmptyClipboard();
                return result;
            });
        }
        /// <summary>
        /// Determines whether the content of the clipboard is <paramref name="dataType"/>.
        /// </summary>
        /// <param name="dataType">Clipboard data format.</param>
        /// <returns><c>true</c> if content of the clipboard data is <paramref name="dataType"/>, otherwise; <c>false</c>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="dataType" /> should be defined in <seealso cref="ClipboardDataType"/> enum.</exception>
        /// <seealso cref="ClipboardDataType"/>
        public Task<bool> IsContentTypeOf(ClipboardDataType dataType)
        {
            ThrowIfNotInRange(dataType);
            return TaskHelper.StartStaTask(() =>
            {
                var result = NativeMethods.IsClipboardFormatAvailable((uint) dataType);
                return result;
            });
        }

        /// <exception cref="ArgumentNullException"><paramref name="data" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="dataType" /> should be defined in <seealso cref="ClipboardDataType"/> enum.</exception>
        /// <exception cref="ArgumentException">Throws if clipboard is closed.</exception>
        public Task<bool> SetDataAsync(ClipboardDataType dataType, byte[] data)
        {
            ThrowIfNotOpen();
            if (data == null) throw new ArgumentNullException(nameof(data));
            ThrowIfNotInRange(dataType);
            return TaskHelper.StartStaTask(() =>
            {
                //prepare variable to retrieve clipboard data
                var sizePtr = new UIntPtr((uint)data.Length);

                var dataPtr = NativeMethods.GlobalAlloc(NativeMethods.GHND, sizePtr);
                if (dataPtr == IntPtr.Zero)
                {
                    Debug.Assert(false);
                    return false;
                }

                Debug.Assert(NativeMethods.GlobalSize(dataPtr).ToUInt64() >= (ulong)data.Length); // Might be larger

                var lockedMemoryPtr = NativeMethods.GlobalLock(dataPtr);
                if (lockedMemoryPtr == IntPtr.Zero)
                {
                    Debug.Assert(false);
                    NativeMethods.GlobalFree(dataPtr);
                    return false;
                }

                Marshal.Copy(data, 0, lockedMemoryPtr, data.Length);
                NativeMethods.GlobalUnlock(dataPtr); // May return false on success
                //retrieve clipboard data
                var uFormat = (uint) dataType;
                if (NativeMethods.SetClipboardData(uFormat, dataPtr) == IntPtr.Zero)
                {
                    Debug.Assert(false);
                    NativeMethods.GlobalFree(dataPtr);
                    return false;
                }
                // SetClipboardData takes ownership of dataPtr upon success.
                dataPtr = IntPtr.Zero;
                return true;
            });
        }

        /// <exception cref="ArgumentException">Throws if clipboard is closed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="dataType" /> should be defined in
        ///     <seealso cref="ClipboardDataType" /> enum.
        /// </exception>
        /// <exception cref="Win32Exception" />
        public Task<byte[]> GetDataAsync(ClipboardDataType dataType)
        {
            ThrowIfNotOpen();
            ThrowIfNotInRange(dataType);
            return TaskHelper.StartStaTask(() =>
            {
                var dataPtr = NativeMethods.GetClipboardData((uint) dataType);
                if (dataPtr == IntPtr.Zero) return null;

                var sizePtr = NativeMethods.GlobalSize(dataPtr);
                if (sizePtr == UIntPtr.Zero) throw new Win32Exception(Marshal.GetLastWin32Error());

                var lockedMemoryPtr = NativeMethods.GlobalLock(dataPtr);
                if (lockedMemoryPtr == IntPtr.Zero) throw new Win32Exception(Marshal.GetLastWin32Error());

                var buffer = new byte[sizePtr.ToUInt64()];
                Marshal.Copy(lockedMemoryPtr, buffer, 0, buffer.Length);

                NativeMethods.GlobalUnlock(dataPtr);

                return buffer;
            });
        }
        /// <summary>
        /// Calls <see cref="ClearAsync"/>
        /// </summary>
        public void Dispose()
        {
            if (_isOpen)
            {
                CloseAsync();
            }
        }
        /// <exception cref="ArgumentException">Throws if clipboard is closed.</exception>
        private void ThrowIfNotOpen()
        {
            if (!_isOpen) throw new ArgumentException($"Clipboard is closed. Open a connection using {nameof(OpenAsync)}");
        }
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="dataType" /> should be defined in <seealso cref="ClipboardDataType"/> enum.</exception>
        private static void ThrowIfNotInRange(ClipboardDataType dataType)
        {
            if (!Enum.IsDefined(typeof(ClipboardDataType), dataType))
                throw new ArgumentOutOfRangeException(nameof(dataType), $"Value must be defined in the {nameof(ClipboardDataType)} enum.");
        }
    }
}
