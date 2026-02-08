using System.Collections.Generic;
using UnityEngine;

namespace ArmyClash.Grid
{
    [CreateAssetMenu(menuName = "ArmyClash/Grid/Entity Prototype Group", fileName = "EntityPrototypeGroup")]
    public class EntityPrototypeGroup : ScriptableObject
    {
        public List<EntityPrototype> Prototypes = new List<EntityPrototype>();
    }
}
