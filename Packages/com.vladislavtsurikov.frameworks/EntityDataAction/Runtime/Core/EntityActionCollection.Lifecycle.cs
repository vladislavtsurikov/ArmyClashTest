namespace VladislavTsurikov.EntityDataAction.Runtime.Core
{
    public partial class EntityActionCollection
    {
        internal void InvokeAwake()
        {
            for (int i = 0; i < ElementList.Count; i++)
            {
                if (ElementList[i] is EntityAction action)
                {
                    action.InvokeAwake();
                }
            }
        }

        internal void InvokeStart()
        {
            for (int i = 0; i < ElementList.Count; i++)
            {
                if (ElementList[i] is EntityAction action)
                {
                    action.InvokeStart();
                }
            }
        }

        internal void InvokeUpdate()
        {
            for (int i = 0; i < ElementList.Count; i++)
            {
                if (ElementList[i] is EntityAction action)
                {
                    action.InvokeUpdate();
                }
            }
        }

        internal void InvokeFixedUpdate()
        {
            for (int i = 0; i < ElementList.Count; i++)
            {
                if (ElementList[i] is EntityAction action)
                {
                    action.InvokeFixedUpdate();
                }
            }
        }

        internal void InvokeLateUpdate()
        {
            for (int i = 0; i < ElementList.Count; i++)
            {
                if (ElementList[i] is EntityAction action)
                {
                    action.InvokeLateUpdate();
                }
            }
        }

        internal void InvokeOnEnable()
        {
            for (int i = 0; i < ElementList.Count; i++)
            {
                if (ElementList[i] is EntityAction action)
                {
                    action.InvokeOnEnable();
                }
            }
        }

        internal void InvokeOnDisable()
        {
            for (int i = 0; i < ElementList.Count; i++)
            {
                if (ElementList[i] is EntityAction action)
                {
                    action.InvokeOnDisable();
                }
            }
        }

        internal void InvokeOnDestroy()
        {
            for (int i = 0; i < ElementList.Count; i++)
            {
                if (ElementList[i] is EntityAction action)
                {
                    action.InvokeOnDestroy();
                }
            }
        }

        internal void InvokeOnValidate()
        {
            for (int i = 0; i < ElementList.Count; i++)
            {
                if (ElementList[i] is EntityAction action)
                {
                    action.InvokeOnValidate();
                }
            }
        }

        internal void InvokeOnApplicationFocus(bool hasFocus)
        {
            for (int i = 0; i < ElementList.Count; i++)
            {
                if (ElementList[i] is EntityAction action)
                {
                    action.InvokeOnApplicationFocus(hasFocus);
                }
            }
        }

        internal void InvokeOnApplicationPause(bool pauseStatus)
        {
            for (int i = 0; i < ElementList.Count; i++)
            {
                if (ElementList[i] is EntityAction action)
                {
                    action.InvokeOnApplicationPause(pauseStatus);
                }
            }
        }
    }
}
