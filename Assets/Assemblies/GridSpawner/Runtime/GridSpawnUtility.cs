using System;
using System.Collections.Generic;
using System.Threading;
using ArmyClash.Grid;
using ArmyClash.MegaWorldGrid.Utility.Spawn;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VladislavTsurikov.ColliderSystem.Runtime;
using VladislavTsurikov.MegaWorld.Runtime.Common.Area;
using VladislavTsurikov.MegaWorld.Runtime.Common.Settings.ScatterSystem;
using VladislavTsurikov.MegaWorld.Runtime.Common.Stamper;
using VladislavTsurikov.MegaWorld.Runtime.Common.Utility;
using VladislavTsurikov.MegaWorld.Runtime.Core.SelectionDatas.Group;
using VladislavTsurikov.MegaWorld.Runtime.Core.SelectionDatas.Group.Prototypes.PrototypeGameObject;

namespace VladislavTsurikov.MegaWorld.Runtime.GridSpawner
{
    public static class GridSpawnUtility
    {
        public static async UniTask<List<GameObject>> SpawnGroup(CancellationToken token, Group group,
            GridGenerator gridGenerator, Action<GameObject> onSpawn = null)
        {
            List<GameObject> instances = new();

            if (!group.HasAllActivePrototypes())
            {
                return instances;
            }

            if (group.PrototypeType != typeof(PrototypeGameObject))
            {
                return instances;
            }

            RandomSeedSettings randomSeedSettings = (RandomSeedSettings)group.GetElement(typeof(RandomSeedSettings));
            randomSeedSettings.GenerateRandomSeedIfNecessary();

            ScatterComponentSettings scatterSettings =
                (ScatterComponentSettings)group.GetElement(typeof(ScatterComponentSettings));

            ScatterStack scatterStack = scatterSettings.ScatterStack;
            scatterStack.Setup(true, new object[] { gridGenerator });

            float size = gridGenerator.GetAreaSize();
            RayHit hit = new(null, Vector3.up, gridGenerator.Origin, 0f);
            BoxArea boxArea = new(hit, size);

            await scatterStack.Samples(boxArea, sample =>
            {
                PrototypeGameObject proto =
                    (PrototypeGameObject)GetRandomPrototype.GetMaxSuccessProto(group.PrototypeList);
                if (proto == null || !proto.Active || proto.Prefab == null)
                {
                    return;
                }

                GameObject instance = SpawnPrototype.SpawnGameObject(group, proto, sample, gridGenerator.Rotation);
                if (instance != null)
                {
                    instances.Add(instance);
                    onSpawn?.Invoke(instance);
                }
            }, token);

            return instances;
        }
    }
}
