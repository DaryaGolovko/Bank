using Models;

namespace Controllers;

public static class EnterpriceController
{
    private static EnterpriceModel _enterprice = new();
    private static SpecialistModel _specialist = new();

    public static void Active(List<string> info, List<string> enterprice)
    {
        _enterprice = new EnterpriceModel(enterprice[0], enterprice[1], enterprice[2], enterprice[3], enterprice[4], enterprice[5]);
        _specialist = new SpecialistModel(info[0], info[1], info[2], info[3], info[4], info[5], enterprice[1]);
    }

    public static List<string> EnterpriceInfo()
    {
        return new()
        {
            "Name: " + _enterprice.Name,
            "Tip: " + _enterprice.Type,
            "UBP: " + _enterprice.Pan,
            "BIK: " + _enterprice.Bik,
            "Address: " + _enterprice.Address,
            "Bank account: " + _enterprice.Account
        };
    }
    
    public static List<string> SpecialistInfo()
    {
        return new List<string>
        {
            "Name: " + _specialist.Name,
            "Surname: " + _specialist.Surname,
            "Passport: " + _specialist.Passport,
            "Identification: " + _specialist.Identification,
            "Phone: " + _specialist.Phone,
            "Email: " + _specialist.Email,
            "Work: " + _enterprice.Name
        };
    }

    public static void TransferMoney(int acc, int money) =>
        MainController.Data.TransferMoney(new TransferModel(Convert.ToInt32(_enterprice.Account), acc, money), "Enterprice");

    public static List<int> Salary() =>
        MainController.Data.SalaryPlan(_enterprice.Name, _specialist.Passport).Result;

    public static int CheckMoney() => 
        MainController.Data.CheckAccountMoney(Convert.ToInt32(_enterprice.Account)).Result;

}