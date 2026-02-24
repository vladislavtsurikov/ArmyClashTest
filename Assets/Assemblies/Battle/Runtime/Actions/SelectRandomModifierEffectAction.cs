using System.Collections.Generic;
using System.Threading;
using ArmyClash.Battle.Data;
using Cysharp.Threading.Tasks;
using OdinSerializer;
using UnityEngine;
using VladislavTsurikov.ActionFlow.Runtime.Stats;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.Actions
{
    [RequiresData(typeof(ModifiersData))]
    [Name("Battle/Modifiers/SelectRandomModifierEffect")]
    public sealed class SelectRandomModifierEffectAction : EntityLifecycleAction
    {
        [OdinSerialize]
        private List<ModifierStatEffect> _options = new();

        protected override void Awake()
        {
            int index = Random.Range(0, _options.Count);
            ModifierStatEffect effect = _options[index];

            ModifiersData data = Get<ModifiersData>();
            data.Add(effect);
        }

        protected override UniTask<bool> Run(CancellationToken token) => UniTask.FromResult(true);
    }
}
