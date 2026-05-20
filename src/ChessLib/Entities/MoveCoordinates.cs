namespace ChessLib.Entities;

public class MoveCoordinates(int a, int b, int x, int y)
{
    public int A { get; set; } = a;
    public int B { get; set; } = b;
    public int X { get; set; } = x;
    public int Y { get; set; } = y;
}