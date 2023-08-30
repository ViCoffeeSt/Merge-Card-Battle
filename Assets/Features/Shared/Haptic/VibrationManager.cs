using System;
using Lofelt.NiceVibrations;
using UnityEngine;

namespace Features.Shared.Haptic
{
    internal sealed class VibrationManager : MonoBehaviour
    {
        internal static VibrationManager Instance
        {
            get
            {
                if (_instance)
                {
                    return _instance;
                }

                var manager = new GameObject(nameof(VibrationManager)).AddComponent<VibrationManager>();
                manager.Init();
                return manager;
            }
        }

        private static VibrationManager _instance;

        private bool isInitialized;
        private bool isEnabled;

        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                if (value == isEnabled)
                {
                    return;
                }

                isEnabled = value;
                OnIsEnabledChanged(isEnabled);
            }
        }

        private void Init()
        {
            if (isInitialized)
            {
                return;
            }

            LofeltHaptics.Initialize();
            HapticController.Init();

            isInitialized = true;

            DontDestroyOnLoad(gameObject);
        }

        private void Release()
        {
            if (!isInitialized)
            {
                return;
            }

            LofeltHaptics.Release();

            isInitialized = false;
        }

        private void OnIsEnabledChanged(bool value)
        {
            if (value)
            {
                Init();
            }
            else
            {
                Release();
            }
        }


        private void Awake()
        {
            if (!_instance)
            {
                _instance = this;
                Init();
                return;
            }

            if (_instance != this)
            {
                Destroy(gameObject);
                return;
            }

            if (!isInitialized)
            {
                Init();
            }
        }

        private void OnDestroy()
        {
            Release();
        }

        public void Vibrate(VibrationPattern pattern = VibrationPattern.Default)
        {
            if (!isEnabled)
            {
                return;
            }

            switch (pattern)
            {
                case VibrationPattern.Default:
                    Handheld.Vibrate();
                    break;
                case VibrationPattern.Selection:
                    HapticPatterns.PlayPreset(HapticPatterns.PresetType.Selection);
                    break;
                case VibrationPattern.Success:
                    HapticPatterns.PlayPreset(HapticPatterns.PresetType.Success);
                    break;
                case VibrationPattern.Warning:
                    HapticPatterns.PlayPreset(HapticPatterns.PresetType.Warning);
                    break;
                case VibrationPattern.Failure:
                    HapticPatterns.PlayPreset(HapticPatterns.PresetType.Failure);
                    break;
                case VibrationPattern.LightImpact:
                    HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
                    break;
                case VibrationPattern.MediumImpact:
                    HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
                    break;
                case VibrationPattern.HeavyImpact:
                    HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);
                    break;
                case VibrationPattern.None:
                    HapticPatterns.PlayPreset(HapticPatterns.PresetType.None);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(pattern), pattern, null);
            }
        }
    }
}