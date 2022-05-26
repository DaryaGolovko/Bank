using Models;

namespace Controllers;

public class OperatorController
{
    private static OperatorModel _operator = new();

    public static void Active(List<string> info)
    {
        _operator = new OperatorModel(info[0], info[4], info[5]);
    }

    public static List<string> Info()
    {
        return new List<string>
        {
            "Name: " + _operator.Name,
            "Phone: 80" + _operator.Phone,
            "Email: " + _operator.Email
        };
    }
    
    public static List<string> CheckTransfersForOperator() =>
        MainController.Data.CheckTransfersForOperator().Result;

    public static List<string> CheckSalaryPlans() =>
        MainController.Data.CheckSalaryPlans().Result;

    public static void ConfirmSalaryPlan(int id) =>
        MainController.Data.ConfirmSalaryPlan(id);

    public static void RejectSalaryPlan(int id) =>
        MainController.Data.RejectSalaryPlan(id);

    public static void CancelTransfer(int id) =>
        MainController.Data.CancelTransfer(id);

    public static List<int> GetAllTransferId() =>
        MainController.Data.GetAllTableId("Transfer").Result;
    
    public static List<int> GetAllSalaryPlanId() =>
        MainController.Data.GetAllTableId("SalaryPlan").Result;
    
    public static bool CheckAccIfExists(string table, int id) =>
        MainController.Data.CheckAccountIfExists(table, id);
}