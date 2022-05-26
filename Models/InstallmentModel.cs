namespace Models;

public class InstallmentModel
{
    public InstallmentModel(string date, int months, int money)
    {
        Date = date;
        Money = money;
        Remains = (int) (Money * 1.05);
        Payment = Remains / months;
    }

    public string Date { get; protected set; }
    public int Payment { get; protected set; }
    public int Remains { get; protected set; }
    public int Money { get; protected set; }
}