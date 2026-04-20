using VoetbalPoolsBase;
using VoetbalPoolsBase.Interfaces;
using System.Collections.Generic;

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
        public Player(string name, string woonplaats, Dictionary<int, Poule> poules, KnockoutPhase ko, BonusQuestions questions) : base(name, woonplaats, questions)
        {
            Poules = poules;
            KnockoutPhase = ko;
        }

        public void CheckPlayer(IHost Host, Dictionary<string, Topscorer> topscorers)
        {
            var host = Host as Player;
            PoulesScore = 0;
            //reset postponement scores

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
