using System.IO;
using ArmyClash.Battle.Installers;
using ArmyClash.MegaWorldGrid;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Zenject;

namespace ArmyClash.Editor
{
    public static class ArmyClashSceneSetup
    {
        private const string BattleUxmlPath = "Assets/Assemblies/Battle/Content/UI/ArmyClashBattle.uxml";
        private const string PanelSettingsPath =
            "Assets/Assemblies/Battle/Content/UI/ArmyClashPanelSettings.asset";

        [MenuItem("ArmyClash/Setup/Prepare Scene For Battle")]
        public static void PrepareScene()
        {
            Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            if (!scene.IsValid())
            {
                return;
            }

            EnsureProjectContextPrefab();

            SceneContext sceneContext = Object.FindObjectOfType<SceneContext>();
            if (sceneContext == null)
            {
                var go = new GameObject("SceneContext");
                Undo.RegisterCreatedObjectUndo(go, "Create SceneContext");
                sceneContext = go.AddComponent<SceneContext>();
            }

            EnsureComponent<BattleWorldInstaller>(sceneContext.gameObject);
            EnsureComponent<GridSpawnerPairInstaller>(sceneContext.gameObject);

            GridSpawnerPair pair = Object.FindObjectOfType<GridSpawnerPair>();
            GridSpawnerPairSetup setup;
            if (pair == null)
            {
                var go = new GameObject("GridSpawnerPair");
                Undo.RegisterCreatedObjectUndo(go, "Create GridSpawnerPair");
                pair = go.AddComponent<GridSpawnerPair>();
                setup = go.AddComponent<GridSpawnerPairSetup>();
            }
            else
            {
                setup = pair.GetComponent<GridSpawnerPairSetup>();
                if (setup == null)
                {
                    setup = pair.gameObject.AddComponent<GridSpawnerPairSetup>();
                }
            }

            SetPrivateField(setup, "_pair", pair);
            setup.CreateSpawners();
            setup.ApplyTransforms();

            EnsureBattleUi();

            EditorSceneManager.MarkSceneDirty(scene);
        }

        private static void EnsureBattleUi()
        {
            var uxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(BattleUxmlPath);
            if (uxml == null)
            {
                return;
            }

            UIDocument document = Object.FindObjectOfType<UIDocument>();
            if (document == null)
            {
                var go = new GameObject("BattleUI");
                Undo.RegisterCreatedObjectUndo(go, "Create Battle UI");
                document = go.AddComponent<UIDocument>();
            }

            if (document.visualTreeAsset != uxml)
            {
                document.visualTreeAsset = uxml;
            }

            PanelSettings panelSettings = AssetDatabase.LoadAssetAtPath<PanelSettings>(PanelSettingsPath);
            if (panelSettings == null)
            {
                panelSettings = ScriptableObject.CreateInstance<PanelSettings>();
                Directory.CreateDirectory(Path.GetDirectoryName(PanelSettingsPath) ?? "Assets");
                AssetDatabase.CreateAsset(panelSettings, PanelSettingsPath);
            }

            if (document.panelSettings != panelSettings)
            {
                document.panelSettings = panelSettings;
            }
        }

        private static void EnsureProjectContextPrefab()
        {
            const string resourcesPath = "Assets/Resources";
            const string prefabPath = "Assets/Resources/ProjectContext.prefab";

            if (File.Exists(prefabPath))
            {
                return;
            }

            Directory.CreateDirectory(resourcesPath);

            var go = new GameObject("ProjectContext");
            go.AddComponent<ProjectContext>();
            PrefabUtility.SaveAsPrefabAsset(go, prefabPath);
            Object.DestroyImmediate(go);
        }

        private static void EnsureComponent<T>(GameObject owner) where T : Component
        {
            if (owner.GetComponent<T>() == null)
            {
                owner.AddComponent<T>();
            }
        }

        private static void SetPrivateField(Object target, string fieldName, Object value)
        {
            var so = new SerializedObject(target);
            SerializedProperty prop = so.FindProperty(fieldName);
            if (prop != null)
            {
                prop.objectReferenceValue = value;
                so.ApplyModifiedPropertiesWithoutUndo();
            }
        }
    }
}
