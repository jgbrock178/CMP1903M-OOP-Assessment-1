namespace CMP1903M_Assessment_1_Base_Code;

/// <summary>
/// Custom exception to ensure valid border levels are passed to ConsoleGui.
/// </summary>
public class InvalidBorderLevelException : Exception
{
    /// <summary>
    /// Default exception message.
    /// </summary>
    public InvalidBorderLevelException() : base("Invalid border level used.") { }

    /// <summary>
    /// Exception message when a border level is included.
    /// </summary>
    /// <param name="level">The level passed to the function.</param>
    public InvalidBorderLevelException(int level) 
        : base($"Border level {level} is not valid. Valid values are 0, 1 or 2.") { }
}

/// <summary>
/// Custom exception to ensure valid border types are used.
/// </summary>
public class InvalidBorderTypeException : Exception
{
    /// <summary>
    /// Default exception message.
    /// </summary>
    public InvalidBorderTypeException() : base("Invalid border type used.") { }

    /// <summary>
    /// Exception message when a border type is included.
    /// </summary>
    /// <param name="borderType">The border type requested.</param>
    public InvalidBorderTypeException(string borderType)
        : base($"Border type {borderType} is not valid. Valid values are 'punched' or 'plain'.") { }
}