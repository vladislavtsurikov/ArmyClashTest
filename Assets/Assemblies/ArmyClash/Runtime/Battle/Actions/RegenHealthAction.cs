using ArmyClash.Battle.Data;
using UnityEngine;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Shared.Runtime.Stats;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.Actions
{
    [RequiresData(typeof(StatsEntityData), typeof(BattleStatIdsData), typeof(BattleLifeData))]
    [Name("Action/RegenHealth")]
    public sealed class RegenHealthAction : CombatEntityAction
    {
        protected override void Update()
        {
            var stateAction = GetStateAction();
            if (!stateAction.IsRunning)
            {
                return;
            }

            if (IsDead())
            {
                return;
            }

            var stats = GetStats();
            var ids = GetStatIds();

            if (!stats.TryGetStatValueById(ids.RegenId, out float regen))
            {
                return;
            }

            if (regen <= 0f)
            {
                return;
            }

            if (!string.IsNullOrEmpty(ids.HealthId))
            {
                stats.AddStatValueById(ids.HealthId, regen * Time.deltaTime);
            }
        }
    }
}
