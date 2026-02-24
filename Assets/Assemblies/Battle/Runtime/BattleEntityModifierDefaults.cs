using System.Collections.Generic;
using ArmyClash.Battle.Actions;
using UnityEditor;
using System.Linq;
using UnityEngine;
using VladislavTsurikov.ActionFlow.Runtime.Stats;
using VladislavTsurikov.EntityDataAction.Runtime.Core;
using VladislavTsurikov.EntityDataAction.Shared.Runtime.Stats;

namespace ArmyClash.Battle.Editor
{
    [ExecuteInEditMode]
    public class BattleEntityModifierDefaults : MonoBehaviour
    {
        private static List<ModifierStatEffect> _colorEffects;
        private static List<ModifierStatEffect> _sizeEffects;
        private static List<ModifierStatEffect> _shapeEffects;

        private static readonly string[] ColorEffectPaths =
        {
            "Assets/Assemblies/Battle/Content/Effects/Color_Blue.asset",
            "Assets/Assemblies/Battle/Content/Effects/Color_Green.asset",
            "Assets/Assemblies/Battle/Content/Effects/Color_Red.asset"
        };

        private static readonly string[] SizeEffectPaths =
        {
            "Assets/Assemblies/Battle/Content/Effects/Size_Big.asset",
            "Assets/Assemblies/Battle/Content/Effects/Size_Small.asset"
        };

        private static readonly string[] ShapeEffectPaths =
        {
            "Assets/Assemblies/Battle/Content/Effects/Shape_Cube.asset",
            "Assets/Assemblies/Battle/Content/Effects/Shape_Sphere.asset"
        };

        private const string BattleEntityCollectionPath =
            "Assets/Assemblies/Battle/Content/Collections/BattleEntityCollection.asset";

        private void OnEnable()
        {
            EntityMonoBehaviour entity = GetComponent<EntityMonoBehaviour>();
            ApplyDefaults(entity);
        }

        public static void ApplyDefaults(EntityMonoBehaviour entity)
        {
            if (_colorEffects == null)
            {
                _colorEffects = LoadEffects(ColorEffectPaths);
                _sizeEffects = LoadEffects(SizeEffectPaths);
                _shapeEffects = LoadEffects(ShapeEffectPaths);
            }

            StatsEntityData stats = entity.GetData<StatsEntityData>();
            if (stats.Collection == null)
            {
                stats.Collection = AssetDatabase.LoadAssetAtPath<StatCollection>(BattleEntityCollectionPath);
            }

            var actions = entity.Actions.ElementList
                .OfType<SelectRandomModifierEffectAction>()
                .ToList();

            SetOptions(actions[0], _colorEffects);
            SetOptions(actions[1], _sizeEffects);
            SetOptions(actions[2], _shapeEffects);
        }

        private static List<ModifierStatEffect> LoadEffects(string[] paths)
        {
            var list = new List<ModifierStatEffect>(paths.Length);
            for (int i = 0; i < paths.Length; i++)
            {
                list.Add(AssetDatabase.LoadAssetAtPath<ModifierStatEffect>(paths[i]));
            }

            return list;
        }

        private static void SetOptions(SelectRandomModifierEffectAction action, List<ModifierStatEffect> options)
        {
            var field = typeof(SelectRandomModifierEffectAction)
                .GetField("_options", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            field.SetValue(action, options);
        }
    }
}
