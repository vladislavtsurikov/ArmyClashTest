using System.Threading;
using Cysharp.Threading.Tasks;
using ArmyClash.UIToolkit.Data;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.UIToolkit.Actions
{
    [RunOnDirtyData(typeof(StartRequestData))]
    [RequiresData(typeof(StartRequestData))]
    [Name("UI/ArmyClash/StartRequestRelayAction")]
    public sealed class StartRequestRelayAction : UIToolkitAction
    {
        private int _lastRequestId;

        protected override UniTask<bool> Run(CancellationToken token)
        {
            StartRequestData data = Get<StartRequestData>();
            if (data.RequestId == _lastRequestId)
            {
                return UniTask.FromResult(true);
            }

            _lastRequestId = data.RequestId;
            BattleWorldSignals.StartRequested?.Invoke();
            return UniTask.FromResult(true);
        }
    }
}
