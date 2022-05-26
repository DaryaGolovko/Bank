namespace Models;

public class EnterpriceModel
{
    public string Name { get; protected set; }
    public string Type { get; protected set; }
    public string Pan { get; protected set; }
    public string Bik { get; protected set; }
    public string Address { get; protected set; }
    public string Account { get; protected set; }

    public EnterpriceModel(string type, string name, string pan, string bik, string address, string account)
    {
        Type = type;
        Name = name;
        Pan = pan;
        Bik = bik;
        Address = address;
        Account = account;
    }

    public EnterpriceModel()
    {
        Name = "Undefined";
        Type = "Undefined";
        Pan = "Undefined";
        Bik = "Undefined";
        Address = "Undefined";
        Account = "Undefined";
    }
}