using System.Collections.Generic;
using UnityEngine;

namespace ArmyClash.Grid
{
    public class GridGenerator
    {
        private List<GridSlot> _slots = new();

        public IReadOnlyList<GridSlot> Slots => _slots;
        public GridConfig Config { get; private set; }

        public Vector3 Origin { get; private set; }

        public Vector3 RightAxis { get; private set; }

        public Vector3 ForwardAxis { get; private set; }

        public Quaternion Rotation { get; private set; } = Quaternion.identity;

        public IReadOnlyList<GridSlot> Build(GridConfig config, Vector3 origin, Vector3 rightAxis, Vector3 forwardAxis,
            Quaternion rotation)
        {
            if (config == null)
            {
                _slots = new List<GridSlot>();
                Config = null;
                Origin = Vector3.zero;
                RightAxis = Vector3.right;
                ForwardAxis = Vector3.forward;
                Rotation = Quaternion.identity;
                return _slots;
            }

            Config = config;
            Origin = origin;
            RightAxis = rightAxis;
            ForwardAxis = forwardAxis;
            Rotation = rotation;

            _slots = new List<GridSlot>(GridLayoutBuilder.Build(config, origin, rightAxis, forwardAxis, rotation));
            return _slots;
        }

        public List<Vector2> GetSlotPoints2D()
        {
            List<Vector2> points = new(_slots.Count);
            for (int i = 0; i < _slots.Count; i++)
            {
                Vector3 position = _slots[i].Position;
                points.Add(new Vector2(position.x, position.z));
            }

            return points;
        }

        public float GetAreaSize()
        {
            if (Config == null)
            {
                return 1f;
            }

            int rows = Mathf.Max(1, Config.Rows);
            int columns = Mathf.Max(1, Config.Columns);

            float width = (columns - 1) * Config.ColumnStep + Config.CellSize;
            float height = (rows - 1) * Config.RowStep + Config.CellSize;
            return Mathf.Max(0.1f, Mathf.Max(width, height));
        }
    }
}
