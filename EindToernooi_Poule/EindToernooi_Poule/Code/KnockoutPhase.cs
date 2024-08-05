using DynamicData;
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
    public class KOMatch : Match
    { 
        public bool AdditionalTime { get; set; }

        //this constructor is needed for deserialization purposes. Dot not actively use it!
        public KOMatch() { }

        public KOMatch(int resA, int resB, bool additionaltime, int postponement = 0) : base(resA, resB, postponement = 0)
        { 
            AdditionalTime = additionaltime;
        }

        public override string MatchToString()
        {
            return base.MatchToString() + ", " + AdditionalTime;
        }
    }

    [Serializable]
    public class Stage
    {
        public int award { get; set; }
        public List<string> teams { get; set; }

        public bool UseMatches { get; set; }
        public KOMatch[] Matches { get; set; }
    }

    [Serializable]
    public class KnockoutPhase
    {
        public Dictionary<KOKeys, Stage> Stages { get; set; }

        public KnockoutPhase()
        {
            Stages = new Dictionary<KOKeys, Stage>()
            {
                {KOKeys.LAST32, new Stage() { award = 30, Matches = new KOMatch[16]
                    {new KOMatch(99,99, false), new KOMatch(99,99, false), new KOMatch(99,99, false), new KOMatch(99,99, false), 
                    new KOMatch(99,99, false), new KOMatch(99,99, false), new KOMatch(99,99, false), new KOMatch(99,99, false),
                    new KOMatch(99,99, false), new KOMatch(99,99, false), new KOMatch(99,99, false), new KOMatch(99,99, false),
                    new KOMatch(99,99, false), new KOMatch(99,99, false), new KOMatch(99,99, false), new KOMatch(99,99, false)}, UseMatches = true } },
                {KOKeys.LAST16, new Stage() { award = 30, teams = new List<string>(){ "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", }, Matches = new KOMatch[8]
                    {new KOMatch(99,99, false), new KOMatch(99,99, false), new KOMatch(99,99, false), new KOMatch(99,99, false),
                    new KOMatch(99,99, false), new KOMatch(99,99, false), new KOMatch(99,99, false), new KOMatch(99,99, false)}, UseMatches = !GeneralConfiguration.Last32 } },
                {KOKeys.QUARTER, new Stage() { award = 55, teams = new List<string>(){ "", "", "", "", "", "", "", "", }, UseMatches = false } },
                {KOKeys.SEMI, new Stage() { award = 110, teams = new List<string>(){ "", "", "", "", }, UseMatches = false } },
                {KOKeys.FINAL, new Stage() { award = 220, teams = new List<string>(){ "", "", }, UseMatches = false } }
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
                Score += CheckStage(stage.Key, stage.Value, KO);
            }

            return Score;
        }

        private int CheckStage(KOKeys stageKey, Stage stage, KnockoutPhase KO)
        {
            int score = 0;
            if (stage.UseMatches)
            {
                foreach (var match in stage.Matches)
                {
                    var hostmatch = KO.Stages[stageKey].Matches[stage.Matches.IndexOf(match)];
                    score += match.CheckMatch(hostmatch);
                    if (match.AdditionalTime == hostmatch.AdditionalTime)
                        score += 5;
                }
            }

            else
            {
                if (GeneralConfiguration.Last32 && stageKey == KOKeys.LAST16)
                    return score;
                if (!GeneralConfiguration.Last32 && stageKey == KOKeys.QUARTER)
                    return score;

                foreach (var team in stage.teams)
                {
                    if (team == "")
                        continue;
                    if (KO.Stages[stageKey].teams.Contains(team))
                    {
                        score += KO.Stages[stageKey].award;
                    }
                }
            }

            return score;
        }
    }
}
