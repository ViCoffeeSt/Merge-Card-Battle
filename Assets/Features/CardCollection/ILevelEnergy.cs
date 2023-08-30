namespace Features.CardCollection
{
    public interface ILevelEnergy
    {
        bool EnoughEnergy { get; }
        void EnergyConsumptionPerClick();
    }
}