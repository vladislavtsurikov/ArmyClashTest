using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace ArmyClash.Grid
{
    [MovedFrom(true, "ArmyClash.Grid", null, "GridGenerator")]
    public sealed class GridGeneratorComponent : MonoBehaviour
    {
        [SerializeField] private GridConfig _config = new GridConfig();
        [SerializeField] private bool _generateOnAwake = true;
        [SerializeField] private Color _gizmoColor = new Color(0.1f, 0.6f, 1f, 0.8f);

        private List<GridSlot> _slots = new List<GridSlot>();

        public GridConfig Config => _config;
        public IReadOnlyList<GridSlot> Slots => _slots;
        public Color GizmoColor => _gizmoColor;

        private void Awake()
        {
            if (_generateOnAwake)
            {
                Generate(null);
            }
        }

        public IReadOnlyList<GridSlot> Generate(GridGenerator generator)
        {
            _config ??= new GridConfig();
            generator ??= new GridGenerator();
            _slots = new List<GridSlot>(generator.Build(_config, transform.position, transform.right, transform.forward,
                transform.rotation));
            return _slots;
        }

        public IReadOnlyList<GridSlot> GetOrGenerate(GridGenerator generator)
        {
            if (_slots.Count == 0)
            {
                return Generate(generator);
            }

            return _slots;
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
