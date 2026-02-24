using UnityEngine;
using VladislavTsurikov.MegaWorld.Runtime.GridSpawner;

namespace ArmyClash.MegaWorldGrid
{
    public sealed class GridSpawnerPairSetup : MonoBehaviour
    {
        [SerializeField]
        private GridSpawnerPair _pair;

        [SerializeField]
        private GridSpawner _leftSpawner;

        [SerializeField]
        private GridSpawner _rightSpawner;

        [SerializeField]
        private string _leftName = "Left Grid Spawner";

        [SerializeField]
        private string _rightName = "Right Grid Spawner";

        [Header("Placement")]
        [SerializeField]
        private bool _useManualPositions;

        [SerializeField]
        private Vector3 _leftPosition = new(-5f, 0f, 0f);

        [SerializeField]
        private Vector3 _rightPosition = new(5f, 0f, 0f);

        [SerializeField]
        private float _distance = 10f;

        [SerializeField]
        private bool _mirrorRightRotation = true;

        public void CreateSpawners()
        {
            ResolvePair();
            if (_pair == null)
            {
                return;
            }

            _leftSpawner = ResolveSpawner(_leftName);
            _rightSpawner = ResolveSpawner(_rightName);
            _pair.SetSpawners(_leftSpawner, _rightSpawner);
        }

        public void ApplyTransforms()
        {
            if (_leftSpawner == null || _rightSpawner == null)
            {
                return;
            }

            ApplyTransforms(_leftSpawner, _rightSpawner);
        }

        private void ResolvePair()
        {
            if (_pair == null)
            {
                _pair = GetComponent<GridSpawnerPair>();
            }
        }

        private GridSpawner ResolveSpawner(string name)
        {
            Transform child = transform.Find(name);
            if (child == null)
            {
                GameObject go = new(name);
                go.transform.SetParent(transform, false);
                child = go.transform;
            }

            GridSpawner spawner = child.GetComponent<GridSpawner>();
            if (spawner == null)
            {
                spawner = child.gameObject.AddComponent<GridSpawner>();
            }

            return spawner;
        }

        public void ApplyTransforms(GridSpawner left, GridSpawner right)
        {
            if (left == null || right == null)
            {
                return;
            }

            Vector3 center = transform.position;
            Vector3 rightAxis = transform.right;
            Quaternion leftRotation = transform.rotation;
            Quaternion rightRotation = _mirrorRightRotation
                ? transform.rotation * Quaternion.Euler(0f, 180f, 0f)
                : transform.rotation;

            if (_useManualPositions)
            {
                left.transform.position = _leftPosition;
                right.transform.position = _rightPosition;
            }
            else
            {
                float half = Mathf.Max(0f, _distance) * 0.5f;
                left.transform.position = center - rightAxis * half;
                right.transform.position = center + rightAxis * half;
            }

            left.transform.rotation = leftRotation;
            right.transform.rotation = rightRotation;
        }
    }
}
