using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emgu.CV;
using Emgu.CV.Structure;

namespace AgeGenderDetect
{
    public static class PublicData
    {
        public readonly static List<string> ageList = new List<string> { "0-2 года", "4-6 лет", "8-12 лет", "15-20 лет", "25-32 года",
            "38-43 года", "48-53 года", "60-100 лет" };

        private static Result genderAge;
        public static Result GenderAge
        {
            get { return genderAge; }
            set
            {
                if (value != null)
                    genderAge = value;
            }
        }

        private static Image<Bgr, byte> image;
        public static Image<Bgr, byte> Image
        {
            get { return image; }
            set
            {
                if (value != null)
                    image = value;
            }
        }
    }
}
