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

        [SerializeField] private Stat _stat;
        [SerializeField] private Operation _operation = Operation.Add;
        [SerializeField] private float _amount;

        public override string Name => _stat == null
            ? "Modify Stat Value"
            : $"{_operation} {_amount} to {_stat.name}";

        protected override UniTask<bool> Run(CancellationToken token)
        {
            if (_stat == null)
            {
                return UniTask.FromResult(false);
            }

            StatValueComponent valueComponent = _stat.ComponentStack.GetElement<StatValueComponent>();
            if (valueComponent == null)
            {
                return UniTask.FromResult(false);
            }

            float value = valueComponent.BaseValue;
            value = _operation == Operation.Add ? value + _amount : value - _amount;
            valueComponent.SetBaseValue(value);

            return UniTask.FromResult(true);
        }
    }
}
