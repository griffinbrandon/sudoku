# Sudoku Cracker
SudokuCracker is a portable Xamarin library which can solve any 9x9 sudoku puzzle in under a millisecond (at least on my machine). 

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
This represents the top most left cell in a grid. The value in this grid is one.
