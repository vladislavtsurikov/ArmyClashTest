using System;
using System.Collections.Generic;
using UniRx;
using ArmyClash.Battle.Data;
using VladislavTsurikov.EntityDataAction.Runtime.Core;

namespace ArmyClash.Battle.Services
{
    public sealed class BattleTeamRoster : IDisposable
    {
        private readonly List<EntityMonoBehaviour> _leftEntities = new();
        private readonly List<EntityMonoBehaviour> _rightEntities = new();
        private readonly Subject<Unit> _rosterChanged = new Subject<Unit>();

        public IReadOnlyList<EntityMonoBehaviour> LeftEntities => _leftEntities;
        public IReadOnlyList<EntityMonoBehaviour> RightEntities => _rightEntities;
        public IObservable<Unit> RosterChanged => _rosterChanged;

        public int LeftCount => _leftEntities.Count;
        public int RightCount => _rightEntities.Count;

        public void Register(EntityMonoBehaviour entity)
        {
            var team = entity.GetData<TeamData>();

            if (team.TeamId == 0)
            {
                if (!_leftEntities.Contains(entity))
                {
                    _leftEntities.Add(entity);
                    _rosterChanged.OnNext(Unit.Default);
                }
                return;
            }

            if (!_rightEntities.Contains(entity))
            {
                _rightEntities.Add(entity);
                _rosterChanged.OnNext(Unit.Default);
            }
        }

        public void Unregister(EntityMonoBehaviour entity)
        {
            _leftEntities.Remove(entity);
            _rightEntities.Remove(entity);
            _rosterChanged.OnNext(Unit.Default);
        }

        public void Clear()
        {
            _leftEntities.Clear();
            _rightEntities.Clear();
            _rosterChanged.OnNext(Unit.Default);
        }

        public void Dispose()
        {
            _rosterChanged.OnCompleted();
            _rosterChanged.Dispose();
        }
    }
}
