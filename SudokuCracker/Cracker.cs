using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuCracker
{
    public class Cracker
    {
        private readonly List<Cell> _grid;
        private readonly int[] _range = {1, 2, 3, 4, 5, 6, 7, 8, 9};
        private readonly Dictionary<int, List<int[]>> _rowPossibilities = new Dictionary<int, List<int[]>>();

        public Cracker(List<ICell> grid)
        {
            _grid = grid.Select(x => new Cell(x.Row, x.Column, x.Value)).ToList();
        }

        public List<ICell> Solve()
        {
            // check for good indexes
            ValidateGrid();
            
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
            for (var r = 0; r < 9; r++)
            {
                var row = _grid.Where(x => x.Row == r).OrderBy(x => x.Column).ToList();

                // add the possible values from the first cell
                var possibles = row.First().PossibleValues.Select(x => new [] {x,0,0,0,0,0,0,0,0}).ToList();

                // loop through all cells after the first one and create all of the possible rows 
                for (var c = 1; c < 9; c++)
                {
                    var cell = row[c];

                    var tmp = new List<int[]>();

                    foreach (var cellPossible in cell.PossibleValues)
                    {
                        foreach (var possible in possibles)
                        {
                            // only keep row if the possibility is good
                            if (Array.IndexOf(possible, cellPossible) == -1)
                            {
                                var p = possible.ToArray();
                                p[c] = cellPossible;
                                tmp.Add(p);
                            }
                        }
                    }

                    possibles = tmp.ToList();
                }

                _rowPossibilities.Add(r, possibles);
            }
        }

        private void DeterminePossiblesPerCell()
        {
            foreach (var cell in _grid)
            {
                if (cell.Value.HasValue)
                {
                    cell.PossibleValues = new List<int> {cell.Value.Value};
                    continue;
                }

                // get values from cells in same row
                var inRow = _grid.Where(x => x.Row == cell.Row && x.Column != cell.Column).Select(x => x.Value);

                // get values from cells in same column
                var inCol = _grid.Where(x => x.Column == cell.Column && x.Row != cell.Row).Select(x => x.Value);

                // get values from cells in same box
                var inBox =
                    _grid.Where(x => x.Box == cell.Box && x.Row != cell.Row && x.Column != cell.Column)
                        .Select(x => x.Value);

                // get a distinct list of numbers that are already used
                var reservedValues = (inRow.Union(inCol).Union(inBox)).Distinct();

                // using the distinct list, get values this cell could be
                var possibles = _range.Where(x => !reservedValues.Contains(x)).ToList();
                cell.PossibleValues = possibles;
            }
        }

        private List<int[]> CheckGrid(List<int[]> grid, int nextRowInx)
        {
            var rowCount = _rowPossibilities[nextRowInx].Count;

            for (var p = 0; p < rowCount; p++)
            {
                var rowPossible = _rowPossibilities[nextRowInx][p];

                // add new row to grid passed in
                var newGrid = grid.ToList();
                newGrid.Add(rowPossible);

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
            var answer = CheckGrid(new List<int[]>(), 0);

            if (answer != null)
                return BuildGrid(answer);

            return null;
        }

        private List<ICell> BuildGrid(List<int[]> answer)
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
                .Select(x => (ICell) x).ToList();
        }

        private IEnumerable<Cell> GetRow(int[] rowPossibles, int rowInx)
        {
            return rowPossibles.Select(x => new Cell
            {
                Value = int.Parse(x.ToString()),
                Row = rowInx,
                Column = Array.IndexOf(rowPossibles, x),
                Box = GetBox(rowInx, Array.IndexOf(rowPossibles, x))
            });
        }

        private bool ValidateColumns(List<int[]> grid)
        {
            var distinct = grid.Count;

            // check the columns
            for (var c = 0; c < 9; c++)
            {
                var columns = grid.Select(r => r[c]).ToList();

                // if all the values are distinct, the column is good
                if (columns.Distinct().Count() != distinct) return false;
            }

            return true;
        }

        private bool ValidateBoxes(List<int[]> grid)
        {
            // figure out boxes
            var boxes = new List<List<int>>
            {
                new List<int>(),
                new List<int>(),
                new List<int>(),
                new List<int>(),
                new List<int>(),
                new List<int>(),
                new List<int>(),
                new List<int>(),
                new List<int>()
            };
            for (var r = 0; r < 9; r++)
            {
                for (var c = 0; c < 9; c++)
                {
                    var box = GetBox(r, c);
                    boxes[box].Add(grid[r][c]);
                }
            }

            // check the boxes
            for (var b = 0; b < 9; b++)
            {
                // if all the values are distinct, the column is good
                if (boxes[b].Distinct().Count() != 9) return false;
            }

            // this is the answer
            return true;
        }

        private void PrepareCells()
        {
            for (var r = 0; r < 9; r++)
            {
                var rows = _grid.Where(x => x.Row == r).ToList();

                for (var c = 0; c < 9; c++)
                {
                    var cell = rows.SingleOrDefault(x => x.Column == c);

                    if (cell == null)
                    {
                        cell = new Cell(r, c);

                        // add the cell to the collection
                        _grid.Add(cell);
                    }

                    // make sure the value passed in is good
                    if (cell.Value.HasValue && Array.IndexOf(_range, cell.Value) == -1)
                    {
                        if (cell.Value == 0)
                        {
                            // account for the one invalid value
                            cell.Value = null;
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException("Value", "invalid values for cell detected, expected values are 1 - 9");
                        }
                    }

                    cell.Box = GetBox(r, c);
                }
            }
        }

        private void ValidateGrid()
        {
            if (_grid.Any(x => x.Row > 8 || x.Row < 0))
            {
                throw new ArgumentOutOfRangeException("Row","invalid values for row index detected, expected values are 0 - 8");
            }

            if (_grid.Any(x => x.Column > 8 || x.Column < 0))
            {
                throw new ArgumentOutOfRangeException("Column", "invalid values for column index detected, expected values are 0 - 8");
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