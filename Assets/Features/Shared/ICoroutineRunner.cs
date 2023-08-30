using System.Collections;
using UnityEngine;

namespace Features.Shared
{
    internal class CoroutineRunner : MonoBehaviour, IRoutineRunner
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public interface IRoutineRunner
    {
        Coroutine StartCoroutine(IEnumerator routine);
    }
}