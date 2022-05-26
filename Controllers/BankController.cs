namespace Controllers;

public static class BankController
{
    public static List<string> GetAllBanks()
    {
        return MainController.Data.ChooseBank().Result;
    }

    public static void ChooseBank(string choice)
    {
        MainController.ChooseBank(choice);
    }
}