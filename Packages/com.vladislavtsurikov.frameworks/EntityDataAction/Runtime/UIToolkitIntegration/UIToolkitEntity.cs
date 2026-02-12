using System;
using VladislavTsurikov.EntityDataAction.Runtime.Core;

namespace VladislavTsurikov.EntityDataAction.Runtime.UIToolkitIntegration
{
    public sealed class UIToolkitEntity : Entity
    {
        private readonly Type[] _dataTypes;
        private readonly Type[] _actionTypes;

        public UIToolkitEntity(Type[] dataTypes, Type[] actionTypes)
        {
            _dataTypes = dataTypes;
            _actionTypes = actionTypes;
        }

        protected override Type[] ComponentDataTypesToCreate()
        {
            return _dataTypes;
        }

        protected override Type[] ActionTypesToCreate()
        {
            return _actionTypes;
        }
    }
}
