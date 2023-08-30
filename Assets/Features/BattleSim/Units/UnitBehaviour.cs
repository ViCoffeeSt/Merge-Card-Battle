using UnityEngine;

namespace Features.BattleSim.Units
{
    public class UnitBehaviour : MonoBehaviour
    {
        public float Speed => _parameters.MovementSpeed;
        public IAnimationMediator Animations => _unitAnimator;

        public float CalculatedDistance { get; set; }
        public float ActiveCooldown { get; set; }

        public UnitBehaviour Target { get; set; }
        public UnitParameters Parameters => _parameters;

        private UnitAnimator _unitAnimator;
        private UnitParameters _parameters;

        public void Construct(UnitParameters unitParameters, UnitAnimator unitAnimator)
        {
            _parameters = unitParameters;
            _unitAnimator = unitAnimator;
        }
    }
}