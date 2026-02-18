using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using ArmyClash.Grid;
using UnityEngine;
using VladislavTsurikov.MegaWorld.Runtime.Common.Area;
using VladislavTsurikov.ReflectionUtility;

namespace VladislavTsurikov.MegaWorld.Runtime.Common.Settings.ScatterSystem
{
    [Name("Grid Scatter")]
    public sealed class GridScatter : Scatter
    {
        private GridScatterContext _context;

        protected override void SetupComponent(object[] setupData = null)
        {
            _context = null;

            if (setupData == null)
            {
                return;
            }

            for (int i = 0; i < setupData.Length; i++)
            {
                if (setupData[i] is GridScatterContext context)
                {
                    _context = context;
                    return;
                }
            }
        }

        public override async UniTask Samples(CancellationToken token, BoxArea boxArea, List<Vector3> samples,
            Action<Vector3> onSpawn = null)
        {
            samples.Clear();

            if (_context == null || _context.Slots == null)
            {
                return;
            }

            var slots = _context.Slots;
            for (int i = 0; i < slots.Count; i++)
            {
                token.ThrowIfCancellationRequested();

                if (ScatterStack.IsWaitForNextFrame())
                {
                    await UniTask.Yield();
                }

                Vector3 position = slots[i].Position;
                var sample = position;
                samples.Add(sample);
                onSpawn?.Invoke(sample);
            }
        }
    }

    public sealed class GridScatterContext
    {
        public IReadOnlyList<GridSlot> Slots { get; }

        public GridScatterContext(IReadOnlyList<GridSlot> slots)
        {
            Slots = slots ?? Array.Empty<GridSlot>();
        }
    }
}
