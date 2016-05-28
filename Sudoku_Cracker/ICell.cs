namespace Sudoku_Cracker
{
    public interface ICell
    {
        int Row { get; set; }
        int Column { get; set; }
        int? Value { get; set; }
    }
}
