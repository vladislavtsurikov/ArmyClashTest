using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ArmyClash.Grid;
using VladislavTsurikov.MegaWorld.Runtime.Core.SelectionDatas.Group;
using VladislavTsurikov.MegaWorld.Runtime.Core.SelectionDatas.Group.Prototypes.PrototypeGameObject;
using VladislavTsurikov.MegaWorld.Runtime.GridSpawner;

namespace ArmyClash.MegaWorldGrid
{
    public partial class GridSpawnerPair : MonoBehaviour
    {
        [Header("Grid Settings")]
        [SerializeField] private GridConfig _config = new GridConfig();
        [SerializeField] private Color _gizmoColor = new Color(0.1f, 0.6f, 1f, 0.8f);

        [Header("Children")]
        [SerializeField] private bool _autoCreateChildren = true;
        [SerializeField] private string _leftName = "Left Grid Spawner";
        [SerializeField] private string _rightName = "Right Grid Spawner";

        [Header("Placement")]
        [SerializeField] private bool _useManualPositions = false;
        [SerializeField] private Vector3 _leftPosition = new Vector3(-5f, 0f, 0f);
        [SerializeField] private Vector3 _rightPosition = new Vector3(5f, 0f, 0f);
        [SerializeField] private float _distance = 10f;
        [SerializeField] private bool _mirrorRightRotation = true;

        [Header("Generation")]
        [SerializeField] private bool _generateOnAwake = true;
        [SerializeField] private bool _autoUpdateInEditor = true;

        [SerializeField] private GridSpawner _leftSpawner;
        [SerializeField] private GridSpawner _rightSpawner;

        public GridSpawner Left => _leftSpawner;
        public GridSpawner Right => _rightSpawner;

        private void Awake()
        {
            EnsureSpawners();
            ApplyConfig();
            UpdateTransforms();

            if (_generateOnAwake)
            {
                SpawnBoth();
            }
        }

        private void OnValidate()
        {
            if (!_autoUpdateInEditor)
            {
                return;
            }

            EnsureSpawners();
            ApplyConfig();
            UpdateTransforms();
        }

        [ContextMenu("Spawn Both")]
        public void SpawnBoth()
        {
            EnsureSpawners();
            ApplyConfig();
            UpdateTransforms();

            _leftSpawner?.SpawnStamper();
            _rightSpawner?.SpawnStamper();
        }

        public async UniTask<(List<GameObject> left, List<GameObject> right)> SpawnBothAsync(
            CancellationToken token,
            bool displayProgressBar = false)
        {
            EnsureSpawners();
            ApplyConfig();
            UpdateTransforms();

            var left = _leftSpawner != null
                ? await _leftSpawner.SpawnAndCollect(token, displayProgressBar)
                : new List<GameObject>();

            var right = _rightSpawner != null
                ? await _rightSpawner.SpawnAndCollect(token, displayProgressBar)
                : new List<GameObject>();

            return (left, right);
        }

        public void ClearSpawnedObjects()
        {
            ClearSpawnerObjects(_leftSpawner);
            ClearSpawnerObjects(_rightSpawner);
        }

        private static void ClearSpawnerObjects(GridSpawner spawner)
        {
            if (spawner == null)
            {
                return;
            }

            for (int i = 0; i < spawner.Data.GroupList.Count; i++)
            {
                Group group = spawner.Data.GroupList[i];
                if (group == null)
                {
                    continue;
                }

                var container = group.GetDefaultElement<ContainerForGameObjects>();
                container?.DestroyGameObjects();
            }
        }

        private void EnsureSpawners()
        {
            if (_leftSpawner == null)
            {
                _leftSpawner = FindOrCreateSpawner(_leftName);
            }

            if (_rightSpawner == null)
            {
                _rightSpawner = FindOrCreateSpawner(_rightName);
            }
        }

        private GridSpawner FindOrCreateSpawner(string name)
        {
            if (!_autoCreateChildren)
            {
                return null;
            }

            var child = transform.Find(name);
            if (child == null)
            {
                var go = new GameObject(name);
                go.transform.SetParent(transform, false);
                child = go.transform;
            }

            var spawner = child.GetComponent<GridSpawner>();
            if (spawner == null)
            {
                spawner = child.gameObject.AddComponent<GridSpawner>();
            }

            return spawner;
        }

        private void ApplyConfig()
        {
            _config ??= new GridConfig();
            _leftSpawner?.ApplyConfig(_config);
            _rightSpawner?.ApplyConfig(_config);
            _leftSpawner?.ApplyGizmoColor(_gizmoColor);
            _rightSpawner?.ApplyGizmoColor(_gizmoColor);
        }

        private void UpdateTransforms()
        {
            if (_leftSpawner == null || _rightSpawner == null)
            {
                return;
            }

            var center = transform.position;
            var right = transform.right;
            var leftRotation = transform.rotation;
            var rightRotation = _mirrorRightRotation ? transform.rotation * Quaternion.Euler(0f, 180f, 0f) : transform.rotation;

            if (_useManualPositions)
            {
                _leftSpawner.transform.position = _leftPosition;
                _rightSpawner.transform.position = _rightPosition;
            }
            else
            {
                float half = Mathf.Max(0f, _distance) * 0.5f;
                _leftSpawner.transform.position = center - right * half;
                _rightSpawner.transform.position = center + right * half;
            }

            _leftSpawner.transform.rotation = leftRotation;
            _rightSpawner.transform.rotation = rightRotation;
        }
    }
}
