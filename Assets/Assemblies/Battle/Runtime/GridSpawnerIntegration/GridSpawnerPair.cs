using System;
using System.Threading;
using ArmyClash.Battle.Data;
using ArmyClash.Battle.Services;
using ArmyClash.Grid;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.MegaWorld.Runtime.Core.SelectionDatas.Group.Prototypes.PrototypeGameObject;
using VladislavTsurikov.MegaWorld.Runtime.GridSpawner;
using Zenject;

namespace ArmyClash.MegaWorldGrid
{
    public sealed class GridSpawnerPair : MonoBehaviour
    {
        [Inject]
        private BattleTeamRoster _roster;

        [SerializeField]
        private GridConfig _config;

        [SerializeField]
        private GridSpawner _leftSpawner;

        [SerializeField]
        private GridSpawner _rightSpawner;

        private CancellationTokenSource _spawnTokenSource;

        public void RespawnBoth()
        {
            _spawnTokenSource?.Cancel();
            _spawnTokenSource = new CancellationTokenSource();

            RespawnBothAsync(_spawnTokenSource.Token).Forget();
        }

        public async UniTask RespawnBothAsync(CancellationToken token)
        {
            ClearSpawnedObjects();

            _roster?.Clear();

            ApplyConfig();

            if (_leftSpawner != null)
            {
                await _leftSpawner.SpawnAndCollect(
                    token,
                    go => OnSpawned(go, 0));
            }

            if (_rightSpawner != null)
            {
                await _rightSpawner.SpawnAndCollect(
                    token,
                    go => OnSpawned(go, 1));
            }
        }

        public void ClearSpawnedObjects()
        {
            ContainerForGameObjectsUtility.DestroyGameObjects<PrototypeGameObject>(_leftSpawner.Data);
            ContainerForGameObjectsUtility.DestroyGameObjects<PrototypeGameObject>(_rightSpawner.Data);
        }

        public void SetSpawners(GridSpawner left, GridSpawner right)
        {
            _leftSpawner = left;
            _rightSpawner = right;
        }

        public void ApplyConfig()
        {
            if (_config == null)
            {
                return;
            }

            _leftSpawner?.ApplyConfig(_config);
            _rightSpawner?.ApplyConfig(_config);
        }

        private void OnSpawned(GameObject go, int teamId)
        {
            EntityMonoBehaviour entity = go.GetComponent<EntityMonoBehaviour>();
            TeamData team = entity.GetData<TeamData>();
            team.TeamId = teamId;
            _roster?.Register(entity);
        }
    }
}
