namespace VidensVejen.SharedKernelLib.Exceptions;

/// <summary>
/// 
/// </summary>
/// <param name="message"></param>
public class UserNotActivatedException(string message) : Exception(message)
{
}
