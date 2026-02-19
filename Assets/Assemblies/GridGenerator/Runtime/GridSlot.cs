using UnityEngine;

namespace ArmyClash.Grid
{
    public readonly struct GridSlot
    {
        public readonly int Row;
        public readonly int Column;
        public readonly int Index;
        public readonly Vector3 Position;
        public readonly Quaternion Rotation;

        public GridSlot(int row, int column, int index, Vector3 position, Quaternion rotation)
        {
            Row = row;
            Column = column;
            Index = index;
            Position = position;
            Rotation = rotation;
        }
    }
}
