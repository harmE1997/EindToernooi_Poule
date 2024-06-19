using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EindToernooi_Poule.Code
{
    public enum KOKeys
    {
        LAST32,
        LAST16,
        QUARTER,
        SEMI,
        FINAL,
        DEFAULT
    }

    [Serializable]
    public class Stage
    {
        public int award { get; set; }
        public List<string> teams { get; set; }
    }

    [Serializable]
    public class KnockoutPhase
    {
        public Dictionary<KOKeys, Stage> Stages { get; set; }

        public KnockoutPhase()
        {
            Stages = new Dictionary<KOKeys, Stage>()
            {
                {KOKeys.LAST32, new Stage() { award = 15, teams = new List<string>(){"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" } } },
                {KOKeys.LAST16, new Stage() { award = 15, teams = new List<string>(){ "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", } } },
                {KOKeys.QUARTER, new Stage() { award = 15, teams = new List<string>(){ "", "", "", "", "", "", "", "", } } },
                {KOKeys.SEMI, new Stage() { award = 15, teams = new List<string>(){ "", "", "", "", } } },
                {KOKeys.FINAL, new Stage() { award = 15, teams = new List<string>(){ "", "", } } }
            };
        }

        public int checkKnockoutPhase(KnockoutPhase KO)
        {
            if (KO == null)
            {
                return 0;
            }

            int Score = 0;

            foreach (var stage in Stages)
            {
                foreach (var team in stage.Value.teams)
                {
                    if (team == "")
                        continue;
                    if (KO.Stages[stage.Key].teams.Contains(team))
                    {
                        Score += stage.Value.award;
                    }
                }
            }

            return Score;
        }
    }
}
