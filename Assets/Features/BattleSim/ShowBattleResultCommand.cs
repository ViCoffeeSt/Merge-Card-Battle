using Features.BattleSim.Progression;
using Features.GameDesign;
using Features.Shared;
using Features.Shared.Haptic;

namespace Features.BattleSim
{
    public class ShowBattleResultCommand : ICommand<BattleResult>
    {
        private readonly ProgressAggregationService _progressService;
        private readonly IAssetProvider<LevelEnemiesSettings> _enemiesConfigProvider;
        private readonly ICommand _rewardClaimCommand;

        public ShowBattleResultCommand(ProgressAggregationService progressService,
            IAssetProvider<LevelEnemiesSettings> enemiesConfigProvider,
            ICommand rewardClaimCommand)
        {
            _progressService = progressService;
            _enemiesConfigProvider = enemiesConfigProvider;
            _rewardClaimCommand = rewardClaimCommand;
        }

        public void Execute(BattleResult payload)
        {
            if (payload.IsOpponentDefeated)
            {
                var unlock = _progressService.GetCurrentUnlock();

                var winScreen = Locator.Instance<BattleWinResultScreen>();
                winScreen.Show(unlock, _rewardClaimCommand);

                Vibro.Vibrate(VibrationPattern.Success);

                _progressService.CurrentLevelCompleted();
                _enemiesConfigProvider.ClearAll();
            }
            else
            {
                var failScreen = Locator.Instance<BattleFailedResultScreen>();
                failScreen.Show(payload, _rewardClaimCommand);
                Vibro.Vibrate(VibrationPattern.Failure);
            }
        }
    }
}