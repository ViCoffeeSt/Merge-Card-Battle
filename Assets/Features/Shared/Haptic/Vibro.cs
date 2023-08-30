namespace Features.Shared.Haptic
{
    public static class Vibro
    {
        public static bool IsEnabled
        {
            get
            {
                var instance = VibrationManager.Instance;
                return instance != null && instance.IsEnabled;
            }
            set
            {
                var instance = VibrationManager.Instance;
                if (instance != null)
                {
                    instance.IsEnabled = value;
                }
            }
        }

        public static void Vibrate(VibrationPattern pattern = VibrationPattern.Default)
        {
            var instance = VibrationManager.Instance;
            if (instance != null)
            {
                instance.Vibrate(pattern);
            }
        }
    }
}