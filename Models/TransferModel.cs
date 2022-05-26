namespace Models;

public class TransferModel
{
    public TransferModel(int firstAccount, int secondAccount, int money)
    {
        FirstAccount = firstAccount;
        SecondAccount = secondAccount;
        Money = money;
        Type = "Common";
    }
    
    public TransferModel(int firstAccount, int secondAccount, int money, string type)
    {
        FirstAccount = firstAccount;
        SecondAccount = secondAccount;
        Money = money;
        Type = type;
    }

    public int FirstAccount { get; protected set; }
    public int SecondAccount { get; protected set; }
    public int Money { get; protected set; }
    
    public string Type { get; protected set; }
}