using System;
using System.Diagnostics;

namespace SudokuSolverSharp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Solve the 9x9 puzzles.
            SolvePuzzle(GridSize.Default, Puzzle9x9.EasyPuzzle, (solution, time) => 
            {
                OutputPuzzleSolved(GridSize.Default, "Easy", time);
                OutputPuzzleSolution(solution.SolvedGrid);
            }, (time) => Console.WriteLine("Solution not found!!"));
            
            SolvePuzzle(GridSize.Default, Puzzle9x9.MediumPuzzle, (solution, time) => 
            {
                OutputPuzzleSolved(GridSize.Default, "Medium", time);
                OutputPuzzleSolution(solution.SolvedGrid);
            }, (time) => Console.WriteLine("Solution not found!!"));
            
            SolvePuzzle(GridSize.Default, Puzzle9x9.HardPuzzle, (solution, time) => 
            {
                OutputPuzzleSolved(GridSize.Default, "Hard", time);
                OutputPuzzleSolution(solution.SolvedGrid);
            }, (time) => Console.WriteLine("Solution not found!!"));
        }
        
        private static void OutputPuzzleSolved(GridSize gridSize, string label, long time)
        {
            Console.WriteLine("{0}x{0} {1} puzzle solved in {2}ms!!", gridSize.Size, label, time);
        }
        
        private static void OutputPuzzleSolution(Grid grid)
        {
            Console.WriteLine("Solution={0}", grid);
        }
        
        private static void SolvePuzzle(
            GridSize gridSize, int[] puzzle, 
            Action<GridSolution, long> onSolutionFound, Action<long> onSolutionNotFound)
        {
            var sw = Stopwatch.StartNew();
            
            var grid = Grid.Load(gridSize, puzzle);
            var solver = new GridSolver();
            var solution = solver.Solve(grid);
            
            sw.Stop();
            
            if (solution == null)
            {
                onSolutionNotFound(sw.ElapsedMilliseconds);
            }
            else
            {
                onSolutionFound(solution, sw.ElapsedMilliseconds);
            }
        }
    }
}
