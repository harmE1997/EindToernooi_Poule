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
    }

    public class Question
    {
        public string[] Answer { get; set; }
        public int[] WeeksAnswered { get; set; }
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

        public BonusQuestions(string[] answers, int[] weeks)
        {
            Answers = new Dictionary<BonusKeys, Question>()
            {
                {BonusKeys.Kampioen, new Question(){Answer = new string[] {answers[0] }, Points = 25, WeeksAnswered = new int[] {weeks[0] } } },
                {BonusKeys.Nederland, new Question(){Answer = new string[] {answers[1] }, Points = 25, WeeksAnswered = new int[] {weeks[1] } } },
                {BonusKeys.Topscorer, new Question(){Answer = new string[] {answers[2] }, Points = 5, WeeksAnswered = new int[] {weeks[2] } } },
                {BonusKeys.Bronze, new Question(){Answer = new string[] {answers[3] }, Points = 25, WeeksAnswered = new int[] {weeks[3] } } },
            };
        }

        public int CheckBonus(BonusQuestions HostQuestions, Dictionary<string, int> topscorers)
        {
            if (HostQuestions == null)
            {
                throw new ArgumentNullException("hostquestions");
            }
            int WeekScore = 0;

            //check all questions except topscorers
            foreach (var a in Answers)
            {
                var ans = HostQuestions.Answers[a.Key];
                if (ans.WeeksAnswered[0] > 0)
                {
                    for (int i = 0; i < ans.Answer.Length; i++)
                    {
                        if (a.Value.Answer.Contains(ans.Answer[i]))
                        {
                            WeekScore += a.Value.Points;
                        }
                    }
                }
            }

            //check the topscorers
            var ansscorer = topscorers[Answers[BonusKeys.Topscorer].Answer[0]];
            WeekScore += ansscorer * 5;
            return WeekScore;
        }
    }
}
