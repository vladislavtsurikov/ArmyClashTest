using System.Threading;
using ArmyClash.Battle.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using Zenject;

namespace ArmyClash.MegaWorldGrid
{
    public partial class GridSpawnerPair
    {
        [Inject]
        private BattleTeamRoster _roster;

        private CancellationTokenSource _spawnTokenSource;

        public void RandomizeArmies()
        {
            _spawnTokenSource?.Cancel();
            _spawnTokenSource = new CancellationTokenSource();

            RandomizeArmiesAsync(_spawnTokenSource.Token).Forget();
        }

        public async UniTask RandomizeArmiesAsync(CancellationToken token)
        {
            ClearSpawnedObjects();

            _roster?.Clear();

            await SpawnBoth(
                token,
                false,
                RegisterSpawned);
        }

        private void RegisterSpawned(GameObject go)
        {
            EntityMonoBehaviour entity = go.GetComponent<EntityMonoBehaviour>();
            _roster?.Register(entity);
        }
    }
}
