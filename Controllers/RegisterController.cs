namespace Controllers;

public static class RegisterController
{
    public static bool Check(string dest, string info)
    {
        return MainController.Data.CheckInfoInUsers(dest, info, MainController.CurrentBank()).Result;
    }

    public static bool CheckLogin(string login)
    {
        return MainController.Data.IsLoginFree(login, MainController.CurrentBank()).Result;
    }

    public static void Register(List<string> info)
    {
        MainController.Data.Register(info, MainController.CurrentBank());
    }
}