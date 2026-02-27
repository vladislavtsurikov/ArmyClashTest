using System;
using ArmyClash.Battle.Data;
using OdinSerializer;
using UniRx;
using UnityEngine;
using VladislavTsurikov.ActionFlow.Runtime.Modifier;
using VladislavTsurikov.ActionFlow.Runtime.Stats;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.ReflectionUtility;

namespace ArmyClash.Battle.Actions
{
    [RequiresData(typeof(ModifiersData))]
    [Name("Battle/Modifiers/ApplyShape")]
    public sealed class ApplyShapeModifierAction : EntityMonoBehaviourAction
    {
        [OdinSerialize]
        private Transform _root;

        private IDisposable _subscription;
        private GameObject _instance;

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
            UnityEngine.Object.Destroy(_instance);
            _instance = null;

            Transform root = _root != null ? _root : EntityMonoBehaviour.transform;
            _instance = UnityEngine.Object.Instantiate(((ShapeModifier)effect.Modifier).Prefab, root, false);
        }
    }
}
