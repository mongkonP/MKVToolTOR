
// ************************************************************************************************************
// Filename:    UnsafeNativeMethods.cs
// Description: Potentially dangerous P/Invoke method declarations; be careful not to expose these publicly.
// Author:      Hiske Bekkering
// License:     Code Project Open License (CPOL)
// History:     March 1, 2016 - Initial Release
// ************************************************************************************************************

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TORServices.FormsTor
{

    [SuppressUnmanagedCodeSecurity]
    internal static class UnsafeNativeMethods
    {

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
        internal static extern IntPtr BeginPaint(HandleRef hWnd, [In][Out] ref PAINTSTRUCT lpPaint);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
        internal static extern bool EndPaint(HandleRef hWnd, ref PAINTSTRUCT lpPaint);

    }

}
