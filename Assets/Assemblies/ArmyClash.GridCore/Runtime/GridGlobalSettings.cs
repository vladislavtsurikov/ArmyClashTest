using UnityEngine;
using VladislavTsurikov.ScriptableObjectUtility.Runtime;

namespace ArmyClash.Grid
{
    [LocationAsset("ArmyClash/GridCore", false)]
    public sealed class GridGlobalSettings : SerializedScriptableObjectSingleton<GridGlobalSettings>
    {
        public bool ShowGizmos = true;
        public bool OverrideGizmoColor = false;
        public Color GizmoColor = new Color(0.1f, 0.6f, 1f, 0.8f);
        public float GizmoHeight = 0.02f;
        public float GizmoSizeScale = 1f;
    }
}
