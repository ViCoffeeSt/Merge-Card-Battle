using System.Collections.Generic;
using Features.BattleSim.Units;

namespace Features.BattleSim
{
    public class UnitsDestroyBehaviour : UnitsBehaviour
    {
        public override void UpdateUnits(IReadOnlyCollection<UnitBehaviour> units,
            IReadOnlyCollection<UnitBehaviour> enemies)
        {
            foreach (var unit in units)
            {
                if (!unit || unit.Parameters.Health > 0)
                {
                    continue;
                }

                unit.Animations.PlayDieAnimation();
                Destroy(unit.gameObject, 2f);
            }

            foreach (var enemy in enemies)
            {
                if (!enemy || enemy.Parameters.Health > 0)
                {
                    continue;
                }

                enemy.Animations.PlayDieAnimation();
                Destroy(enemy.gameObject, 2f);
            }
        }
    }
}
