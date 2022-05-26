using Controllers;
using Entities;

namespace Views;

public static class AuthView
{
    public static int Auth()
    {
        Console.WriteLine("Введите логин от 3 до 12 символов: ");
        var login = InputHelper.GetStrWithLettersAndNumbers(3, 12);

        var checkLogin = AuthController.Auth(login);
        while (checkLogin != 0)
        {
            Console.WriteLine($"Нет пользователя с таким логином: '{login}'. Попробуйте снова");
            Console.WriteLine("Введите логин от 3 до 12 символов: ");
            login = InputHelper.GetStrWithLettersAndNumbers(3, 12);
            checkLogin = AuthController.Auth(login);
        }

        Console.WriteLine("Введите логин от 3 до 12 символов: ");
        var password = InputHelper.GetStrWithLettersAndNumbers(3, 12);

        var checkUser = AuthController.Auth(login, password);
        while (checkUser == -1)
        {
            Console.WriteLine("Неверный пароль, попробуйте снова");
            Console.WriteLine("Введите логин от 3 до 12 символов: ");
            password = InputHelper.GetStrWithLettersAndNumbers(3, 12);
            checkUser = AuthController.Auth(login, password);
        }

        return checkUser;
    }
}