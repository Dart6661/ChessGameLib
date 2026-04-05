namespace ChessLib.Exceptions;

public class InputException(string message) : Exception(message);

public class OptionException(string message) : Exception(message);