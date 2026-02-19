using Zenject;

namespace ArmyClash.MegaWorldGrid
{
    public sealed class GridSpawnerPairInstaller : MonoInstaller
    {
        public override void InstallBindings() =>
            Container.Bind<GridSpawnerPair>().FromComponentInHierarchy().AsSingle();
    }
}
