namespace PoolsBase
{
    public class BonusQuestion
    {
        public string[] Answer { get; set; }
        public int[] RoundsAnswered { get; set; }
        public int Points { get; set; }

        public BonusQuestion()
        {
            //this parameterless constructor is used for json deserialization. Do not use it for implementations!
        }
    }
}
