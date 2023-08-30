using System.Collections.Generic;
using Features.BattleSim.Units;
using UnityEngine;

namespace Features.BattleSim
{
    public interface ILevelWavesService
    {
        IReadOnlyCollection<UnitBehaviour> SpawnEnemiesWave(Transform parent = null);
    }
}