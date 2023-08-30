using System.Collections.Generic;
using Features.GameDesign.Units;
using UnityEngine;

namespace Features.BattleSim.Units
{
    public class UnitDesignsProvider : IUnitDesignsProvider
    {
        private Dictionary<string, UnitDesignObject> _cache = new(24);

        public UnitDesignObject GetDesign(string unitKey)
        {
            if (_cache.TryGetValue(unitKey, out var design))
            {
                return design;
            }

            design = Resources.Load<UnitDesignObject>("Units/" + unitKey);
            _cache.Add(unitKey, design);

            return design;
        }
    }
}