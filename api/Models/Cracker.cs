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

        public Cracker(List<ICell> grid)
        {
            this._grid = grid.Select(x => new Cell(x.Row, x.Column, x.Value)).ToList();
        }

        public List<ICell> Solve()
        {
            // check for invalid values and missing cells
            PrepareCells();

            // find all the values each cell can possibly be
            DeterminePossibles();

            return null;
        }

        private void DeterminePossibles()
        {
            foreach (var cell in this._grid)
            {
                if (cell.Value.HasValue)
                {
                    cell.PossibleValues.Add(cell.Value.Value);
                    continue;
                }


            }
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

            if (row <= 2 && column <= 8)
            {
                return 2;
            }
        }

    }
}