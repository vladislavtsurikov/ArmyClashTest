using System.Collections.Generic;
using UnityEngine;

namespace ArmyClash.Grid
{
    public class GridArmySpawner : MonoBehaviour
    {
        [SerializeField] private GridGenerator _leftGrid;
        [SerializeField] private GridGenerator _rightGrid;
        [SerializeField] private Transform _leftRoot;
        [SerializeField] private Transform _rightRoot;
        [SerializeField] private List<GameObject> _leftPrefabs = new List<GameObject>();
        [SerializeField] private List<GameObject> _rightPrefabs = new List<GameObject>();
        [SerializeField] private List<EntityPrototypeGroup> _leftPrototypeGroups = new List<EntityPrototypeGroup>();
        [SerializeField] private List<EntityPrototypeGroup> _rightPrototypeGroups = new List<EntityPrototypeGroup>();
        [SerializeField] private int _unitsPerSide = 20;
        [SerializeField] private bool _randomizeOrder = true;
        [SerializeField] private bool _applyRandomProperties = true;
        [SerializeField] private bool _useSeed = false;
        [SerializeField] private int _seed = 12345;
        [SerializeField] private bool _clearBeforeSpawn = true;

        public void SpawnArmies()
        {
            if (_leftGrid == null || _rightGrid == null)
            {
                return;
            }

            if (_clearBeforeSpawn)
            {
                ClearArmies();
            }

            var spawner = new DefaultGridSpawner();

            var random = CreateRandom();
            SpawnSide(spawner, _leftGrid, _leftPrefabs, _leftPrototypeGroups, _leftRoot, random);
            SpawnSide(spawner, _rightGrid, _rightPrefabs, _rightPrototypeGroups, _rightRoot, random);
        }

        public void ClearArmies()
        {
            ClearChildren(_leftRoot ? _leftRoot : transform);
            ClearChildren(_rightRoot ? _rightRoot : transform);
        }

        private void SpawnSide(
            GridSpawnerBase spawner,
            GridGenerator generator,
            List<GameObject> prefabs,
            List<EntityPrototypeGroup> prototypeGroups,
            Transform root,
            System.Random random)
        {
            var slots = generator.GetOrGenerate();
            int maxCount = Mathf.Min(_unitsPerSide, slots.Count);
            var provider = new PrototypeEntityProvider(prefabs, prototypeGroups, maxCount, _randomizeOrder, random);
            spawner.Spawn(slots, provider, root ? root : transform, maxCount, random, _applyRandomProperties);
        }

        private System.Random CreateRandom()
        {
            return _useSeed ? new System.Random(_seed) : new System.Random();
        }

        private static void ClearChildren(Transform root)
        {
            for (int i = root.childCount - 1; i >= 0; i--)
            {
                var child = root.GetChild(i);
                if (Application.isPlaying)
                {
                    Destroy(child.gameObject);
                }
                else
                {
                    DestroyImmediate(child.gameObject);
                }
            }
        }
    }
}
