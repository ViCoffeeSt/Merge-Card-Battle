using System;
using Features.Cards;
using Unity.Mathematics;
using UnityEngine;

namespace Features.GameDesign
{
    [CreateAssetMenu(menuName = "Semki Games/Settings/Create LevelEnemiesSettings",
        fileName = "DefaultEnemiesSettings",
        order = 0)]
    public class LevelEnemiesSettings : ScriptableObject, IMutator
    {
        public UnitSpawnInfo[] UnitsPlacements => _unitPlacements;

        [SerializeField] private float _beta = 5;
        [Range(-10, 10)] [SerializeField] private float _alpha = .3f;
        [SerializeField] private float _omega = -20;

        [Space(8)] [SerializeField] private UnitSpawnInfo[] _unitPlacements;

        public float GetTierMutation(CardTier tier)
        {
            var value = 0f;
            switch (tier)
            {
                case CardTier.Basic:
                    value = _beta;
                    break;
                case CardTier.Advanced:
                    value = math.ceil((_beta - _omega) * _alpha);
                    break;
                case CardTier.Excellent:
                    value = _omega;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tier), tier, null);
            }

            return value;
        }
    }

    public interface IMutator
    {
        float GetTierMutation(CardTier tier);
    }
}