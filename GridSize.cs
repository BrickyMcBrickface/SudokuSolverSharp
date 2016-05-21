using System;

namespace SudokuSolverSharp
{
    public class GridSize
    {
        public int BandSize { get { return BoxHeight; } }
        
        public int BoxHeight { get; private set; }
        
        public int BoxWidth { get; private set; }
        
        public int CellCount { get { return Size * Size; } }
        
        public int Size { get { return BoxWidth * BoxHeight; } }
        
        public int StackSize { get { return BoxWidth; } }
        
        public GridSize(int boxWidth, int boxHeight)
        {
            if (boxWidth <= 0)
            {
                throw new ArgumentException("The box width must be greater than 0.", nameof(boxWidth));
            }
            
            if (boxHeight <= 0)
            {
                throw new ArgumentException("The box height must be greater than 0.", nameof(boxHeight));
            }
            
            this.BoxWidth = boxWidth;
            this.BoxHeight = boxHeight;
        }
    }
}