using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EindToernooi_Poule.Code
{
    public class Match
    {
        private int resulta;
        public int ResultA { get {return resulta ; } set { resulta = value; SetWinner(); } }

        private int resultb;
        public int ResultB { get { return resultb; } set { resultb = value; SetWinner(); } }
        public string Winner { get; set; }
        public int Postponement { get; set; }

        public Match()
        {
            //this parameterless constructor is used for json deserialization. Do not use it for implementations!
        }

        public Match(int resA, int resB, int postponement=0)
        {
            ResultA = resA;
            ResultB = resB;
            Postponement = postponement;
        }

        public void SetWinner()
        {
            if (ResultA > ResultB)
            {
                Winner = "A";
            }

            else if (ResultB > ResultA)
            {
                Winner = "B";
            }

            else
            {
                Winner = "D";
            }
        }

        public int CheckMatch(Match hostmatch)
        {
            var matchScore = 0;

            if (hostmatch.ResultA != 99 && ResultA != 99)
            {
                if (Winner == hostmatch.Winner)
                {
                    matchScore += 15;
                }

                if (ResultA == hostmatch.ResultA)
                {
                    matchScore += 5;
                }

                if (ResultB == hostmatch.ResultB)
                {
                    matchScore += 5;
                }
            }
            return matchScore;
        }

        public virtual string MatchToString()
        { 
            var res = ResultA.ToString() + " - " + ResultB.ToString();
            return res;
        }
    }
}
