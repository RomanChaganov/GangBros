using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Emgu.CV;
using Emgu.CV.Structure;
using AgeGenderDetect;
using System.Windows.Forms;

namespace AgeGenderDetect
{
    public static class VideoStart
    {
        private static VideoCapture capture = null;
        private static Mat frame = new Mat();
        private static Image<Bgr, byte> currentFrame;
        private static StartForm startForm;

        public static bool IsEnable
        {
            get;
            set;
        }

        public static IAsyncResult Result
        {
            get;
            set;
        }

        public delegate void View();
        public static View ViewVideo;

        public static void StartVideo(int index, StartForm start)
        {
            startForm = start;

            Action action = new Action(() =>
            {
                capture = new VideoCapture(index);
                capture.ImageGrabbed += Capture_ImageGrabbed;
                capture.Start();
                IsEnable = true;
            });
            
            Result = action.BeginInvoke(null, null);
        }

        public static void StartAfterPause()
        {
            capture.Start();
        }

        public static void PauseVideo()
        {
            if (capture != null)
                capture.Pause();
        }

        public static void StopVideo()
        {
            if (capture != null)
            {
                capture.Pause();
                capture.Dispose();
                capture = null;
            }
            IsEnable = false;
        }

        private static void Capture_ImageGrabbed(object sender, EventArgs e)
        {
            capture.Retrieve(frame);
            currentFrame = frame.ToImage<Bgr, byte>();
            PublicData.GenderAge = AnalyzeVideo.AnalyzingVideo(currentFrame);
            if (!startForm.ViewButtonEnable)
            {
                PublicData.Image = currentFrame;
                ViewVideo?.Invoke();
            }
        }

        public static void VideoStart_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (capture != null)
            {
                capture.Stop();
                capture.Dispose();
            }
        }
    }
}
