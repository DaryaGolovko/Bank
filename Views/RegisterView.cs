using Controllers;
using Entities;

namespace Views;

public static class RegisterView
{
    public static void Register()
    {
        var info = new List<string>();

        Console.WriteLine("Придумате логин от 3 до 12 символов:");
        var tmp = InputHelper.GetStrWithLettersAndNumbers(3, 12);

        while (!RegisterController.CheckLogin(tmp))
        {
            Console.WriteLine("Такой логин уже существует");
            tmp = InputHelper.GetStrWithLettersAndNumbers(3, 12);
        }

        info.Add(tmp);

        Console.WriteLine("Введите пароль от 4 до 30 символов:");
        tmp = InputHelper.GetCorrectPassword();
        info.Add(tmp);

        Console.WriteLine("Введите имя:");
        tmp = InputHelper.GetStrWithLetters();
        info.Add(tmp);

        Console.WriteLine("Введите фамилию:");
        tmp = InputHelper.GetStrWithLetters();
        info.Add(tmp);

        Console.WriteLine("Введите серию и номер паспорта:");
        tmp = InputHelper.GetCorrectPassport();
        while (RegisterController.Check("Passport", tmp))
        {
            Console.WriteLine("Этот паспорт уже зарегестрирован");
            tmp = InputHelper.GetCorrectPassport();
        }

        info.Add(tmp);

        Console.WriteLine("Введите id:");
        tmp = InputHelper.GetCorrectIdentification();
        while (RegisterController.Check("Identification", tmp))
        {
            Console.WriteLine("Этот id уже используется");
            tmp = InputHelper.GetCorrectIdentification();
        }

        info.Add(tmp);

        Console.WriteLine("Введите номер телефона в формате +375ххххххххх:");
        tmp = InputHelper.GetCorrectPhone();
        while (RegisterController.Check("Phone", tmp))
        {
            Console.WriteLine("Этот номер уже используется");
            tmp = InputHelper.GetCorrectPhone();
        }

        info.Add(tmp);

        Console.WriteLine("Введите email:");
        tmp = InputHelper.GetCorrectEmail();
        while (RegisterController.Check("Email", tmp))
        {
            Console.WriteLine("Этот email уже используется");
            tmp = InputHelper.GetCorrectEmail();
        }

        info.Add(tmp);

        RegisterController.Register(info);
        Console.Clear();
        Console.WriteLine("Запрос на регистрацию отправлен.");
    }
}