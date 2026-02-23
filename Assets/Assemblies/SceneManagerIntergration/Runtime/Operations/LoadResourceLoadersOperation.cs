using System.Threading;
using Cysharp.Threading.Tasks;
using OdinSerializer;
using VladislavTsurikov.AddressableLoaderSystem.Runtime.Core;
using VladislavTsurikov.Core.Runtime;
using VladislavTsurikov.ReflectionUtility;
using VladislavTsurikov.SceneManagerTool.Runtime.SettingsSystem.OperationSystem;
using VladislavTsurikov.SceneUtility.Runtime;
using Zenject;
using Action = VladislavTsurikov.ActionFlow.Runtime.Actions.Action;

namespace ArmyClash.SceneManager
{
    [Name("Addressables/Load Resource Loaders")]
    [BeforeLoadSceneComponent]
    public sealed class LoadResourceLoadersOperation : Action
    {
        [Inject]
        private ResourceLoaderManager _manager;

        [OdinSerialize]
        private SceneReference _sceneReference = new();

        protected override async UniTask<bool> Run(CancellationToken token)
        {
            await _manager.Load(attribute =>
            {
                if (attribute is GlobalFilterAttribute)
                {
                    return true;
                }

                if (_sceneReference != null &&
                    !string.IsNullOrEmpty(_sceneReference.SceneName) &&
                    attribute is SceneFilterAttribute sceneFilter &&
                    sceneFilter.Matches(_sceneReference.SceneName))
                {
                    return true;
                }

                return false;
            }, token);

            return true;
        }
    }
}
