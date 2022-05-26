namespace Models;

public class ClientModel : UserModel
{
    private const int AccessLevel = 1;

    public ClientModel(string name, string surname, string passport,
        string identification, string phone, string email, int cash, string work) :
        base(name, AccessLevel)
    {
        Surname = surname;
        Passport = passport;
        Identification = identification;
        Phone = phone;
        Email = email;
        BankAccsList = new List<BankAccModel>();
        Cash = cash;
        Work = work;
    }

    public ClientModel()
    {
        Name = "Undefined";
        Surname = "Undefined";
        Passport = "Undefined";
        Identification = "Undefined";
        Phone = "Undefined";
        Email = "Undefined";
        Cash = 0;
        Work = "Undefined";
        BankAccsList = new List<BankAccModel>();
    }

    public string Surname { get; protected set; }
    public int Cash { get; set; }
    public string Work { get; protected set; }
    public string Passport { get; protected set; }
    public string Identification { get; protected set; }
    public string Phone { get; protected set; }
    public string Email { get; protected set; }
    public List<BankAccModel> BankAccsList { get; protected set; }
}