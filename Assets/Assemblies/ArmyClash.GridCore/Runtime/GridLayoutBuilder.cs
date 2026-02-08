using System;
using System.Collections.Generic;
using UnityEngine;

namespace ArmyClash.Grid
{
    public class GridLayoutBuilder
    {
        public IReadOnlyList<GridSlot> Build(GridConfig config, Vector3 origin, Vector3 rightAxis, Vector3 forwardAxis, Quaternion rotation)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            int rows = Mathf.Max(1, config.Rows);
            int columns = Mathf.Max(1, config.Columns);
            float rowStep = config.RowStep;
            float columnStep = config.ColumnStep;

            var rightAxisNormalized = NormalizeAxis(rightAxis, Vector3.right);
            var forwardAxisNormalized = NormalizeAxis(forwardAxis, Vector3.forward);

            var slots = new List<GridSlot>(rows * columns);
            float columnStart = -(columns - 1) * 0.5f;
            float rowStart = -(rows - 1) * 0.5f;

            for (int row = 0; row < rows; row++)
            {
                float rowOffset = (rowStart + row) * rowStep;
                for (int column = 0; column < columns; column++)
                {
                    float columnOffset = (columnStart + column) * columnStep;
                    Vector3 position = origin + rightAxisNormalized * columnOffset + forwardAxisNormalized * rowOffset;
                    int index = row * columns + column;
                    slots.Add(new GridSlot(row, column, index, position, rotation));
                }
            }

            return slots;
        }

        private static Vector3 NormalizeAxis(Vector3 axis, Vector3 fallback)
        {
            if (axis.sqrMagnitude < 0.0001f)
            {
                return fallback;
            }

            return axis.normalized;
        }
    }
}
