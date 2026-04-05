namespace ChessLib.MoveOptions;

public class ReplacementOption(Type selectedFigure) : MoveOption
{
    public Type SelectedFigure { get; init; } = selectedFigure;
}