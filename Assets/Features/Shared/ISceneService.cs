using System;

namespace Features.Shared
{
    public interface ISceneService
    {
        void LoadScene(string sceneName, Action onLoaded = null);
        void Reload(Action onLoaded = null);
    }
}