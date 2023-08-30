using System.Collections;
using Features.Shared;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.BattleSim
{
    public class BattleFailedResultScreen : MonoBehaviour
    {
        private const float DisplayPercentLabelThreshold = 0.3f;

        [SerializeField] private Slider _progressBar;
        [SerializeField] private TMP_Text _damagePercentageLabel;

        private IRoutineRunner _routineRunner;
        private ICommand _claimCommand;

        private void Awake()
        {
            Locator.Add(this);
        }

        private void Start()
        {
            Construct(Locator.Instance<IRoutineRunner>());
            gameObject.SetActive(false);
        }

        private void Construct(IRoutineRunner routineRunner)
        {
            _routineRunner = routineRunner;
        }

        public void Show(BattleResult battleResult, ICommand rewardClaimCommand)
        {
            _claimCommand = rewardClaimCommand;
            gameObject.SetActive(true);
            _routineRunner.StartCoroutine(VisualizeProgress(battleResult.PercentDone));
        }

        private IEnumerator VisualizeProgress(float percentDone)
        {
            var progress = 0f;
            _damagePercentageLabel.text = string.Empty;
            while (progress < percentDone)
            {
                progress += Time.deltaTime;
                _progressBar.value = progress;
                if (progress > DisplayPercentLabelThreshold)
                {
                    _damagePercentageLabel.text = progress.ToString("P1");
                }

                yield return null;
            }

            if (percentDone > DisplayPercentLabelThreshold)
            {
                _damagePercentageLabel.text = percentDone.ToString("P1");
            }
        }

        [UsedImplicitly]
        public void OnClaim() => _claimCommand?.Execute();

        private void OnDestroy() => Locator.Add<BattleFailedResultScreen>(null);
    }
}