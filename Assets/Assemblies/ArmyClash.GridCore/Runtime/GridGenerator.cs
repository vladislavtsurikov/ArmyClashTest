using System.Collections.Generic;
using UnityEngine;

namespace ArmyClash.Grid
{
    public class GridGenerator
    {
        private List<GridSlot> _slots = new();
        private GridConfig _config;
        private Vector3 _origin;
        private Vector3 _rightAxis;
        private Vector3 _forwardAxis;
        private Quaternion _rotation = Quaternion.identity;

        public IReadOnlyList<GridSlot> Slots => _slots;
        public GridConfig Config => _config;
        public Vector3 Origin => _origin;
        public Vector3 RightAxis => _rightAxis;
        public Vector3 ForwardAxis => _forwardAxis;
        public Quaternion Rotation => _rotation;

        public IReadOnlyList<GridSlot> Build(GridConfig config, Vector3 origin, Vector3 rightAxis, Vector3 forwardAxis,
            Quaternion rotation)
        {
            if (config == null)
            {
                _slots = new List<GridSlot>();
                _config = null;
                _origin = Vector3.zero;
                _rightAxis = Vector3.right;
                _forwardAxis = Vector3.forward;
                _rotation = Quaternion.identity;
                return _slots;
            }

            _config = config;
            _origin = origin;
            _rightAxis = rightAxis;
            _forwardAxis = forwardAxis;
            _rotation = rotation;

            _slots = new List<GridSlot>(GridLayoutBuilder.Build(config, origin, rightAxis, forwardAxis, rotation));
            return _slots;
        }

        public List<Vector2> GetSlotPoints2D()
        {
            var points = new List<Vector2>(_slots.Count);
            for (int i = 0; i < _slots.Count; i++)
            {
                Vector3 position = _slots[i].Position;
                points.Add(new Vector2(position.x, position.z));
            }

            return points;
        }

        public float GetAreaSize()
        {
            if (_config == null)
            {
                return 1f;
            }

            int rows = Mathf.Max(1, _config.Rows);
            int columns = Mathf.Max(1, _config.Columns);

            float width = (columns - 1) * _config.ColumnStep + _config.CellSize;
            float height = (rows - 1) * _config.RowStep + _config.CellSize;
            return Mathf.Max(0.1f, Mathf.Max(width, height));
        }
    }
}
