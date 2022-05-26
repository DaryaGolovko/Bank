namespace Models;

public class DepositModel
{
    public DepositModel(string date, int percent, int money)
    {
        Date = date;
        Percent = percent;
        Money = money;
        Profit = money * (percent / 100 + 1);
    }

    public string Date { get; protected set; }
    public int Profit { get; protected set; }
    public int Percent { get; protected set; }
    public int Money { get; protected set; }
}