using System.Collections.Generic;
using VoetbalPoolsBase;
using VoetbalPoolsBase.Interfaces;

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

        public override void CheckPlayer(IHost Host, Dictionary<string, Topscorer> topscorers, int startWeek, int endWeek, bool periodCalculation)
        {
            PoulesScore = 0;
            var host = Host as Player;

            foreach (var poule in Poules)
            {
                if (poule.Value == null)
                    break;

                if (poule.Value.Poulenr > LocalConfiguration.NrPoules)
                    break;

                poule.Value.CheckPoule(host);
                PoulesScore += poule.Value.PouleMatchesScore;
            }

            KnockoutScore = KnockoutPhase.checkKnockoutPhase(host.KnockoutPhase);
            BonusScore = Questions.CheckBonus(host.Questions, topscorers);
            TotalScore = PoulesScore + KnockoutScore + BonusScore;
        }
    }
}
