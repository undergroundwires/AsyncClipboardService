using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using AsyncWindowsClipboard.Clipboard.Exceptions;
using AsyncWindowsClipboard.Clipboard.Native;
using AsyncWindowsClipboard.Clipboard.Result;

namespace AsyncWindowsClipboard.Clipboard
{
    /// <summary>
    ///     A wrapper for native windows methods. It should be used to communicate with the native methods.
    /// </summary>
    /// <remarks>
    ///     <p>This class is not thread safe and should be consumed in the same thread.</p>
    ///     <p>Calls <see cref="Close" /> when being on <see cref="IDisposable.Dispose()" />.</p>
    /// </remarks>
    /// <seealso cref="IWindowsClipboardSession" />
    /// <seealso cref="IDisposable" />
    internal class WindowsClipboardSession : IWindowsClipboardSession, IDisposable
    {
        /// <summary>
        ///     Calls <see cref="Clear" />
        /// </summary>
        /// <exception cref="T:System.ArgumentException">Throws if clipboard is closed.</exception>
        public void Dispose()
        {
            if (IsOpen)
                Close();
        }

        /// <inheritdoc />
        public bool IsOpen { get; private set; }

        /// <inheritdoc />
        /// <remarks>
        ///     <p>
        ///         Before calling <seealso cref="Clear" />, an application must open the clipboard by using the
        ///         <seealso cref="Open" /> function in the same thread.
        ///     </p>
        /// </remarks>
        /// <exception cref="ArgumentException">Throws if clipboard is closed.</exception>
        public IClipboardOperationResult Clear()
        {
            ThrowIfNotOpen();
            var result = NativeMethods.EmptyClipboard();
            return result
                ? ClipboardOperationResult.SuccessResult
                : new ClipboardOperationResult(ClipboardOperationResultCode.ErrorClearClipboard);
        }

        /// <inheritdoc />
        /// <remarks>
        ///     <p>A connection must be made using <see cref="Open" /> method in the same thread.</p>
        ///     <p>Data might not be set before <see cref="Close" /> or <see cref="Dispose" /> methods are called.</p>
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

            var result = TryCreateNativeBytes(data, out var nativeBytes);
            if (result != ClipboardOperationResultCode.Success)
                return new ClipboardOperationResult(result);

            // Set clipboard data
            if (NativeMethods.SetClipboardData((uint) dataType, nativeBytes) == IntPtr.Zero)
            {
                NativeMethods.GlobalFree(nativeBytes);
                return new ClipboardOperationResult(ClipboardOperationResultCode.ErrorSetClipboardData);
            }
            // ReSharper disable once RedundantAssignment
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            nativeBytes = IntPtr.Zero; // SetClipboardData takes ownership of dataPtr upon success 
#pragma warning restore IDE0059 // Unnecessary assignment of a value
            return ClipboardOperationResult.SuccessResult;
        }


        /// <inheritdoc />
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
        /// <exception cref="ClipboardWindowsApiException">
        ///     <p>If the <c>GlobalSize</c> function of windows api for the size of the clipboard handle fails.</p>
        ///     <p>If the <c>GlobalLock</c> function of windows api for the size of the clipboard handle fails.</p>
        /// </exception>
        public byte[] GetData(ClipboardDataType dataType)
        {
            ThrowIfNotOpen();
            ThrowIfNotInRange(dataType);

            var dataPtr = NativeMethods.GetClipboardData((uint) dataType);
            if (dataPtr == IntPtr.Zero) return null;

            var buffer = GetManagedBuffer(dataPtr);
            Marshal.Copy(dataPtr, buffer, 0, buffer.Length);

            return buffer;
        }


        /// <inheritdoc />
        public IClipboardOperationResult Open()
        {
            if (IsOpen) return ClipboardOperationResult.SuccessResult;
            // OpenClipboard expects a window pointer or a NULL pointer.
            //  - However when we send NULL pointer is associates the clipboard connection with the current thread.
            //  - Only one thread can access a shared resource at one time and this library ensures that we always run in a single thread.
            //  - We lose connection if Window become invalid, however if we send null PTR we keep the connection as thread is active.
            // The decision is to go with the NULL pointer.
            var result = NativeMethods.OpenClipboard(IntPtr.Zero);
            if (!result)
                return new ClipboardOperationResult(ClipboardOperationResultCode.ErrorOpenClipboard);
            IsOpen = true;
            return ClipboardOperationResult.SuccessResult;
        }

        /// <inheritdoc />
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
            if (!result)
                return new ClipboardOperationResult(ClipboardOperationResultCode.ErrorCloseClipboard);
            IsOpen = false;
            return ClipboardOperationResult.SuccessResult;
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

        private static ClipboardOperationResultCode TryCreateNativeBytes(byte[] data, out IntPtr bytesPointer)
        {
            var bufferResult = TryCreateNativeBuffer(data.Length, out bytesPointer);
            if (bufferResult != ClipboardOperationResultCode.Success)
                return bufferResult;
            var copyResult = TryCopyToNative(data, bytesPointer);
            return copyResult;
        }

        private static ClipboardOperationResultCode TryCreateNativeBuffer(int length, out IntPtr dataPtr)
        {
            var sizePtr = new UIntPtr((uint) length);
            dataPtr = NativeMethods.GlobalAlloc(NativeMethods.GHND, sizePtr);
            if (dataPtr == IntPtr.Zero)
                return ClipboardOperationResultCode.ErrorGlobalAlloc;
            Debug.Assert(NativeMethods.GlobalSize(dataPtr).ToUInt64() >= (ulong) length); // Might be larger
            return ClipboardOperationResultCode.Success;
        }

        private static ClipboardOperationResultCode TryCopyToNative(byte[] source, IntPtr destination)
        {
            var lockedMemoryPtr = NativeMethods.GlobalLock(destination);
            if (lockedMemoryPtr == IntPtr.Zero)
            {
                NativeMethods.GlobalFree(destination);
                return ClipboardOperationResultCode.ErrorGlobalLock;
            }

            Marshal.Copy(source, 0, lockedMemoryPtr, source.Length);
            NativeMethods.GlobalUnlock(lockedMemoryPtr); // May return false on success
            return ClipboardOperationResultCode.Success;
        }

        private static byte[] GetManagedBuffer(IntPtr nativeBytesPtr)
        {
            if (nativeBytesPtr == IntPtr.Zero)
                throw new ClipboardTimeoutException($"{nameof(nativeBytesPtr)} is zero");
            var sizePtr = NativeMethods.GlobalSize(nativeBytesPtr);
            if (sizePtr == UIntPtr.Zero)
                throw new ClipboardWindowsApiException(NativeMethods.GetLastError());
            var buffer = new byte[sizePtr.ToUInt64()];
            return buffer;
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