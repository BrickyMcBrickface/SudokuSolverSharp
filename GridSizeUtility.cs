using System;

namespace SudokuSolverSharp
{
    public static class GridSizeUtility
    {
        public static int GetBoxBand(this GridSize gridSize, int box)
        {
            return (box - 1) / gridSize.BandSize + 1;
        }
        
        public static int GetBoxStack(this GridSize gridSize, int box)
        {
            return ((box - 1) % gridSize.BandSize) + 1;
        }
    }
}