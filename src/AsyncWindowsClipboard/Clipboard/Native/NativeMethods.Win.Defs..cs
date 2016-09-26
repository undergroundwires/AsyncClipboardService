using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncWindowsClipboard.Native
{
    internal static partial class NativeMethods
    {
        //https://msdn.microsoft.com/en-us/library/windows/desktop/ff729168%28v=vs.85%29.aspx?f=255&MSPPError=-2147217396
        internal const uint CF_TEXT = 1;
        internal const uint CF_UNICODETEXT = 13;
        /// <summary>
        /// Combines GMEM_MOVEABLE and GMEM_ZEROINIT.
        /// GMEM_MOVEABLE : Allocates movable memory. Memory blocks are never moved in physical memory, but they can be moved within the default heap.
        ///                 The return value is a handle to the memory object. To translate the handle into a pointer, use the GlobalLock function.
        ///                 This value cannot be combined with GMEM_FIXED.
        /// GMEM_ZEROINIT : Initializes memory contents to zero.
        /// </summary>
        /// <remarks>https://msdn.microsoft.com/en-us/library/windows/desktop/aa366574%28v=vs.85%29.aspx</remarks>
        internal const uint GHND = 0x0042;
    }
}
