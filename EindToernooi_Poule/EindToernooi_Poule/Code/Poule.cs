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
            Poule hostpoule = host.Poules[Poulenr];
            PouleMatchesScore = 0;            
            for(int counter = 0; counter < Matches.Length; counter++)
            {
                var hostmatch = hostpoule.Matches[counter];
                int matchscore = Matches[counter].CheckMatch(hostmatch);
                PouleMatchesScore += matchscore;

            }
        }
    }
}
