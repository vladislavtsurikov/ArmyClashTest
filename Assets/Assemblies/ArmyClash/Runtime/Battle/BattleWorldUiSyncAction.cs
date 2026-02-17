using ArmyClash.Battle.Ui;
using ArmyClash.UIToolkit.Data;
using UnityEngine.UIElements;
using VladislavTsurikov.EntityDataAction.Runtime.Core;

namespace ArmyClash.Battle
{
    public sealed class BattleWorldUiSyncAction : EntityMonoBehaviourAction
    {
        private BattleUIView _uiView;
        private BattleUIToolkitEntity _uiEntity;

        private BattleWorldEntity World => Host as BattleWorldEntity;

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

        public void UpdateArmyCountUI()
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
    }
}
