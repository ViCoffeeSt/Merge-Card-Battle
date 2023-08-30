using System.Collections;
using Features.BattleSim.Progression;
using Features.BattleSim.Units;
using Features.Cards;
using Features.Shared;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.BattleSim
{
    public class BattleWinResultScreen : MonoBehaviour
    {
        [SerializeField] private Image _unitPreview;
        [SerializeField] private Image _unitPreviewFill;
        [SerializeField] private Slider _progressBar;

        [SerializeField] private TMP_Text _claimButtonText;

        private IUnitSpritesProvider _spritesProvider;
        private IRoutineRunner _routineRunner;

        private ICommand _claimCommand;

        private void Awake()
        {
            Locator.Add(this);
        }

        private void Start()
        {
            Construct(Locator.Instance<IUnitSpritesProvider>(), Locator.Instance<IRoutineRunner>());
            gameObject.SetActive(false);
            _unitPreview.gameObject.SetActive(false);
            _unitPreviewFill.gameObject.SetActive(false);
            _progressBar.gameObject.SetActive(false);
        }

        private void Construct(IUnitSpritesProvider spritesProvider, IRoutineRunner routineRunner)
        {
            _routineRunner = routineRunner;
            _spritesProvider = spritesProvider;

            _unitPreview.fillAmount = 0;
            _unitPreviewFill.fillAmount = 1f;
            _progressBar.value = 0;
        }

        public void Show(UnitUnlockProgress unitUnlockProgress, ICommand rewardClaimCommand)
        {
            _claimCommand = rewardClaimCommand;
            if (unitUnlockProgress.IsAvailable)
            {
                _unitPreview.sprite = _spritesProvider.GetUnitSprite(unitUnlockProgress.UnitKey, CardTier.Excellent);
                _unitPreviewFill.sprite = _spritesProvider.GetUnitFillerSprite(unitUnlockProgress.UnitKey);

                _unitPreview.gameObject.SetActive(true);
                _unitPreviewFill.gameObject.SetActive(true);
                _progressBar.gameObject.SetActive(true);
            }

            _claimButtonText.text =
                unitUnlockProgress.IsAvailable && unitUnlockProgress.Progress >= 1f ? "Claim" : "Next";

            gameObject.SetActive(true);

            _routineRunner.StartCoroutine(VisualizeProgress(unitUnlockProgress));
        }

        private IEnumerator VisualizeProgress(UnitUnlockProgress unitUnlockProgress)
        {
            if (!unitUnlockProgress.IsAvailable)
            {
                yield break;
            }

            yield return null;
            var progress = 0f;
            var unlockProgress = unitUnlockProgress.Progress;

            while (progress < unlockProgress)
            {
                progress += Time.deltaTime;
                _unitPreview.fillAmount = progress;
                _unitPreviewFill.fillAmount = 1f - progress;
                _progressBar.value = progress;
                yield return null;
            }

            _unitPreview.fillAmount = unlockProgress;
            _unitPreviewFill.fillAmount = 1f - unlockProgress;
            _progressBar.value = unlockProgress;
        }

        [UsedImplicitly]
        public void OnClaim() => _claimCommand?.Execute();

        private void OnDestroy() => Locator.Add<BattleWinResultScreen>(null);
    }
}