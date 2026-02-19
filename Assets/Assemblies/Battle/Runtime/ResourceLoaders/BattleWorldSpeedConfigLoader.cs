#if ADDRESSABLE_LOADER_SYSTEM_ADDRESSABLES
#if ADDRESSABLE_LOADER_SYSTEM_ZENJECT
using System.Threading;
using Cysharp.Threading.Tasks;
using VladislavTsurikov.AddressableLoaderSystem.Runtime.ZenjectIntegration;
using VladislavTsurikov.Core.Runtime;
using Zenject;

namespace ArmyClash.Battle.Config
{
    [GlobalFilter]
    public sealed class BattleWorldSpeedConfigLoader : BindableResourceLoader
    {
        public BattleWorldSpeedConfigLoader(DiContainer container) : base(container)
        {
        }

        public BattleWorldSpeedConfig Config { get; private set; }

        public override async UniTask LoadResourceLoader(CancellationToken token) =>
            Config = await LoadAndBind<BattleWorldSpeedConfig>(token, "BattleWorldSpeedConfig");
    }
}
#endif
#endif
