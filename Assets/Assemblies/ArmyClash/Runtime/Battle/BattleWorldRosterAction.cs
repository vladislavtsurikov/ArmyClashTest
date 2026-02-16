using System.Collections.Generic;
using UnityEngine;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using ArmyClash.Battle.Data;

namespace ArmyClash.Battle
{
    public sealed class BattleWorldRosterAction : EntityMonoBehaviourAction
    {
        private readonly List<BattleEntity> _leftEntities = new();
        private readonly List<BattleEntity> _rightEntities = new();

        public IReadOnlyList<BattleEntity> LeftEntities => _leftEntities;
        public IReadOnlyList<BattleEntity> RightEntities => _rightEntities;

        public void RegisterEntity(BattleEntity entity)
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
                context.World = Host as BattleWorldEntity;
            }

            var team = entity.GetData<BattleTeamData>();

            var target = entity.GetData<TargetData>();
            if (target != null)
            {
                target.Target = null;
            }

            var life = entity.GetData<BattleLifeData>();
            if (life != null)
            {
                life.IsDead = false;
            }

            if (team == null)
            {
                Debug.LogWarning($"{nameof(BattleWorldRosterAction)}: {entity.name} has no {nameof(BattleTeamData)}");
                return;
            }

            if (team.TeamId == 0)
            {
                if (!_leftEntities.Contains(entity))
                {
                    _leftEntities.Add(entity);
                }
                return;
            }

            if (!_rightEntities.Contains(entity))
            {
                _rightEntities.Add(entity);
            }
        }

        public void UnregisterEntity(BattleEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            _leftEntities.Remove(entity);
            _rightEntities.Remove(entity);
        }

        public void ClearEntities()
        {
            _leftEntities.Clear();
            _rightEntities.Clear();
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

        public int LeftCount => _leftEntities.Count;
        public int RightCount => _rightEntities.Count;
    }
}
