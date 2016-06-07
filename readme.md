# Sudoku Cracker
SudokuCracker is a portable Xamarin library which can solve most 9x9 sudoku puzzles in under a millisecond (at least on my machine). 
It is also able to solve the worlds hardest sudoku puzzle (according to http://www.telegraph.co.uk/science/science-news/9359579/Worlds-hardest-sudoku-can-you-crack-it.html) in 1.5 seconds.

## Usage
Add a referenece to SudokuCracker, from your project. Initialize the Cracker object by passing in a list of ICell objects and call the Solve method. 
```
var cracker = new Cracker(grid);
var solution = cracker.Solve();
```
The indexes for the cells you pass in are zero-based and should look something like this:
```
var cell = new Cell(0, 0, 1);
```
This represents the top most left cell in a grid. The value in this cell is one.
