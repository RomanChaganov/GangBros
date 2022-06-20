using AgeGenderDetect;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Dnn;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeGenderDetect
{
    public static class AnalyzeVideo
    {
        private static Mat grayImage = null;
        private static Image<Bgr, byte> face;
        private static int Padding = 10;
        private static readonly MCvScalar meanValue = new MCvScalar(78.4263377603, 87.7689143744, 114.895847746);
        private static CascadeClassifier facesClassifier = new CascadeClassifier("Models/haarcascade_frontalface_alt.xml");
        private static Net ageNet = DnnInvoke.ReadNetFromCaffe("Models/age_deploy.prototxt", "Models/age_net.caffemodel");
        private static Net genderNet = DnnInvoke.ReadNetFromCaffe("Models/gender_deploy.prototxt", "Models/gender_net.caffemodel");
        private static readonly List<string> genderList = new List<string> { "Мужчин", "Женщин" };
        private static List<Tuple<string, string>> ageGenderTuple;
        private static Tuple<string, string> genderAge;
        private static readonly Size size = new Size(227, 227);
        private static int iterator = 1;

        public static Result AnalyzingVideo(Image<Bgr, byte> currentFrame)
        {
            grayImage = new Mat();
            CvInvoke.CvtColor(currentFrame, grayImage, ColorConversion.Bgr2Gray);
            CvInvoke.EqualizeHist(grayImage, grayImage);
            ageGenderTuple = new List<Tuple<string, string>>();

            Rectangle[] faces = facesClassifier.DetectMultiScale(grayImage, 1.1, 3, Size.Empty, Size.Empty);
            if (faces.Length > 0)
            {
                foreach(var face in faces)
                {
                    genderAge = AnalyzingPhoto(face, currentFrame);

                    CvInvoke.Rectangle(currentFrame, face, new Bgr(Color.Purple).MCvScalar, 2);
                    CvInvoke.PutText(currentFrame, $"{genderAge.Item1}а,{genderAge.Item2}", new Point(face.X - 10, face.Bottom + 30), 
                        FontFace.HersheyComplex, 1, new MCvScalar(255, 0, 0), 1);

                    ageGenderTuple.Add(genderAge);
                }

                if (iterator >= 10000)
                    iterator = 1;
                else
                    iterator++;
                return new Result(ageGenderTuple, iterator);
            }

            return null;
        }

        private static Tuple<string, string> AnalyzingPhoto(Rectangle rectangle, Image<Bgr, byte> currentFrame)
        {
            var x1 = rectangle.X - Padding;
            var y1 = rectangle.Y - Padding;
            var width = rectangle.Width + Padding * 3;
            var height = rectangle.Height + Padding * 3;

            Rectangle rect = new Rectangle(x1, y1, width, height);
            face = currentFrame.Clone();
            face.ROI = rect;

            Mat blobAgeGender = DnnInvoke.BlobFromImage(face, 1.1, size, meanValue, false);
            var gender = GenderDetection(blobAgeGender);
            var age = AgeDetection(blobAgeGender);

            return new Tuple<string, string>(gender, age);
        }

        private static string AgeDetection(Mat blobAgeGender)
        {
            ageNet.SetInput(blobAgeGender);
            Mat agePreds = ageNet.Forward();
            int classId = 0;
            double classProb = 0;
            GetMaxClass(ref agePreds, ref classId, ref classProb);
            return PublicData.ageList[classId];
        }

        private static string GenderDetection(Mat blobAgeGender)
        {
            genderNet.SetInput(blobAgeGender);
            Mat genderPreds = genderNet.Forward();
            int classId = 0;
            double classProb = 0;
            GetMaxClass(ref genderPreds, ref classId, ref classProb);
            return genderList[classId];
        }

        static void GetMaxClass(ref Mat probBlob, ref int classId, ref double classProb)
        {
            Mat probMat = probBlob.Reshape(1, 1); //reshape the blob to 1x1000 matrix
            Point classNumber = new Point();

            var tmp = new Point();
            double tmpdouble = 0;
            CvInvoke.MinMaxLoc(probMat, ref tmpdouble, ref classProb, ref tmp, ref classNumber);

            classId = classNumber.X;
        }
    }
}
