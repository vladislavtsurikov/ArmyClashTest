using System.Threading;
using Cysharp.Threading.Tasks;
using ArmyClash.Battle.Config;
using ArmyClash.UIToolkit.Data;
using UnityEngine;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration;
using VladislavTsurikov.ReflectionUtility;
using Zenject;

namespace ArmyClash.UIToolkit.Actions
{
    [RunOnDirtyData(typeof(BattleSpeedData))]
    [RequiresData(typeof(BattleSpeedData))]
    [Name("UI/ArmyClash/ApplyBattleSpeedTimeScaleAction")]
    public sealed class ApplySpeedTimeScaleAction : UIToolkitAction
    {
        private BattleSpeed _lastSpeed = BattleSpeed.Normal;

        [Inject] private BattleWorldSpeedConfig _config;

        protected override UniTask<bool> Run(CancellationToken token)
        {
            var data = Get<BattleSpeedData>();
            if (data.Speed == _lastSpeed)
            {
                return UniTask.FromResult(true);
            }

            _lastSpeed = data.Speed;
            float fastScale = _config != null ? _config.FastTimeScale : 1f;
            Time.timeScale = _lastSpeed == BattleSpeed.Fast ? fastScale : 1f;
            return UniTask.FromResult(true);
        }

        protected override void OnDisable()
        {
            Time.timeScale = 1f;
        }
    }
}
