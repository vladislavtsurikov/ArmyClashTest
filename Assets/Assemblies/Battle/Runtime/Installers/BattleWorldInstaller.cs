using ArmyClash.Battle.Services;
using Zenject;

namespace ArmyClash.Battle.Installers
{
    public sealed class BattleWorldInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BattleTeamRoster>().AsSingle();
            Container.Bind<BattleStateService>().AsSingle();
        }
    }
}
