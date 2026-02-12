using System.Threading;
using Cysharp.Threading.Tasks;
using OdinSerializer;
using VladislavTsurikov.ActionFlow.Runtime.Stats;
using VladislavTsurikov.EntityDataAction.Runtime;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace VladislavTsurikov.EntityDataAction.Shared.Runtime.Stats
{
    [RequiresData(typeof(StatsEntityData))]
    [Name("Stats/Apply Modifier Stat Effect Action")]
    public sealed class ApplyModifierStatEffectAction : EntityAction
    {
        [OdinSerialize]
        private ModifierStatEffect _effect;

        protected override UniTask<bool> Run(CancellationToken token)
        {
            if (_effect == null)
            {
                return UniTask.FromResult(true);
            }

            StatsEntityData data = Get<StatsEntityData>();
            if (data == null)
            {
                return UniTask.FromResult(true);
            }

            var entries = _effect.Entries;
            if (entries == null)
            {
                return UniTask.FromResult(true);
            }

            for (int i = 0; i < entries.Count; i++)
            {
                Stat stat = entries[i].Stat;
                if (stat == null)
                {
                    continue;
                }

                data.AddStatValue(stat, entries[i].Delta);
            }

            return UniTask.FromResult(true);
        }
    }
}
