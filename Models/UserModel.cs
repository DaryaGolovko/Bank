namespace Models;

public class UserModel
{
    protected UserModel(string name, int accessLevel)
    {
        Name = name;
        AccessLevel = accessLevel;
    }

    protected UserModel()
    {
        Name = "Undefined";
        AccessLevel = 0;
    }

    public string Name { get; protected set; }
    private int AccessLevel { get; set; }
}