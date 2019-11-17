using System;
using System.Drawing;
using AppKit;
using Foundation;
using System.Timers;
using ImageIO;
using CoreServices;
using System.IO;
using System.Threading;
using CoreGraphics;

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

        private NSTimer mUpdateTimer;

        private System.Timers.Timer mCaptureTimer;

        partial void ClickedCaptureButton(NSObject sender)
        {
            if (mUpdateTimer != null)
            {
                mUpdateTimer.Dispose();
                return;
            }

            mMainThreadId = Thread.CurrentThread.ManagedThreadId;
            var id1 = (NSNumber)NSScreen.MainScreen.DeviceDescription["NSScreenNumber"];
            mScreenDeviceId = id1.UInt32Value;

            if (mUpdateTimer == null)
            {
                mUpdateTimer = NSTimer.CreateRepeatingTimer(0.3f, (timer) => CaptureInMouse());
                NSRunLoop.Current.AddTimer(mUpdateTimer, NSRunLoopMode.Default);
            }


        }

        private void MCaptureTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CaptureInMouse();
        }

        private int mFileNameId = 0;
        private int mMainThreadId = -1;

        private CGPoint mPreCursorPos;

        private void CaptureInMouse()
        {
            if((NSEvent.CurrentMouseLocation.X- mPreCursorPos.X)* (NSEvent.CurrentMouseLocation.X - mPreCursorPos.X) +
                (NSEvent.CurrentMouseLocation.Y - mPreCursorPos.Y) * (NSEvent.CurrentMouseLocation.Y - mPreCursorPos.Y)
                < 0.1f*0.1f)
            {
                return;
            }

            mPreCursorPos = NSEvent.CurrentMouseLocation;

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

                if (Thread.CurrentThread.ManagedThreadId != mMainThreadId)
                    return; // 不在主线程,所以不能更新下面的界面显示
                                
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
