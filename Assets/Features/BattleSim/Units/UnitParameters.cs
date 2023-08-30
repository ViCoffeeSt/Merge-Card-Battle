using Unity.Mathematics;

namespace Features.BattleSim
{
    public sealed class UnitParameters
    {
        public float Health => _health;

        public float Damage => _damage;

        public float MovementSpeed => _health > 0 ? _movementSpeed : 0;

        public float AttackDistance => _health > 0 ? _attackDistance : 0;

        public float AttackCooldown => _attackCooldown;

        private float _health;
        private readonly float _movementSpeed;
        private readonly float _attackDistance;
        private readonly float _attackCooldown;
        private readonly float _damage;

        public UnitParameters(float health,
            float damage,
            float movementSpeed,
            float attackDistance,
            float attackCooldown)
        {
            _health = health;
            _damage = damage;
            _movementSpeed = movementSpeed;
            _attackDistance = attackDistance;
            _attackCooldown = attackCooldown;
        }

        public void TakeDamage(float damage)
        {
            _health = math.max(0, _health - damage);
        }
    }
}