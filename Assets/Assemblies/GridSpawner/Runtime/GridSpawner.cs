using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VladislavTsurikov.MegaWorld.Runtime.Common.Settings;
using VladislavTsurikov.MegaWorld.Runtime.Common.Settings.ScatterSystem;
using VladislavTsurikov.MegaWorld.Runtime.Common.Stamper;
using VladislavTsurikov.MegaWorld.Runtime.Core.MonoBehaviour;
using VladislavTsurikov.MegaWorld.Runtime.Core.SelectionDatas.Attributes;
using VladislavTsurikov.MegaWorld.Runtime.Core.SelectionDatas.ElementsSystem;
using VladislavTsurikov.MegaWorld.Runtime.Core.SelectionDatas.Group.Prototypes;
using VladislavTsurikov.MegaWorld.Runtime.Core.SelectionDatas.Group.Prototypes.PrototypeGameObject;
using VladislavTsurikov.ReflectionUtility;
using ArmyClash.Grid;

namespace VladislavTsurikov.MegaWorld.Runtime.GridSpawner
{
    [ExecuteInEditMode]
    [Name("Grid Spawner")]
    [SupportMultipleSelectedGroups]
    [SupportedPrototypeTypes(new[] { typeof(PrototypeGameObject) })]
    [AddMonoBehaviourComponents(new[] { typeof(GridStamperControllerSettings) })]
    [AddGeneralPrototypeComponents(typeof(PrototypeGameObject), new[] { typeof(SuccessSettings) })]
    [AddGeneralGroupComponents(new[] { typeof(PrototypeGameObject) },
        new[] { typeof(RandomSeedSettings), typeof(ScatterComponentSettings) })]
    public class GridSpawner : StamperTool
    {
        private GridGenerator _gridGenerator;

        [NonSerialized]
        private GridStamperControllerSettings _stamperControllerSettings;

        public StamperControllerSettings StamperControllerSettings
        {
            get
            {
                if (_stamperControllerSettings == null || _stamperControllerSettings.IsHappenedReset)
                {
                    _stamperControllerSettings =
                        (GridStamperControllerSettings)GetElement(typeof(GridStamperControllerSettings));
                }

                return _stamperControllerSettings;
            }
        }

        public GridGenerator GridGenerator
        {
            get
            {
                _gridGenerator ??= new GridGenerator();
                return _gridGenerator;
            }
        }

        public GridConfig Config
        {
            get
            {
                var settings = (GridStamperControllerSettings)StamperControllerSettings;
                if (settings == null)
                {
                    return null;
                }

                settings.Config ??= new GridConfig();
                return settings.Config;
            }
        }

        protected override async UniTask Spawn(CancellationToken token, bool displayProgressBar)
        {
            await SpawnAndCollect(token, displayProgressBar);
        }

        public void ApplyConfig(GridConfig config)
        {
            var settings = (GridStamperControllerSettings)StamperControllerSettings;

            settings.Config ??= new GridConfig();
            settings.Config.CopyFrom(config);
        }

        public async UniTask SpawnAndCollect(CancellationToken token, bool displayProgressBar)
        {
            await SpawnAndCollect(token, displayProgressBar, null);
        }

        public async UniTask SpawnAndCollect(
            CancellationToken token,
            bool displayProgressBar,
            Action<GameObject> onSpawn)
        {
            var gridGenerator = GridGenerator;
            var slots = gridGenerator.Build(Config, transform.position, transform.right, transform.forward,
                transform.rotation);
            if (slots.Count == 0)
            {
                return;
            }

            var maxTypes = Data.GroupList.Count;
            var completedTypes = 0;

            for (var typeIndex = 0; typeIndex < Data.GroupList.Count; typeIndex++)
            {
                token.ThrowIfCancellationRequested();
#if UNITY_EDITOR
                if (displayProgressBar)
                {
                    UpdateDisplayProgressBar("Running", "Running " + Data.GroupList[typeIndex].name);
                }
#endif

                await GridSpawnUtility.SpawnGroup(
                    token,
                    Data.GroupList[typeIndex],
                    gridGenerator,
                    onSpawn);

                completedTypes++;
                SpawnProgress = completedTypes / (float)maxTypes;
            }
        }

        private void OnDrawGizmos()
        {
            var config = Config;
            if (config == null)
            {
                return;
            }

            var settings = (GridStamperControllerSettings)StamperControllerSettings;

            GridGizmoDrawer.Draw(config, transform.position, transform.right, transform.forward, transform.rotation,
                settings.GizmoColor);
        }
    }
}
