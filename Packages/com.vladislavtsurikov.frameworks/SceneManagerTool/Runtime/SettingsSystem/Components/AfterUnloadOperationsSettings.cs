using System.Threading;
using Cysharp.Threading.Tasks;
using VladislavTsurikov.ActionFlow.Runtime;
using VladislavTsurikov.ReflectionUtility;

namespace VladislavTsurikov.SceneManagerTool.Runtime.SettingsSystem
{
    [Name("After Unload")]
    public class AfterUnloadOperationsSettings : SettingsComponent
    {
        public ActionCollection OperationStack = new();

        public async UniTask DoOperations(CancellationToken token = default)
        {
            await OperationStack.Run(token);
        }
    }
}
