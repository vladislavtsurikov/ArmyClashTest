using System.Threading;
using ArmyClash.MegaWorldGrid;
using Cysharp.Threading.Tasks;
using OdinSerializer;
using VladislavTsurikov.ReflectionUtility;
using VladislavTsurikov.SceneManagerTool.Runtime.SettingsSystem.OperationSystem;
using Zenject;
using Action = VladislavTsurikov.ActionFlow.Runtime.Actions.Action;

namespace ArmyClash.SceneManager
{
    [Name("Battle/Spawn Armies")]
    [AfterLoadSceneComponent]
    public sealed class SpawnBattleArmiesOperation : Action
    {
        [OdinSerialize]
        private bool _randomize = true;

        [Inject]
        private GridSpawnerPair _spawnerPair;

        protected override async UniTask<bool> Run(CancellationToken token)
        {
            if (_randomize)
            {
                await _spawnerPair.RandomizeArmiesAsync(token);
                return true;
            }

            await _spawnerPair.SpawnBoth(token, false, null);
            return true;
        }
    }
}
