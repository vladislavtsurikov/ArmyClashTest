using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VladislavTsurikov.CustomInspector.Runtime;
using VladislavTsurikov.MegaWorld.Runtime.Common.Area;
using VladislavTsurikov.MegaWorld.Runtime.Core.PreferencesSystem;
using VladislavTsurikov.ReflectionUtility;
using Random = UnityEngine.Random;

namespace VladislavTsurikov.MegaWorld.Runtime.Common.Settings.ScatterSystem
{
    [Name("Random Point")]
    public class RandomPoint : Scatter
    {
        [HideInInspector]
        public int MaxChecks = 15;

        [MinMaxSlider(1f, 15f, nameof(MaxChecks), MaxValueMemberName = nameof(MaxChecksLimit), LabelOverride = "Checks")]
        public int MinChecks = 15;

        public int MaxChecksLimit => PreferenceElementSingleton<ScatterPreferenceSettings>.Instance.MaxChecks;

        public override async UniTask Samples(CancellationToken token, BoxArea boxArea, List<Vector3> samples,
            Action<Vector3> onSpawn = null)
        {
            var numberOfChecks = Random.Range(MinChecks, MaxChecks);

            for (var checks = 0; checks < numberOfChecks; checks++)
            {
                token.ThrowIfCancellationRequested();

                if (ScatterStack.IsWaitForNextFrame())
                {
                    await UniTask.Yield();
                }

                Vector3 point = GetRandomPoint(boxArea);
                onSpawn?.Invoke(point);
                samples.Add(point);
            }
        }

        private Vector3 GetRandomPoint(BoxArea boxArea)
        {
            Vector2 spawnOffset = new Vector2(Random.Range(-boxArea.Radius, boxArea.Radius),
                Random.Range(-boxArea.Radius, boxArea.Radius));
            return new Vector3(spawnOffset.x + boxArea.RayHit.Point.x, boxArea.RayHit.Point.y,
                spawnOffset.y + boxArea.RayHit.Point.z);
        }
    }
}
