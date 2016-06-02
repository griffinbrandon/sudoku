namespace SudokuCrackerConsole
{
    using Sudoku_Cracker;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    class Program
    {
        static void Main(string[] args)
        {
            if (args != null)
            {
                var sudoku = File.ReadAllLines(args[0]).ToList();
                var grid = new List<ICell>();
                
                for (var row = 0; row < sudoku.Count; row++)
                {
                    var columns = sudoku[row].Split(',').ToList();

                    for (var column = 0; column < columns.Count; column++)
                    {
                        var value = Convert.ToInt32(columns[column]);

                        grid.Add(new Cell(row, column, value == 0 ? null : (int?)value));
                    }
                }

                Console.WriteLine("Input:");
                Render(grid);

                Console.WriteLine("\nSolution:");
                var cracker = new Cracker(grid.Where(x => x.Value.HasValue).ToList());
                var solution = cracker.Solve();
                Render(solution);
            }

            Console.ReadLine();
        }

        static void Render(List<ICell> grid)
        {
            if (grid != null)
            {
                for (var cell = 0; cell < grid.OrderBy(x => x.Row).ThenBy(x => x.Column).Count(); cell++)
                {
                    var value = grid[cell].Value == null ? " " : grid[cell].Value.ToString();

                    Console.Write($"{value} ");

                    if ((grid[cell].Column + 1) % 3 == 0 && (grid[cell].Column + 1) < 9)
                    {
                        Console.Write("| ");
                    }

                    if ((grid[cell].Column + 1) % 9 == 0)
                    {
                        if ((grid[cell].Row + 1) % 3 == 0 && (grid[cell].Row + 1) < 9)
                        {
                            Console.WriteLine("\n---------------------");
                        }
                        else
                        {
                            Console.Write("\n");
                        }
                    }
                }
            }
        }
    }
}
