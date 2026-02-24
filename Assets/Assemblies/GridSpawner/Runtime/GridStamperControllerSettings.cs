using ArmyClash.Grid;
using UnityEngine;
using VladislavTsurikov.MegaWorld.Runtime.Common.Stamper;
using VladislavTsurikov.ReflectionUtility;

namespace VladislavTsurikov.MegaWorld.Runtime.GridSpawner
{
    [Name("Grid Stamper Controller")]
    public class GridStamperControllerSettings : StamperControllerSettings
    {
        public GridConfig Config;
        public Color GizmoColor = new(0.1f, 0.6f, 1f, 0.8f);
    }
}
