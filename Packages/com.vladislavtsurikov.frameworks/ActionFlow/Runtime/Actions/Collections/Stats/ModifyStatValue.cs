using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VladislavTsurikov.ActionFlow.Runtime.Stats;
using VladislavTsurikov.ReflectionUtility;

namespace VladislavTsurikov.ActionFlow.Runtime.Actions.Stats
{
    [Name("Stats/Modify Stat Value")]
    public sealed class ModifyStatValue : Action
    {
        public enum Operation
        {
            Add,
            Subtract
        }

        [SerializeField] private StatValue _statValue;
        [SerializeField] private Operation _operation = Operation.Add;
        [SerializeField] private float _amount;

        public override string Name => _statValue == null
            ? "Modify Stat Value"
            : $"{_operation} {_amount} to {_statValue.name}";

        protected override UniTask<bool> Run(CancellationToken token)
        {
            if (_statValue == null)
            {
                return UniTask.FromResult(false);
            }

            float value = _statValue.Value;
            value = _operation == Operation.Add ? value + _amount : value - _amount;
            _statValue.Value = value;

            return UniTask.FromResult(true);
        }
    }
}
