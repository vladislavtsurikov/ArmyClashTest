using System;
using System.Collections.Generic;
using UnityEngine;

namespace ArmyClash.Grid
{
    public class DefaultGridSpawner : GridSpawnerBase
    {
        public override void Spawn(
            IReadOnlyList<GridSlot> slots,
            GridPrefabProviderBase provider,
            Transform parent,
            int maxCount,
            System.Random random,
            bool applyRandomProperties)
        {
            if (slots == null)
            {
                throw new ArgumentNullException(nameof(slots));
            }

            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            int count = Mathf.Min(slots.Count, provider.Count, Mathf.Max(0, maxCount));
            for (int i = 0; i < count; i++)
            {
                var prefab = provider.GetPrefab(i);
                if (prefab == null)
                {
                    continue;
                }

                var slot = slots[i];
                var instance = UnityEngine.Object.Instantiate(prefab, slot.Position, slot.Rotation, parent);
                if (applyRandomProperties && random != null)
                {
                    ApplyRandomizers(instance, random);
                }
            }
        }

        private static void ApplyRandomizers(GameObject instance, System.Random random)
        {
            var randomizers = instance.GetComponentsInChildren<SpawnRandomizer>(true);
            for (int i = 0; i < randomizers.Length; i++)
            {
                randomizers[i].ApplyRandom(random);
            }
        }
    }
}
