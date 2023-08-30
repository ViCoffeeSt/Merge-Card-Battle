using System.Collections.Generic;
using Features.BattleSim.Progression;
using Features.BattleSim.Units;
using Features.CardCollection;
using Features.GameDesign;
using Features.Shared;
using UnityEngine;

namespace Features.BattleSim
{
    public class LevelWavesService : ILevelWavesService
    {
        private const string LevelEnemies = "LevelEnemies_";

        private readonly PlayerProgress _playerProgress;
        private readonly IUnitsFactory _unitsFactory;
        private readonly Bounds _spawnArea;
        private readonly IAssetProvider<LevelEnemiesSettings> _enemiesConfigProvider;

        public LevelWavesService(
            PlayerProgress playerProgress,
            IUnitsFactory unitsFactory,
            Bounds spawnArea,
            IAssetProvider<LevelEnemiesSettings> enemiesConfigProvider)
        {
            _playerProgress = playerProgress;
            _unitsFactory = unitsFactory;
            _spawnArea = spawnArea;
            _enemiesConfigProvider = enemiesConfigProvider;
        }

        public IReadOnlyCollection<UnitBehaviour> SpawnEnemiesWave(Transform parent = null)
        {
            var level = _playerProgress.CurrentLevel % GameSettings.LevelsPoolSize;
            var levelConfig = _enemiesConfigProvider.GetAssets(LevelEnemies + level);

            var enemies = new UnitBehaviour[levelConfig.UnitsPlacements.Length];

            for (var i = 0; i < levelConfig.UnitsPlacements.Length; i++)
            {
                var placement = levelConfig.UnitsPlacements[i];

                var instance = _unitsFactory.CreateUnit(_spawnArea, placement, parent, levelConfig);

                instance.name = $"[Enemy] {placement.UnitKey} #{i:00}";
                instance.transform.rotation = Quaternion.Euler(0, 180, 0);

                enemies[i] = instance;
            }

            return enemies;
        }
    }
}