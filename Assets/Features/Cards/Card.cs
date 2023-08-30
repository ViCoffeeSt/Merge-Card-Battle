using System.Threading.Tasks;
using DG.Tweening;
using Features.BattleSim.Units;
using Features.Shared;
using Features.Shared.Haptic;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Cards
{
    public class Card : MonoBehaviour
    {
        public bool Active => _container.activeSelf && gameObject.activeSelf;
        public UnitCard UnitCard { get; private set; }

        [SerializeField] private GameObject _container;
        [SerializeField] private Image _faceImage;
        [SerializeField] private Image _backImage;
        [SerializeField] private Image _tierImage;
        [SerializeField] private Image _unitIcon;
        [SerializeField] private Button _button;
        [SerializeField] private Sprite[] _starsTier;
        [SerializeField] private GameObject _mergeFx;

        private ICommand<Card> _clickCommand;
        private IUnitSpritesProvider _unitSpritesProvider;

        public void Construct(UnitCard unitCard, IUnitSpritesProvider unitSpritesProvider, ICommand<Card> clickCommand)
        {
            _unitSpritesProvider = unitSpritesProvider;
            _clickCommand = clickCommand;
            UnitCard = unitCard;

            Redraw();
        }

        public Task Merge(Card duplicate)
        {
            _mergeFx.gameObject.SetActive(false);

            UnitCard.Tier = UnitCard.Tier == CardTier.Excellent ? CardTier.Excellent : UnitCard.Tier + 1;

            var moveTween = duplicate._container
                .transform
                .DOMove(_container.transform.position + Vector3.forward, 0.2f).SetEase(Ease.InOutQuart);
            
            moveTween.onComplete = () =>
            {
                duplicate.Dismiss();
                Redraw();
                _mergeFx.gameObject.SetActive(true);
                Vibro.Vibrate(VibrationPattern.MediumImpact);
            };

            return moveTween.AsyncWaitForCompletion();
        }

        private void Dismiss()
        {
            _container.SetActive(false);
        }

        public void ClickOnCard()
        {
            var tween = transform.DORotate(new Vector3(0, -180, 0), 0.5f);
            tween.onComplete = () => _clickCommand.Execute(this);
        }

        public void FlipCard()
        {
            transform.DORotate(new Vector3(0, 0, 0), 0.5f);
        }

        public void ShowFrontImage()
        {
            transform.DORotate(new Vector3(0, -180, 0), 0.5f);
        }

        private void Redraw()
        {
            Sprite tierSprite = _starsTier[(int)UnitCard.Tier];

            _tierImage.sprite = tierSprite;

            RectTransform tierImageRectTransform = _tierImage.GetComponent<RectTransform>();

            if (tierSprite != null && tierImageRectTransform != null)
            {
                tierImageRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tierSprite.rect.width);
                tierImageRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, tierSprite.rect.height);
            }
            
            _faceImage.sprite = _unitSpritesProvider.GetFaceImage(UnitCard.UnitKey, UnitCard.Tier);
            _unitIcon.sprite = _unitSpritesProvider.GetUnitSprite(UnitCard.UnitKey, UnitCard.Tier);
        }
    }
}