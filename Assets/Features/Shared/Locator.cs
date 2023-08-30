namespace Features.Shared
{
    public static class Locator
    {
        private static class Bind<T>
        {
            public static T BoundInstance;
        }

        public static void Add<T>(T instance) where T : class
        {
            Bind<T>.BoundInstance = instance;
        }

        public static T Instance<T>()
        {
            return Bind<T>.BoundInstance;
        }
    }
}