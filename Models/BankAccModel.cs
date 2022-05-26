namespace Models;

public class BankAccModel
{
    public BankAccModel(int accountNumber, int balance, int bank, string state)
    {
        AccountNumber = accountNumber;
        Balance = balance;
        State = state;
        Bank = bank;
    }

    public int AccountNumber { get; protected set; }
    public int Balance { get; protected set; }
    public string State { get; set; }
    private int Bank { get; set; }
}