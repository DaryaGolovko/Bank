namespace Models;

public class SpecialistModel : UserModel
{
    private const int AccessLevel = 2;

    public SpecialistModel(string name, string surname, string passport,
        string identification, string phone, string email, string work) :
        base(name, AccessLevel)
    {
        Surname = surname;
        Passport = passport;
        Identification = identification;
        Phone = phone;
        Email = email;
        Work = work;
    }

    public SpecialistModel()
    {
        Name = "Undefined";
        Surname = "Undefined";
        Passport = "Undefined";
        Identification = "Undefined";
        Phone = "Undefined";
        Email = "Undefined";
        Work = "Undefined";
    }

    public string Surname { get; protected set; }
    public string Work { get; protected set; }
    public string Passport { get; protected set; }
    public string Identification { get; protected set; }
    public string Phone { get; protected set; }
    public string Email { get; protected set; }
}