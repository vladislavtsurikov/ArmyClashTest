using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.MegaWorld.Runtime.Core.SelectionDatas.Group;
using VladislavTsurikov.MegaWorld.Runtime.GridSpawner;
using ArmyClash.UIToolkit.Actions;
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

        protected override void OnEnable()
        {
            BattleWorldSignals.RandomizeRequested += RandomizeArmies;

            var settings = Get<Battle.Data.BattleWorldAutoRandomizeData>();
            if (settings != null && settings.AutoRandomizeOnAwake)
            {
                RandomizeArmies();
            }
        }

        protected override void OnDisable()
        {
            BattleWorldSignals.RandomizeRequested -= RandomizeArmies;
        }

        public void RandomizeArmies()
        {
            State?.SetSimulationState(SimulationState.Idle);

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
                    State?.UpdateArmyCountUi();
                    return;
                }

                List<BattleEntity> left = await SpawnFromSpawner(World.SpawnerPair.Left, token);
                List<BattleEntity> right = await SpawnFromSpawner(World.SpawnerPair.Right, token);

                for (int i = 0; i < left.Count; i++)
                {
                    Roster?.RegisterEntity(left[i]);
                }

                for (int i = 0; i < right.Count; i++)
                {
                    Roster?.RegisterEntity(right[i]);
                }

                State?.UpdateArmyCountUi();
            }
            catch (OperationCanceledException)
            {
                State?.UpdateArmyCountUi();
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
