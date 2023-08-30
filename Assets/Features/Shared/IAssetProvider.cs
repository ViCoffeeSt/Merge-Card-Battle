using UnityEngine;

namespace Features.Shared
{
    public interface IAssetProvider<out T> : IClearable where T : Object
    {
        T GetAssets(string assetName);
    }

    public interface IClearable
    {
        void ClearAll();
    }
}