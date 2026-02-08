using System;
using System.Collections.Generic;
using UnityEngine;

namespace ArmyClash.Grid
{
    public class PrototypeEntityProvider : GridPrefabProviderBase
    {
        private readonly GameObject[] _prefabs;

        public override int Count => _prefabs.Length;

        public PrototypeEntityProvider(
            List<GameObject> basePrefabs,
            List<EntityPrototypeGroup> prototypeGroups,
            int count,
            bool randomizeOrder,
            System.Random random)
        {
            if (count <= 0)
            {
                _prefabs = Array.Empty<GameObject>();
                return;
            }

            random ??= new System.Random();
            _prefabs = new GameObject[count];

            var baseOrder = BuildBaseOrder(basePrefabs, count, randomizeOrder, random);

            for (int i = 0; i < count; i++)
            {
                var chosen = TryPickFromGroups(prototypeGroups, random);
                _prefabs[i] = chosen != null ? chosen : baseOrder[i];
            }
        }

        public override GameObject GetPrefab(int index)
        {
            if (_prefabs.Length == 0)
            {
                return null;
            }

            index = Mathf.Clamp(index, 0, _prefabs.Length - 1);
            return _prefabs[index];
        }

        private static GameObject[] BuildBaseOrder(
            List<GameObject> basePrefabs,
            int count,
            bool randomizeOrder,
            System.Random random)
        {
            if (basePrefabs == null || basePrefabs.Count == 0)
            {
                return new GameObject[count];
            }

            var order = new int[count];
            for (int i = 0; i < count; i++)
            {
                order[i] = i % basePrefabs.Count;
            }

            if (randomizeOrder)
            {
                Shuffle(order, random);
            }

            var result = new GameObject[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = basePrefabs[order[i]];
            }

            return result;
        }

        private static GameObject TryPickFromGroups(List<EntityPrototypeGroup> groups, System.Random random)
        {
            if (groups == null || groups.Count == 0)
            {
                return null;
            }

            var order = BuildGroupOrder(groups.Count, random);
            for (int i = 0; i < order.Length; i++)
            {
                var group = groups[order[i]];
                if (group == null || group.Prototypes == null || group.Prototypes.Count == 0)
                {
                    continue;
                }

                var candidate = TryPickPrototype(group.Prototypes, random);
                if (candidate != null)
                {
                    return candidate;
                }
            }

            return null;
        }

        private static GameObject TryPickPrototype(List<EntityPrototype> prototypes, System.Random random)
        {
            List<EntityPrototype> success = null;
            for (int i = 0; i < prototypes.Count; i++)
            {
                var prototype = prototypes[i];
                if (prototype == null || prototype.Prefab == null)
                {
                    continue;
                }

                float chance = Mathf.Clamp01(prototype.Success);
                if (chance <= 0f)
                {
                    continue;
                }

                if (random.NextDouble() <= chance)
                {
                    success ??= new List<EntityPrototype>();
                    success.Add(prototype);
                }
            }

            if (success == null || success.Count == 0)
            {
                return null;
            }

            int index = random.Next(0, success.Count);
            return success[index].Prefab;
        }

        private static void Shuffle(int[] array, System.Random random)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                int temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }

        private static int[] BuildGroupOrder(int count, System.Random random)
        {
            var order = new int[count];
            for (int i = 0; i < count; i++)
            {
                order[i] = i;
            }

            Shuffle(order, random);
            return order;
        }
    }
}
