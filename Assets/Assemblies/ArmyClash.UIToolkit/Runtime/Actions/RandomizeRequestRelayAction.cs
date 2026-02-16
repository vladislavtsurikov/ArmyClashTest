using System.Threading;
using Cysharp.Threading.Tasks;
using ArmyClash.UIToolkit.Data;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.UIToolkit.Actions
{
    [RunOnDirtyData(typeof(RandomizeRequestData))]
    [RequiresData(typeof(RandomizeRequestData))]
    [Name("UI/ArmyClash/RandomizeRequestRelayAction")]
    public sealed class RandomizeRequestRelayAction : UIToolkitAction
    {
        private int _lastRequestId;

        protected override UniTask<bool> Run(CancellationToken token)
        {
            RandomizeRequestData data = Get<RandomizeRequestData>();
            if (data.RequestId == _lastRequestId)
            {
                return UniTask.FromResult(true);
            }

            _lastRequestId = data.RequestId;
            BattleWorldSignals.RandomizeRequested?.Invoke();
            return UniTask.FromResult(true);
        }
    }
}
