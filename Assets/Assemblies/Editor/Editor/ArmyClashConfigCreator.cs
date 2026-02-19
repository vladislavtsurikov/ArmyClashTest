#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using VladislavTsurikov.ActionFlow.Runtime.Modifier;
using UnityEditor;
using UnityEngine;
using VladislavTsurikov.ActionFlow.Runtime.Stats;

namespace ArmyClash.Editor.Modifier
{
    public static class ArmyClashConfigCreator
    {
        private const string RootPath = "Assets/Content";
        private const string StatsPath = RootPath + "/Stats";
        private const string EffectsPath = RootPath + "/Effects";
        private const string ModifiersPath = RootPath + "/Modifiers";
        private const string ShapesPath = ModifiersPath + "/Shapes";
        private const string SizesPath = ModifiersPath + "/Sizes";
        private const string ColorsPath = ModifiersPath + "/Colors";

        [MenuItem("ArmyClash/Configs/Create Default Modifier Configs")]
        public static void CreateDefaults()
        {
            EnsureFolders();

            Stat hp = CreateStat("HP", 100f);
            Stat atk = CreateStat("ATK", 10f);
            Stat speed = CreateStat("SPEED", 10f);
            Stat atkSpd = CreateStat("ATKSPD", 1f);
            Stat regen = CreateStat("REGEN", 0f);

            StatCollection collection = CreateStatCollection(new[] { hp, atk, speed, atkSpd, regen });

            ModifierStatEffect cubeEffect = CreateEffect("Shape_Cube", new[]
            {
                (hp, 100f),
                (atk, 10f)
            });
            ModifierStatEffect sphereEffect = CreateEffect("Shape_Sphere", new[]
            {
                (hp, 50f),
                (atk, 20f)
            });

            ModifierStatEffect bigEffect = CreateEffect("Size_Big", new[]
            {
                (hp, 50f)
            });
            ModifierStatEffect smallEffect = CreateEffect("Size_Small", new[]
            {
                (hp, -50f)
            });

            ModifierStatEffect blueEffect = CreateEffect("Color_Blue", new[]
            {
                (atk, -15f),
                (atkSpd, 4f),
                (speed, 10f)
            });
            ModifierStatEffect greenEffect = CreateEffect("Color_Green", new[]
            {
                (hp, -50f),
                (atk, 20f),
                (speed, -5f)
            });
            ModifierStatEffect redEffect = CreateEffect("Color_Red", new[]
            {
                (hp, 200f),
                (atk, 40f),
                (speed, -9f)
            });

            ModifierStatEffect goldEffect = CreateEffect("Color_Gold", new[]
            {
                (hp, 80f),
                (atk, 25f),
                (regen, 3f)
            });

            CreateShapeModifier("CUBE", "Cube", cubeEffect, null);
            CreateShapeModifier("SPHERE", "Sphere", sphereEffect, null);

            CreateSizeModifier("BIG", "Big", bigEffect, Vector3.one * 1.2f);
            CreateSizeModifier("SMALL", "Small", smallEffect, Vector3.one * 0.8f);

            CreateColorModifier("BLUE", "Blue", blueEffect, new Color(0.2f, 0.4f, 1f));
            CreateColorModifier("GREEN", "Green", greenEffect, new Color(0.2f, 0.8f, 0.3f));
            CreateColorModifier("RED", "Red", redEffect, new Color(1f, 0.2f, 0.2f));
            CreateColorModifier("GOLD", "Gold", goldEffect, new Color(1f, 0.85f, 0.1f));

            EditorUtility.SetDirty(collection);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void EnsureFolders()
        {
            CreateFolderIfMissing("Assets", "Content");
            CreateFolderIfMissing(RootPath, "Stats");
            CreateFolderIfMissing(RootPath, "Effects");
            CreateFolderIfMissing(RootPath, "Modifiers");
            CreateFolderIfMissing(ModifiersPath, "Shapes");
            CreateFolderIfMissing(ModifiersPath, "Sizes");
            CreateFolderIfMissing(ModifiersPath, "Colors");
        }

        private static void CreateFolderIfMissing(string parent, string name)
        {
            string path = parent + "/" + name;
            if (!AssetDatabase.IsValidFolder(path))
            {
                AssetDatabase.CreateFolder(parent, name);
            }
        }

        private static Stat CreateStat(string id, float baseValue)
        {
            string assetPath = Path.Combine(StatsPath, id + ".asset").Replace("\\", "/");
            Stat stat = AssetDatabase.LoadAssetAtPath<Stat>(assetPath);
            if (stat == null)
            {
                stat = ScriptableObject.CreateInstance<Stat>();
                AssetDatabase.CreateAsset(stat, assetPath);
            }

            SetPrivateField(stat, "_id", id);
            EnsureStatValueComponent(stat, baseValue);
            EditorUtility.SetDirty(stat);
            return stat;
        }

        private static void EnsureStatValueComponent(Stat stat, float baseValue)
        {
            if (stat.ComponentStack.GetElement<StatValueComponent>() == null)
            {
                stat.ComponentStack.CreateIfMissingType(typeof(StatValueComponent));
            }

            StatValueComponent valueComponent = stat.ComponentStack.GetElement<StatValueComponent>();
            if (valueComponent != null)
            {
                valueComponent.SetBaseValue(baseValue);
            }
        }

        private static StatCollection CreateStatCollection(IEnumerable<Stat> stats)
        {
            string assetPath = Path.Combine(StatsPath, "StatCollection.asset").Replace("\\", "/");
            StatCollection collection = AssetDatabase.LoadAssetAtPath<StatCollection>(assetPath);
            if (collection == null)
            {
                collection = ScriptableObject.CreateInstance<StatCollection>();
                AssetDatabase.CreateAsset(collection, assetPath);
            }

            var so = new SerializedObject(collection);
            var list = so.FindProperty("_stats");
            list.ClearArray();
            foreach (Stat stat in stats)
            {
                int index = list.arraySize;
                list.InsertArrayElementAtIndex(index);
                list.GetArrayElementAtIndex(index).objectReferenceValue = stat;
            }
            so.ApplyModifiedProperties();
            return collection;
        }

        private static ModifierStatEffect CreateEffect(string name, (Stat stat, float delta)[] entries)
        {
            string assetPath = Path.Combine(EffectsPath, name + ".asset").Replace("\\", "/");
            ModifierStatEffect effect = AssetDatabase.LoadAssetAtPath<ModifierStatEffect>(assetPath);
            if (effect == null)
            {
                effect = ScriptableObject.CreateInstance<ModifierStatEffect>();
                AssetDatabase.CreateAsset(effect, assetPath);
            }

            var so = new SerializedObject(effect);
            var list = so.FindProperty("_entries");
            list.ClearArray();
            for (int i = 0; i < entries.Length; i++)
            {
                list.InsertArrayElementAtIndex(i);
                var entry = list.GetArrayElementAtIndex(i);
                entry.FindPropertyRelative("Stat").objectReferenceValue = entries[i].stat;
                entry.FindPropertyRelative("Delta").floatValue = entries[i].delta;
            }
            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(effect);
            return effect;
        }

        private static void CreateShapeModifier(string id, string displayName, ModifierStatEffect effect, GameObject prefab)
        {
            string assetPath = Path.Combine(ShapesPath, id + ".asset").Replace("\\", "/");
            ShapeModifier modifier = AssetDatabase.LoadAssetAtPath<ShapeModifier>(assetPath);
            if (modifier == null)
            {
                modifier = ScriptableObject.CreateInstance<ShapeModifier>();
                AssetDatabase.CreateAsset(modifier, assetPath);
            }

            SetPrivateField(modifier, "_id", id);
            SetPrivateField(modifier, "_displayName", displayName);
            SetPrivateField(modifier, "_statEffect", effect);
            SetPrivateField(modifier, "_prefab", prefab);
            EditorUtility.SetDirty(modifier);
            SetEffectModifier(effect, modifier);
        }

        private static void CreateSizeModifier(string id, string displayName, ModifierStatEffect effect, Vector3 scale)
        {
            string assetPath = Path.Combine(SizesPath, id + ".asset").Replace("\\", "/");
            SizeModifier modifier = AssetDatabase.LoadAssetAtPath<SizeModifier>(assetPath);
            if (modifier == null)
            {
                modifier = ScriptableObject.CreateInstance<SizeModifier>();
                AssetDatabase.CreateAsset(modifier, assetPath);
            }

            SetPrivateField(modifier, "_id", id);
            SetPrivateField(modifier, "_displayName", displayName);
            SetPrivateField(modifier, "_statEffect", effect);
            SetPrivateField(modifier, "_scale", scale);
            EditorUtility.SetDirty(modifier);
            SetEffectModifier(effect, modifier);
        }

        private static void CreateColorModifier(string id, string displayName, ModifierStatEffect effect, Color color)
        {
            string assetPath = Path.Combine(ColorsPath, id + ".asset").Replace("\\", "/");
            ColorModifier modifier = AssetDatabase.LoadAssetAtPath<ColorModifier>(assetPath);
            if (modifier == null)
            {
                modifier = ScriptableObject.CreateInstance<ColorModifier>();
                AssetDatabase.CreateAsset(modifier, assetPath);
            }

            SetPrivateField(modifier, "_id", id);
            SetPrivateField(modifier, "_displayName", displayName);
            SetPrivateField(modifier, "_statEffect", effect);
            SetPrivateField(modifier, "_color", color);
            EditorUtility.SetDirty(modifier);
            SetEffectModifier(effect, modifier);
        }

        private static void SetEffectModifier(ModifierStatEffect effect, VladislavTsurikov.ActionFlow.Runtime.Modifier.Modifier modifier)
        {
            if (effect == null || modifier == null)
            {
                return;
            }

            var so = new SerializedObject(effect);
            var prop = so.FindProperty("_modifier");
            if (prop != null)
            {
                prop.objectReferenceValue = modifier;
                so.ApplyModifiedProperties();
                EditorUtility.SetDirty(effect);
            }
        }

        private static void SetPrivateField(UnityEngine.Object obj, string fieldName, object value)
        {
            var so = new SerializedObject(obj);
            var prop = so.FindProperty(fieldName);
            if (prop == null)
            {
                return;
            }

            switch (prop.propertyType)
            {
                case SerializedPropertyType.String:
                    prop.stringValue = value as string;
                    break;
                case SerializedPropertyType.ObjectReference:
                    prop.objectReferenceValue = value as UnityEngine.Object;
                    break;
                case SerializedPropertyType.Vector3:
                    prop.vector3Value = (Vector3)value;
                    break;
                case SerializedPropertyType.Color:
                    prop.colorValue = (Color)value;
                    break;
            }

            so.ApplyModifiedProperties();
        }
    }
}
#endif
