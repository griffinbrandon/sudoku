using refs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit.Sudoku
{
    class Cell : ICell
    {
        public Cell(int row, int column, int? value = null)
        {
            this.Row = row;
            this.Column = column;
            this.Value = value;
        }

        public int Column { get; set; }           

        public int Row { get; set; }

        public int? Value { get; set; }

    }
}
