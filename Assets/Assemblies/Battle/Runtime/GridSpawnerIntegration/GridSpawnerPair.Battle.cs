using System.Collections.Generic;
using System.Threading;
using ArmyClash.Battle;
using ArmyClash.Battle.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
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

            (List<GameObject> leftObjects, List<GameObject> rightObjects) = await SpawnBothAsync(token);

            List<BattleEntity> left = CollectBattleEntities(leftObjects);
            List<BattleEntity> right = CollectBattleEntities(rightObjects);

            for (var i = 0; i < left.Count; i++)
            {
                BattleEntity entity = left[i];
                _roster?.Register(entity);
            }

            for (var i = 0; i < right.Count; i++)
            {
                BattleEntity entity = right[i];
                _roster?.Register(entity);
            }
        }

        private static List<BattleEntity> CollectBattleEntities(List<GameObject> objects)
        {
            var entities = new List<BattleEntity>();

            for (var i = 0; i < objects.Count; i++)
            {
                BattleEntity battleEntity = objects[i] != null ? objects[i].GetComponent<BattleEntity>() : null;
                if (battleEntity == null)
                {
                    continue;
                }

                entities.Add(battleEntity);
            }

            return entities;
        }
    }
}
