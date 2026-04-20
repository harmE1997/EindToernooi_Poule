using System.Collections.Generic;
using VoetbalPoolsBase;

namespace EindToernooi_Poule.Code
{
    public class Player : PlayerBase<BonusQuestions>
    {
        public int PoulesScore { get; set; }
        public int KnockoutScore { get; set; }
        public Dictionary<int, Poule> Poules { get; set; }
        public KnockoutPhase KnockoutPhase { get; set; }

        public Player()
        {
            //this parameterless constructor is used for json deserialization. Do not use it for implementations!
        }
        public Player(string name, string woonplaats, Dictionary<int, Poule> weeks, KnockoutPhase ko, BonusQuestions questions)
        {
            Poules = weeks;
            Name = name;
            Town = woonplaats;
            TotalScore = 0;
            KnockoutPhase = ko;
            Questions = questions;
            RankingDifference = 0;
            Ranking = 0;
        }

        public void CheckPlayer(Player Host, Dictionary<string, int> topscorers)
        {
            PoulesScore = 0;
            //reset postponement scores

            foreach (var poule in Poules)
            {
                if (poule.Value == null)
                    break;

                if (poule.Value.Poulenr > LocalConfiguration.NrPoules)
                    break;

                poule.Value.CheckPoule(Host);
                PoulesScore += poule.Value.PouleMatchesScore;
            }

            KnockoutScore = KnockoutPhase.checkKnockoutPhase(Host.KnockoutPhase);
            BonusScore = Questions.CheckBonus(Host.Questions, topscorers);
            TotalScore = PoulesScore + KnockoutScore + BonusScore;
        }
    }
}
