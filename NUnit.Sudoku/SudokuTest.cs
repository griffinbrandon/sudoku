using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Sudoku_Cracker;

namespace NUnit.Sudoku
{
    [TestFixture]
    public class SudokuTest
    {
        private List<ICell> GetEasyGrid()
        {
            var grid = new List<ICell>();

            // row 1
            grid.Add(new Cell(0, 2, 3));
            grid.Add(new Cell(0, 5, 1));
            grid.Add(new Cell(0, 6, 9));

            // row 2
            grid.Add(new Cell(1, 0, 8));
            grid.Add(new Cell(1, 3, 7));
            grid.Add(new Cell(1, 5, 6));
            grid.Add(new Cell(1, 6, 1));

            // row 3
            grid.Add(new Cell(2, 3, 3));
            grid.Add(new Cell(2, 4, 5));
            grid.Add(new Cell(2, 7, 7));

            // row 4
            grid.Add(new Cell(3, 0, 3));
            grid.Add(new Cell(3, 6, 2));
            grid.Add(new Cell(3, 7, 4));

            // row 5
            grid.Add(new Cell(4, 0, 9));
            grid.Add(new Cell(4, 1, 7));
            grid.Add(new Cell(4, 7, 3));
            grid.Add(new Cell(4, 8, 6));

            // row 6
            grid.Add(new Cell(5, 1, 1));
            grid.Add(new Cell(5, 2, 4));
            grid.Add(new Cell(5, 8, 9));

            // row 7
            grid.Add(new Cell(6, 1, 8));
            grid.Add(new Cell(6, 4, 2));
            grid.Add(new Cell(6, 5, 4));

            // row 8
            grid.Add(new Cell(7, 2, 6));
            grid.Add(new Cell(7, 3, 1));
            grid.Add(new Cell(7, 5, 7));
            grid.Add(new Cell(7, 8, 8));

            // row 9
            grid.Add(new Cell(8, 2, 9));
            grid.Add(new Cell(8, 3, 8));
            grid.Add(new Cell(8, 6, 6));

            return grid;
        }

        private List<ICell> GetEasyGridSolution()
        {
            var grid = new List<ICell>();

            // row 1
            grid.Add(new Cell(0, 0, 7));
            grid.Add(new Cell(0, 1, 5));
            grid.Add(new Cell(0, 2, 3));
            grid.Add(new Cell(0, 3, 2));
            grid.Add(new Cell(0, 4, 8));
            grid.Add(new Cell(0, 5, 1));
            grid.Add(new Cell(0, 6, 9));
            grid.Add(new Cell(0, 7, 6));
            grid.Add(new Cell(0, 8, 4));

            // row 2
            grid.Add(new Cell(1, 0, 8));
            grid.Add(new Cell(1, 1, 9));
            grid.Add(new Cell(1, 2, 2));
            grid.Add(new Cell(1, 3, 7));
            grid.Add(new Cell(1, 4, 4));
            grid.Add(new Cell(1, 5, 6));
            grid.Add(new Cell(1, 6, 1));
            grid.Add(new Cell(1, 7, 5));
            grid.Add(new Cell(1, 8, 3));

            // row 3
            grid.Add(new Cell(2, 0, 6));
            grid.Add(new Cell(2, 1, 4));
            grid.Add(new Cell(2, 2, 1));
            grid.Add(new Cell(2, 3, 3));
            grid.Add(new Cell(2, 4, 5));
            grid.Add(new Cell(2, 5, 9));
            grid.Add(new Cell(2, 6, 8));
            grid.Add(new Cell(2, 7, 7));
            grid.Add(new Cell(2, 8, 2));

            // row 4
            grid.Add(new Cell(3, 0, 3));
            grid.Add(new Cell(3, 1, 6));
            grid.Add(new Cell(3, 2, 5));
            grid.Add(new Cell(3, 3, 9));
            grid.Add(new Cell(3, 4, 7));
            grid.Add(new Cell(3, 5, 8));
            grid.Add(new Cell(3, 6, 2));
            grid.Add(new Cell(3, 7, 4));
            grid.Add(new Cell(3, 8, 1));

            // row 5
            grid.Add(new Cell(4, 0, 9));
            grid.Add(new Cell(4, 1, 7));
            grid.Add(new Cell(4, 2, 8));
            grid.Add(new Cell(4, 3, 4));
            grid.Add(new Cell(4, 4, 1));
            grid.Add(new Cell(4, 5, 2));
            grid.Add(new Cell(4, 6, 5));
            grid.Add(new Cell(4, 7, 3));
            grid.Add(new Cell(4, 8, 6));

            // row 6
            grid.Add(new Cell(5, 0, 2));
            grid.Add(new Cell(5, 1, 1));
            grid.Add(new Cell(5, 2, 4));
            grid.Add(new Cell(5, 3, 5));
            grid.Add(new Cell(5, 4, 6));
            grid.Add(new Cell(5, 5, 3));
            grid.Add(new Cell(5, 6, 7));
            grid.Add(new Cell(5, 7, 8));
            grid.Add(new Cell(5, 8, 9));

            // row 7
            grid.Add(new Cell(6, 0, 1));
            grid.Add(new Cell(6, 1, 8));
            grid.Add(new Cell(6, 2, 7));
            grid.Add(new Cell(6, 3, 6));
            grid.Add(new Cell(6, 4, 2));
            grid.Add(new Cell(6, 5, 4));
            grid.Add(new Cell(6, 6, 3));
            grid.Add(new Cell(6, 7, 9));
            grid.Add(new Cell(6, 8, 5));

            // row 8
            grid.Add(new Cell(7, 0, 5));
            grid.Add(new Cell(7, 1, 3));
            grid.Add(new Cell(7, 2, 6));
            grid.Add(new Cell(7, 3, 1));
            grid.Add(new Cell(7, 4, 9));
            grid.Add(new Cell(7, 5, 7));
            grid.Add(new Cell(7, 6, 4));
            grid.Add(new Cell(7, 7, 2));
            grid.Add(new Cell(7, 8, 8));

            // row 9
            grid.Add(new Cell(8, 0, 4));
            grid.Add(new Cell(8, 1, 2));
            grid.Add(new Cell(8, 2, 9));
            grid.Add(new Cell(8, 3, 8));
            grid.Add(new Cell(8, 4, 3));
            grid.Add(new Cell(8, 5, 5));
            grid.Add(new Cell(8, 6, 6));
            grid.Add(new Cell(8, 7, 1));
            grid.Add(new Cell(8, 8, 7));

            return grid;
        }

        private List<ICell> GetHardGrid()
        {
            var grid = new List<ICell>();

            // row 1
            grid.Add(new Cell(0, 4, 1));
            grid.Add(new Cell(0, 8, 9));

            // row 2
            grid.Add(new Cell(1, 6, 6));
            grid.Add(new Cell(1, 7, 5));

            // row 3
            grid.Add(new Cell(2, 2, 7));
            grid.Add(new Cell(2, 3, 6));
            grid.Add(new Cell(2, 6, 1));
            grid.Add(new Cell(2, 7, 3));

            // row 4
            grid.Add(new Cell(3, 0, 9));
            grid.Add(new Cell(3, 3, 1));
            grid.Add(new Cell(3, 5, 2));
            grid.Add(new Cell(3, 6, 3));
            grid.Add(new Cell(3, 8, 6));

            // row 5
            grid.Add(new Cell(4, 3, 7));
            grid.Add(new Cell(4, 5, 5));

            // row 6
            grid.Add(new Cell(5, 0, 8));
            grid.Add(new Cell(5, 2, 1));
            grid.Add(new Cell(5, 3, 9));
            grid.Add(new Cell(5, 5, 3));
            grid.Add(new Cell(5, 8, 2));

            // row 7
            grid.Add(new Cell(6, 1, 3));
            grid.Add(new Cell(6, 2, 8));
            grid.Add(new Cell(6, 5, 4));
            grid.Add(new Cell(6, 6, 9));

            // row 8
            grid.Add(new Cell(7, 1, 5));
            grid.Add(new Cell(7, 2, 9));

            // row 9
            grid.Add(new Cell(8, 0, 4));
            grid.Add(new Cell(8, 4, 9));

            return grid;
        }

        private List<ICell> GetHardGridSolution()
        {
            var grid = new List<ICell>();

            // row 1
            grid.Add(new Cell(0, 0, 5));
            grid.Add(new Cell(0, 1, 8));
            grid.Add(new Cell(0, 2, 6));
            grid.Add(new Cell(0, 3, 3));
            grid.Add(new Cell(0, 4, 1));
            grid.Add(new Cell(0, 5, 7));
            grid.Add(new Cell(0, 6, 4));
            grid.Add(new Cell(0, 7, 2));
            grid.Add(new Cell(0, 8, 9));

            // row 2
            grid.Add(new Cell(1, 0, 1));
            grid.Add(new Cell(1, 1, 9));
            grid.Add(new Cell(1, 2, 3));
            grid.Add(new Cell(1, 3, 4));
            grid.Add(new Cell(1, 4, 2));
            grid.Add(new Cell(1, 5, 8));
            grid.Add(new Cell(1, 6, 6));
            grid.Add(new Cell(1, 7, 5));
            grid.Add(new Cell(1, 8, 7));

            // row 3
            grid.Add(new Cell(2, 0, 2));
            grid.Add(new Cell(2, 1, 4));
            grid.Add(new Cell(2, 2, 7));
            grid.Add(new Cell(2, 3, 6));
            grid.Add(new Cell(2, 4, 5));
            grid.Add(new Cell(2, 5, 9));
            grid.Add(new Cell(2, 6, 1));
            grid.Add(new Cell(2, 7, 3));
            grid.Add(new Cell(2, 8, 8));

            // row 4
            grid.Add(new Cell(3, 0, 9));
            grid.Add(new Cell(3, 1, 7));
            grid.Add(new Cell(3, 2, 5));
            grid.Add(new Cell(3, 3, 1));
            grid.Add(new Cell(3, 4, 8));
            grid.Add(new Cell(3, 5, 2));
            grid.Add(new Cell(3, 6, 3));
            grid.Add(new Cell(3, 7, 4));
            grid.Add(new Cell(3, 8, 6));

            // row 5
            grid.Add(new Cell(4, 0, 3));
            grid.Add(new Cell(4, 1, 2));
            grid.Add(new Cell(4, 2, 4));
            grid.Add(new Cell(4, 3, 7));
            grid.Add(new Cell(4, 4, 6));
            grid.Add(new Cell(4, 5, 5));
            grid.Add(new Cell(4, 6, 8));
            grid.Add(new Cell(4, 7, 9));
            grid.Add(new Cell(4, 8, 1));

            // row 6
            grid.Add(new Cell(5, 0, 8));
            grid.Add(new Cell(5, 1, 6));
            grid.Add(new Cell(5, 2, 1));
            grid.Add(new Cell(5, 3, 9));
            grid.Add(new Cell(5, 4, 4));
            grid.Add(new Cell(5, 5, 3));
            grid.Add(new Cell(5, 6, 5));
            grid.Add(new Cell(5, 7, 7));
            grid.Add(new Cell(5, 8, 2));

            // row 7
            grid.Add(new Cell(6, 0, 6));
            grid.Add(new Cell(6, 1, 3));
            grid.Add(new Cell(6, 2, 8));
            grid.Add(new Cell(6, 3, 2));
            grid.Add(new Cell(6, 4, 7));
            grid.Add(new Cell(6, 5, 4));
            grid.Add(new Cell(6, 6, 9));
            grid.Add(new Cell(6, 7, 1));
            grid.Add(new Cell(6, 8, 5));

            // row 8
            grid.Add(new Cell(7, 0, 7));
            grid.Add(new Cell(7, 1, 5));
            grid.Add(new Cell(7, 2, 9));
            grid.Add(new Cell(7, 3, 8));
            grid.Add(new Cell(7, 4, 3));
            grid.Add(new Cell(7, 5, 1));
            grid.Add(new Cell(7, 6, 2));
            grid.Add(new Cell(7, 7, 6));
            grid.Add(new Cell(7, 8, 4));

            // row 9
            grid.Add(new Cell(8, 0, 4));
            grid.Add(new Cell(8, 1, 1));
            grid.Add(new Cell(8, 2, 2));
            grid.Add(new Cell(8, 3, 5));
            grid.Add(new Cell(8, 4, 9));
            grid.Add(new Cell(8, 5, 6));
            grid.Add(new Cell(8, 6, 7));
            grid.Add(new Cell(8, 7, 8));
            grid.Add(new Cell(8, 8, 3));

            return grid;
        }

        private bool Evaluate(List<ICell> solution, List<ICell> guess)
        {
            foreach (var guessCell in guess)
            {
                var solutionCell = solution.Single(x => x.Row == guessCell.Row && x.Column == guessCell.Column);
                if (solutionCell.Value != guessCell.Value)
                {
                    return false;
                }
            }

            return true;
        }

        [Test]
        public void EasySudoku()
        {
            var easyGrid = GetEasyGrid();

            var cracker = new Cracker(easyGrid);
            var guess = cracker.Solve();

            Assert.IsTrue(Evaluate(GetEasyGridSolution(), guess));
        }

        [Test]
        public void HardSudoku()
        {
            var hardGrid = GetHardGrid();

            var cracker = new Cracker(hardGrid);
            var guess = cracker.Solve();

            Assert.IsTrue(Evaluate(GetHardGridSolution(), guess));
        }
    }
}