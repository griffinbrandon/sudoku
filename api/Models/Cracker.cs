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
        private Dictionary<int, List<List<int>>> _rowPossibilities = new Dictionary<int, List<List<int>>>();

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

            FindSolution();

            return null;
        }

        private void DetermineRowPossibilities()
        {
            for (var i = 0; i < 9; i++)
            {
                var row = this._grid.Where(x => x.Row == i).OrderBy(x => x.Column);

                var possibles = new List<string>();

                foreach (var value in row.First().PossibleValues)
                {
                    possibles.Add(value.ToString());
                }

                foreach (var cell in row.Skip(1))
                {
                    var tmp = new List<string>();

                    foreach (var value in cell.PossibleValues)
                    {
                        foreach (var possible in possibles)
                        {
                            tmp.Add($"{possible},{value}");
                        }

                        possibles = tmp;
                    }
                }

                var rowPossibles = new List<List<int>>();
                foreach (var p in possibles)
                {
                    var arr = p.Split(',').Select(x => int.Parse(x)).ToList();

                    // duplicate numbers in this make this no longer a possibility
                    if (arr.Distinct().Count() != 9)
                    {
                        continue;
                    }

                    rowPossibles.Add(arr);
                }

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
            return null;
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