using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using ArmyClash.Grid;
using UnityEngine;
using VladislavTsurikov.MegaWorld.Runtime.Common.Settings;
using VladislavTsurikov.MegaWorld.Runtime.Common.Stamper;
using VladislavTsurikov.MegaWorld.Runtime.Common.Utility;
using VladislavTsurikov.MegaWorld.Runtime.Core.SelectionDatas.Group;
using VladislavTsurikov.MegaWorld.Runtime.Core.SelectionDatas.Group.Prototypes.PrototypeGameObject;
using VladislavTsurikov.MegaWorld.Runtime.Core.Utility;

namespace VladislavTsurikov.MegaWorld.Runtime.GridSpawner
{
    public static class GridSpawnUtility
    {
        public static async UniTask SpawnGroup(CancellationToken token, Group group, IReadOnlyList<GridSlot> slots)
        {
            if (!group.HasAllActivePrototypes())
            {
                return;
            }

            if (group.PrototypeType != typeof(PrototypeGameObject))
            {
                return;
            }

            var randomSeedSettings = (RandomSeedSettings)group.GetElement(typeof(RandomSeedSettings));
            randomSeedSettings.GenerateRandomSeedIfNecessary();

            for (int i = 0; i < slots.Count; i++)
            {
                token.ThrowIfCancellationRequested();

                var proto = (PrototypeGameObject)GetRandomPrototype.GetMaxSuccessProto(group.PrototypeList);
                if (proto == null || proto.Active == false || proto.Prefab == null)
                {
                    continue;
                }

                var slot = slots[i];
                var instance = Object.Instantiate(proto.Prefab, slot.Position, slot.Rotation);
                group.GetDefaultElement<ContainerForGameObjects>().ParentGameObject(instance);
            }

            await UniTask.CompletedTask;
        }
    }
}
