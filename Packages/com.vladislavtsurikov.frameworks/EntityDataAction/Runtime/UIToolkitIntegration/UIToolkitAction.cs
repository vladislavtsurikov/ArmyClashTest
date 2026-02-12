using UnityEngine.UIElements;
using VladislavTsurikov.EntityDataAction.Runtime.Core;

namespace VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration
{
    public abstract class UIToolkitAction : EntityAction
    {
        protected VisualElement Root { get; private set; }

        protected sealed override void OnFirstSetupComponent(object[] setupData = null)
        {
            Root = FindRoot(setupData);
            OnFirstSetupComponentUi(setupData);
        }

        protected virtual void OnFirstSetupComponentUi(object[] setupData = null)
        {
        }

        protected TElement Query<TElement>(string name) where TElement : VisualElement
        {
            return Root == null ? null : Root.Q<TElement>(name);
        }

        private static VisualElement FindRoot(object[] setupData)
        {
            if (setupData == null)
            {
                return null;
            }

            for (int i = 0; i < setupData.Length; i++)
            {
                if (setupData[i] is VisualElement root)
                {
                    return root;
                }
            }

            return null;
        }
    }
}
