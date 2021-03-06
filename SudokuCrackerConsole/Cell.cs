﻿using SudokuCracker;

namespace SudokuCrackerConsole
{
    internal class Cell : ICell
    {
        public Cell(int row, int column, int? value = null)
        {
            Row = row;
            Column = column;
            Value = value;
        }

        public int Column { get; set; }

        public int Row { get; set; }

        public int? Value { get; set; }
    }
}