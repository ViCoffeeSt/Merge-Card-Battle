using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.Shared
{
    public class SceneService : ISceneService
    {
        public void LoadScene(string sceneName, Action onLoaded = null)
        {
            var op = SceneManager.LoadSceneAsync(sceneName);
            op.completed += OnOperationDone;

            void OnOperationDone(AsyncOperation asyncOperation)
            {
                onLoaded?.Invoke();
            }
        }

        public void Reload(Action onLoaded = null)
        {
            var activeScene = SceneManager.GetActiveScene();
            LoadScene(activeScene.name, onLoaded);
        }
    }
}