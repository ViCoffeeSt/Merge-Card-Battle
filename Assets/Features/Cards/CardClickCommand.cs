using Features.CardCollection;
using Features.Shared;
using Features.Shared.Haptic;

namespace Features.Cards
{
    public sealed class CardClickCommand : ICommand<Card>
    {
        private readonly ICardCollectionStrategy _collectionStrategy;

        public CardClickCommand(ICardCollectionStrategy collectionStrategy)
        {
            _collectionStrategy = collectionStrategy;
        }

        public void Execute(Card card)
        {
            Vibro.Vibrate(VibrationPattern.LightImpact);
            _collectionStrategy.CollectCard(card);
        }
    }
}