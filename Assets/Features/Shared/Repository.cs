using UnityEngine;

namespace Features.Shared
{
    public class Repository<T> where T : Object
    {
        private readonly string _domain;

        public Repository(string domain)
        {
            _domain = domain.EndsWith("/") ? domain : domain + "/";
        }

        public T Get(string name)
        {
            var path = _domain + name;
            return Resources.Load<T>(path);
        }
    }
}