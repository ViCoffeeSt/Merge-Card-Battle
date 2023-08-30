using UnityEngine;
using UnityEngine.UI;

namespace Features.Shared
{
    public class LoadSceneButtonBehaviour : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private string _sceneName;

        private void Start()
        {
            if (string.IsNullOrEmpty(_sceneName))
            {
                return;
            }

            _button.onClick.AddListener(() => Locator.Instance<ISceneService>().LoadScene(_sceneName));
        }
    }
}