using Models;

namespace Controllers;

public class ManagerController : OperatorController
{
    private static ManagerModel _manager = new();

    public new static void Active(List<string> info)
    {
        _manager = new ManagerModel(info[0], info[4], info[5]);
    }

    public new static List<string> Info()
    {
        return new List<string>
        {
            "Name: " + _manager.Name,
            "Phone: 80" + _manager.Phone,
            "Email: " + _manager.Email
        };
    }

    public static void ConfirmLoan(int id) => 
        MainController.Data.ConfirmLoan(id);

    public static List<string> CheckLoans(string status = "All") =>
        MainController.Data.CheckLoans(MainController.CurrentBank(), status).Result;

    public static List<int> GetAllLoanId() =>
        MainController.Data.GetAllTableId("Loans").Result;

    public static List<string> CheckRegInfo() =>
        MainController.Data.CheckRegInfo().Result;
    
    public static void ConfirmRegistration(int id) =>
        MainController.Data.ConfirmRegistration(id);

    public static void CancelRegistration(int id) =>
        MainController.Data.CancelRegistration(id);

    public static List<int> GetAllRegId() =>
        MainController.Data.GetAllTableId("DeclaredUsers").Result;
    public static List<string> CheckTransfers() =>
        MainController.Data.CheckTransfers().Result;
}