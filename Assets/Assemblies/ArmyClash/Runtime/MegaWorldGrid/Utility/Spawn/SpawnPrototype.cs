using UnityEngine;
using VladislavTsurikov.MegaWorld.Runtime.Core.SelectionDatas.Group;
using VladislavTsurikov.MegaWorld.Runtime.Core.SelectionDatas.Group.Prototypes.PrototypeGameObject;

namespace ArmyClash.MegaWorldGrid.Utility.Spawn
{
    public static class SpawnPrototype
    {
        public static GameObject SpawnGameObject(Group group, PrototypeGameObject proto, Vector3 position,
            Quaternion rotation)
        {
            if (group == null || proto == null || proto.Prefab == null)
            {
                return null;
            }

            var instance = Object.Instantiate(proto.Prefab, position, rotation);
            group.GetDefaultElement<ContainerForGameObjects>().ParentGameObject(instance);
            return instance;
        }
    }
}
