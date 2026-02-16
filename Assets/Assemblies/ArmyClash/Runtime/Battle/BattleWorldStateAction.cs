using ArmyClash.Battle.Ui;
using ArmyClash.UIToolkit.Actions;
using ArmyClash.UIToolkit.Data;
using UniRx;
using UnityEngine.UIElements;
using VladislavTsurikov.EntityDataAction.Runtime.Core;

namespace ArmyClash.Battle
{
    public sealed class BattleWorldStateAction : EntityMonoBehaviourAction
    {
        private ReactiveProperty<bool> _isRunning;
        private BattleUIView _uiView;
        private BattleUIToolkitEntity _uiEntity;

        private BattleWorldEntity World => Host as BattleWorldEntity;

        public bool IsRunning
        {
            get => EnsureIsRunning().Value;
            private set => EnsureIsRunning().Value = value;
        }

        public IReadOnlyReactiveProperty<bool> IsRunningReactive => EnsureIsRunning();

        public void StartBattle()
        {
            var roster = Entity?.GetAction<BattleWorldRosterAction>();
            if (roster == null || roster.LeftCount == 0 || roster.RightCount == 0)
            {
                return;
            }

            IsRunning = true;
            SetSimulationState(SimulationState.Running);
        }

        public void FinishBattle()
        {
            IsRunning = false;
            SetSimulationState(SimulationState.Finished);
        }

        public void HandleEntityDeath(BattleEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            var roster = Entity?.GetAction<BattleWorldRosterAction>();
            roster?.UnregisterEntity(entity);
            UpdateArmyCountUi();

            UnityEngine.Object.Destroy(entity.gameObject);

            if (roster == null || roster.LeftCount == 0 || roster.RightCount == 0)
            {
                FinishBattle();
            }
        }

        protected override void OnEnable()
        {
            BattleWorldSignals.StartRequested += StartBattle;
        }

        protected override void OnDisable()
        {
            BattleWorldSignals.StartRequested -= StartBattle;
        }

        public void SetSimulationState(SimulationState state)
        {
            var uiEntity = GetUiEntity();
            if (uiEntity == null)
            {
                return;
            }

            var data = uiEntity.GetData<SimulationStateData>();
            if (data != null)
            {
                data.State = state;
            }
        }

        public void UpdateArmyCountUi()
        {
            var uiEntity = GetUiEntity();
            if (uiEntity == null)
            {
                return;
            }

            var data = uiEntity.GetData<ArmyCountData>();
            if (data == null)
            {
                return;
            }

            var roster = Entity?.GetAction<BattleWorldRosterAction>();
            if (roster == null)
            {
                return;
            }

            data.LeftCount = roster.LeftCount;
            data.RightCount = roster.RightCount;
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

        private ReactiveProperty<bool> EnsureIsRunning()
        {
            if (_isRunning == null)
            {
                _isRunning = new ReactiveProperty<bool>();
            }

            return _isRunning;
        }
    }
}
