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
        private const string GroundMaterialPath = "Assets/Assemblies/Battle/Content/Materials/Ground.mat";

        private static readonly Vector3 DefaultPlanePosition = Vector3.zero;
        private static readonly Vector3 DefaultPlaneScale = new Vector3(10f, 1f, 10f);
        private static readonly Vector3 DefaultCameraPosition = new Vector3(0f, 10f, -10f);
        private static readonly Vector3 DefaultCameraRotation = new Vector3(35f, 0f, 0f);
        private static readonly Vector3 DefaultLightRotation = new Vector3(50f, -30f, 0f);
        private static readonly Color DefaultGroundColor = new Color(0.72f, 0.72f, 0.72f, 1f);

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

        [MenuItem("ArmyClash/Setup/Prepare Scene Environment")]
        public static void PrepareEnvironment()
        {
            Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            if (!scene.IsValid())
            {
                return;
            }

            EnsureGroundPlane();
            EnsureMainCamera();
            EnsureDirectionalLight();

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

        private static void EnsureGroundPlane()
        {
            GameObject plane = GameObject.Find("Ground");
            if (plane == null)
            {
                plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                plane.name = "Ground";
                Undo.RegisterCreatedObjectUndo(plane, "Create Ground");
            }

            Transform t = plane.transform;
            t.position = DefaultPlanePosition;
            t.rotation = Quaternion.identity;
            t.localScale = DefaultPlaneScale;

            MeshRenderer renderer = plane.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                Material material = EnsureGroundMaterial();
                if (material != null)
                {
                    renderer.sharedMaterial = material;
                }
            }
        }

        private static void EnsureMainCamera()
        {
            Camera camera = Camera.main;
            if (camera == null)
            {
                var go = new GameObject("Main Camera");
                Undo.RegisterCreatedObjectUndo(go, "Create Main Camera");
                camera = go.AddComponent<Camera>();
                go.tag = "MainCamera";
            }

            Transform t = camera.transform;
            t.position = DefaultCameraPosition;
            t.rotation = Quaternion.Euler(DefaultCameraRotation);

            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.72f, 0.82f, 0.92f, 1f);
        }

        private static void EnsureDirectionalLight()
        {
            Light light = Object.FindObjectOfType<Light>();
            if (light == null || light.type != LightType.Directional)
            {
                var go = new GameObject("Directional Light");
                Undo.RegisterCreatedObjectUndo(go, "Create Directional Light");
                light = go.AddComponent<Light>();
                light.type = LightType.Directional;
            }

            Transform t = light.transform;
            t.rotation = Quaternion.Euler(DefaultLightRotation);

            light.color = Color.white;
            light.intensity = 1f;
            light.shadows = LightShadows.Soft;
        }

        private static Material EnsureGroundMaterial()
        {
            Material material = AssetDatabase.LoadAssetAtPath<Material>(GroundMaterialPath);
            if (material != null)
            {
                if (material.color != DefaultGroundColor)
                {
                    material.color = DefaultGroundColor;
                    EditorUtility.SetDirty(material);
                }

                return material;
            }

            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null)
            {
                shader = Shader.Find("Standard");
            }

            if (shader == null)
            {
                Debug.LogWarning("Failed to find a shader for Ground material.");
                return null;
            }

            material = new Material(shader)
            {
                color = DefaultGroundColor
            };

            Directory.CreateDirectory(Path.GetDirectoryName(GroundMaterialPath) ?? "Assets");
            AssetDatabase.CreateAsset(material, GroundMaterialPath);
            AssetDatabase.SaveAssets();

            return material;
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
