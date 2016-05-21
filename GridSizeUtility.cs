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
        
        public static int GetBoxBand(this GridSize gridSize, GridLocation location)
        {
            return (location.Row - 1) / gridSize.BoxHeight + 1;
        }
        
        public static int GetBoxStack(this GridSize gridSize, GridLocation location)
        {
            return (location.Column - 1) / gridSize.BoxWidth + 1;
        }
        
        public static int GetBoxNumber(this GridSize gridSize, GridLocation location)
        {
            var stackNumber = gridSize.GetBoxStack(location);
            var bandNumber = gridSize.GetBoxBand(location);
            
            return stackNumber + gridSize.BandSize * (bandNumber - 1);
        }
    }
}