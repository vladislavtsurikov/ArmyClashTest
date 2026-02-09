using UnityEngine;
using ArmyClash.Grid;
using VladislavTsurikov.MegaWorld.Runtime.GridSpawner;

namespace ArmyClash.MegaWorldGrid
{
    public class GridSpawnerPair : MonoBehaviour
    {
        [SerializeField] private GridPairGenerator _gridPair;
        [SerializeField] private bool _autoCreatePair = true;
        [SerializeField] private string _pairName = "Grid Pair";

        [Header("Generation")]
        [SerializeField] private bool _generateOnAwake = true;
        [SerializeField] private bool _autoUpdateInEditor = true;

        [SerializeField] private GridSpawner _leftSpawner;
        [SerializeField] private GridSpawner _rightSpawner;

        public GridSpawner Left => _leftSpawner;
        public GridSpawner Right => _rightSpawner;

        private void Awake()
        {
            EnsurePair();
            EnsureSpawners();

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

            EnsurePair();
            EnsureSpawners();
        }

        [ContextMenu("Spawn Both")]
        public void SpawnBoth()
        {
            EnsurePair();
            EnsureSpawners();

            _leftSpawner?.SpawnStamper();
            _rightSpawner?.SpawnStamper();
        }

        private void EnsurePair()
        {
            if (_gridPair != null)
            {
                return;
            }

            if (!_autoCreatePair)
            {
                return;
            }

            var child = transform.Find(_pairName);
            if (child == null)
            {
                var go = new GameObject(_pairName);
                go.transform.SetParent(transform, false);
                child = go.transform;
            }

            _gridPair = child.GetComponent<GridPairGenerator>();
            if (_gridPair == null)
            {
                _gridPair = child.gameObject.AddComponent<GridPairGenerator>();
            }
        }

        private void EnsureSpawners()
        {
            if (_gridPair == null)
            {
                return;
            }

            if (_leftSpawner == null)
            {
                _leftSpawner = EnsureSpawner(_gridPair.Left);
            }

            if (_rightSpawner == null)
            {
                _rightSpawner = EnsureSpawner(_gridPair.Right);
            }
        }

        private static GridSpawner EnsureSpawner(GridGenerator generator)
        {
            if (generator == null)
            {
                return null;
            }

            var spawner = generator.GetComponent<GridSpawner>();
            if (spawner == null)
            {
                spawner = generator.gameObject.AddComponent<GridSpawner>();
            }

            spawner.GridGenerator = generator;
            return spawner;
        }
    }
}
