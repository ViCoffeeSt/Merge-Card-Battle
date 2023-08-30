using Features.Cards;
using UnityEngine;

namespace Features.CardCollection
{
    [CreateAssetMenu(menuName = "Semki Games/Settings/Create GameSettings",
        fileName = "GameSettings", order = 0)]
    public class GameSettings : ScriptableObject
    {
        public const int LevelsPoolSize = 30;
        public float CardFlipDelayInSeconds => _delayFlipCard;
        public float InitialCardsPreviewTime => _initialCardsPreviewTime;
        public float MinEnergy => _minEnergy;
        public float MaxEnergy => _maxEnergy;
        public float EnergyCostPerCard => _energyPerClick;

        public Card UniversalCardPrefab => _universalCardPrefab;
        public bool KeepCardsOpen => _keepCardsOpen;

        [SerializeField] private bool _keepCardsOpen;
        [SerializeField] [Range(0, 100)] private float _delayFlipCard = 1;
        [SerializeField] [Range(0, 100)] private float _initialCardsPreviewTime = 1;

        [Space(12f)] [Header("Energy Settings")] [SerializeField] [Range(0, 100)]
        private float _energyPerClick = 10;

        [SerializeField] [Range(1, 1000)] private float _maxEnergy = 100f;
        [SerializeField] [Range(0, 999)] private float _minEnergy;

        [Space(12f)] [Header("Cards Spawn")] [SerializeField]
        private Card _universalCardPrefab;
    }
}