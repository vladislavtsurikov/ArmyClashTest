using System.Collections.Generic;
using UnityEngine;

namespace ArmyClash.Grid
{
    public abstract class GridSpawnerBase
    {
        public abstract void Spawn(
            IReadOnlyList<GridSlot> slots,
            GridPrefabProviderBase provider,
            Transform parent,
            int maxCount,
            System.Random random,
            bool applyRandomProperties);
    }
}
