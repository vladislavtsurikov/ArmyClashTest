using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ArmyClash.Battle;
using ArmyClash.Battle.Data;
using ArmyClash.Battle.Services;
using Zenject;

namespace ArmyClash.MegaWorldGrid
{
    public partial class GridSpawnerPair
    {
        [Inject] private BattleTeamRoster _roster;

        private CancellationTokenSource _spawnTokenSource;

        public void RandomizeArmies()
        {
            _spawnTokenSource?.Cancel();
            _spawnTokenSource = new CancellationTokenSource();

            RandomizeArmiesAsync(_spawnTokenSource.Token).Forget();
        }

        public async UniTask RandomizeArmiesAsync(CancellationToken token)
        {
            try
            {
                ClearSpawnedObjects();

                _roster?.Clear();

                var (leftObjects, rightObjects) = await SpawnBothAsync(token);

                var left = CollectBattleEntities(leftObjects);
                var right = CollectBattleEntities(rightObjects);

                for (int i = 0; i < left.Count; i++)
                {
                    var entity = left[i];
                    SetTeamId(entity, 0);
                    PrepareSpawnedEntity(entity);
                    _roster?.Register(entity);
                }

                for (int i = 0; i < right.Count; i++)
                {
                    var entity = right[i];
                    SetTeamId(entity, 1);
                    PrepareSpawnedEntity(entity);
                    _roster?.Register(entity);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private static List<BattleEntity> CollectBattleEntities(List<GameObject> objects)
        {
            var entities = new List<BattleEntity>();
            if (objects == null)
            {
                return entities;
            }

            for (int i = 0; i < objects.Count; i++)
            {
                var battleEntity = objects[i] != null ? objects[i].GetComponent<BattleEntity>() : null;
                if (battleEntity == null)
                {
                    continue;
                }

                entities.Add(battleEntity);
            }

            return entities;
        }

        private static void SetTeamId(BattleEntity entity, int teamId)
        {
            if (entity == null)
            {
                return;
            }

            var team = entity.GetData<TeamData>();
            if (team == null)
            {
                return;
            }

            team.TeamId = teamId;
        }

        private void PrepareSpawnedEntity(BattleEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            if (!entity.IsSetup)
            {
                entity.Setup();
            }

            var target = entity.GetData<TargetData>();
            target.Target = null;

            var life = entity.GetData<LifeData>();
            life.IsDead = false;
        }
    }
}
