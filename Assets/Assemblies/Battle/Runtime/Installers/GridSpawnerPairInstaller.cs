using UnityEngine;
using Zenject;

namespace ArmyClash.MegaWorldGrid
{
    public sealed class GridSpawnerPairInstaller : MonoInstaller
    {
        [SerializeField]
        private GridSpawnerPair _pair;

        public override void InstallBindings()
        {
            Container.Bind<GridSpawnerPair>().FromInstance(_pair).AsSingle();
        }
    }
}
