using System.Threading;
using ArmyClash.Battle.Config;
using ArmyClash.Battle.UI.Data;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration;
using VladislavTsurikov.ReflectionUtility;
using Zenject;

namespace ArmyClash.Battle.UI.Actions
{
    [RunOnDirtyData(typeof(BattleSpeedData))]
    [RequiresData(typeof(BattleSpeedData))]
    [Name("UI/ArmyClash/ApplyBattleSpeedTimeScaleAction")]
    public sealed class ApplySpeedTimeScaleAction : UIToolkitAction
    {
        [Inject]
        private BattleWorldSpeedConfig _config;

        private BattleSpeed _lastSpeed = BattleSpeed.Normal;

        protected override UniTask<bool> Run(CancellationToken token)
        {
            BattleSpeedData data = Get<BattleSpeedData>();
            if (data.Speed == _lastSpeed)
            {
                return UniTask.FromResult(true);
            }

            _lastSpeed = data.Speed;
            var fastScale = _config != null ? _config.FastTimeScale : 1f;
            Time.timeScale = _lastSpeed == BattleSpeed.Fast ? fastScale : 1f;
            return UniTask.FromResult(true);
        }

        protected override void OnDisable() => Time.timeScale = 1f;
    }
}
