using System.Collections.Generic;
using Features.BattleSim.Units;
using UnityEngine;

namespace Features.BattleSim
{
    public class UnitsMovementBehaviour : UnitsBehaviour
    {
        public override void UpdateUnits(
            IReadOnlyCollection<UnitBehaviour> units,
            IReadOnlyCollection<UnitBehaviour> enemies
        )
        {
            foreach (var unit in units)
            {
                unit.CalculatedDistance = float.MaxValue;
                if (!unit || !unit.Target || Mathf.Approximately(0, unit.Speed))
                {
                    continue;
                }

                var cachedTransform = unit.transform;
                var position = cachedTransform.position;
                var targetPosition = unit.Target.transform.position;

                var dir = (targetPosition - position).normalized;
                var lookRotation = Quaternion.LookRotation(dir);
                cachedTransform.rotation = Quaternion.Slerp(cachedTransform.rotation, lookRotation, Time.deltaTime);

                position += cachedTransform.forward * (unit.Speed * Time.deltaTime);

                unit.CalculatedDistance = Vector3.Distance(position, targetPosition);

                if (unit.CalculatedDistance > unit.Parameters.AttackDistance)
                {
                    cachedTransform.position = position;
                    unit.Animations.PlayMoveAnimation();
                }
            }

            foreach (var enemy in enemies)
            {
                enemy.CalculatedDistance = float.MaxValue;

                if (!enemy || !enemy.Target || Mathf.Approximately(0, enemy.Speed))
                {
                    continue;
                }

                var cachedTransform = enemy.transform;
                var position = cachedTransform.position;
                var targetPosition = enemy.Target.transform.position;

                var dir = (targetPosition - position).normalized;

                var lookRotation = Quaternion.LookRotation(dir);
                cachedTransform.rotation = Quaternion.Slerp(cachedTransform.rotation, lookRotation, Time.deltaTime);

                position += cachedTransform.forward * (enemy.Speed * Time.deltaTime);

                enemy.CalculatedDistance = Vector3.Distance(position, targetPosition);

                if (enemy.CalculatedDistance > enemy.Parameters.AttackDistance)
                {
                    cachedTransform.position = position;
                    enemy.Animations.PlayMoveAnimation();
                }
            }
        }
    }
}