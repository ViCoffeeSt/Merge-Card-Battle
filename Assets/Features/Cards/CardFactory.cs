using Features.BattleSim.Units;
using Features.CardCollection;
using Features.GameDesign.Units;
using UnityEngine;

namespace Features.Cards
{
    public class CardFactory : ICardFactory
    {
        private readonly IUnitSpritesProvider _unitSpritesProvider;
        private readonly CardClickCommand _cardClickCommand;
        private readonly GameSettings _config;

        public CardFactory(IUnitSpritesProvider unitSpritesProvider,
            CardClickCommand cardClickCommand,
            GameSettings config)
        {
            _unitSpritesProvider = unitSpritesProvider;
            _cardClickCommand = cardClickCommand;
            _config = config;
        }

        public Card Create(string unitKey, CardTier tier)
        {
            var cardPrefab = _config.UniversalCardPrefab;
            var card = Object.Instantiate(cardPrefab);
            var unitCard = new UnitCard
            {
                UnitKey = unitKey,
                Tier = tier
            };

            card.Construct(unitCard, _unitSpritesProvider, _cardClickCommand);

            return card;
        }
    }
}