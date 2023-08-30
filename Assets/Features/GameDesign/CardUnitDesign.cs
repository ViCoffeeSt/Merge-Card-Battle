using Features.Cards;
using Features.Shared;
using UnityEngine;

[CreateAssetMenu(menuName = "Semki Games/Settings/Units/Create CardUnitDesign",
    fileName = "NewCardUnitDesignObject",
    order = 1)]
public class CardUnitDesign : ScriptableObject
{
    [SerializeField] private Sprite[] _faceImage;
    [SerializeField] private Sprite[] _tierImage;

    public Sprite GetFaceImage(CardTier tier)
    {
        var i = (int)tier % _faceImage.Length;
        return _faceImage[i];
    }

    public Sprite GetTierImage(CardTier tier)
    {
        return _tierImage[1];
    }
}
