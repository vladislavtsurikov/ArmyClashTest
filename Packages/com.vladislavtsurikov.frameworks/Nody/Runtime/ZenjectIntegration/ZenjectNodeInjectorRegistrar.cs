#if COMPONENT_STACK_ZENJECT
using UnityEngine;
using VladislavTsurikov.Nody.Runtime.Core;
using Zenject;

namespace VladislavTsurikov.Nody.Runtime
{
    public sealed class ZenjectNodeInjectorRegistrar : NodeInjectorRegistrar
    {
        public override void Inject(Node node, object[] setupData = null)
        {
            var container = ProjectContext.Instance != null
                ? ProjectContext.Instance.Container
                : Object.FindObjectOfType<SceneContext>()?.Container;

            container?.Inject(node);
        }
    }
}
#endif
