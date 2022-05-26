namespace Models;

public class BankModel
{
    public BankModel(string name)
    {
        Name = name;
    }

    public BankModel()
    {
        Name = "Undefined";
    }

    public string Name { get; protected set; }
}