using System;

namespace SudokuSolverSharp
{
    public class Cell
    {
        public GridLocation Location { get; private set; }
        
        public int Value { get; private set; }
        
        public Cell(GridLocation location, int value)
        {
            if (location == null)
            {
                throw new ArgumentNullException(nameof(location));
            }
            
            if (value < 0)
            {
                throw new ArgumentException("The value must be greater than or equal to 0.", nameof(value));
            }
            
            this.Location = location;
            this.Value = value;
        }
        
        public bool IsEmpty()
        {
            return (Value == 0);
        }
    }
}