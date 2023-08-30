namespace Features.BattleSim
{
    public class BattleResult
    {
        public bool IsOpponentDefeated { get; }

        public float PercentDone { get; }

        public BattleResult(bool isOpponentDefeated, float percentDone)
        {
            IsOpponentDefeated = isOpponentDefeated;
            PercentDone = percentDone;
        }
    }
}