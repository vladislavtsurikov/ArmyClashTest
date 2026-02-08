using UnityEngine;

namespace ArmyClash.Grid
{
    public abstract class GridPrefabProviderBase
    {
        public abstract int Count { get; }
        public abstract GameObject GetPrefab(int index);
    }
}
