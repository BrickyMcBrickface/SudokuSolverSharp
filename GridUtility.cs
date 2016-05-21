using System;

namespace SudokuSolverSharp
{
    public static class GridUtility
    {
        public static int GetGridIndex(this GridSize gridSize, GridLocation location)
        {
            return (location.Row - 1) * gridSize.Size + (location.Column - 1);
        }
        
        public static int GetRowForGridIndex(this GridSize gridSize, int index)
        {
            return index / gridSize.Size + 1;
        }
        
        public static int GetColumnForGridIndex(this GridSize gridSize, int index)
        {
            return (index % gridSize.Size) + 1;
        }
    }
}