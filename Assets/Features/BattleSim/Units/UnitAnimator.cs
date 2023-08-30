using Features.Shared.Animator;
using UnityEngine;
using UnityEngine.Assertions;

namespace Features.BattleSim.Units
{
    public class UnitAnimator : MonoBehaviour, IAnimationMediator, IAnimationStateReader
    {
        private static readonly int Die = Animator.StringToHash("Die");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int MeleeAttack = Animator.StringToHash("MeleeAttack");

        private readonly int _idleStateHash = Animator.StringToHash("Idle");
        private readonly int _attackStateHash = Animator.StringToHash("Base Attack");
        private readonly int _walkingStateHash = Animator.StringToHash("Move Forward In Place");
        private readonly int _deathStateHash = Animator.StringToHash("Die");

        public AnimatorState State { get; private set; }

        [SerializeField] private Animator _animator;

        private bool _isMoving;

        private void OnEnable()
        {
            _animator.enabled = false;
        }

        private void Start()
        {
            _animator.enabled = true;
        }

        [ContextMenu(nameof(PlayMoveAnimation))]
        public void PlayMoveAnimation()
        {
            if (_isMoving)
            {
                return;
            }

            _isMoving = true;
            _animator.SetBool(IsMoving, true);
        }

        [ContextMenu(nameof(StopMoving))]
        public void StopMoving()
        {
            if (!_isMoving)
            {
                return;
            }

            _isMoving = false;

            _animator.SetBool(IsMoving, false);
        }

        [ContextMenu(nameof(PlayMeleeAttackAnimation))]
        public void PlayMeleeAttackAnimation()
        {
            _isMoving = false;
            _animator.SetTrigger(MeleeAttack);
        }

        [ContextMenu(nameof(PlayDieAnimation))]
        public void PlayDieAnimation()
        {
            _isMoving = false;
            _animator.SetTrigger(Die);
        }

        public void Stop()
        {
            _animator.SetBool(IsMoving, false);
        }

        public void EnteredState(int stateHash)
        {
            State = StateFor(stateHash);
        }

        public void ExitedState(int stateHash)
        {
        }

        private AnimatorState StateFor(int stateHash)
        {
            AnimatorState state;
            if (stateHash == _idleStateHash)
                state = AnimatorState.Idle;
            else if (stateHash == _attackStateHash)
                state = AnimatorState.Attack;
            else if (stateHash == _walkingStateHash)
                state = AnimatorState.Walking;
            else if (stateHash == _deathStateHash)
                state = AnimatorState.Died;
            else
                state = AnimatorState.Unknown;

            Assert.IsFalse(state == AnimatorState.Unknown, gameObject.name);
            return state;
        }
    }
}