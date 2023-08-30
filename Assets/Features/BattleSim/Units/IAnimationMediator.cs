namespace Features.BattleSim.Units
{
    public interface IAnimationMediator
    {
        void PlayMoveAnimation();
        void PlayMeleeAttackAnimation();
        void PlayDieAnimation();
        void Stop();
    }
}
