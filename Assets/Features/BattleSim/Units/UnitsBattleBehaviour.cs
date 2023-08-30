using System.Collections.Generic;
using Features.BattleSim.Units;
using UnityEngine;

namespace Features.BattleSim
{
    public class UnitsBattleBehaviour : UnitsBehaviour
    {
        public override void UpdateUnits(IReadOnlyCollection<UnitBehaviour> units,
            IReadOnlyCollection<UnitBehaviour> enemies)
        {
            var deltaTime = Time.deltaTime;

            foreach (var unit in units)
            {
                if (!unit)
                {
                    continue;
                }

                unit.ActiveCooldown -= deltaTime;

                if (unit.ActiveCooldown > 0 || unit.CalculatedDistance > unit.Parameters.AttackDistance)
                {
                    continue;
                }

                if (!unit.Target)
                {
                    continue;
                }

                unit.Animations.PlayMeleeAttackAnimation();

                unit.Target.Parameters.TakeDamage(unit.Parameters.Damage);
                if (unit.Target.Parameters.Health <= 0)
                {
                    unit.Target = null;
                }

                unit.ActiveCooldown = unit.Parameters.AttackCooldown;
            }

            foreach (var enemy in enemies)
            {
                if (!enemy)
                {
                    continue;
                }

                enemy.ActiveCooldown -= deltaTime;

                if (enemy.ActiveCooldown > 0 || enemy.Parameters.Health <= 0)
                {
                    continue;
                }

                if (!enemy.Target)
                {
                    continue;
                }

                if (enemy.CalculatedDistance > enemy.Parameters.AttackDistance)
                {
                    continue;
                }

                enemy.Animations.PlayMeleeAttackAnimation();

                enemy.Target.Parameters.TakeDamage(enemy.Parameters.Damage);
                enemy.ActiveCooldown = enemy.Parameters.AttackCooldown;
            }
        }
    }
}
