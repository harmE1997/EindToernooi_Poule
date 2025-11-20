using PoolsBase;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EindToernooi_Poule.Code
{
    [Serializable]
    public enum BonusKeys
    {
        GroupWinners,
        GroupRunnerups,
        Kampioen,
        Nederland,
        Topscorer,
        Bronze,
        Default
    }

    public class BonusQuestions
    {
        public Dictionary<BonusKeys, BonusQuestion> Answers { get; set; }

        public BonusQuestions()
        {
            //this parameterless constructor is used for json deserialization. Do not use it for implementations!
        }

        public BonusQuestions(Dictionary<string, int> rawanswers)
        {
            var answers = new List<string>();
            for (int i = 0; i < 28; i++)
                answers.Add(string.Empty);

            if (rawanswers.Count == 28)
                answers = rawanswers.Keys.ToList();

            Answers = new Dictionary<BonusKeys, BonusQuestion>()
            {
                {BonusKeys.GroupWinners, new BonusQuestion(){Answer = new string[]{answers[0], answers[2], answers[4], answers[6], answers[8],answers[10],answers[12],answers[14],answers[16],answers[18],answers[20],answers[22] }, Points=15 } },
                {BonusKeys.GroupRunnerups, new BonusQuestion(){Answer = new string[]{answers[1], answers[3], answers[5], answers[7], answers[9],answers[11],answers[13],answers[15],answers[17],answers[19],answers[21],answers[23] }, Points=15 } },
                {BonusKeys.Kampioen, new BonusQuestion(){Answer = new string[] {answers[24] }, Points = 50 } },
                {BonusKeys.Topscorer, new BonusQuestion(){Answer = new string[] {answers[25] }, Points = 5} },
                {BonusKeys.Nederland, new BonusQuestion(){Answer = new string[] {answers[26] }, Points = 25 } },
                {BonusKeys.Bronze, new BonusQuestion(){Answer = new string[] {answers[27] }, Points = 25 } },
            };
        }

        public int CheckBonus(BonusQuestions HostQuestions, Dictionary<string, Topscorer> topscorers)
        {
            if (HostQuestions == null)
            {
                throw new ArgumentNullException("hostquestions");
            }
            int Score = 0;

            //check all questions except topscorers
            foreach (var a in Answers)
            {
                var ans = HostQuestions.Answers[a.Key];
                if (a.Key == BonusKeys.Bronze && !LocalConfiguration.Bronze)
                    continue;
                if (a.Key == BonusKeys.Nederland && !LocalConfiguration.NlPresent)
                    continue;
                for (int i = 0; i < ans.Answer.Length; i++)
                {
                    if (a.Value.Answer.Contains(ans.Answer[i]) && !string.IsNullOrEmpty(ans.Answer[i]))
                    {
                        Score += a.Value.Points;
                    }
                }
            }

            //check the topscorers
            var topscorerkey = Answers[BonusKeys.Topscorer].Answer[0];
            if (!topscorers.ContainsKey(topscorerkey))
                throw new KeyNotFoundException("Topscorer " + topscorerkey + " does not exist.");

            Score += topscorers[topscorerkey].Total * 5;
            return Score;
        }
    }
}
