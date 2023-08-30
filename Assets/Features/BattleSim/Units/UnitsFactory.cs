using Features.Cards;
using Features.GameDesign;
using Features.GameDesign.Units;
using UnityEngine;

namespace Features.BattleSim.Units
{
    public class UnitsFactory : IUnitsFactory
    {
        private readonly IUnitDesignsProvider _designsProvider;
        private UnitBehaviour _prototype;

        public UnitsFactory(IUnitDesignsProvider designsProvider)
        {
            _designsProvider = designsProvider;
        }

        public UnitBehaviour CreateUnit(Bounds spawnArea,
            UnitSpawnInfo unitPlacement,
            Transform parent = null,
            IMutator mutator = null)
        {
            var w = spawnArea.size.x;
            var h = spawnArea.size.z;

            var cellW = w / 4;
            var cellH = h / 3;

            var x = unitPlacement.Col * cellW + cellW * 0.5f;
            var z = unitPlacement.Row * cellH + cellH * 0.5f;

            var position = new Vector3(spawnArea.min.x + x, 0,
                unitPlacement.FrontFace ? spawnArea.min.z + z : spawnArea.max.z - z);

            var unitDesign = _designsProvider.GetDesign(unitPlacement.UnitKey);
            return CreateUnitAt(position, unitDesign, unitPlacement.UnitTier, parent, mutator);
        }

        private UnitBehaviour CreateUnitAt(Vector3 point,
            UnitDesignObject unitDesign,
            CardTier tier,
            Transform parent = null,
            IMutator mutator = null)
        {
            if (!_prototype)
            {
                _prototype = Resources.Load<UnitBehaviour>("UnitBase");
            }

            var instance = Object.Instantiate(_prototype, parent);
            instance.transform.position = point;

            var visualPrefab = unitDesign.VisualForTier(tier);
            var visualInstance = Object.Instantiate(visualPrefab, instance.transform);

            instance.Construct(
                unitDesign.GetParametersObject(tier, mutator),
                visualInstance.GetComponent<UnitAnimator>());

            return instance;
        }
    }
}