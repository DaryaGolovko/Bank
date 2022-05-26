namespace Models;

public class OperatorModel : UserModel
{
    private const int AccessLevel = 3;

    public OperatorModel(string name, string phone, string email) :
        base(name, AccessLevel)
    {
        Phone = phone;
        Email = email;
    }

    public OperatorModel()
    {
        Name = "Undefined";
        Phone = "Undefined";
        Email = "Undefined";
    }

    public string Phone { get; protected set; }
    public string Email { get; protected set; }
}