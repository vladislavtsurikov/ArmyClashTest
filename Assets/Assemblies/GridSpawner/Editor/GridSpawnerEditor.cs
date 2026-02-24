#if UNITY_EDITOR
using UnityEditor;
using VladislavTsurikov.MegaWorld.Editor.Core.MonoBehaviour;
using VladislavTsurikov.ReflectionUtility;

namespace VladislavTsurikov.MegaWorld.Editor.GridSpawner
{
    [Name("Grid Spawner")]
    [CustomEditor(typeof(Runtime.GridSpawner.GridSpawner))]
    public sealed class GridSpawnerEditor : MonoBehaviourToolEditor
    {
    }
}
#endif
