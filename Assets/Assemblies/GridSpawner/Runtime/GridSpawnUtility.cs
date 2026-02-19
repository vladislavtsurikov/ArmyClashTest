using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using ArmyClash.Grid;
using ArmyClash.MegaWorldGrid.Utility.Spawn;
using UnityEngine;
using VladislavTsurikov.ColliderSystem.Runtime;
using VladislavTsurikov.MegaWorld.Runtime.Common.Area;
using VladislavTsurikov.MegaWorld.Runtime.Common.Stamper;
using VladislavTsurikov.MegaWorld.Runtime.Common.Settings.ScatterSystem;
using VladislavTsurikov.MegaWorld.Runtime.Common.Utility;
using VladislavTsurikov.MegaWorld.Runtime.Core.SelectionDatas.Group;
using VladislavTsurikov.MegaWorld.Runtime.Core.SelectionDatas.Group.Prototypes.PrototypeGameObject;

namespace VladislavTsurikov.MegaWorld.Runtime.GridSpawner
{
    public static class GridSpawnUtility
    {
        public static async UniTask<List<GameObject>> SpawnGroup(CancellationToken token, Group group,
            GridGenerator gridGenerator)
        {
            var instances = new List<GameObject>();

            if (!group.HasAllActivePrototypes())
            {
                return instances;
            }

            if (group.PrototypeType != typeof(PrototypeGameObject))
            {
                return instances;
            }

            var randomSeedSettings = (RandomSeedSettings)group.GetElement(typeof(RandomSeedSettings));
            randomSeedSettings.GenerateRandomSeedIfNecessary();

            var slots = gridGenerator?.Slots;
            if (slots == null || slots.Count == 0)
            {
                return instances;
            }

            var scatterSettings = (ScatterComponentSettings)group.GetElement(typeof(ScatterComponentSettings));
            if (scatterSettings == null || scatterSettings.ScatterStack == null)
            {
                for (int i = 0; i < slots.Count; i++)
                {
                    token.ThrowIfCancellationRequested();

                    var proto = (PrototypeGameObject)GetRandomPrototype.GetMaxSuccessProto(group.PrototypeList);
                    if (proto == null || proto.Active == false || proto.Prefab == null)
                    {
                        continue;
                    }

                    var instance = SpawnPrototype.SpawnGameObject(group, proto, slots[i].Position,
                        gridGenerator.Rotation);
                    if (instance != null)
                    {
                        instances.Add(instance);
                    }
                }

                return instances;
            }

            var scatterStack = scatterSettings.ScatterStack;
            scatterStack.Setup(true, new object[] { gridGenerator });

            float size = gridGenerator.GetAreaSize();
            var hit = new RayHit(null, Vector3.up, gridGenerator.Origin, 0f);
            var boxArea = new BoxArea(hit, size);

            await scatterStack.Samples(boxArea, sample =>
            {
                var proto = (PrototypeGameObject)GetRandomPrototype.GetMaxSuccessProto(group.PrototypeList);
                if (proto == null || proto.Active == false || proto.Prefab == null)
                {
                    return;
                }

                var instance = SpawnPrototype.SpawnGameObject(group, proto, sample, gridGenerator.Rotation);
                if (instance != null)
                {
                    instances.Add(instance);
                }
            }, token);

            return instances;
        }
    }
}
