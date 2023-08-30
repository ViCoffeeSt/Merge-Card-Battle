using System;
using System.Runtime.CompilerServices;
using Features.BattleSim;
using Features.Cards;
using Features.Shared;
using Unity.Mathematics;
using UnityEngine;

namespace Features.GameDesign.Units
{
    [CreateAssetMenu(menuName = "Semki Games/Settings/Units/Create UnitDesignObject",
        fileName = "NewUnitDesignObject",
        order = 0)]
    public class UnitDesignObject : ScriptableObject
    {
        [SerializeField] [NotEditable] private string _key;

        [SerializeField] private UnitParameterSetting _healthParameter;
        [SerializeField] private UnitParameterSetting _speedParameter;
        [SerializeField] private UnitParameterSetting _damageParameter;
        [SerializeField] private UnitParameterSetting _attackDistanceParameter;
        [SerializeField] private UnitParameterSetting _attackCooldownParameter;

        [SerializeField] private GameObject[] _prefabs;

        public GameObject VisualForTier(CardTier tier)
        {
            var i = (int) tier % _prefabs.Length;
            return _prefabs[i];
        }

        private void OnValidate()
        {
            _key = name;
        }

        public UnitParameters GetParametersObject(CardTier tier, IMutator mutator = null)
        {
            var mutation = mutator?.GetTierMutation(tier) ?? 0f;
            return new UnitParameters(
                _healthParameter.CalculateFinal((int) tier) + mutation,
                _damageParameter.CalculateFinal((int) tier) + mutation,
                _speedParameter.CalculateFinal((int) tier),
                _attackDistanceParameter.CalculateFinal((int) tier),
                _attackCooldownParameter.CalculateFinal((int) tier));
        }
    }

    [Serializable]
    public struct UnitParameterSetting
    {
        public float BaseValue;
        public float GrowthFactor;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float CalculateFinal(int tier) => BaseValue * math.pow(GrowthFactor, tier);
    }
}