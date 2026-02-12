using System;
using System.Collections.Generic;
using UnityEngine;
using VladislavTsurikov.ActionFlow.Runtime.Modifier;
using VladislavTsurikov.ActionFlow.Runtime.Stats;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Shared.Runtime.Stats;
using ArmyClash.Battle.Actions;
using ArmyClash.Battle.Data;
using ArmyClash.Battle.Modifiers;

namespace ArmyClash.Battle
{
    public sealed class BattleEntity : EntityMonoBehaviour
    {
        public void Initialize(BattleController controller, int teamId, StatCollection statCollection,
            IReadOnlyList<Modifier> modifiers)
        {
            if (!IsSetup)
            {
                Setup();
            }

            var context = GetData<BattleContextData>();
            if (context != null)
            {
                context.Controller = controller;
            }

            var team = GetData<BattleTeamData>();
            if (team != null)
            {
                team.TeamId = teamId;
            }

            var target = GetData<BattleTargetData>();
            if (target != null)
            {
                target.Target = null;
            }

            var life = GetData<BattleLifeData>();
            if (life != null)
            {
                life.IsDead = false;
            }

            var stats = GetData<StatsEntityData>();
            if (stats != null)
            {
                stats.Collection = statCollection;
                ApplyModifiers(stats, modifiers);
            }
        }

        protected override Type[] ComponentDataTypesToCreate()
        {
            return new[]
            {
                typeof(StatsEntityData),
                typeof(BattleContextData),
                typeof(BattleTeamData),
                typeof(BattleTargetData),
                typeof(BattleLifeData)
            };
        }

        protected override Type[] ActionTypesToCreate()
        {
            return new[]
            {
                typeof(BattleTargetAction),
                typeof(BattleMoveAction),
                typeof(BattleAttackAction),
                typeof(BattleDeathAction)
            };
        }

        private static void ApplyModifiers(StatsEntityData stats, IReadOnlyList<Modifier> modifiers)
        {
            if (stats == null || modifiers == null)
            {
                return;
            }

            for (int i = 0; i < modifiers.Count; i++)
            {
                Modifier modifier = modifiers[i];
                if (modifier == null)
                {
                    continue;
                }

                StatEffect effect = modifier.StatEffect;
                if (effect == null)
                {
                    continue;
                }

                var entries = effect.Entries;
                if (entries == null)
                {
                    continue;
                }

                for (int j = 0; j < entries.Count; j++)
                {
                    Stat stat = entries[j].Stat;
                    if (stat == null)
                    {
                        continue;
                    }

                    stats.AddStatValue(stat, entries[j].Delta);
                }
            }
        }
    }
}
