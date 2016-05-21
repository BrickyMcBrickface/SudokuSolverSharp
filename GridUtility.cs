using System;

namespace SudokuSolverSharp
{
    public static class GridUtility
    {
        public static int GetGridIndex(GridLocation location, GridSize gridSize)
        {
            return (location.Row - 1) * gridSize.Size + (location.Column - 1);
        }
        
        public static int GetRowForGridIndex(GridSize gridSize, int index)
        {
            return index / gridSize.Size + 1;
        }
        
        public static int GetColumnForGridIndex(GridSize gridSize, int index)
        {
            return (index % gridSize.Size) + 1;
        }
    }
}