using ArmyClash.Grid;
using UnityEngine;
using VladislavTsurikov.MegaWorld.Runtime.Common.Stamper;
using VladislavTsurikov.ReflectionUtility;

namespace VladislavTsurikov.MegaWorld.Runtime.GridSpawner
{
    [Name("Grid Stamper Controller")]
    public class GridStamperControllerSettings : StamperControllerSettings
    {
        public GridConfig Config = new GridConfig();
        public Color GizmoColor = new Color(0.1f, 0.6f, 1f, 0.8f);
    }
}
