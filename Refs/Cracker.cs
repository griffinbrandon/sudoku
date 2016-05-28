using System;
using System.Collections.Generic;
using System.Linq;

namespace Refs
{
    public class Cracker
    {
        private readonly List<Cell> _grid;
        private readonly int[] _range = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        private readonly Dictionary<int, List<string>> _rowPossibilities = new Dictionary<int, List<string>>();

        public Cracker(List<ICell> grid)
        {
            this._grid = grid.Select(x => new Cell(x.Row, x.Column, x.Value)).ToList();
        }

        public List<ICell> Solve()
        {
            // check for invalid values and missing cells
            PrepareCells();

            // find all the values each cell can possibly be
            DeterminePossiblesPerCell();

            // find all possibilities for each row
            DetermineRowPossibilities();            

            var solution = FindSolution();

            return solution;
        }

        private void DetermineRowPossibilities()
        {
            // loop through each row
            for (var i = 0; i < 9; i++)
            {
                var row = this._grid.Where(x => x.Row == i).OrderBy(x => x.Column);

                // add the possible values from the first cell
                var possibles = row.First().PossibleValues.Select(x => x.ToString()).ToList();                               

                // loop through all cells after the first one
                possibles = row.Skip(1)
                    .Aggregate(possibles, (current, cell) => cell.PossibleValues.SelectMany(x => current.Select(possible => $"{possible},{x}")).ToList());

                // clean out any rows that have duplicates
                var rowPossibles = possibles.Where(p => p.Split(',').Distinct().Count() == 9).ToList();

                _rowPossibilities.Add(i, rowPossibles);
            }
        }

        private void DeterminePossiblesPerCell()
        {
            foreach (var cell in this._grid)
            {
                if (cell.Value.HasValue)
                {
                    cell.PossibleValues = new List<int> { cell.Value.Value };
                    continue;
                }

                // get values from cells in same row
                var inRow = this._grid.Where(x => x.Row == cell.Row && x.Column != cell.Column).Select(x => x.Value);

                // get values from cells in same column
                var inCol = this._grid.Where(x => x.Column == cell.Column && x.Row != cell.Row).Select(x => x.Value);

                // get values from cells in same box
                var inBox = this._grid.Where(x => x.Box == cell.Box && x.Row != cell.Row && x.Column != cell.Column).Select(x => x.Value);

                // get a distinct list of numbers that are already used
                var reservedValues = (inRow.Union(inCol).Union(inBox)).Distinct();

                // using the distinct list, get values this cell could be
                var possibles = _range.Where(x => !reservedValues.Contains(x)).ToList();
                cell.PossibleValues = possibles;
            }
        }

        private List<string[]> CheckGrid(List<string[]> grid, int nextRowInx)
        {
            var rowCount = _rowPossibilities[nextRowInx].Count;

            for (var p = 0; p < rowCount; p++)
            {
                var rowPossible = _rowPossibilities[nextRowInx][p];

                // add new row to grid passed in
                var newGrid = grid.ToList();
                newGrid.Add(rowPossible.Split(','));

                // check if columns of grid contain distinct values
                var isGood = ValidateColumns(newGrid);

                if (isGood && nextRowInx < 8)
                {
                    // grid is good so far, add the next row and see what happens
                    var grd = CheckGrid(newGrid, nextRowInx + 1);
                    if (grd != null)
                    {
                        return grd;
                    }
                }
                else if (isGood && nextRowInx == 8)
                {
                    // make sure the boxes are correct
                    if (ValidateBoxes(newGrid))
                    {
                        // the answer
                        return newGrid;
                    }
                }
            }

            return null;
        }

        private List<ICell> FindSolution()
        {
            var row1Count = _rowPossibilities[0].Count;

            var answer = CheckGrid(new List<string[]>(), 0);

            if (answer != null)
                return BuildGrid(answer);

            return null;
        }

        private List<ICell> BuildGrid(List<string[]> answer)
        {
            return (GetRow(answer[0], 0)
                .Union(GetRow(answer[1], 1))
                .Union(GetRow(answer[2], 2))
                .Union(GetRow(answer[3], 3))
                .Union(GetRow(answer[4], 4))
                .Union(GetRow(answer[5], 5))
                .Union(GetRow(answer[6], 6))
                .Union(GetRow(answer[7], 7))
                .Union(GetRow(answer[8], 8)))
                .Select(x => (ICell)x).ToList();
        }

        private IEnumerable<Cell> GetRow(string[] rowPossibles, int rowInx)
        {
            return rowPossibles.Select(x => new Cell
            {
                Value = int.Parse(x),
                Row = rowInx,
                Column = Array.IndexOf(rowPossibles, x),
                Box = GetBox(rowInx, Array.IndexOf(rowPossibles, x))
            });
        }

        private bool ValidateColumns(List<string[]> grid)
        {
            var distinct = grid.Count;

            // check the columns
            for (var c = 0; c < 9; c++)
            {
                var column = grid.SelectMany(x => x[c]);

                // if all the values are distinct, the column is good
                if (column.Distinct().Count() != distinct) return false;

            }

            return true;
        }

        private bool ValidateBoxes(List<string[]> grid)
        {
            // figure out boxes
            var boxes = new string[] { "", "", "", "", "", "", "", "", "" };
            for (var r = 0; r < 9; r++)
            {
                for (var c = 0; c < 9; c++)
                {
                    var box = GetBox(r, c);
                    
                    boxes[box] += (string.IsNullOrEmpty(boxes[box]) ? grid[r][c] : $",{grid[r][c]}");
                }
            }

            // check the boxes
            for (var b = 0; b < 9; b++)
            {
                // if all the values are distinct, the column is good
                if (boxes[b].Split(',').Distinct().Count() != 9) return false;

            }

            // this is the answer
            return true;

        }

        private void PrepareCells()
        {
            for (var r = 0; r < 9; r++)
            {
                var rows = this._grid.Where(x => x.Row == r);

                for (var c = 0; c < 9; c++)
                {
                    var cell = rows.SingleOrDefault(x => x.Column == c);

                    if (cell == null)
                    {
                        cell = new Cell(r, c);

                        // add the cell to the collection
                        this._grid.Add(cell);
                    }

                    if (cell.Row < 0 || cell.Row > 8 || cell.Column < 0 || cell.Column > 8)
                    {
                        throw new ArgumentOutOfRangeException($"coordinates {cell.Row}, {cell.Column} are invalid");
                    }

                    cell.Box = GetBox(r, c);
                }
            }

        }

        private int GetBox(int row, int column)
        {
            if (row <= 2 && column <= 2)
            {
                return 0;
            }

            if (row <= 2 && column <= 5)
            {
                return 1;
            }

            // no need to check the column on this, it would just be in the last box
            if (row <= 2)
            {
                return 2;
            }

            if (row <= 5 && column <= 2)
            {
                return 3;
            }

            if (row <= 5 && column <= 5)
            {
                return 4;
            }

            // no need to check the column on this, it would just be in the last box
            if (row <= 5)
            {
                return 5;
            }

            // no longer need to check the row
            if (column <= 2)
            {
                return 6;
            }

            if (column <= 5)
            {
                return 7;
            }

            // last box
            return 8;
        }

    }
}