using System.Collections.Generic;
using VladislavTsurikov.ReflectionUtility.Runtime;

namespace VladislavTsurikov.Nody.Runtime.Core
{
    public static class NodeInjectionUtility
    {
        private static List<NodeInjectorRegistrar> _registrars;

        public static void Inject(Element node, object[] setupData = null)
        {
            if (node == null)
            {
                return;
            }

            EnsureRegistrars();

            for (int i = 0; i < _registrars.Count; i++)
            {
                _registrars[i].Inject(node, setupData);
            }
        }

        private static void EnsureRegistrars()
        {
            if (_registrars != null)
            {
                return;
            }

            _registrars = new List<NodeInjectorRegistrar>(
                ReflectionFactory.CreateAllInstances<NodeInjectorRegistrar>());
        }
    }
}
