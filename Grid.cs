using System;
using System.Linq;

namespace SudokuSolverSharp
{
    public class Grid
    {
        public Cell[] Cells { get { return _cells.ToArray(); } }
        
        public GridSize GridSize { get; private set; }
        
        private Grid(GridSize gridSize)
        {
            this.GridSize = gridSize;
            this._cells = new Cell[gridSize.CellCount];
        }
        
        public override string ToString()
        {
            return String.Join(", ", _cells.Select(cell => cell.Value));
        }
        
        public static Grid Load(GridSize gridSize, int[] values)
        {
            if (gridSize == null)
            {
                throw new ArgumentNullException(nameof(gridSize));
            }
            
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            
            if (values.Length != gridSize.CellCount)
            {
                throw new ArgumentException("The number of values does not match the grid size.", nameof(values));
            }
            
            var grid = new Grid(gridSize);
            
            for (int i = 0; i < gridSize.CellCount; i++)
            {
                var location = new GridLocation(
                    GridUtility.GetRowForGridIndex(gridSize, i), 
                    GridUtility.GetColumnForGridIndex(gridSize, i));
                    
                var cell = new Cell(location, values[i]);
                
                if (cell.Value > gridSize.Size)
                {
                    throw new ArgumentException("Invalid value found for the grid size.", nameof(values));
                }
                
                grid._cells[i] = cell;
            }
            
            return grid;
        }
        
        private readonly Cell[] _cells;
    }
}