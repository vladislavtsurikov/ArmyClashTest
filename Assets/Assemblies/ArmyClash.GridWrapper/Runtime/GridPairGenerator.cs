using UnityEngine;

namespace ArmyClash.Grid
{
    public class GridPairGenerator : MonoBehaviour
    {
        [SerializeField] private GridConfig _config = new GridConfig();
        [SerializeField] private bool _autoCreateChildren = true;
        [SerializeField] private string _leftName = "Left Grid";
        [SerializeField] private string _rightName = "Right Grid";

        [Header("Placement")]
        [SerializeField] private bool _useManualPositions = false;
        [SerializeField] private Vector3 _leftPosition = new Vector3(-5f, 0f, 0f);
        [SerializeField] private Vector3 _rightPosition = new Vector3(5f, 0f, 0f);
        [SerializeField] private float _distance = 10f;
        [SerializeField] private bool _mirrorRightRotation = true;

        [Header("Generation")]
        [SerializeField] private bool _generateOnAwake = true;
        [SerializeField] private bool _autoUpdateInEditor = true;

        [SerializeField] private GridGeneratorComponent _leftGenerator;
        [SerializeField] private GridGeneratorComponent _rightGenerator;

        public GridGeneratorComponent Left => _leftGenerator;
        public GridGeneratorComponent Right => _rightGenerator;

        private void Awake()
        {
            EnsureGenerators();
            ApplyConfig();
            UpdateTransforms();

            if (_generateOnAwake)
            {
                GenerateBoth();
            }
        }

        private void OnValidate()
        {
            if (!_autoUpdateInEditor)
            {
                return;
            }

            EnsureGenerators();
            ApplyConfig();
            UpdateTransforms();
        }

        [ContextMenu("Generate Both")]
        public void GenerateBoth()
        {
            EnsureGenerators();
            ApplyConfig();
            UpdateTransforms();
            _leftGenerator?.Generate(null);
            _rightGenerator?.Generate(null);
        }

        private void EnsureGenerators()
        {
            if (!_autoCreateChildren)
            {
                return;
            }

            if (_leftGenerator == null)
            {
                _leftGenerator = FindOrCreateGenerator(_leftName);
            }

            if (_rightGenerator == null)
            {
                _rightGenerator = FindOrCreateGenerator(_rightName);
            }
        }

        private GridGeneratorComponent FindOrCreateGenerator(string name)
        {
            var child = transform.Find(name);
            if (child == null)
            {
                var go = new GameObject(name);
                go.transform.SetParent(transform, false);
                child = go.transform;
            }

            var generator = child.GetComponent<GridGeneratorComponent>();
            if (generator == null)
            {
                generator = child.gameObject.AddComponent<GridGeneratorComponent>();
            }

            return generator;
        }

        private void ApplyConfig()
        {
            _config ??= new GridConfig();
            _leftGenerator?.ApplyConfig(_config);
            _rightGenerator?.ApplyConfig(_config);
        }

        private void UpdateTransforms()
        {
            if (_leftGenerator == null || _rightGenerator == null)
            {
                return;
            }

            var center = transform.position;
            var right = transform.right;
            var leftRotation = transform.rotation;
            var rightRotation = _mirrorRightRotation ? transform.rotation * Quaternion.Euler(0f, 180f, 0f) : transform.rotation;

            if (_useManualPositions)
            {
                _leftGenerator.transform.position = _leftPosition;
                _rightGenerator.transform.position = _rightPosition;
            }
            else
            {
                float half = Mathf.Max(0f, _distance) * 0.5f;
                _leftGenerator.transform.position = center - right * half;
                _rightGenerator.transform.position = center + right * half;
            }

            _leftGenerator.transform.rotation = leftRotation;
            _rightGenerator.transform.rotation = rightRotation;
        }
    }
}
