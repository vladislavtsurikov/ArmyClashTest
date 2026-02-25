using Cysharp.Threading.Tasks;
using VladislavTsurikov.EntityDataAction.Runtime.Core;

namespace VladislavTsurikov.EntityDataAction.Runtime
{
    public abstract class FilteredEntity : EntityMonoBehaviour
    {
        public virtual string[] GetAllowedActionGroups() => null;

        protected override void OnSetupEntity()
        {
            if (Active)
            {
                Actions.SetAllowedGroupAttributes(GetAllowedActionGroups());

                Data.Setup();
                Actions.Setup();

                Data.ElementAdded += HandleDataChanged;
                Data.ElementRemoved += HandleDataChanged;

                Actions.Run().Forget();
            }
        }
    }
}
