using Features.BattleSim.Progression;
using Features.BattleSim.Units;
using Features.GameDesign;
using Features.Shared;
using Features.Shared.Haptic;
using UnityEngine;

namespace Features.Bootstrap
{
    public static class RuntimeInit
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void InitializeRuntime()
        {
            Locator.Add<ISceneService>(new SceneService());

            var runner = new GameObject("CoroutineRunner");
            Locator.Add<IRoutineRunner>(runner.AddComponent<CoroutineRunner>());
            Locator.Add<IUnitSpritesProvider>(new UnitSpritesProvider());
            Locator.Add<IUnitDesignsProvider>(new UnitDesignsProvider());

            Locator.Add<IAssetProvider<CardDeckSettings>>(
                new CachingProvider<CardDeckSettings>(new Repository<CardDeckSettings>("CardDecks")));
            Locator.Add<IAssetProvider<LevelEnemiesSettings>>(
                new CachingProvider<LevelEnemiesSettings>(new Repository<LevelEnemiesSettings>("LevelEnemies")));

            // TODO: refactor - deal with progress via ProgressAggregationService
            var playerProgress = new PlayerProgress();
            Locator.Add(playerProgress);

            var progressMapAsset = Resources.Load<UnitsUnlockMap>("UnitsUnlockMap");
            var progressService = new ProgressAggregationService(playerProgress, new UnlockProgress(progressMapAsset));
            Locator.Add(progressService);

            Vibro.IsEnabled = true;
        }
    }
}