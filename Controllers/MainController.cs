using Entities;
using Models;

namespace Controllers;

public static class MainController
{
    private static BankModel _bank = new();
    public static readonly Database Data = Database.CreateAsync("moiSecretnyKluchik").Result;

    public static void ChooseBank(string name) => _bank = new BankModel(name);

    public static void PrintCurrentBank() =>
        Console.WriteLine($"Текущий банк: {_bank.Name}");
    
    public static string CurrentBank() => _bank.Name;

    public static int ChooseUser(List<string> info)
    {
        var whoami = int.Parse(info[6]);
        switch (whoami)
        {
            case 1:
                ClientController.Active(info);
                break;
            case 2:
                EnterpriceController.Active(info, Data.Enterprice(info[2]).Result);
                break;
            case 3:
                OperatorController.Active(info);
                break;
            case 4:
                ManagerController.Active(info);
                break;
            case 5:
                AdminController.Active(info);
                break;
        }

        return whoami;
    }
}