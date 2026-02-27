using System;
using ArmyClash.Battle.Data;
using UniRx;
using UnityEngine;
using VladislavTsurikov.ActionFlow.Runtime.Modifier;
using VladislavTsurikov.ActionFlow.Runtime.Stats;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.Actions
{
    [RequiresData(typeof(ModifiersData))]
    [Name("Battle/Modifiers/ApplyColor")]
    public sealed class ApplyColorModifierAction : EntityMonoBehaviourAction
    {
        private IDisposable _subscription;

        protected override void Awake()
        {
            ModifiersData data = Get<ModifiersData>();
            _subscription?.Dispose();
            _subscription = data.Effects.ObserveAdd().Subscribe(change => Apply(change.Value));
        }

        protected override void OnDisable()
        {
            _subscription?.Dispose();
            _subscription = null;
        }

        private void Apply(ModifierStatEffect effect)
        {
            ColorModifier colorModifier = (ColorModifier)effect.Modifier;
            Renderer renderer = EntityMonoBehaviour.GetComponentInChildren<Renderer>();
            renderer.material.color = colorModifier.Color;
        }
    }
}
