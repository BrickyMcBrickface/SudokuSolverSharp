using System;

namespace SudokuSolverSharp
{
    public class GridLocation
    {
        public int Column { get; private set; }
        
        public int Row { get; private set; }
        
        public GridLocation(int row, int column)
        {
            if (row <= 0)
            {
               throw new ArgumentException("The row must be greater than 0.", nameof(row)); 
            }
            
            if (column <= 0)
            {
                throw new ArgumentException("The column must be greater than 0.", nameof(column));
            }
            
            this.Row = row;
            this.Column = column;
        }
    }
}