using System.Collections.Generic;
using UnityEngine;

namespace ArmyClash.Grid
{
    public class GridGenerator : MonoBehaviour
    {
        [SerializeField] private GridConfig _config = new GridConfig();
        [SerializeField] private bool _generateOnAwake = true;
        [SerializeField] private Color _gizmoColor = new Color(0.1f, 0.6f, 1f, 0.8f);

        private readonly GridLayoutBuilder _builder = new GridLayoutBuilder();
        private List<GridSlot> _slots = new List<GridSlot>();

        public GridConfig Config => _config;
        public IReadOnlyList<GridSlot> Slots => _slots;

        private void Awake()
        {
            if (_generateOnAwake)
            {
                Generate();
            }
        }

        public IReadOnlyList<GridSlot> Generate()
        {
            _config ??= new GridConfig();
            _slots = new List<GridSlot>(_builder.Build(_config, transform.position, transform.right, transform.forward, transform.rotation));
            return _slots;
        }

        public IReadOnlyList<GridSlot> GetOrGenerate()
        {
            if (_slots.Count == 0)
            {
                return Generate();
            }

            return _slots;
        }

        private void OnDrawGizmos()
        {
            if (_config == null)
            {
                return;
            }

            var settings = GridGlobalSettings.Instance;
            if (settings != null && !settings.ShowGizmos)
            {
                return;
            }

            var slots = _builder.Build(_config, transform.position, transform.right, transform.forward, transform.rotation);
            Color color = _gizmoColor;
            float height = 0.02f;
            float sizeScale = 1f;

            if (settings != null)
            {
                if (settings.OverrideGizmoColor)
                {
                    color = settings.GizmoColor;
                }

                height = Mathf.Max(0.001f, settings.GizmoHeight);
                sizeScale = Mathf.Max(0.01f, settings.GizmoSizeScale);
            }

            Gizmos.color = color;
            float size = Mathf.Max(0.05f, _config.CellSize) * sizeScale;
            var cubeSize = new Vector3(size, height, size);

            for (int i = 0; i < slots.Count; i++)
            {
                Gizmos.DrawWireCube(slots[i].Position, cubeSize);
            }
        }

        public void ApplyConfig(GridConfig config)
        {
            if (config == null)
            {
                return;
            }

            _config ??= new GridConfig();
            _config.CopyFrom(config);
        }
    }
}
