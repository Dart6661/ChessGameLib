namespace ChessLib.Exceptions;

public class MovementException(string message) : Exception(message);

public class ReplacementException(string message) : Exception(message);