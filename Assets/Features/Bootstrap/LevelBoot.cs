using Features.BattleSim;
using Features.BattleSim.Progression;
using Features.BattleSim.Units;
using Features.CardCollection;
using Features.Cards;
using Features.GameDesign;
using Features.Shared;
using UnityEngine;

namespace Features.Bootstrap
{
    public class LevelBoot : MonoBehaviour
    {
        [SerializeField] private bool _previewCards;

        [SerializeField] private Transform _arenaRoot;
        [SerializeField] private GridManager _gridManager;

        [Header("Settings")] [SerializeField] private GameSettings _gameSettings;

        private void Start()
        {
            Application.targetFrameRate = 60;

            var playerProgress = Locator.Instance<PlayerProgress>();
            CreateBattleArena(playerProgress);

            var routineRunner = Locator.Instance<IRoutineRunner>();

            var mergeCompletionCommand = new MergeCompletedCommand(_gridManager,
                Locator.Instance<BattleSimulator>(),
                Locator.Instance<SpawnArea>().UserUnitsArea,
                Locator.Instance<IUnitsFactory>(),
                routineRunner);

            ICardMergeStrategy mergeStrategy = _gameSettings.EnergyCostPerCard > 0
                ? new EnergyBasedMergeStrategy(_gameSettings,
                    _gridManager,
                    mergeCompletionCommand,
                    routineRunner)
                : new PoolBasedMergeStrategy(_gameSettings,
                    _gridManager,
                    mergeCompletionCommand,
                    routineRunner);

            ICardCollectionStrategy collectionStrategy = _gameSettings.EnergyCostPerCard > 0
                ? new EnergyBasedCardCollectionStrategy(_gridManager, mergeStrategy)
                : new PoolBasedCardCollectionStrategy(mergeStrategy);

            var cardFactory = new CardFactory(
                Locator.Instance<IUnitSpritesProvider>(),
                new CardClickCommand(collectionStrategy), _gameSettings);

            var deckProvider = Locator.Instance<IAssetProvider<CardDeckSettings>>();
            var levelNumber = playerProgress.CurrentLevel % GameSettings.LevelsPoolSize;
            var deck = deckProvider.GetAssets("Level_" + levelNumber);
            _gridManager.PopulateGrid(_gameSettings, cardFactory, deck);

            if (_previewCards)
            {
                _gridManager.ShowAll();
                Invoke(nameof(CloseCards), _gameSettings.InitialCardsPreviewTime);
            }
            else
            {
                CloseCards();
            }
        }

        private void CreateBattleArena(PlayerProgress playerProgress)
        {
            var visualConfig = Resources.Load<LevelVisualSettings>("Levels/Level_1");
            var spawnArea = Instantiate(visualConfig.Arena, _arenaRoot).GetComponent<SpawnArea>();
            Locator.Add(spawnArea);

            Instantiate(visualConfig.Decoration, _arenaRoot);

            var battleSimulatorPrefab = Resources.Load<BattleSimulator>("BattleSimulator");
            var battleSimulatorInstance = Instantiate(battleSimulatorPrefab, _arenaRoot);
            battleSimulatorInstance.enabled = false;

            var unitsFactory = new UnitsFactory(Locator.Instance<IUnitDesignsProvider>());
            var enemiesConfigProvider = Locator.Instance<IAssetProvider<LevelEnemiesSettings>>();

            var waveService = new LevelWavesService(playerProgress,
                unitsFactory,
                spawnArea.LevelUnitsArea,
                enemiesConfigProvider);

            battleSimulatorInstance.Construct(waveService,
                new ShowBattleResultCommand(Locator.Instance<ProgressAggregationService>(),
                    enemiesConfigProvider,
                    new ReloadLevelCommand())
            );

            Locator.Add<ILevelWavesService>(waveService);
            Locator.Add<IUnitsFactory>(unitsFactory);

            Locator.Add(battleSimulatorInstance);
        }

        private void OnDisable()
        {
            Locator.Add<BattleSimulator>(null);
            Locator.Add<SpawnArea>(null);
            Locator.Add<ILevelWavesService>(null);
            Locator.Add<IUnitsFactory>(null);
        }

        private void CloseCards()
        {
            _gridManager.HideAll();
        }
    }
}