#if COMPONENT_STACK_ZENJECT
using VladislavTsurikov.Nody.Runtime.Core;
using Zenject;

namespace VladislavTsurikov.Nody.Runtime
{
    public sealed class ZenjectNodeInjectorRegistrar : NodeInjectorRegistrar
    {
        public override void Inject(Element node, object[] setupData = null)
        {
            ProjectContext.Instance.Container?.Inject(node);
        }
    }
}
#endif
