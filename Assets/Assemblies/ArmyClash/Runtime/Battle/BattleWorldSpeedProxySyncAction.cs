using ArmyClash.Battle.Data;
using ArmyClash.Battle.Ui;
using ArmyClash.UIToolkit.Data;
using UniRx;
using UnityEngine.UIElements;
using VladislavTsurikov.EntityDataAction.Runtime.Core;

namespace ArmyClash.Battle
{
    public sealed class BattleWorldSpeedProxySyncAction : EntityMonoBehaviourAction
    {
        private BattleUIToolkitEntity _uiEntity;
        private BattleUIView _uiView;
        private readonly CompositeDisposable _subscriptions = new CompositeDisposable();

        private BattleWorldEntity World => Host as BattleWorldEntity;

        protected override void OnEnable()
        {
            _subscriptions.Clear();

            var worldSpeed = Get<BattleWorldSpeedData>();
            if (worldSpeed == null)
            {
                return;
            }

            worldSpeed.FastTimeScaleReactive
                .Subscribe(ApplySpeedToProxy)
                .AddTo(_subscriptions);

            ApplySpeedToProxy(worldSpeed.FastTimeScale);
        }

        protected override void OnDisable()
        {
            _subscriptions.Clear();
        }

        private void ApplySpeedToProxy(float value)
        {
            var uiEntity = GetUiEntity();
            if (uiEntity == null)
            {
                return;
            }

            var proxy = uiEntity.GetData<WorldSpeedProxyData>();
            if (proxy != null)
            {
                proxy.FastTimeScale = value;
            }
        }

        private BattleUIToolkitEntity GetUiEntity()
        {
            if (_uiEntity != null)
            {
                return _uiEntity;
            }

            ResolveUIView();
            return _uiEntity;
        }

        private void ResolveUIView()
        {
            _uiView = null;
            _uiEntity = null;

            if (World == null || World.UiDocument == null)
            {
                return;
            }

            VisualElement root = World.UiDocument.rootVisualElement;
            if (root == null)
            {
                return;
            }

            _uiView = root.Q<BattleUIView>("battleView") ?? root.Q<BattleUIView>();
            _uiEntity = _uiView != null ? _uiView.Entity : null;
        }
    }
}
