namespace Controllers;

public static class AuthController
{
    public static int Auth(string login, string password = "")
    {
        if (MainController.Data.CheckLoginInDb(login, MainController.CurrentBank()).Result)
        {
            if (password == "")
                return 0;
            if (MainController.Data.CheckUserInDb(login, password).Result == "Good")
                return MainController.ChooseUser(MainController.Data.Authorize
                    (login, password, MainController.CurrentBank()).Result);
            if (MainController.Data.CheckUserInDb(login, password).Result == "WrongPass")
                return -1;
        }

        return -2;
    }
}