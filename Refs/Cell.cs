using System.Collections.Generic;

namespace Refs
{
    internal class Cell : ICell
    {
        public Cell()
        {
        }

        public Cell(int row, int column)
        {
            Column = column;
            Row = row;
        }

        internal Cell(int row, int column, int? value) : this(row, column)
        {
            Value = value;
        }

        public int Box { get; set; }

        public bool ReadOnly { get; set; }

        public List<int> PossibleValues { get; set; }

        public int Column { get; set; }

        public int Row { get; set; }

        public int? Value { get; set; }
    }
}