namespace Models;

public class CreditModel
{
    public CreditModel(string date, int months, int percent, int money)
    {
        Date = date;
        Months = months;
        Percent = percent;
        Money = money;
        Remains = Money * percent / 100 + Money;
        Payment = Remains / Months;
    }

    public string Date { get; protected set; }
    public int Payment { get; protected set; }
    public int Months { get; protected set; }
    public int Remains { get; protected set; }
    public int Percent { get; protected set; }
    public int Money { get; protected set; }
}