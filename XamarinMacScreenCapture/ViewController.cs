using System;
using System.Drawing;
using AppKit;
using Foundation;
using System.Timers;
using ImageIO;
using CoreServices;
using System.IO;

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

        private int mFileNameId = 0;

        private void CaptureInMouse()
        {
            // var pos = NSEvent.CurrentMouseLocation;
            using (var cgScreenImage = ScreenCapture.CreateImage(mScreenDeviceId))
            {
                // 保存到本地
                var fileRootPath = "/Users/match/Documents/Temp/WowMonsterNamePics/monster_name_";
                var filePath = fileRootPath + mFileNameId + ".png";
                while (File.Exists(filePath))
                {
                    mFileNameId++;
                    filePath = fileRootPath + mFileNameId + ".png";
                }

                using (var url = new NSUrl("file", "localhost", filePath))
                using (var destination = CGImageDestination.Create(url, "public.png", 1))
                {
                    destination.AddImage(cgScreenImage);
                    destination.Close();
                }
                
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
