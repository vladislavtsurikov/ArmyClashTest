using System;
using ArmyClash.Battle.Data;
using UnityEngine;
using UnityEngine.UIElements;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using ArmyClash.MegaWorldGrid;

namespace ArmyClash.Battle
{
    public sealed class BattleWorldEntity : EntityMonoBehaviour
    {
        [Header("Spawn")]
        [SerializeField] private GridSpawnerPair _spawnerPair;

        [Header("UI")]
        [SerializeField] private UIDocument _uiDocument;

        public GridSpawnerPair SpawnerPair => _spawnerPair;
        public UIDocument UiDocument => _uiDocument;

        protected override Type[] ComponentDataTypesToCreate()
        {
            return new[]
            {
                typeof(BattleWorldAutoRandomizeData),
                typeof(BattleWorldSpeedData),
                typeof(BattleWorldAttackDistanceData)
            };
        }

        protected override Type[] ActionTypesToCreate()
        {
            return new[]
            {
                typeof(BattleWorldRosterAction),
                typeof(BattleWorldStateAction),
                typeof(BattleWorldSpawnAction),
                typeof(BattleWorldUiSyncAction),
                typeof(BattleWorldInputAction),
                typeof(BattleWorldAutoRandomizeAction),
                typeof(BattleWorldSpeedProxySyncAction)
            };
        }
    }
}
