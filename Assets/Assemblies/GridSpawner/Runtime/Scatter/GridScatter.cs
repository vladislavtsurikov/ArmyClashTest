using System;
using System.Collections.Generic;
using System.Threading;
using ArmyClash.Grid;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VladislavTsurikov.MegaWorld.Runtime.Common.Area;
using VladislavTsurikov.ReflectionUtility;

namespace VladislavTsurikov.MegaWorld.Runtime.Common.Settings.ScatterSystem
{
    [Name("Grid Scatter")]
    public sealed class GridScatter : Scatter
    {
        private GridGenerator _gridGenerator;

        protected override void SetupComponent(object[] setupData = null)
        {
            if (setupData == null)
            {
                return;
            }

            for (int i = 0; i < setupData.Length; i++)
            {
                if (setupData[i] is GridGenerator gridGenerator)
                {
                    _gridGenerator = gridGenerator;
                    return;
                }
            }
        }

        public override async UniTask Samples(CancellationToken token, BoxArea boxArea, List<Vector3> samples,
            Action<Vector3> onSpawn = null)
        {
            samples.Clear();

            if (_gridGenerator.Slots == null)
            {
                return;
            }

            IReadOnlyList<GridSlot> slots = _gridGenerator.Slots;
            for (int i = 0; i < slots.Count; i++)
            {
                token.ThrowIfCancellationRequested();

                if (ScatterStack.IsWaitForNextFrame())
                {
                    await UniTask.Yield();
                }

                Vector3 position = slots[i].Position;
                Vector3 sample = position;
                samples.Add(sample);
                onSpawn?.Invoke(sample);
            }
        }
    }
}
