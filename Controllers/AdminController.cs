using Models;

namespace Controllers;

public static class AdminController
{
    private static AdminModel _admin = new();

    public static void Active(List<string> info)
    {
        _admin = new AdminModel(info[0], info[4], info[5]);
    }

    public static List<string> Info()
    {
        return new List<string>
        {
            "Name: " + _admin.Name,
            "Phone: 80" + _admin.Phone,
            "Email: " + _admin.Email
        };
    }

    public static List<string> Logs() =>
        MainController.Data.CheckLogs().Result;

    public static void BlockUser(int id) =>
        MainController.Data.BlockClient(id);

    public static List<string> ShowUsers() =>
        MainController.Data.ShowUsers().Result;

    public static List<int> GetAllUserId() =>
        MainController.Data.GetAllTableId("UsersAuth").Result;

    public static bool CheckAccIfExists(string table, int id) =>
        MainController.Data.CheckAccountIfExists(table, id);

    public static void SkipMonth() =>
        MainController.Data.SkipMonth();
}