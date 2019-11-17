using System;
using System.Drawing;
using AppKit;
using Foundation;
using System.Timers;

namespace XamarinMacScreenCapture
{
    public partial class ViewController : NSViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
            }
        }

        private uint mScreenDeviceId;

        private NSTimer mCaptureTimer;

        partial void ClickedCaptureButton(NSObject sender)
        {
            var id1 = (NSNumber)NSScreen.MainScreen.DeviceDescription["NSScreenNumber"];
            mScreenDeviceId = id1.UInt32Value;

            if (mCaptureTimer != null)
                mCaptureTimer.Dispose();

            mCaptureTimer = NSTimer.CreateRepeatingTimer(0.5f, (timer)=>CaptureInMouse());
            NSRunLoop.Current.AddTimer(mCaptureTimer, NSRunLoopMode.Default);
        }

        private void MCaptureTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CaptureInMouse();
        }

        private void CaptureInMouse()
        {
            // var pos = NSEvent.CurrentMouseLocation;
            using (var cgScreenImage = ScreenCapture.CreateImage(mScreenDeviceId))
            {
                // var rect = new CoreGraphics.CGRect(pos.X / 1920f * 3840f, (1f - pos.Y / 1080) * 2160f, 100f / 1920.0f * 3840, 100f / 1080f * 2160f);
                //using (var cgImage = cgScreenImage.WithImageInRect(rect))
                var cgImage = cgScreenImage;
                using (var nsImage = new NSImage(cgImage, new SizeF(cgImage.Width, cgImage.Height)))
                {
                    ImageView.Image = nsImage;
                }
            }
        }
    }
}
