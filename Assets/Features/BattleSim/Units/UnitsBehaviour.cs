using System.Collections.Generic;
using Features.BattleSim.Units;
using UnityEngine;

namespace Features.BattleSim
{
    public abstract class UnitsBehaviour : MonoBehaviour
    {
        public abstract void UpdateUnits(IReadOnlyCollection<UnitBehaviour> units,
            IReadOnlyCollection<UnitBehaviour> enemies);
    }
}