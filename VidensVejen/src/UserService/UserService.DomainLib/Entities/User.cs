using UserService.DomainLib.ValueObjects;
using VidensVejen.SharedKernelLib.Exceptions;

namespace UserService.DomainLib.Entities;

/// <summary>
/// 
/// </summary>
public class User
{
    /// <summary>
    /// 
    /// </summary>
    public UserId Id { get; init; } = null!;
    /// <summary>
    /// 
    /// </summary>
    public string Firstname { get; private set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string Surname { get; private set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public Email EmailAddress { get; private set; } = null!;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="firstname"></param>
    /// <param name="surname"></param>
    /// <param name="emailAddress"></param>
    /// <returns></returns>
    /// <exception cref="DomainException"></exception>
    public static User Create(UserId id, string firstname, string surname, string emailAddress)
    {
        if (id == null || id.Value == Guid.Empty)
            throw new DomainException("Id could not be set, no value");

        if (string.IsNullOrWhiteSpace(firstname))
            throw new DomainException("Firstname could not be set, no value");

        if (string.IsNullOrWhiteSpace(surname))
            throw new DomainException("Surname could not be set, no value");

        if (string.IsNullOrWhiteSpace(emailAddress))
            throw new DomainException("Email could not be set, no value");

        var email = new Email(emailAddress);

        var user = new User(id, firstname, surname, email);

        return user;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newFirstname"></param>
    /// <exception cref="DomainException"></exception>
    public void ChangeFirstname(string newFirstname)
    {
        if (string.IsNullOrWhiteSpace(newFirstname))
            throw new DomainException("Firstname could not be set, no value");

        Firstname = newFirstname;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newSurname"></param>
    /// <exception cref="DomainException"></exception>
    public void ChangeSurname(string newSurname)
    {
        if (string.IsNullOrWhiteSpace(newSurname))
            throw new DomainException("Firstname could not be set, no value");

        Firstname = newSurname;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newEmail"></param>
    /// <exception cref="DomainException"></exception>
    public void ChangeEmail(string newEmail)
    {
        var email = new Email(newEmail);

        if (EmailAddress == email)
            throw new DomainException("New and old email can not be the same");

        EmailAddress = email;
    }

    private User(UserId id, string firstname, string surname, Email email)
    {
        Id = id;
        Firstname = firstname;
        Surname = surname;
        EmailAddress = email;
    }

    // EF constructor
    private User() { }
}
