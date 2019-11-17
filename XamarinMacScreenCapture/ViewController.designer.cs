// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace XamarinMacScreenCapture
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        AppKit.NSImageCell ImageView { get; set; }

        [Action ("ClickedCaptureButton:")]
        partial void ClickedCaptureButton (Foundation.NSObject sender);
        
        void ReleaseDesignerOutlets ()
        {
            if (ImageView != null) {
                ImageView.Dispose ();
                ImageView = null;
            }
        }
    }
}
