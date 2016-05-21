using System;
using System.Linq;

namespace SudokuSolverSharp
{
    public class GridSolver
    {
        public GridSolution Solve(Grid grid)
        {
            if (grid == null)
            {
                throw new ArgumentNullException(nameof(grid));
            }
            
            var state = new GridSolverState(grid);
            
            for (var cell = state.CurrentCellState; cell != null; cell = state.MovePrevious())
            {
                for (var valueFlag = cell.NextValueFlag(); valueFlag != null; valueFlag = cell.NextValueFlag())
                {
                    if ((cell = state.MoveNext()) == null)
                    {
                        return GridSolution.Create(grid, state);
                    }
                }
            }
            
            return null;
        }
    }
    
    public class GridSolverState
    {
        public CellState[] CellStates
        {
            get { return _cellStates.Where(x => x != null).ToArray(); }
        }
        
        public CellState CurrentCellState
        {
            get { return _cellStates[_cellStateIndex]; }
        }
        
        public GridSolverState(Grid grid)
        {
            if (grid == null)
            {
                throw new ArgumentNullException(nameof(grid));
            }
            
            var gridSize = grid.GridSize;
            var completeValueFlags = (1 << gridSize.Size) - 1;
            
            _boxStates = new GroupValueFlagState[gridSize.Size + 1];
            _columnStates = new GroupValueFlagState[gridSize.Size + 1];
            _rowStates = new GroupValueFlagState[gridSize.Size + 1];
            
            _cellStates = new CellState[gridSize.CellCount + 1];
            
            for (int i = 1; i <= gridSize.Size; i++)
            {
                _boxStates[i] = new GroupValueFlagState(gridSize.CellCount);
                _columnStates[i] = new GroupValueFlagState(gridSize.CellCount);
                _rowStates[i] = new GroupValueFlagState(gridSize.CellCount);
            }
            
            var cellStateIndex = 1;
            
            foreach (var cell in grid.Cells)
            {
                var location = cell.Location;
                var boxNumber = gridSize.GetBoxNumber(location);
                var columnNumber = location.Column;
                var rowNumber = location.Row;
                
                if (!cell.IsEmpty())
                {
                    var value = (1 << (cell.Value - 1));
                    
                    _boxStates[boxNumber].Push(value);
                    _columnStates[columnNumber].Push(value);
                    _rowStates[rowNumber].Push(value);
                    
                    continue;
                }
                
                var cellState = new CellState(
                    boxNumber, rowNumber, columnNumber,
                    _boxStates[boxNumber], _rowStates[rowNumber], _columnStates[columnNumber],
                    completeValueFlags
                );
                
                _cellStates[cellStateIndex++] = cellState;
            }
            
            _cellStateIndex = 1;
            _lastCellStateIndex = cellStateIndex - 1;
            
            _cellStates[_cellStateIndex].RecalculateValueFlags();
        }
        
        public CellState MoveNext()
        {
            var currentCell = _cellStates[_cellStateIndex];
            
            currentCell.AssignValueFlag();
            
            if (_cellStateIndex == _lastCellStateIndex)
            {
                return null;
            }
            
            var nextCell = _cellStates[++_cellStateIndex];
            
            nextCell.RecalculateValueFlags();
            
            return nextCell;
        }
        
        public CellState MovePrevious()
        {
            if (_cellStateIndex == 1)
            {
                return null;
            }
            
            for (var previousCell = _cellStates[--_cellStateIndex]; 
                previousCell != null; 
                previousCell = _cellStates[--_cellStateIndex])
            {
                previousCell.UnassignValueFlag();
                
                if (previousCell.IsComplete())
                {
                    continue;
                }
                
                return previousCell;
            }
            
            if (_cellStateIndex < 1)
            {
                _cellStateIndex = 1;
            }
            
            return null;
        }
        
        private readonly GroupValueFlagState[] _boxStates;
        private readonly GroupValueFlagState[] _columnStates;
        private readonly GroupValueFlagState[] _rowStates;
        
        private readonly CellState[] _cellStates;
        private readonly int _lastCellStateIndex;
        
        private int _cellStateIndex;
    }
    
    public class GroupValueFlagState
    {
        public int CurrentValueFlags
        {
            get { return _items[_itemsIndex]; }
        }
        
        public GroupValueFlagState(int size)
        {
            _items = new int[size];
        }
        
        public int Push(int valueFlag)
        {
            var currentValueFlags = _items[_itemsIndex];
            
            return _items[++_itemsIndex] = currentValueFlags | valueFlag;
        }
        
        public int Pop()
        {
            return _items[--_itemsIndex];
        }
        
        private readonly int[] _items;
        
        private int _itemsIndex = 0;
    }
    
    public class CellState
    {
        public int BoxNumber { get; private set; }
        
        public int ColumnNumber { get; private set; }
        
        public int RowNumber { get; private set; }
        
        public int ValueFlag { get; private set; }
        
        public CellState(
            int boxNumber, int rowNumber, int columnNumber,
            GroupValueFlagState boxState, GroupValueFlagState rowState, GroupValueFlagState columnState,
            int completeValueFlags)
        {
            this.BoxNumber = boxNumber;
            this.RowNumber = rowNumber;
            this.ColumnNumber = columnNumber;
            
            _boxState = boxState;
            _rowState = rowState;
            _columnState = columnState;
            
            _completeValueFlags = completeValueFlags;
        }
        
        public bool IsComplete()
        {
            return _valueFlags == _completeValueFlags;
        }
        
        public void AssignValueFlag()
        {
            _boxState.Push(ValueFlag);
            _rowState.Push(ValueFlag);
            _columnState.Push(ValueFlag);
        }
        
        public void UnassignValueFlag()
        {
            _boxState.Pop();
            _rowState.Pop();
            _columnState.Pop();
        }
        
        public void RecalculateValueFlags()
        {
            _valueFlags = 
                _boxState.CurrentValueFlags |
                _rowState.CurrentValueFlags |
                _columnState.CurrentValueFlags;
                
            ValueFlag = 0;
        }
        
        public int? NextValueFlag()
        {
            if(_valueFlags == _completeValueFlags)
            {
                return null;
            }
            
            _valueFlags |= (ValueFlag = (_valueFlags + 1) & ~_valueFlags);
            
            return ValueFlag;
        }
        
        private readonly GroupValueFlagState _boxState;
        private readonly GroupValueFlagState _columnState;
        private readonly GroupValueFlagState _rowState;
        private readonly int _completeValueFlags;
        
        private int _valueFlags;
    }
    
    public class GridSolution
    {
        public Grid OriginalGrid { get; private set; }
        
        public Grid SolvedGrid { get; private set; }
        
        private GridSolution(Grid originalGrid)
        {
            this.OriginalGrid = originalGrid;
        }
        
        public static GridSolution Create(Grid originalGrid, GridSolverState state)
        {
            var solution = new GridSolution(originalGrid);
            var gridSize = originalGrid.GridSize;
            
            var values = new int[originalGrid.GridSize.CellCount];
            
            foreach (var cell in originalGrid.Cells.Where(cell => !cell.IsEmpty()))
            {
                values[gridSize.GetGridIndex(cell.Location)] = cell.Value;
            }
            
            foreach (var cellState in state.CellStates)
            {
                var value = (int)Math.Log(cellState.ValueFlag, 2) + 1;
                var location = new GridLocation(cellState.RowNumber, cellState.ColumnNumber);
                
                values[gridSize.GetGridIndex(location)] = value;
            }
            
            solution.SolvedGrid = Grid.Load(gridSize, values);
            
            return solution;
        }
    }
}