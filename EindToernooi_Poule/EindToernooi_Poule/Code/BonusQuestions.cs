using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Avalonia;

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

    public class Question
    {
        public string[] Answer { get; set; }
        public int Points { get; set; }

        public Question()
        {
            //this parameterless constructor is used for json deserialization. Do not use it for implementations!
        }
    }

    public class BonusQuestions
    {
        public Dictionary<BonusKeys, Question> Answers { get; set; }

        public BonusQuestions()
        {
            //this parameterless constructor is used for json deserialization. Do not use it for implementations!
        }

        public BonusQuestions(string[] answers)
        {
            Answers = new Dictionary<BonusKeys, Question>()
            {
                {BonusKeys.GroupWinners, new Question(){Answer = new string[]{answers[0], answers[2], answers[4], answers[6], answers[8],answers[10],answers[12],answers[14],answers[16],answers[18],answers[20],answers[22] }, Points=15 } },
                {BonusKeys.GroupRunnerups, new Question(){Answer = new string[]{answers[1], answers[3], answers[5], answers[7], answers[9],answers[11],answers[13],answers[15],answers[17],answers[19],answers[21],answers[23] }, Points=15 } },
                {BonusKeys.Kampioen, new Question(){Answer = new string[] {answers[24] }, Points = 50 } },
                {BonusKeys.Topscorer, new Question(){Answer = new string[] {answers[25] }, Points = 5} },
                {BonusKeys.Nederland, new Question(){Answer = new string[] {answers[26] }, Points = 25 } },
                {BonusKeys.Bronze, new Question(){Answer = new string[] {answers[27] }, Points = 25 } },
            };
        }

        public int CheckBonus(BonusQuestions HostQuestions, Dictionary<string, int> topscorers)
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
                if (a.Key == BonusKeys.Bronze && !GeneralConfiguration.Bronze)
                    continue;
                if (a.Key == BonusKeys.Nederland && !GeneralConfiguration.NlPresent)
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
            if(!topscorers.ContainsKey(topscorerkey))
                throw new KeyNotFoundException("Topscorer " + topscorerkey + " does not exist.");
            
            Score += topscorers[topscorerkey] * 5;
            return Score;
        }
    }
}
