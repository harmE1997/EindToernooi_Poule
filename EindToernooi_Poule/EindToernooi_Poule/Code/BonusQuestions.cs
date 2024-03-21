using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

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
                {BonusKeys.Kampioen, new Question(){Answer = new string[] {answers[0] }, Points = 25 } },
                {BonusKeys.Topscorer, new Question(){Answer = new string[] {answers[1] }, Points = 5} },
                {BonusKeys.Nederland, new Question(){Answer = new string[] {answers[2] }, Points = 25 } },
                {BonusKeys.Bronze, new Question(){Answer = new string[] {answers[3] }, Points = 25 } },
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
                    if (a.Value.Answer.Contains(ans.Answer[i]))
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
