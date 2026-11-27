namespace VidensVejen.SharedKernelLib.Exceptions;

/// <summary>
/// 
/// </summary>
/// <param name="message"></param>
/// <param name="innerException"></param>
public class ConcurrencyConflictException(string message, Exception? innerException = null) : Exception(message, innerException)
{
}
