using System;
using System.Threading;
using ArmyClash.Grid;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VladislavTsurikov.MegaWorld.Runtime.Core.SelectionDatas.Group.Prototypes.PrototypeGameObject;
using VladislavTsurikov.MegaWorld.Runtime.GridSpawner;

namespace ArmyClash.MegaWorldGrid
{
    public partial class GridSpawnerPair : MonoBehaviour
    {
        [SerializeField]
        private GridConfig _config = new();

        [SerializeField]
        private GridSpawner _leftSpawner;

        [SerializeField]
        private GridSpawner _rightSpawner;

        public async UniTask SpawnBoth(
            CancellationToken token,
            bool displayProgressBar,
            Action<GameObject> onSpawn)
        {
            ApplyConfig();

            if (_leftSpawner != null)
            {
                await _leftSpawner.SpawnAndCollect(token, displayProgressBar, onSpawn);
            }

            if (_rightSpawner != null)
            {
                await _rightSpawner.SpawnAndCollect(token, displayProgressBar, onSpawn);
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
            _leftSpawner?.ApplyConfig(_config);
            _rightSpawner?.ApplyConfig(_config);
        }
    }
}
