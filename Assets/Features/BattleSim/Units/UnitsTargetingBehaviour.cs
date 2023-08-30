using System.Collections.Generic;
using Features.BattleSim.Units;

namespace Features.BattleSim
{
    public class UnitsTargetingBehaviour : UnitsBehaviour
    {
        public override void UpdateUnits(IReadOnlyCollection<UnitBehaviour> units,
            IReadOnlyCollection<UnitBehaviour> enemies)
        {
            foreach (var unit in units)
            {
                if (!unit || unit.Target)
                {
                    continue;
                }

                var closestEnemy = default(UnitBehaviour);
                var closestEnemySqrDistance = float.MaxValue;
                foreach (var enemy in enemies)
                {
                    if (!enemy)
                    {
                        continue;
                    }

                    var sqrDistance = (enemy.transform.position - unit.transform.position).magnitude;
                    if (sqrDistance >= closestEnemySqrDistance)
                    {
                        continue;
                    }

                    closestEnemySqrDistance = sqrDistance;
                    closestEnemy = enemy;
                }

                unit.Target = closestEnemy;
            }

            foreach (var enemy in enemies)
            {
                if (!enemy || enemy.Target)
                {
                    continue;
                }

                var closestUnit = default(UnitBehaviour);
                var closestUnitSqrDistance = float.MaxValue;
                foreach (var unit in units)
                {
                    if (!unit)
                    {
                        continue;
                    }

                    var sqrDistance = (enemy.transform.position - unit.transform.position).sqrMagnitude;
                    if (sqrDistance >= closestUnitSqrDistance)
                    {
                        continue;
                    }

                    closestUnitSqrDistance = sqrDistance;
                    closestUnit = unit;
                }

                enemy.Target = closestUnit;
            }
        }
    }
}