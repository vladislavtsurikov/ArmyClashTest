using ArmyClash.UIToolkit.Actions;
using VladislavTsurikov.EntityDataAction.Runtime.Core;

namespace ArmyClash.Battle
{
    public sealed class BattleWorldInputAction : EntityMonoBehaviourAction
    {
        protected override void OnEnable()
        {
            BattleWorldSignals.StartRequested += HandleStartRequested;
            BattleWorldSignals.RandomizeRequested += HandleRandomizeRequested;
        }

        protected override void OnDisable()
        {
            BattleWorldSignals.StartRequested -= HandleStartRequested;
            BattleWorldSignals.RandomizeRequested -= HandleRandomizeRequested;
        }

        private void HandleStartRequested()
        {
            Entity?.GetAction<BattleWorldStateAction>()?.StartBattle();
        }

        private void HandleRandomizeRequested()
        {
            Entity?.GetAction<BattleWorldSpawnAction>()?.RandomizeArmies();
        }
    }
}
