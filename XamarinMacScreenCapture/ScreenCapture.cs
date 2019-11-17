using System;
using System.Runtime.InteropServices;
using AppKit;
using CoreGraphics;

namespace XamarinMacScreenCapture
{
    public static class ScreenCapture
    {
        public const string DllName = "/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics";

        [DllImport(DllName)]
        extern static IntPtr CGDisplayCreateImage(UInt32 displayId);
        
        [DllImport(DllName)]
        extern static IntPtr CGDisplayCreateImageForRect(UInt32 display, CGRect rect);

        [DllImport(DllName)]
        extern static void CFRelease(IntPtr handle);

        public static CGImage CreateImage(UInt32 displayId)
        {
            IntPtr handle = IntPtr.Zero;

            try
            {
                // handle = CGDisplayCreateImage(displayId);
                // 鼠标位置
                var mousePos = NSEvent.CurrentMouseLocation;
                const float width = 100f;
                const float height = 100f;
                var rect = new CoreGraphics.CGRect(mousePos.X - width*0.5f, 1080 - (mousePos.Y + height*0.5f), width, height);

                handle = CGDisplayCreateImageForRect(displayId, rect);
                return new CGImage(handle);
            }
            finally
            {
                if (handle != IntPtr.Zero)
                {
                    CFRelease(handle);
                }
            }
        }
    }
}
