namespace Features.Shared
{
    public sealed class ReloadLevelCommand : ICommand
    {
        public void Execute() => Locator.Instance<ISceneService>().Reload();
    }
}