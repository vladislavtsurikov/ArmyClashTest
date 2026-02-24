using UnityEngine;

namespace ArmyClash.Grid
{
    [CreateAssetMenu(menuName = "Configs/Battle/GridConfig", fileName = "GridConfig")]
    public sealed class GridConfig : ScriptableObject
    {
        [Min(1)]
        public int Rows = 4;

        [Min(1)]
        public int Columns = 5;

        [Min(0.1f)]
        public float CellSize = 1f;

        [Min(0f)]
        public float RowSpacing = 0.2f;

        [Min(0f)]
        public float ColumnSpacing = 0.2f;

        public float ColumnStep => CellSize + ColumnSpacing;
        public float RowStep => CellSize + RowSpacing;

        public int SlotCount => Mathf.Max(1, Rows) * Mathf.Max(1, Columns);

        public void CopyFrom(GridConfig source)
        {
            if (source == null)
            {
                return;
            }

            Rows = source.Rows;
            Columns = source.Columns;
            CellSize = source.CellSize;
            RowSpacing = source.RowSpacing;
            ColumnSpacing = source.ColumnSpacing;
        }
    }
}
