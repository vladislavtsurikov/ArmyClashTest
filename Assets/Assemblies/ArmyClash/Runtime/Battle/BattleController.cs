using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VladislavTsurikov.ActionFlow.Runtime.Stats;
using VladislavTsurikov.EntityDataAction.Shared.Runtime.Stats;
using VladislavTsurikov.MegaWorld.Runtime.Core.SelectionDatas.Group;
using VladislavTsurikov.MegaWorld.Runtime.Core.SelectionDatas.Group.Prototypes.PrototypeGameObject;
using VladislavTsurikov.MegaWorld.Runtime.GridSpawner;
using ArmyClash.Battle.Data;
using ArmyClash.Battle.Ui;
using ArmyClash.MegaWorldGrid;
using ArmyClash.UIToolkit.Data;

namespace ArmyClash.Battle
{
    public sealed class BattleController : MonoBehaviour
    {
        [Header("Spawn")]
        [SerializeField] private GridSpawnerPair _spawnerPair;
        [SerializeField] private bool _autoRandomizeOnAwake = true;

        [Header("UI")]
        [SerializeField] private BattleUIDriver _uiDriver;
        [SerializeField] private float _fastTimeScale = 2f;

        [Header("Stats")]
        [SerializeField] private StatCollection _statCollection;
        [SerializeField] private Stat _healthStat;
        [SerializeField] private Stat _attackStat;
        [SerializeField] private Stat _speedStat;
        [SerializeField] private Stat _attackSpeedStat;
        [SerializeField] private Stat _regenStat;

        [Header("Battle")]
        [SerializeField] private float _attackRange = 1.2f;
        [SerializeField] private float _stopDistance = 0.1f;

        private readonly List<BattleEntity> _leftEntities = new();
        private readonly List<BattleEntity> _rightEntities = new();

        private int _lastStartRequestId;
        private int _lastRandomizeRequestId;
        private BattleSpeed _lastBattleSpeed = BattleSpeed.Normal;

        private CancellationTokenSource _spawnTokenSource;

        public bool IsRunning { get; private set; }
        public float AttackRange => _attackRange;
        public float StopDistance => _stopDistance;

        private void Awake()
        {
            SetSimulationState(SimulationState.Idle);

            if (_autoRandomizeOnAwake)
            {
                RandomizeArmies();
            }
        }

        private void Update()
        {
            HandleUiRequests();
            UpdateBattleSpeed();
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
        }

        public void RandomizeArmies()
        {
            IsRunning = false;
            SetSimulationState(SimulationState.Idle);

            _spawnTokenSource?.Cancel();
            _spawnTokenSource = new CancellationTokenSource();

            SpawnArmiesAsync(_spawnTokenSource.Token).Forget();
        }

        public void StartBattle()
        {
            if (_leftEntities.Count == 0 || _rightEntities.Count == 0)
            {
                return;
            }

            IsRunning = true;
            SetSimulationState(SimulationState.Running);
        }

        public void HandleEntityDeath(BattleEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            RemoveFromLists(entity);
            UpdateArmyCountUi();

            Destroy(entity.gameObject);

            if (_leftEntities.Count == 0 || _rightEntities.Count == 0)
            {
                FinishBattle();
            }
        }

        public bool IsEntityAlive(BattleEntity entity)
        {
            if (entity == null)
            {
                return false;
            }

            var life = entity.GetData<BattleLifeData>();
            return life == null || !life.IsDead;
        }

        public BattleEntity FindClosestOpponent(BattleEntity requester)
        {
            if (requester == null)
            {
                return null;
            }

            var team = requester.GetData<BattleTeamData>();
            if (team == null)
            {
                return null;
            }

            List<BattleEntity> list = team.TeamId == 0 ? _rightEntities : _leftEntities;
            if (list.Count == 0)
            {
                return null;
            }

            Vector3 position = requester.transform.position;
            float bestSqr = float.MaxValue;
            BattleEntity best = null;

            for (int i = 0; i < list.Count; i++)
            {
                var candidate = list[i];
                if (candidate == null || !IsEntityAlive(candidate))
                {
                    continue;
                }

                float sqr = (candidate.transform.position - position).sqrMagnitude;
                if (sqr < bestSqr)
                {
                    bestSqr = sqr;
                    best = candidate;
                }
            }

            return best;
        }

        public bool TryGetHealth(StatsEntityData stats, out float value) => TryGetStat(stats, _healthStat, out value);
        public bool TryGetAttack(StatsEntityData stats, out float value) => TryGetStat(stats, _attackStat, out value);
        public bool TryGetSpeed(StatsEntityData stats, out float value) => TryGetStat(stats, _speedStat, out value);
        public bool TryGetAttackSpeed(StatsEntityData stats, out float value) => TryGetStat(stats, _attackSpeedStat, out value);
        public bool TryGetRegen(StatsEntityData stats, out float value) => TryGetStat(stats, _regenStat, out value);

        public void ApplyDamage(BattleEntity target, float damage)
        {
            if (target == null || damage <= 0f)
            {
                return;
            }

            var stats = target.GetData<StatsEntityData>();
            if (stats == null || _healthStat == null)
            {
                return;
            }

            stats.AddStatValue(_healthStat, -damage);
        }

        public void ApplyHealing(BattleEntity target, float amount)
        {
            if (target == null || amount <= 0f)
            {
                return;
            }

            var stats = target.GetData<StatsEntityData>();
            if (stats == null || _healthStat == null)
            {
                return;
            }

            stats.AddStatValue(_healthStat, amount);
        }

        private bool TryGetStat(StatsEntityData stats, Stat stat, out float value)
        {
            value = 0f;
            if (stats == null || stat == null)
            {
                return false;
            }

            return stats.TryGetStatValue(stat, out value);
        }

        private void HandleUiRequests()
        {
            var uiEntity = _uiDriver != null ? _uiDriver.Entity : null;
            if (uiEntity == null)
            {
                return;
            }

            var startRequest = uiEntity.GetData<StartRequestData>();
            if (startRequest != null && startRequest.RequestId != _lastStartRequestId)
            {
                _lastStartRequestId = startRequest.RequestId;
                StartBattle();
            }

            var randomizeRequest = uiEntity.GetData<RandomizeRequestData>();
            if (randomizeRequest != null && randomizeRequest.RequestId != _lastRandomizeRequestId)
            {
                _lastRandomizeRequestId = randomizeRequest.RequestId;
                RandomizeArmies();
            }
        }

        private void UpdateBattleSpeed()
        {
            var uiEntity = _uiDriver != null ? _uiDriver.Entity : null;
            if (uiEntity == null)
            {
                return;
            }

            var speedData = uiEntity.GetData<BattleSpeedData>();
            if (speedData == null)
            {
                return;
            }

            if (speedData.Speed == _lastBattleSpeed)
            {
                return;
            }

            _lastBattleSpeed = speedData.Speed;
            Time.timeScale = _lastBattleSpeed == BattleSpeed.Fast ? _fastTimeScale : 1f;
        }

        private void SetSimulationState(SimulationState state)
        {
            var uiEntity = _uiDriver != null ? _uiDriver.Entity : null;
            if (uiEntity == null)
            {
                return;
            }

            var data = uiEntity.GetData<SimulationStateData>();
            if (data != null)
            {
                data.State = state;
            }
        }

        private void UpdateArmyCountUi()
        {
            var uiEntity = _uiDriver != null ? _uiDriver.Entity : null;
            if (uiEntity == null)
            {
                return;
            }

            var data = uiEntity.GetData<ArmyCountData>();
            if (data == null)
            {
                return;
            }

            data.LeftCount = _leftEntities.Count;
            data.RightCount = _rightEntities.Count;
        }

        private void FinishBattle()
        {
            IsRunning = false;
            SetSimulationState(SimulationState.Finished);
        }

        private async UniTask SpawnArmiesAsync(CancellationToken token)
        {
            try
            {
                ClearSpawnedObjects();

                _leftEntities.Clear();
                _rightEntities.Clear();

                if (_spawnerPair == null || _spawnerPair.Left == null || _spawnerPair.Right == null)
                {
                    UpdateArmyCountUi();
                    return;
                }

                List<BattleEntity> left = await SpawnFromSpawner(_spawnerPair.Left, token);
                List<BattleEntity> right = await SpawnFromSpawner(_spawnerPair.Right, token);

                for (int i = 0; i < left.Count; i++)
                {
                    RegisterEntity(left[i], 0);
                }

                for (int i = 0; i < right.Count; i++)
                {
                    RegisterEntity(right[i], 1);
                }

                UpdateArmyCountUi();
            }
            catch (OperationCanceledException)
            {
                UpdateArmyCountUi();
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

        private void RegisterEntity(BattleEntity entity, int teamId)
        {
            if (entity == null)
            {
                return;
            }

            if (!entity.IsSetup)
            {
                entity.Setup();
            }

            var context = entity.GetData<BattleContextData>();
            if (context != null)
            {
                context.Controller = this;
            }

            var team = entity.GetData<BattleTeamData>();
            if (team != null)
            {
                team.TeamId = teamId;
            }

            var target = entity.GetData<BattleTargetData>();
            if (target != null)
            {
                target.Target = null;
            }

            var life = entity.GetData<BattleLifeData>();
            if (life != null)
            {
                life.IsDead = false;
            }

            var stats = entity.GetData<StatsEntityData>();
            if (stats != null)
            {
                stats.Collection = _statCollection;
            }

            if (teamId == 0)
            {
                if (!_leftEntities.Contains(entity))
                {
                    _leftEntities.Add(entity);
                }
            }
            else
            {
                if (!_rightEntities.Contains(entity))
                {
                    _rightEntities.Add(entity);
                }
            }
        }

        private void RemoveFromLists(BattleEntity entity)
        {
            _leftEntities.Remove(entity);
            _rightEntities.Remove(entity);
        }

        private void ClearSpawnedObjects()
        {
            if (_spawnerPair == null)
            {
                return;
            }

            ClearSpawnerObjects(_spawnerPair.Left);
            ClearSpawnerObjects(_spawnerPair.Right);
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
