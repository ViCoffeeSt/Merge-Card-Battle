namespace Features.BattleSim.Progression
{
    public class ProgressAggregationService
    {
        private readonly PlayerProgress _playerProgress;
        private readonly UnlockProgress _unlockProgress;

        public ProgressAggregationService(PlayerProgress playerProgress, UnlockProgress unlockProgress)
        {
            _playerProgress = playerProgress;
            _unlockProgress = unlockProgress;
        }

        public void CurrentLevelCompleted()
        {
            _unlockProgress.UpdateCompletedLevel(_playerProgress.CurrentLevel);
            _playerProgress.ProgressToNextLevel();
        }

        public UnitUnlockProgress GetCurrentUnlock()
        {
            var data = _unlockProgress.FindCurrentUnlock(_playerProgress.CurrentLevel);

            return new UnitUnlockProgress(data, _unlockProgress.PreviousUnlockLevel, _playerProgress.CurrentLevel);
        }
    }
}