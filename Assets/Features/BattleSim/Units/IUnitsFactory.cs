using Features.GameDesign;
using UnityEngine;

namespace Features.BattleSim.Units
{
    public interface IUnitsFactory
    {
        UnitBehaviour CreateUnit(Bounds spawnArea,
            UnitSpawnInfo unitPlacement,
            Transform parent = null,
            IMutator mutator = null);
    }
}