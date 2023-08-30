using System.Collections.Generic;
using UnityEngine;

namespace Features.Shared
{
    public class CachingProvider<T> : IAssetProvider<T> where T : Object
    {
        private readonly Repository<T> _repository;
        private Dictionary<string, T> _cache = new(24);

        public CachingProvider(Repository<T> repository)
        {
            _repository = repository;
        }

        public T GetAssets(string assetName)
        {
            if (_cache.TryGetValue(assetName, out var asset))
            {
                return asset;
            }

            asset = _repository.Get(assetName);
            _cache.Add(assetName, asset);

            return asset;
        }

        public void ClearAll() => _cache.Clear();
    }
}