using System;
using System.Collections.Generic;
using System.Linq;
using VoetbalPoolsBase;

namespace EindToernooi_Poule.Code
{
    [Serializable]
    public enum BonusKeys
    {
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

        public BonusQuestions(List<KeyValuePair<string, int>> rawanswers)
        {
            var answers = new List<string>();
            for (int i = 0; i < 4; i++)
                answers.Add(string.Empty);

            if (rawanswers.Count == 4)
            {
                answers.Clear();
                foreach (var pair in rawanswers)
                    answers.Add(pair.Key);
            }

            Answers = new Dictionary<BonusKeys, BonusQuestion>()
            {
                {BonusKeys.Kampioen, new BonusQuestion(){Answer = new string[] {answers[0] }, Points = 50 } },
                {BonusKeys.Topscorer, new BonusQuestion(){Answer = new string[] {answers[1] }, Points = 5} },
                {BonusKeys.Nederland, new BonusQuestion(){Answer = new string[] {answers[2] }, Points = 25 } },
                {BonusKeys.Bronze, new BonusQuestion(){Answer = new string[] {answers[3] }, Points = 25 } },
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
                if (a.Key == BonusKeys.Topscorer)
                    continue;

                for (int i = 0; i < ans.Answer.Length; i++)
                {
                    if (a.Value.Answer.Contains(ans.Answer[i]))
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
