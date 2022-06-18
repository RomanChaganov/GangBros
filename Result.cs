using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgeGenderDetect
{
    public class Result
    {
        private List<Tuple<string, string>> genderAge;
        private int number;
        private int randomRandomNuber;

        public List<Tuple<string, string>> GenderAge
        {
            get { return genderAge; }
        }

        public Result(List<Tuple<string, string>> currentGenderAge, int iterator)
        {
            genderAge = currentGenderAge;
            Random rnd = new Random(iterator);
            number = rnd.Next();
            rnd = new Random();
            Random rnd1 = new Random(rnd.Next());
            randomRandomNuber = rnd1.Next();
        }

        private Result(List<Tuple<string, string>> currentGenderAge, int number1, int randomRandomNumber1)
        {
            genderAge = currentGenderAge;
            number = number1;
            randomRandomNuber = randomRandomNumber1;
        }

        public Result ToClone()
        {
            return new Result(genderAge, number, randomRandomNuber);
        }

        public bool Equals(Result result)
        {
            if (genderAge == null && result.genderAge == null)
            {
                return true;
            }

            if (this.genderAge.Equals(result.genderAge) && (this.number == result.number) && (this.randomRandomNuber == result.randomRandomNuber))
                return true;
            else
                return false;
        }
    }
}
