using System;
using System.Collections.Generic;
using System.Threading;
using ArmyClash.Battle.Data;
using Cysharp.Threading.Tasks;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.MegaWorld.Runtime.Core.SelectionDatas.Group;
using VladislavTsurikov.MegaWorld.Runtime.GridSpawner;
using ArmyClash.UIToolkit.Data;
using VladislavTsurikov.MegaWorld.Runtime.Core.SelectionDatas.Group.Prototypes.PrototypeGameObject;

namespace ArmyClash.Battle
{
    public sealed class BattleWorldSpawnAction : EntityMonoBehaviourAction
    {
        private CancellationTokenSource _spawnTokenSource;

        private BattleWorldEntity World => Host as BattleWorldEntity;
        private BattleWorldRosterAction Roster => Entity?.GetAction<BattleWorldRosterAction>();
        private BattleWorldStateAction State => Entity?.GetAction<BattleWorldStateAction>();
        private BattleWorldUiSyncAction UiSync => Entity?.GetAction<BattleWorldUiSyncAction>();

        protected override void OnEnable()
        {
        }

        protected override void OnDisable()
        {
        }

        public void RandomizeArmies()
        {
            UiSync?.SetSimulationState(SimulationState.Idle);

            _spawnTokenSource?.Cancel();
            _spawnTokenSource = new CancellationTokenSource();

            SpawnArmiesAsync(_spawnTokenSource.Token).Forget();
        }

        public async UniTask SpawnArmiesAsync(CancellationToken token)
        {
            try
            {
                ClearSpawnedObjects();

                Roster?.ClearEntities();

                if (World == null || World.SpawnerPair == null || World.SpawnerPair.Left == null || World.SpawnerPair.Right == null)
                {
                    UiSync?.UpdateArmyCountUI();
                    return;
                }

                List<BattleEntity> left = await SpawnFromSpawner(World.SpawnerPair.Left, token);
                List<BattleEntity> right = await SpawnFromSpawner(World.SpawnerPair.Right, token);

                for (int i = 0; i < left.Count; i++)
                {
                    var entity = left[i];
                    SetTeamId(entity, 0);
                    Roster?.RegisterEntity(entity);
                }

                for (int i = 0; i < right.Count; i++)
                {
                    var entity = right[i];
                    SetTeamId(entity, 1);
                    Roster?.RegisterEntity(entity);
                }

                UiSync?.UpdateArmyCountUI();
            }
            catch (OperationCanceledException)
            {
                UiSync?.UpdateArmyCountUI();
            }
        }

        private async UniTask<List<BattleEntity>> SpawnFromSpawner(GridSpawner spawner, CancellationToken token)
        {
            var entities = new List<BattleEntity>();
            if (spawner == null)
            {
                return entities;
            }

            var gridGenerator = spawner.GridGenerator;
            var config = spawner.Config;
            if (gridGenerator == null || config == null)
            {
                return entities;
            }

            gridGenerator.Build(config, spawner.transform.position, spawner.transform.right, spawner.transform.forward,
                spawner.transform.rotation);
            if (gridGenerator.Slots.Count == 0)
            {
                return entities;
            }

            for (int i = 0; i < spawner.Data.GroupList.Count; i++)
            {
                Group group = spawner.Data.GroupList[i];
                if (group == null)
                {
                    continue;
                }

                var spawned = await GridSpawnUtility.SpawnGroup(token, group, gridGenerator);
                for (int j = 0; j < spawned.Count; j++)
                {
                    var battleEntity = spawned[j] != null ? spawned[j].GetComponent<BattleEntity>() : null;
                    if (battleEntity == null)
                    {
                        continue;
                    }

                    entities.Add(battleEntity);
                }
            }

            return entities;
        }

        private static void SetTeamId(BattleEntity entity, int teamId)
        {
            if (entity == null)
            {
                return;
            }

            var team = entity.GetData<BattleTeamData>();
            if (team == null)
            {
                return;
            }

            team.TeamId = teamId;
        }

        private void ClearSpawnedObjects()
        {
            if (World == null || World.SpawnerPair == null)
            {
                return;
            }

            ClearSpawnerObjects(World.SpawnerPair.Left);
            ClearSpawnerObjects(World.SpawnerPair.Right);
        }

        private static void ClearSpawnerObjects(GridSpawner spawner)
        {
            if (spawner == null)
            {
                return;
            }

            for (int i = 0; i < spawner.Data.GroupList.Count; i++)
            {
                Group group = spawner.Data.GroupList[i];
                if (group == null)
                {
                    continue;
                }

                var container = group.GetDefaultElement<ContainerForGameObjects>();
                container?.DestroyGameObjects();
            }
        }
    }
}
