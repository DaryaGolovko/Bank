namespace Models;

public class AdminModel : UserModel
{
    private const int AccessLevel = 5;

    public AdminModel(string name, string phone, string email) :
        base(name, AccessLevel)
    {
        Phone = phone;
        Email = email;
    }

    public AdminModel()
    {
        Name = "Undefined";
        Phone = "Undefined";
        Email = "Undefined";
        }

    public string Phone { get; protected set; }
    public string Email { get; protected set; }
}