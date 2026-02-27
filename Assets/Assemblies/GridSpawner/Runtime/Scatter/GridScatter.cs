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

            foreach (object t in setupData)
            {
                if (t is GridGenerator gridGenerator)
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

            IReadOnlyList<GridSlot> slots = _gridGenerator.Slots;
            for (int i = 0; i < slots.Count; i++)
            {
                token.ThrowIfCancellationRequested();

                Vector3 position = slots[i].Position;
                Vector3 sample = position;
                samples.Add(sample);
                onSpawn?.Invoke(sample);
            }
        }
    }
}
