using System;
using UnityEngine;

namespace ArmyClash.Grid
{
    [Serializable]
    public class EntityPrototype
    {
        public GameObject Prefab;

        [Range(0f, 1f)]
        public float Success = 0.1f;
    }
}
