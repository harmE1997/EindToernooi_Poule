using PoolsBase.Interfaces;

namespace PoolsBase
{
    public class PlayerBase<T>
    {
        public string Name { get; set; }
        public string Town { get; set; }
        public int TotalScore { get; set; }
        public int Ranking { get; set; }
        public int RankingDifference { get; set; }
        public int BonusScore { get; set; }

        public T Questions { get; set; }

        public PlayerBase()
        {
            //this parameterless constructor is used for json deserialization. Do not use it for implementations!
        }
        public PlayerBase(string name, string woonplaats, T questions)
        {
            Name = name;
            Town = woonplaats;
            TotalScore = 0;
            Questions = questions;
            RankingDifference = 0;
            Ranking = 0;
        }

        public virtual void CheckPlayer(IHost Host, Dictionary<string, Topscorer> topscorers) { }
    }
}
