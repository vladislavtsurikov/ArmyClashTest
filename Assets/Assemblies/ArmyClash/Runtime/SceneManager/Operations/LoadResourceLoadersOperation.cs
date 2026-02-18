using Cysharp.Threading.Tasks;
using OdinSerializer;
using VladislavTsurikov.AddressableLoaderSystem.Runtime.Core;
using VladislavTsurikov.Core.Runtime;
using VladislavTsurikov.ReflectionUtility;
using VladislavTsurikov.SceneManagerTool.Runtime.SettingsSystem.OperationSystem;
using Zenject;
using Action = VladislavTsurikov.ActionFlow.Runtime.Actions.Action;

namespace ArmyClash.SceneManager
{
    [Name("Addressables/Load Resource Loaders")]
    [BeforeLoadSceneComponent]
    public sealed class LoadResourceLoadersOperation : Action
    {
        [OdinSerialize] private string _sceneName;

        protected override async UniTask<bool> Run(System.Threading.CancellationToken token)
        {
            var container = ProjectContext.Instance != null ? ProjectContext.Instance.Container : null;
            if (container == null)
            {
                return true;
            }

            var manager = container.Resolve<ResourceLoaderManager>();
            if (manager == null)
            {
                return true;
            }

            await manager.Load(attribute =>
            {
                if (attribute is GlobalFilterAttribute)
                {
                    return true;
                }

                if (!string.IsNullOrEmpty(_sceneName) &&
                    attribute is SceneFilterAttribute sceneFilter &&
                    sceneFilter.Matches(_sceneName))
                {
                    return true;
                }

                return false;
            }, token);

            return true;
        }
    }
}
