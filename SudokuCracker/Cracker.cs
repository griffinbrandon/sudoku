﻿using System;
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
            _grid = new List<Cell>(grid.Count);
            foreach (var cell in grid)
            {
                _grid.Add(new Cell(cell.Row, cell.Column, cell.Value));
            }
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
                // order the row by the column
                var rowCells = GetCellsForRow(r).OrderBy(x => x.Column).ToList();

                // create array of int for all possible values of the first cell in this row
                var firstCell = rowCells.First();
                var possibles = firstCell.PossibleValues.Select(x => new [] {x,0,0,0,0,0,0,0,0}).ToList();

                // loop through all cells after the first one and create all of the possible rows 
                for (var c = 1; c < 9; c++)
                {
                    var cell = rowCells[c];

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

                var usedValues = new List<int>();                

                // get values from cells in same row
                var valuesFromRow = GetUsedValuesForRow(cell.Row, cell.Column);
                AddPossibleValuesFromList(usedValues, valuesFromRow);

                // get values from cells in same column
                var valuesFromColumn = GetUsedValuesForColumn(cell.Column, cell.Row);
                AddPossibleValuesFromList(usedValues, valuesFromColumn);

                // get values from cells in same box              
                var valuesFromBox = GetUsedValuesForBox(cell.Box, cell.Row, cell.Column);
                AddPossibleValuesFromList(usedValues, valuesFromBox);

                // using list of values already taken, determine possible values for this cell
                var possibles = new List<int>();
                foreach (var value in _range)
                {
                    if (!usedValues.Contains(value))
                    {
                        possibles.Add(value);
                    }  
                }
                
                cell.PossibleValues = possibles;
            }
        }

        private void AddPossibleValuesFromList(List<int> allUsedValues, List<int> cellValuesFromOrdinal)
        {
            foreach (var value in cellValuesFromOrdinal)
            {
                if (!allUsedValues.Contains(value))
                {
                    allUsedValues.Add(value);   
                }                
            }
        }

        private List<Cell> GetCellsForRow(int rowInx)
        {
            var row = new List<Cell>();

            foreach (var cell in _grid)
            {
                if (cell.Row == rowInx)
                {
                    row.Add(cell);
                }    
            }

            return row;
        } 

        private List<int> GetUsedValuesForRow(int row, int excludeColumn)
        {
            var usedValues = new List<int>();
            var rowCells = GetCellsForRow(row);
             
            foreach (var cell in rowCells)
            {
                if (cell.Value.HasValue && cell.Value > 0 && cell.Column != excludeColumn)
                {
                    usedValues.Add(cell.Value.Value);
                }
            }

            return usedValues;
        }

        private List<int> GetUsedValuesForColumn(int column, int excludeRow)
        {
            var usedValues = new List<int>();

            foreach (var cell in _grid)
            {
                if (cell.Value.HasValue && cell.Value > 0 && cell.Column == column && cell.Row != excludeRow)
                {
                    usedValues.Add(cell.Value.Value);
                }
            }

            return usedValues;
        }

        private List<int> GetUsedValuesForBox(int box, int excludeRow, int excludeColumn)
        {
            var usedValues = new List<int>();

            foreach (var cell in _grid)
            {
                if (cell.Value.HasValue && cell.Value > 0 && cell.Box == box && cell.Row != excludeRow && cell.Column != excludeColumn)
                {
                    usedValues.Add(cell.Value.Value);
                }
            }

            return usedValues;
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
            var solution = new List<ICell>();

            for (var i = 0; i < 9; i++)
            {
                solution.AddRange(GetRow(answer[i], i));
            }

            return solution;
        }

        private IEnumerable<Cell> GetRow(int[] rowPossibles, int rowInx)
        {
            var row = new List<Cell>();

            for (var c = 0; c < 9; c++)
            {
                row.Add(new Cell(rowInx, c, rowPossibles[c])
                {
                    Box = GetBox(rowInx, c)
                });
            }

            return row;
        }

        private bool ValidateColumns(List<int[]> grid)
        {
            // check each column in the row
            for (var c = 0; c < 9; c++)
            {
                var columns = new List<int>();
                foreach (var r in grid)
                {
                    if (columns.Contains(r[c]))
                    {
                        return false;
                    }
                    else
                    {
                        columns.Add(r[c]);
                    }
                }
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
                    if (boxes[box].Contains(grid[r][c]))
                    {
                        return false;
                    }
                    else
                    {
                        boxes[box].Add(grid[r][c]);
                    }
                }
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