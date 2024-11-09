namespace CustomExceptions;

public class CannotAddItem(string message) : Exception(message);