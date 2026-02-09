using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VladislavTsurikov.MegaWorld.Runtime.Common.Settings;
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
    [AddGeneralGroupComponents(new[] { typeof(PrototypeGameObject) }, new[] { typeof(RandomSeedSettings) })]
    public class GridSpawner : StamperTool
    {
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
            get => _stamperControllerSettings != null ? _stamperControllerSettings.GridGenerator : null;
            set
            {
                if (_stamperControllerSettings != null)
                {
                    _stamperControllerSettings.GridGenerator = value;
                }
            }
        }

        protected override void OnStamperEnable()
        {
            var settings = (GridStamperControllerSettings)StamperControllerSettings;
            if (settings == null)
            {
                return;
            }

            if (settings.GridGenerator == null)
            {
                settings.GridGenerator = GetComponent<GridGenerator>();
            }
        }

        protected override void OnUpdate()
        {
        }

        protected override async UniTask Spawn(CancellationToken token, bool displayProgressBar)
        {
            var gridGenerator = GridGenerator;
            if (gridGenerator == null)
            {
                return;
            }

            var slots = gridGenerator.GetOrGenerate();
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
                UpdateDisplayProgressBar("Running", "Running " + Data.GroupList[typeIndex].name);
#endif

                await GridSpawnUtility.SpawnGroup(token, Data.GroupList[typeIndex], slots);

                completedTypes++;
                SpawnProgress = completedTypes / (float)maxTypes;
            }
        }

    }
}
