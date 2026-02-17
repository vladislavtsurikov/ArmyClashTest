using ArmyClash.UIToolkit.Data;
using UniRx;
using VladislavTsurikov.EntityDataAction.Runtime.Core;

namespace ArmyClash.Battle
{
    public sealed class BattleWorldStateAction : EntityMonoBehaviourAction
    {
        private readonly ReactiveProperty<bool> _isRunning = new ReactiveProperty<bool>();

        public bool IsRunning
        {
            get => _isRunning.Value;
            private set => _isRunning.Value = value;
        }

        public IReadOnlyReactiveProperty<bool> IsRunningReactive => _isRunning;

        public void StartBattle()
        {
            var roster = Entity?.GetAction<BattleWorldRosterAction>();
            if (roster == null || roster.LeftCount == 0 || roster.RightCount == 0)
            {
                return;
            }

            IsRunning = true;
            Entity?.GetAction<BattleWorldUiSyncAction>()?.SetSimulationState(SimulationState.Running);
        }

        public void FinishBattle()
        {
            IsRunning = false;
            Entity?.GetAction<BattleWorldUiSyncAction>()?.SetSimulationState(SimulationState.Finished);
        }

        public void HandleEntityDeath(BattleEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            var roster = Entity?.GetAction<BattleWorldRosterAction>();
            roster?.UnregisterEntity(entity);
            Entity?.GetAction<BattleWorldUiSyncAction>()?.UpdateArmyCountUI();

            UnityEngine.Object.Destroy(entity.gameObject);

            if (roster == null || roster.LeftCount == 0 || roster.RightCount == 0)
            {
                FinishBattle();
            }
        }
    }
}
