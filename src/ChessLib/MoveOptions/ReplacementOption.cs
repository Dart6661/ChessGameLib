using ChessLib.Entities;

namespace ChessLib.MoveOptions;

public class ReplacementOption(FigureType selectedFigure) : MoveOption
{
    public FigureType SelectedFigure { get; init; } = selectedFigure;
}