using refs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.Models
{
    public class Cell : ICell
    {
        public Cell() { }
        public Cell(int row, int column)
        {
            this.Column = column;
            this.Row = row;
        }
        internal Cell(int row, int column, int? value) : this(row, column)
        {
            this.Value = value;
        }

        public int Box { get; set; }

        public int Column { get; set; }

        public bool ReadOnly { get; set; }

        public int Row { get; set; }
        
        public int? Value { get; set; }

        public List<int> PossibleValues { get; set; } = new List<int>();

        internal Guid Id { get; private set; } = Guid.NewGuid();

    }
}