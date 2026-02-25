using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration
{
    public abstract class EntityUIView<T> : VisualElement where T : class, IDisposable
    {
        private T _entity;
        private bool _initialized;

        protected EntityUIView()
        {
            if (Application.isPlaying)
            {
                RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
                RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
            }
        }

        public T Entity
        {
            get
            {
                EnsureInitialized();
                return _entity;
            }
        }

        protected abstract T CreateEntity();

        private void OnAttachToPanel(AttachToPanelEvent evt) => EnsureInitialized();

        private void OnDetachFromPanel(DetachFromPanelEvent evt)
        {
            _entity?.Dispose();
            _entity = null;
            _initialized = false;
        }

        private void EnsureInitialized()
        {
            if (_initialized)
            {
                return;
            }

            InitializeWhenReady().Forget();
        }

        private async UniTask InitializeWhenReady()
        {
            await UniTask.WaitUntil(ZenjectUtility.Runtime.ZenjectUtility.IsSceneContextReady);
            _entity = CreateEntity();
        }
    }
}
