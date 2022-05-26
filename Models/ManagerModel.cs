namespace Models;

public class ManagerModel : UserModel
{
    private const int AccessLevel = 4;

    public ManagerModel(string name, string phone, string email) :
        base(name, AccessLevel)
    {
        Phone = phone;
        Email = email;
    }

    public ManagerModel()
    {
        Name = "Undefined";
        Phone = "Undefined";
        Email = "Undefined";
    }

    public string Phone { get; protected set; }
    public string Email { get; protected set; }
}