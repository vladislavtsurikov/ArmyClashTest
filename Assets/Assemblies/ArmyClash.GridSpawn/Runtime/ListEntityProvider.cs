using System.Collections.Generic;
using UnityEngine;

namespace ArmyClash.Grid
{
    public class ListEntityProvider : GridPrefabProviderBase
    {
        private readonly List<GameObject> _prefabs;
        private readonly int[] _order;

        public override int Count => _order.Length;

        public ListEntityProvider(List<GameObject> prefabs, int count, bool randomize, System.Random random)
        {
            _prefabs = prefabs ?? new List<GameObject>();
            if (_prefabs.Count == 0 || count <= 0)
            {
                _order = System.Array.Empty<int>();
                return;
            }

            _order = new int[count];
            for (int i = 0; i < count; i++)
            {
                _order[i] = i % _prefabs.Count;
            }

            if (randomize)
            {
                Shuffle(_order, random);
            }
        }

        public override GameObject GetPrefab(int index)
        {
            if (_prefabs.Count == 0)
            {
                return null;
            }

            index = Mathf.Clamp(index, 0, _order.Length - 1);
            return _prefabs[_order[index]];
        }

        private static void Shuffle(int[] array, System.Random random)
        {
            if (array.Length == 0)
            {
                return;
            }

            random ??= new System.Random();
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                int temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }
    }
}
