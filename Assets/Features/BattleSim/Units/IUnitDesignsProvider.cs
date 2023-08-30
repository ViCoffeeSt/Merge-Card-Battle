using Features.GameDesign.Units;

namespace Features.BattleSim.Units
{
    public interface IUnitDesignsProvider
    {
        UnitDesignObject GetDesign(string unitKey);
    }
}