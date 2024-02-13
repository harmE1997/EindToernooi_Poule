using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EindToernooi_Poule.Code
{
    public class Poule
    {
        public int Poulenr { get; set; }
        public int PouleMatchesScore { get; set; }
        public int PouleTotalScore { get; set; }
        public Match[] Matches { get; set; }

        public Poule()
        {
            //this parameterless constructor is used for json deserialization. Do not use it for implementations!
        }

        public Poule(int nr, Match[] matches)
        {
            Matches = matches;
            Poulenr = nr;
            PouleMatchesScore = 0;
        }

        public void CheckPoule(Player host)
        {
            Poule hostweek = host.Poules[Poulenr];
            PouleMatchesScore = 0;            
            Dictionary<int, int> postponementscores = new Dictionary<int, int>();
            for(int counter = 0; counter < Matches.Length; counter++)
            {
                var hostmatch = hostweek.Matches[counter];
                int matchscore = Matches[counter].CheckMatch(hostmatch);
                PouleMatchesScore += matchscore;

            }
        }

        public int CheckMatchOnResultOnly(Match[] Host, int matchID)
        {
            //MOTW has matchID 0.
            Match HostMatch = Host[matchID];
            Match ThisMatch = Matches[matchID];

            if (HostMatch.ResultA != 99 && ThisMatch.ResultA != 99)
            {
                if (HostMatch.ResultA == ThisMatch.ResultA && ThisMatch.ResultB == HostMatch.ResultB)
                {
                    return 2;
                }

                else if (ThisMatch.Winner == HostMatch.Winner)
                {
                    return 1;
                }

                else
                {
                    return 0;
                }
            }

            return -1;
        }
    }
}
