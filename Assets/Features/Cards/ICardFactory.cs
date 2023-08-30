using Features.GameDesign.Units;

namespace Features.Cards
{
    public interface ICardFactory
    {
        Card Create(string unitKey, CardTier tier);
    }
}