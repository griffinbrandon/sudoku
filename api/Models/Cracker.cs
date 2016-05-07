using refs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.Models
{
    public class Cracker
    {
        private List<Cell> _grid;
        private readonly int[] _range = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        private Dictionary<int, List<List<int>>> _rowPossibilities = new Dictionary<int, List<List<Cell>>>();

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

                var possibles = new List<string>();

                // add the possible values from the first cell
                foreach (var value in row.First().PossibleValues)
                {
                    possibles.Add(value.ToString());
                }

                // loop through all cells after the first one
                foreach (var cell in row.Skip(1))
                {
                    var tmp = new List<string>();

                    // loop through possible values for the cell
                    foreach (var value in cell.PossibleValues)
                    {
                        // append each of these cells possible values onto the already possible values
                        foreach (var possible in possibles)
                        {
                            tmp.Add($"{possible},{value}");
                        }

                        // set row possibles to the possibles with this cell
                        possibles = tmp;
                    }
                }

                var rowPossibles = new List<List<int>>();
                // loop through all row possibles
                foreach (var p in possibles)
                {
                    var arr = p.Split(',').Select(x => int.Parse(x)).ToList();

                    // duplicate numbers in this make this no longer a possibility
                    if (arr.Distinct().Count() != 9)
                    {
                        continue;
                    }

                    // add distinct possibles
                    rowPossibles.Add(arr);
                }

                // add all of the possibles using the row as the key
                _rowPossibilities.Add(i, rowPossibles);
            }
        }

        private void DeterminePossiblesPerCell()
        {
            foreach (var cell in this._grid)
            {
                if (cell.Value.HasValue)
                {
                    cell.PossibleValues.Add(cell.Value.Value);
                    continue;
                }

                // get values from cells in same row
                var inRow = this._grid.Where(x => x.Row == cell.Row && x.Id != cell.Id).Select(x => x.Value);

                // get values from cells in same column
                var inCol = this._grid.Where(x => x.Column == cell.Column && x.Id != cell.Id).Select(x => x.Value);

                // get values from cells in same box
                var inBox = this._grid.Where(x => x.Box == cell.Box && x.Id != cell.Id).Select(x => x.Value);

                // get a distinct list of numbers that are already used
                var reservedValues = (inRow.Union(inCol).Union(inBox)).Distinct();

                // using the distinct list, get values this cell could be
                var possibles = _range.Where(x => !reservedValues.Contains(x)).ToList();
                cell.PossibleValues = possibles;
            }
        }

        private List<ICell> FindSolution()
        {
            // loop through row one
            foreach (var row1Possible in _rowPossibilities[0])
            {
                var row1 = GetRow(row1Possible, 0);

                // row two
                foreach (var row2Possible in _rowPossibilities[1])
                {
                    var row2 = GetRow(row2Possible, 1);

                    // row three
                    foreach (var row3Possible in _rowPossibilities[2])
                    {
                        var row3 = GetRow(row3Possible, 2);

                        // row four
                        foreach (var row4Possible in _rowPossibilities[3])
                        {
                            var row4 = GetRow(row4Possible, 3);

                            // row five
                            foreach (var row5Possible in _rowPossibilities[4])
                            {
                                var row5 = GetRow(row5Possible, 4);

                                // row six
                                foreach (var row6Possible in _rowPossibilities[5])
                                {
                                    var row6 = GetRow(row6Possible, 5);

                                    // row seven
                                    foreach (var row7Possible in _rowPossibilities[6])
                                    {
                                        var row7 = GetRow(row7Possible, 6);

                                        // row eight
                                        foreach (var row8Possible in _rowPossibilities[7])
                                        {
                                            var row8 = GetRow(row8Possible, 7);

                                            // row nine
                                            foreach (var row9Possible in _rowPossibilities[8])
                                            {
                                                var row9 = GetRow(row9Possible, 8);

                                                var grid = row1.Union(row2).Union(row3).Union(row4).Union(row5).Union(row6).Union(row7).Union(row8).Union(row9);

                                                if (ValidatePossible(grid.ToList()))
                                                {
                                                    return grid.Select(x => (ICell)x).ToList();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }

        private IEnumerable<Cell> GetRow(List<int> rowPossibles, int rowInx)
        {
            return rowPossibles.Select(x => new Cell
            {
                Value = x,
                Row = rowInx,
                Column = rowPossibles.IndexOf(x),
                Box = GetBox(rowInx, rowPossibles.IndexOf(x))
            });
        }

        private bool ValidatePossible(List<Cell> grid)
        {
            // check the rows
            for (var i = 0; i < 9; i++)
            {
                var row = grid.Where(x => x.Row == i);

                // if all the values are distinct, the row is good
                if (row.Select(x => x.Value).Distinct().Count() != 9) return false;

            }

            // check the columns
            for (var i = 0; i < 9; i++)
            {
                var column = grid.Where(x => x.Column == i);

                // if all the values are distinct, the column is good
                if (column.Select(x => x.Value).Distinct().Count() != 9) return false;

            }

            // check the boxes
            for (var i = 0; i < 9; i++)
            {
                var box = grid.Where(x => x.Box == i);

                // if all the values are distinct, the column is good
                if (box.Select(x => x.Value).Distinct().Count() != 9) return false;

            }

            // this is the answer
            return true;

        }

        private void PrepareCells()
        {
            for (var r = 0; r < 9; r++)
            {
                var rows = this._grid.Where(x => x.Row == 0);

                for (var c = 0; c < 9; c++)
                {
                    var cell = rows.SingleOrDefault(x => x.Column == c);

                    if (cell == null)
                    {
                        // add the cell to the collection
                        this._grid.Add(new Cell(r, c));
                    }

                    if (cell.Value.HasValue)
                    {
                        if (cell.Value >= 1 && cell.Value <= 9)
                        {
                            // set this to readonly because the value came from the user
                            cell.ReadOnly = true;
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException($"{cell.Value} is not a recognized value. Only values 1 - 9 are allowed");
                        }
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