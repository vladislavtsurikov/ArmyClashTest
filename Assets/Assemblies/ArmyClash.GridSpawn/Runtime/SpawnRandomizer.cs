using UnityEngine;

namespace ArmyClash.Grid
{
    public abstract class SpawnRandomizer : MonoBehaviour
    {
        public abstract void ApplyRandom(System.Random random);
    }
}
